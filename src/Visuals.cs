using DuckGame;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrowseGamesPlus
{
    public static class Visuals
    {
        private static FancyBitmapFont s_smallFont = new FancyBitmapFont("smallFont") 
        { 
            scale = new Vec2(0.8f) 
        };

        private static FancyBitmapFont s_bigFont = new FancyBitmapFont("smallFont") 
        { 
            scale = new Vec2(1.2f) 
        };

        private static Dictionary<User, Sprite> s_avatars = new Dictionary<User, Sprite>();    

        private static Sprite s_normalMapsSprite;
        private static Sprite s_randomMapsSprite;
        private static Sprite s_customMapsSprite;
        private static Sprite s_internetMapsSprite;

        private static SpriteMap s_heartSprite;

        private static int s_pointsTimer = 0;

        static Visuals()
        {
            s_normalMapsSprite = new Sprite("normalIcon") 
            { 
                scale = new Vec2(1.1f) 
            };

            s_randomMapsSprite = new Sprite("randomIcons") 
            { 
                scale = new Vec2(1.1f) 
            };

            s_customMapsSprite = new Sprite("customIcon") 
            { 
                scale = new Vec2(1.1f) 
            };

            s_internetMapsSprite = new Sprite("rainbowIcon") 
            { 
                scale = new Vec2(1.1f) 
            };

            s_heartSprite = new SpriteMap("hats/hearts", 32, 32) 
            { 
                scale = new Vec2(0.45f) 
            };

            s_smallFont.maxRows = 11;
            s_smallFont.maxWidth = 55;
            s_smallFont.singleLine = true;
        }

        public static void Draw(UIServerBrowser.LobbyData data, float x, float y)
        {
            if (data is null || data.lobby is null || data.hasPassword)
                return;

            Lobby lobby = data.lobby;
            Color color = Options.Data.Color;

            if (Options.Data.Maps)
            {
                var mapsPosition = new Vec2(x + 410f, y - 0.5f);

                Graphics.Draw(s_normalMapsSprite, mapsPosition.x, mapsPosition.y, 0.6f);
                s_smallFont.Draw($"{lobby.GetNormalMapsCount()}%", new Vec2(mapsPosition.x + 10f, mapsPosition.y + 1f), color, 0.5f);

                Graphics.Draw(s_randomMapsSprite, mapsPosition.x, mapsPosition.y + 9f, 0.6f);
                s_smallFont.Draw($"{lobby.GetRandomMapsCount()}%", new Vec2(mapsPosition.x + 10f, mapsPosition.y + 10f), color, 0.5f);

                Graphics.Draw(s_customMapsSprite, mapsPosition.x, mapsPosition.y + 18f, 0.6f);
                s_smallFont.Draw($"{lobby.GetCustomMapsCount()}%", new Vec2(mapsPosition.x + 10f, mapsPosition.y + 19f), color, 0.5f);

                Graphics.Draw(s_internetMapsSprite, mapsPosition.x, mapsPosition.y + 27f, 0.6f);
                s_smallFont.Draw($"{lobby.GetInternetMapsCount()}%", new Vec2(mapsPosition.x + 10f, mapsPosition.y + 28f), color, 0.5f);
            }

            if (!Options.Data.ConnectionRequired)
                return;

            if (lobby.users.Where(user => user != null).Any())
            {
                if (Options.Data.Players)
                {
                    var position = new Vec2(x + 277f, y + 1f);
                    IEnumerable<User> users = lobby.users.Where(user => user != Steam.user && user != lobby.owner);

                    for (int i = 0; i < users.Count(); i++)
                    {
                        var offset = new Vec2(i / 4 < 1 ? 0f : s_smallFont.maxWidth + 8f, 9f * (i > 3 ? i - 4 : i));
                        User user = users.ElementAt(i);

                        s_smallFont.Draw(user.name, position + offset, color, 0.5f);

                        if (Options.Data.Friends && user.relationship == FriendRelationship.Friend)
                            Graphics.Draw(s_heartSprite, position.x + s_smallFont.GetWidth(user.name.LimitLength(11)) + offset.x, y + offset.y - 5f, 0.5f);
                    }
                }

                if (Options.Data.Avatars)
                {
                    Sprite avatar;

                    if (s_avatars.TryGetValue(lobby.owner, out avatar))
                    {
                        Graphics.Draw(avatar, x + 2f, y + 2f, 0.6f);
                    }
                    else
                    {
                        avatar = Utilities.SpriteFromBytes(lobby.owner.avatarMedium);

                        if (avatar is not null)
                            s_avatars.Add(lobby.owner, avatar);
                    }
                }
            }
            else
            {
                var builder = new StringBuilder("LOADING");

                for (int i = 0; i < s_pointsTimer / 30; i++)
                    builder.Append(".");

                s_bigFont.Draw(builder.ToString(), new Vec2(x + 240f, y + 12f), Color.Red, 0.5f);

                s_pointsTimer++;

                if (s_pointsTimer > 119)
                    s_pointsTimer = 0;
            }
        }
    }
}
