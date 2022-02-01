using DuckGame;

namespace BrowseGamesPlus
{
    public static class Options
    {
        private static OptionsData _data = new OptionsData();

        public static OptionsData Data => _data;
        private static string FileName => DuckFile.optionsDirectory + "DGData.dat";

        public static void Save()
        {
            var data = new DXMLNode("Data");
            data.Add(_data.Serialize());

            var xml = new DuckXML();
            xml.Add(data);

            DuckFile.SaveDuckXML(xml, FileName);
        }

        public static void Load()
        {
            DuckXML xml = DuckFile.LoadDuckXML(FileName);

            if (xml is null || xml.Elements("Data") is null)
                return;

            foreach (DXMLNode data in xml.Elements("Data").Elements())
            {
                if (data.Name == "Options")
                {
                    _data.Deserialize(data);
                    break;
                }
            }
        }
    }
}
