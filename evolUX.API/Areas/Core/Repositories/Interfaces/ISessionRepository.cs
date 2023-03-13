using Shared.Models.Areas.Core;
using Shared.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Areas.Core.Repositories.Interfaces
{
    public interface ISessionRepository
    {
        public bool HasEvolDP();
        public Task<IEnumerable<int>> GetProfile(int user);
        public Task<IEnumerable<string>> GetServers(IEnumerable<int> profiles);
        public Task<DataTable> GetCompanies(IEnumerable<string> servers, string CompanyType);
        public Task<IEnumerable<SideBarAction>> GetSideBarActions(IEnumerable<int> profiles);
        public Task<DataTable> GetCompanyBusiness(IEnumerable<string> servers, string CompanyType);
        public Task<IEnumerable<string>> GetPermissions(IEnumerable<int> profiles);
        public Task<IEnumerable<string>> GetPermissions(IEnumerable<int> profiles, string localizationKey);
        public Task<DataTable> evolDP_DESCRIPTION();
    }
}
