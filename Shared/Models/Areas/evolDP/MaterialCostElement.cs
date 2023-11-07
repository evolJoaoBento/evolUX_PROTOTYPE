using Shared.Models.Areas.evolDP;

namespace Shared.Models.Areas.evolDP
{
    public class MaterialCostElement
    {
        public int GroupID { get; set; }
        public int MaterialID { get; set; }
        public int ServiceCompanyID { get; set; }
        public int ProviderCompanyID { get; set; }
        public int CostDate { get; set; }
        public double MaterialCost { get; set; }
        public int MaterialBinPosition { get; set; }
        public int ServiceCompanyMaterialPosition { get; set; }
    }
}
