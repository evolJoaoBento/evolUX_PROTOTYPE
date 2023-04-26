namespace Shared.Models.Areas.evolDP
{
    public class ServiceCompanyExpCodeElement: ExpCenterElement
    {
        public int ExpCompanyID { get; set; }
        public string ExpCompanyName { get; set; }
        public int ServiceTaskID { get; set; }
        public string ServiceTaskDesc { get; set; }
    }
}
