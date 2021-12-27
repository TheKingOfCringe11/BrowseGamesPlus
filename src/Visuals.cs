using DuckGame;
using System.Collections.Generic;
using System.Linq;

namespace BrowseGamesPlus
{
    static class Visuals
    {
        private static FancyBitmapFont _smallFont = new FancyBitmapFont("smallFont") { scale = new Vec2(0.8f) };
        private static FancyBitmapFont _bigFont = new FancyBitmapFont("smallFont") { scale = new Vec2(1.2f) };

        private static Dictionary<User, Sprite> _avatars = new Dictionary<User, Sprite>();    

        private static Sprite _normalMaps;
        private static Sprite _randomMaps;
        private static Sprite _customMaps;
        private static Sprite _internetMaps;

        private static SpriteMap _heart;

        private static int _pointTimer = 0;

        public static void Initialize()
        {
            _normalMaps = new Sprite("normalIcon") { scale = new Vec2(1.1f) };
            _randomMaps = new Sprite("randomIcons") { scale = new Vec2(1.1f) };
            _customMaps = new Sprite("customIcon") { scale = new Vec2(1.1f) };
            _internetMaps = new Sprite("rainbowIcon") { scale = new Vec2(1.1f) };

            _heart = new SpriteMap("hats/hearts", 32, 32) { scale = new Vec2(0.45f) };

            _smallFont.maxRows = 11;
            _smallFont.maxWidth = 55;
            _smallFont.singleLine = true;
        }

        public static void Draw(UIServerBrowser.LobbyData data, float x, float y)
        {
            if (data is null || data.lobby is null || data.hasPassword)
                return;

            Lobby lobby = data.lobby;
            Color color = Options.Data.Color;

            if (Options.Data.Maps)
            {
                Vec2 mapsPosition = new Vec2(x + 410f, y - 0.5f);

                Graphics.Draw(_normalMaps, mapsPosition.x, mapsPosition.y, 0.6f);
                _smallFont.Draw($"{lobby.GetNormalMapsCount()}%", new Vec2(mapsPosition.x + 10f, mapsPosition.y + 1f), color, 0.5f);

                Graphics.Draw(_randomMaps, mapsPosition.x, mapsPosition.y + 9f, 0.6f);
                _smallFont.Draw($"{lobby.GetRandomMapsCount()}%", new Vec2(mapsPosition.x + 10f, mapsPosition.y + 9f + 1f), color, 0.5f);

                Graphics.Draw(_customMaps, mapsPosition.x, mapsPosition.y + 18f, 0.6f);
                _smallFont.Draw($"{lobby.GetCustomMapsCount()}%", new Vec2(mapsPosition.x + 10f, mapsPosition.y + 18f + 1f), color, 0.5f);

                Graphics.Draw(_internetMaps, mapsPosition.x, mapsPosition.y + 27f, 0.6f);
                _smallFont.Draw($"{lobby.GetInternetMapsCount()}%", new Vec2(mapsPosition.x + 10f, mapsPosition.y + 27f + 1f), color, 0.5f);
            }

            if (!Options.Data.ConnectionRequired)
                return;

            if (lobby.users.Where(user => user != null).Any())
            {
                if (Options.Data.Players)
                {
                    Vec2 position = new Vec2(x + 277f, y + 1f);
                    var users = lobby.users.Where(user => user != Steam.user && user != lobby.owner);

                    for (int i = 0; i < users.Count(); i++)
                    {
                        Vec2 offset = new Vec2(i / 4 < 1 ? 0f : _smallFont.maxWidth + 8f, 9f * (i > 3 ? i - 4 : i));
                        User user = users.ElementAt(i);

                        _smallFont.Draw(user.name, position + offset, color, 0.5f);

                        if (Options.Data.Friends && user.relationship == FriendRelationship.Friend)
                            Graphics.Draw(_heart, position.x + _smallFont.GetWidth(user.name.LimitLength(11)) + offset.x, y + offset.y - 5f, 0.5f);
                    }
                }

                if (Options.Data.Avatars)
                {
                    Sprite avatar;

                    if (_avatars.TryGetValue(lobby.owner, out avatar))
                    {
                        Graphics.Draw(avatar, x + 2f, y + 2f, 0.6f);
                    }
                    else
                    {
                        avatar = Utilities.SpriteFromBytes(lobby.owner.avatarMedium);

                        if (avatar is not null)
                            _avatars.Add(lobby.owner, avatar);
                    }
                }
            }
            else
            {
                string loading = "LOADING";

                for (int i = 0; i < _pointTimer / 30; i++)
                    loading += ".";

                _bigFont.Draw(loading, new Vec2(x + 240f, y + 12f), Color.Red, 0.5f);

                _pointTimer++;

                if (_pointTimer > 119)
                    _pointTimer = 0;
            }
        }
    }
}
