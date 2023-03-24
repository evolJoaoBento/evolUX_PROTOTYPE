using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class DocCodeConfigOptionsViewModel: ItemPermissions
    {
        public DocCode DocCodeConfig { get; set; }
        public IEnumerable<ExceptionLevel> Exceptionslevel1List { get; set; }
        public IEnumerable<ExceptionLevel> Exceptionslevel2List { get; set; }
        public IEnumerable<ExceptionLevel> Exceptionslevel3List { get; set; }
        public IEnumerable<EnvelopeMediaGroup> EnvMediaGroups { get; set; }
        public IEnumerable<int> AggregationList { get; set; }
        public IEnumerable<string> PrintMatchCodeList { get; set; }
        public IEnumerable<ExpCompanyServiceTask> ExpCodeList { get; set; }
        public IEnumerable<Company> ExpCompanies { get; set; }
        public IEnumerable<ExpeditionTypeElement> ExpeditionTypes{ get; set; }
        public IEnumerable<ServiceTaskElement> ServiceTasks{ get; set; }
         public GenericOptionList SuportTypeList{ get; set; }
    }
}
