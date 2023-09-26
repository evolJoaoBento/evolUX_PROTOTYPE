using Shared.ViewModels.Areas.Reports;
using System.Data;

namespace evolUX.UI.Areas.Reports.Repositories.Interfaces
{
    public interface IDependentProductionRepository
    {
        public Task<DependentProductionViewModel> GetDependentPrintsProduction(DataTable serviceCompanyList);
    }
}
