using System.Data;

namespace SharedModels.BindingModels.Finishing
{
    public class RegistPermissionLevel
    {
        public string FileBarcode { get; set; }
        public string User { get; set; }
        public DataTable ServiceCompanyList { get; set; }
        public bool PermissionLevel { get; set; }
    }
}
