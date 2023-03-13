using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace evolUX.API.Areas.Core.Services.Interfaces
{
    public interface ISessionService
    {
        public Task<IEnumerable<int>> GetProfile(int user);
        public Task<IEnumerable<string>> GetServers(IEnumerable<int> profiles);
        public Task<Dictionary<string, string>> GetSessionVariables([FromQuery] int User);
    }
}
