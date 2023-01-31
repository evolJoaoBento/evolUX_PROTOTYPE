using Shared.Models.Areas.evolDP;
using Shared.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Areas.Finishing.Repositories.Interfaces
{
    public interface IExpeditionRepository
    {
        public Task<IEnumerable<Business>> GetCompanyBusiness(DataTable CompanyBusinessList);
        public Task<IEnumerable<ExpServiceCompanyFileElement>> GetPendingExpeditionFiles(int BusinessID, DataTable ServiceCompanyList);
        public Task<int> RegistFileList(DataTable fileList, string userName);
    }
}
