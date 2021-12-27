using DuckGame;
using System.Collections.Generic;

namespace BrowseGamesPlus
{
    static class OptionsMenu
    {
        public static void AddToOptionsMenu(UIMenu optionsMenu, UIComponent optionsGroup)
        {
            UIMenu menu = Build(optionsMenu);

            optionsMenu.Add(new UIText("", Color.White));
            optionsMenu.Add(new UIMenuItem("BG+ OPTIONS", new UIMenuActionOpenMenu(optionsMenu, menu),
                c: Color.LightGreen, backButton: true));

            optionsGroup.Add(menu, false);
        }

        private static UIMenu Build(UIMenu optionsMenu)
        {
            var menu = new UIMenu("BG+ OPTIONS", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 
                190f, -1f, "@CANCEL@BACK", null, false);
            var data = Options.Data;

            menu.Add(new UIMenuItemToggle("PLAYERS", field: new FieldBinding(data, "Players")));

            menu.Add(new UIText("", default(Color)));

            menu.Add(new UIMenuItemNumber("SORT BY", field: new FieldBinding(data, "SortingMethodIndex", max: 4), valStrings: new List<string>
            {
                "DEFAULT",
                "PING",
                "MAPS",
                "PLAYERS COUNT",
                "NAME"
            }));
            menu.Add(new UIMenuItemNumber("TEXT COLOR", field: new FieldBinding(data, "ColorIndex", max: 5), valStrings: new List<string>
            {
                "|GREEN|GREEN ",
                "|DGBLUE|BLUE",
                "|PURPLE|PURPLE",
                "|ORANGE|ORANGE",
                "|YELLOW|YELLOW",
                "|WHITE|WHITE "
            }));

            menu.Add(new UIText("", default(Color)));

            menu.Add(new UIMenuItemToggle("FRIENDS", field: new FieldBinding(data, "Friends")));
            menu.Add(new UIMenuItemToggle("AVATARS", field: new FieldBinding(data, "Avatars")));
            menu.Add(new UIMenuItemToggle("MAPS", field: new FieldBinding(data, "Maps")));

            menu.Add(new UIText("", default(Color)));

            var backFunction = new UIMenuActionOpenMenuCallFunction(menu, optionsMenu, new UIMenuActionOpenMenuCallFunction.Function(Options.Save));
            menu.Add(new UIMenuItem("BACK", backFunction));
            menu.SetBackFunction(backFunction);

            menu.Update();

            return menu;
        }
    }
}
