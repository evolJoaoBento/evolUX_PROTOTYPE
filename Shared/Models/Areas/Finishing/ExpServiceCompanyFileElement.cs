using Shared.Models.Areas.Core;

namespace Shared.Models.Areas.Finishing
{
    public class ExpServiceCompanyFileElement
    {
        public int ServiceCompanyID { get; set; }
        public string ServiceCompanyCode { get; set; }
        public string ServiceCompanyName { get; set; }
        public List<ExpCompanyFileElement> ExpCompanyList { get; set; }
        public List<Job> ExpeditionReportsJobs { get; set; }
        public ExpServiceCompanyFileElement()
        {
            ExpCompanyList = new List<ExpCompanyFileElement>();
            ExpeditionReportsJobs = new List<Job>();
            ServiceCompanyCode = string.Empty;
            ServiceCompanyName = string.Empty;
        }
    }
}
