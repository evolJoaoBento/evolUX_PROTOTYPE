using System.Data;

namespace evolUX.API.BindingModels.Finishing
{
    public class RegistWithPermissionLevel
    {
        public string FileBarcode { get; set; }
        public string User { get; set; }
        public DataTable ServiceCompanyList { get; set; }
        public bool PermissionLevel { get; set; }
    }
}
