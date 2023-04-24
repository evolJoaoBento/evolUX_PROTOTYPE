using Shared.Models.Areas.evolDP;
using System.Text.RegularExpressions;

namespace Shared.Models.Areas.evolDP
{
    public class MaterialElement
    {
        public int GroupID { get; set; }
        public string GroupCode { get; set; }
        public string GroupDescription { get; set; }
        public int MaterialID { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public int MaterialTypeID { get; set; }
        public double MaterialWeight { get; set; }
        public string MaterialRef { get; set; }
        public int FullFillSheets { get; set; }
        public string FullFillMaterialCode { get; set; }
        public double ExpeditionMinWeight { get; set; }
        public IEnumerable<MaterialCostElement> CostList { get; set; }
    }
}