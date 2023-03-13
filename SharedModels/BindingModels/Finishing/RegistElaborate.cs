using System.Data;

namespace Shared.BindingModels.Finishing
{
    public class RegistElaborate
    {
        public string StartBarcode { get; set; }
        public string EndBarcode { get; set; }
        public string User { get; set; }
        public DataTable ServiceCompanyList { get; set; }
    }
}
