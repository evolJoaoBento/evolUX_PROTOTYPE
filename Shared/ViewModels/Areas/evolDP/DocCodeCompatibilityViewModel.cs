using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class DocCodeCompatibilityViewModel
    {
        public DocCode DocCode { get; set; }
        public IEnumerable<AggregateDocCode> AggDocCodeList { get; set; }


    }
}
