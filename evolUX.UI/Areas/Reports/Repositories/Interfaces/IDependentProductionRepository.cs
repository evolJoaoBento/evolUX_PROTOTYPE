using Shared.ViewModels.Areas.Reports;

namespace evolUX.UI.Areas.Reports.Repositories.Interfaces
{
    public interface IDependentProductionRepository
    {
        public Task<DependentProductionViewModel> GetDependentPrintsProduction(int RunID, List<int> serviceCompanyList);
    }
}
