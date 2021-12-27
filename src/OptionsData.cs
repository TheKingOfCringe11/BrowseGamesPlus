using DuckGame;

namespace BrowseGamesPlus
{
    public enum SortingMethod
    {
        Default,
        Ping,
        Maps,
        PlayersCount,
        Name
    }

    public class OptionsData : DataClass
    {
        private readonly Color[] _colors = new Color[]
        {
            Color.YellowGreen,
            Color.CornflowerBlue,
            Color.MediumPurple,
            Color.Orange,
            Color.Khaki,
            Color.White
        };

        public OptionsData()
        {
            Players = true;
            Avatars = true;
            Friends = true;
            Maps = true;

            _nodeName = "Options";
        }

        public bool ConnectionRequired => Players || Avatars;
        public SortingMethod SortingMethod => Utilities.GetEnumValueByIndex<SortingMethod>(SortingMethodIndex);
        public Color Color => _colors[ColorIndex];

        public int SortingMethodIndex { get; set; }
        public int ColorIndex { get; set; }
        public bool Players { get; set; }
        public bool Avatars { get; set; }
        public bool Friends { get; set; }
        public bool Maps { get; set; }
    }
}
