using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class DocCodeCompatabilityViewModel
    {
        public AggregateDocCode DocCode { get; set; }
        public IEnumerable<AggregateDocCode> DocCodeList { get; set; }


    }
}
