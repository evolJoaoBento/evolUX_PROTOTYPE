using Shared.ViewModels.Areas.Reports;

namespace evolUX.UI.Areas.Reports.Services.Interfaces
{
    public interface IDependentProductionService
    {
        public Task<DependentProductionViewModel> GetDependentPrintsProduction(List<int> serviceCompanyList);
        public Task<DependentProductionViewModel> GetDependentFullfillProduction(List<int> serviceCompanyList);
    }
}
