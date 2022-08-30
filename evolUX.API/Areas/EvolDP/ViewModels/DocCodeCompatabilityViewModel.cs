using evolUX.API.Areas.EvolDP.Models;

namespace evolUX.API.Areas.EvolDP.ViewModels
{
    public class DocCodeCompatabilityViewModel
    {
        public AggregateDocCode DocCode { get; set; }
        public IEnumerable<AggregateDocCode> DocCodeList { get; set; }


    }
}
