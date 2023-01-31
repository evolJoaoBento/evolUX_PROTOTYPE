using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using evolUX.UI.Exceptions;
using evolUX.UI.Repositories;
using Flurl.Http;
using Flurl.Http.Configuration;
using Shared.BindingModels.Finishing;
using Shared.ViewModels.Areas.Finishing;
using Shared.ViewModels.General;
using System.Data;
using System.Net;

namespace evolUX.UI.Areas.Finishing.Repositories
{
    public class ConcludedFullfillRepository : RepositoryBase, IConcludedFullfillRepository
    {
        public ConcludedFullfillRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<ResultsViewModel> RegistFullFill(string FileBarcode, string user, string ServiceCompanyList)
        {
            Regist bindingModel = new Regist();
            bindingModel.FileBarcode = FileBarcode;
            bindingModel.User = user;
            bindingModel.ServiceCompanyList = ServiceCompanyList;
            var response = await _flurlClient.Request("/API/finishing/PendingRegist/RegistFullFill")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, bindingModel);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ResultsViewModel>();
        }
    }
}
