using System.Data;

namespace evolUX.API.BindingModels.Finishing
{
    public class RegistElaboratePermissionLevel
    {
        public string StartBarcode { get; set; }
        public string EndBarcode { get; set; }
        public string User { get; set; }
        public DataTable ServiceCompanyList { get; set; }
        public bool PermissionLevel { get; set; }
    }
}
