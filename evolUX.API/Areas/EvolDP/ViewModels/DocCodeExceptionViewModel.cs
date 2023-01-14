using evolUX.API.Areas.EvolDP.Models;

namespace evolUX.API.Areas.EvolDP.ViewModels
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
        public IEnumerable<TreatmentType> TreatmentTypes{ get; set; }
        public IEnumerable<int> FinishingList{ get; set; }
        public IEnumerable<int> ArchiveList{ get; set; }
        public IEnumerable<Email> EmailList { get; set; }
        public IEnumerable<int> EmailHideList { get; set; }
        public IEnumerable<Electronic> ElectronicList { get; set; }
        public IEnumerable<int> ElectronicHideList { get; set; }


    }
}
