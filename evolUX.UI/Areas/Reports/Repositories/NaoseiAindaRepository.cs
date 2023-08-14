using Shared.ViewModels.Areas.Finishing;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using evolUX.UI.Repositories;
using Shared.Models.Areas.Reports;
using evolUX.UI.Areas.Reports.Repositories.Interfaces;

namespace evolUX.UI.Areas.Reports.Repositories
{
    public class NaoseiAindaRepository : RepositoryBase, INaoseiAindaRepository
    {
        public NaoseiAindaRepository() : base()
        {

        }
    }
}
