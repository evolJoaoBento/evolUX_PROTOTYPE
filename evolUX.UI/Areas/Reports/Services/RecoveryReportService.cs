using evolUX.UI.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Shared.ViewModels.Areas.Reports;
using Flurl.Http;
using System.Data;
using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using evolUX.UI.Exceptions;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using Shared.Models.Areas.Finishing;
using evolUX.UI.Areas.Reports.Services.Interfaces;
using evolUX.UI.Areas.Reports.Repositories.Interfaces;
using Microsoft.VisualBasic;


namespace evolUX.UI.Areas.Reports.Services
{
    public class RecoveryReportService
    {
        private readonly IRecoveryReportRepository _recoveryReportRepository;
        public RecoveryReportService(IRecoveryReportRepository recoveryReportRepository)
        {
            _recoveryReportRepository = recoveryReportRepository;
        }
    }
}
