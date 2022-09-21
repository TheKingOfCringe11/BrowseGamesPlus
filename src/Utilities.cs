using DuckGame;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BrowseGamesPlus
{
    public static class Utilities
    {
        public static void InitializeLobbyUserData(UIServerBrowser.LobbyData data)
        {
            if (!Options.Data.ConnectionRequired || data is null || data.lobby is null)
                return;

            Steam.LeaveLobby(Steam.JoinLobby(data.lobby.id));
        }

        public static Sprite SpriteFromBytes(byte[] bytes)
        {
            try
            {
                if (bytes is null || bytes.Length != 16384)
                    return null;

                var texture = new Texture2D(Graphics.device, 64, 64);
                texture.SetData(bytes);

                return new Sprite(texture) 
                { 
                    scale = new Vec2(0.5f) 
                };
            }
            catch
            {
                return null; 
            }
        }

        public static T GetEnumValueByIndex<T>(int index)
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ElementAt(index);
        }

        public static void SortLobbies(List<UIServerBrowser.LobbyData> lobbyData)
        {
            lobbyData = lobbyData.Where(data => data.userCount > 0).ToList();

            if (Options.Data.SortingMethod == SortingMethod.Default)
            {
                lobbyData.Sort();
                return;
            }

            Comparison<UIServerBrowser.LobbyData> comparison = Options.Data.SortingMethod switch
            {
                SortingMethod.Ping => (a, b) => a.estimatedPing.CompareTo(b.estimatedPing),

                SortingMethod.Name => (a, b) => a.name.CompareTo(b.name),

                SortingMethod.PlayersCount => (a, b) => -a.userCount.CompareTo(b.userCount),

                SortingMethod.Maps => (a, b) =>
                {
                    return GetMapsValue(a).CompareTo(GetMapsValue(b));

                    int GetMapsValue(UIServerBrowser.LobbyData data)
                    {
                        Lobby lobby = data.lobby;

                        int result = lobby.GetNormalMapsCount() + lobby.GetRandomMapsCount() * 2 + 
                            lobby.GetCustomMapsCount() * 3 + lobby.GetInternetMapsCount() * 5;

                        return result;
                    }
                }
            };

            lobbyData.Sort(comparison);
        } 
    }
}
