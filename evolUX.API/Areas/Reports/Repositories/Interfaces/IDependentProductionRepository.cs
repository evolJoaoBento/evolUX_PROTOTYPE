using Shared.Models.Areas.Reports;
using System.Data;

namespace evolUX.API.Areas.Reports.Repositories.Interfaces
{
    public interface IDependentProductionRepository
    {
        public Task<IEnumerable<DependentPrintsInfo>> GetDependentPrintsProduction(DataTable ServiceCompanyList);
        public Task<IEnumerable<DependentFullfillInfo>> GetDependentFullfillProduction(DataTable ServiceCompanyList);
    }
}
