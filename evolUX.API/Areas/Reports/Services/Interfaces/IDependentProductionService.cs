using Shared.ViewModels.Areas.Reports;
using Shared.Models.Areas.Reports;
using System.Data;

namespace evolUX.API.Areas.Reports.Services.Interfaces
{
    public interface IDependentProductionService
    {
        public Task<DependentProductionViewModel> GetDependentPrintsProduction(DataTable ServiceCompanyList);
    }
}
