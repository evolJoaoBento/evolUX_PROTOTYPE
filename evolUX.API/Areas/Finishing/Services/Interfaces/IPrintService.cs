﻿using Shared.Models.Areas.Finishing;
using System.Data;
using Shared.Models.General;
using Shared.ViewModels.General;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.Finishing;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IPrintService
    {
        public Task<PrinterViewModel> GetPrinters(IEnumerable<int> profileList, string filesSpecs, bool ignoreProfiles);
        public Task<Result> Print(int runID, int fileID, string printer, string serviceCompanyCode, string username, int userID, string filePath, string fileName, string shortFileName);
        public void GetPrinterFeatures(string specs, ref bool printColor, ref bool printBlack, ref int plexFeature);
    }
}
