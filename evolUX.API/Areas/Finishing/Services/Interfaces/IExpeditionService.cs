﻿using Shared.ViewModels.Areas.Finishing;
using System.Data;
using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Areas.Finishing;
using Shared.ViewModels.General;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IExpeditionService
    {
        public Task<IEnumerable<Business>> GetCompanyBusiness(DataTable CompanyBusinessList);
        public Task<ExpeditionFilesViewModel> GetPendingExpeditionFiles(int businessID, DataTable ServiceCompanyList);
        public Task<Result> RegistExpeditionReport(List<RegistExpReportElement> expFiles, string userName, int userID);
    }
}
