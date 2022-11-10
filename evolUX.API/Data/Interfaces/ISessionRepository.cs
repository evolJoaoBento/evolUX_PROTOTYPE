﻿

using SharedModels.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface ISessionRepository
    {
        public Task<IEnumerable<int>> GetProfile(int user);
        public Task<IEnumerable<string>> GetServers(IEnumerable<int> profiles);
        public Task<DataTable> GetServiceCompanies(IEnumerable<string> servers);
    }
}
