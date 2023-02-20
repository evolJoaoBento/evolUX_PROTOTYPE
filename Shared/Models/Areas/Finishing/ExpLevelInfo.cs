using Shared.Models.Areas.evolDP;

namespace Shared.Models.Areas.Finishing
{
    public class ExpLevelInfo : FileBase
    {
        public int ExpCompanyLevel { get; set; }
        public int MaxWeight { get; set; }
        public string ExpeditionLevelDesc { get; set; }
        public int PostalObjects { get; set; }
        public ExpLevelInfo(int expCompanyLevel, int maxWeight, string expeditionLevelDesc, int postalObjects) 
        { 
            ExpCompanyLevel = expCompanyLevel;
            MaxWeight = maxWeight;
            ExpeditionLevelDesc = expeditionLevelDesc;
            //if (string.IsNullOrEmpty(ExpeditionLevelDesc))
                ExpeditionLevelDesc = maxWeight < 0 ? "Unlimited" : maxWeight.ToString() + " g";
            PostalObjects = postalObjects;
        }
    }
}
