using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class DocCodeExceptionViewModel
    {
        public DocCodeConfig DocCodeConfig { get; set; }
        public IEnumerable<ExceptionLevel> DocExceptionslevel1 { get; set; }
        public IEnumerable<ExceptionLevel> DocExceptionslevel2 { get; set; }
        public IEnumerable<ExceptionLevel> DocExceptionslevel3 { get; set; }
        public IEnumerable<EnvelopeMedia> EnvelopeMediaGroups { get; set; }
        public IEnumerable<int> AggregationList { get; set; }
        public IEnumerable<Company> ExpeditionCompanies { get; set; }
        public IEnumerable<ExpeditionsType> ExpeditionTypes{ get; set; }
        public IEnumerable<ServiceTask> TreatmentTypes{ get; set; }
        public IEnumerable<int> FinishingList{ get; set; }
        public IEnumerable<int> ArchiveList{ get; set; }
        public IEnumerable<Email> EmailList { get; set; }
        public IEnumerable<int> EmailHideList { get; set; }
        public IEnumerable<Electronic> ElectronicList { get; set; }
        public IEnumerable<int> ElectronicHideList { get; set; }


    }
}
