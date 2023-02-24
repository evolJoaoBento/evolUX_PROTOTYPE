using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;
using System.Data;
using System.Data.SqlTypes;

namespace Shared.ViewModels.Areas.evolDP
{
    public class DocCodeData4ScriptViewModel: ItemPermissions
    {
        public DocCodeConfigScript Doc { get; set; }
        public IEnumerable<ExceptionLevelScript> ExceptionLevelList { get; set; }
        public IEnumerable<DocCodeScript> AggDocCodeList { get; set; }
    }
}
