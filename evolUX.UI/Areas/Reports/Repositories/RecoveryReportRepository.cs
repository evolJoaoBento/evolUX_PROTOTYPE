using Shared.ViewModels.Areas.Finishing;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using evolUX.UI.Repositories;
using Shared.Models.Areas.Finishing;
using evolUX.UI.Areas.Reports.Repositories.Interfaces;
using Shared.ViewModels.Areas.Reports;

namespace evolUX.UI.Areas.Reports.Repositories
{
    public class RecoveryReportRepository : RepositoryBase, IRecoveryReportRepository
    {
        public RecoveryReportRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }
    }
}
