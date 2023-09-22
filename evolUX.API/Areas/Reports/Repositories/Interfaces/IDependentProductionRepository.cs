using Shared.Models.Areas.Reports;
using System.Data;

namespace evolUX.API.Areas.Reports.Repositories.Interfaces
{
    public interface IDependentProductionRepository
    {
        public Task<IEnumerable<DependentPrintsInfo>> GetDependentPrintsProduction(int RunID, DataTable ServiceCompanyList);
        public Task<IEnumerable<DependentFullfillInfo>> GetDependentFullfillProduction(int RunID, DataTable ServiceCompanyList);
    }
}
