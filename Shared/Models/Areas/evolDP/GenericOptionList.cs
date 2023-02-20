using Shared.Models.Areas.evolDP;

namespace Shared.Models.Areas.evolDP
{
    public class GenericOptionList
    {
        public IEnumerable<GenericOptionValue> List { get; set; }
        public IEnumerable<int> ValidList { get; set; }
        public IEnumerable<GenericOptionValue> OptionList { get; set; }
    }
}
