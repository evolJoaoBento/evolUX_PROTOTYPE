using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ExceptionLevelViewModel
    {
        public int Level { get; set; }
        public IEnumerable<ExceptionLevel> ExceptionslevelList { get; set; }
    }
}
