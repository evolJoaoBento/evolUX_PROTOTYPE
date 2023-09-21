using Shared.ViewModels.Areas.Reports;

namespace evolUX.UI.Areas.Reports.Repositories.Interfaces
{
    public interface IDependentProductionRepository
    {
        public Task<DependentProductionViewModel> GetDependentPrintsProduction(List<int> serviceCompanyList);
        public Task<DependentProductionViewModel> GetDependentFullfillProduction(List<int> serviceCompanyList);
    }
}
