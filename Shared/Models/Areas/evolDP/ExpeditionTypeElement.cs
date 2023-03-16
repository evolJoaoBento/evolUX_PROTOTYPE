namespace Shared.Models.Areas.evolDP
{
    public class ExpeditionTypeElement
    {
        public int ExpeditionType { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
        public IEnumerable<ExpCompanyType> ExpCompanyTypesList { get; set; } = new List<ExpCompanyType>();
    }
}
