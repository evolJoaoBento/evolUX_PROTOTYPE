namespace Shared.Models.Areas.evolDP
{
    public class ExpeditionZoneElement
    {
        public int ExpeditionZone { get; set; }
        public string Description { get; set; }
        public IEnumerable<ExpCompanyZone> ExpCompanyZonesList { get;set; } = new List<ExpCompanyZone>();
    }
}
