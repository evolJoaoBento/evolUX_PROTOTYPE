namespace Shared.Models.Areas.Finishing
{
    public class ProdMaterialElement
    {
        public int PaperMediaID { get; set; }
        public string PaperMaterialList { get; set; }

        public int StationMediaID { get; set; }
        public string StationMaterialList { get; set; }
        public List<ProdFileInfo> FileList { get; set; }
        public string[] _paperList;
        public string[] _stationList;

        public string[] PaperList { 
            get 
            {
                if (_paperList == null && !string.IsNullOrEmpty(PaperMaterialList))
                    _paperList = PaperMaterialList.Split('|');
                if (_paperList == null)
                    _paperList = new string[0];
                return(_paperList);
            } 
        }
        public string[] StationList
        {
            get
            {
                if (_stationList == null && !string.IsNullOrEmpty(StationMaterialList))
                    _stationList = StationMaterialList.Split('|');
                if (_stationList == null)
                    _stationList = new string[0];
                return (_stationList);
            }
        }


        public ProdMaterialElement()
        {
            FileList = new List<ProdFileInfo>();
            PaperMaterialList = string.Empty;
            StationMaterialList = string.Empty;
            _paperList = null;
            _stationList = null; 
        }
    }
}
