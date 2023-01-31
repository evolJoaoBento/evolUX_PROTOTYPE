using Shared.Models.Areas.Core;

namespace Shared.Models.Areas.Finishing
{
    public class RegistExpReportElement
    {
        public string ServiceCompanyCode { get; set; }
        public List<ExpFileElement> ExpFileList { get; set; }
        public RegistExpReportElement(string serviceCompanyCode)
        {
            ExpFileList = new List<ExpFileElement>();
            ServiceCompanyCode = serviceCompanyCode;
        }
    }
}
