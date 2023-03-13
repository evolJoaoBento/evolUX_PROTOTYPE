using Shared.Models.Areas.evolDP;

namespace Shared.Models.Areas.Finishing
{
    public class RegistExpReportElement
    {
        public string ServiceCompanyCode { get; set; }
        public List<FileBase> ExpFileList { get; set; }
        public RegistExpReportElement(string serviceCompanyCode)
        {
            ExpFileList = new List<FileBase>();
            ServiceCompanyCode = serviceCompanyCode;
        }
    }
}
