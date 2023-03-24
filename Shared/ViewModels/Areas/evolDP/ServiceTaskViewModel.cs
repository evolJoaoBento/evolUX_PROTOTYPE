using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;
using System.Data.SqlTypes;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ServiceTaskViewModel : ServiceWorkFlowViewModel
    {
        public int ServiceTaskID { get; set; }
        public IEnumerable<ExpCodeElement> ExpCodes { get; set; }
        public IEnumerable<Company> ExpCompanies { get; set; } = new List<Company>();
    }
}
