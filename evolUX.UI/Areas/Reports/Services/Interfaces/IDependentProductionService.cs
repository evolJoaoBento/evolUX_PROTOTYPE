using Shared.ViewModels.Areas.Reports;
using System.Data;

namespace evolUX.UI.Areas.Reports.Services.Interfaces
{
    public interface IDependentProductionService
    {
        public Task<DependentProductionViewModel> GetDependentPrintsProduction(DataTable ServiceCompanyList);
    }
}
