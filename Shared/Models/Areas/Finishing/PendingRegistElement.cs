﻿namespace Shared.Models.Areas.Finishing
{
    public class PendingRegistElement
    {
        public string FileName { get; set; }
        public int RunID { get; set; }
        public int FileID { get; set; }
        public string ServiceCompanyCode { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Operator { get; set; }
        public string Machine { get; set; }
        public int TotalPrint { get; set; }
        public int TotalPostObjs { get; set; }
        public string FullFillMaterialCode { get; set; }
        public PendingRegistElement()
        {
            FileName = string.Empty;
            ServiceCompanyCode = string.Empty;
            Operator = string.Empty;
            Machine = string.Empty;
        }
    }
}
