using Shared.ViewModels.Areas.Reports;

namespace evolUX.UI.Areas.Reports.Services.Interfaces
{
    public interface IDependentProductionService
    {
        public Task<DependentProductionViewModel> GetDependentPrintsProduction(int RunID, List<int> ServiceCompanyList);
    }
}
