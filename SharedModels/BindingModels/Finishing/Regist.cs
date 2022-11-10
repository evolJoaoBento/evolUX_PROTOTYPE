using System.Data;

namespace SharedModels.BindingModels.Finishing
{
    public class Regist
    {
        public string FileBarcode { get; set; }
        public string User { get; set; }
        public DataTable ServiceCompanyList { get; set; }
    }
}
