using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using evolUX.UI.Exceptions;
using evolUX.UI.Repositories;
using Flurl.Http;
using Flurl.Http.Configuration;
using Shared.BindingModels.Finishing;
using Shared.ViewModels.General;
using System.Data;
using System.Net;

namespace evolUX.UI.Areas.Finishing.Repositories
{
    public class RecoverRepository : RepositoryBase, IRecoverRepository
    {
        public RecoverRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<ResultsViewModel> RegistTotalRecover(string FileBarcode, string user, string ServiceCompanyList, bool PermissionLevel)
        {
            Regist bindingModel = new Regist();
            bindingModel.FileBarcode = string.IsNullOrEmpty(FileBarcode) ? "-1" : FileBarcode;
            bindingModel.User = user;
            bindingModel.ServiceCompanyList = ServiceCompanyList;
            bindingModel.PermissionLevel = PermissionLevel;
            var response = await _flurlClient.Request("/API/finishing/PendingRegist/RegistTotalRecover")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, bindingModel);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ResultsViewModel>();
        }
        public async Task<ResultsViewModel> RegistPartialRecover(string StartBarcode, string EndBarcode, string user, string ServiceCompanyList, bool PermissionLevel)
        {
            RegistElaborate bindingModel = new RegistElaborate();
            bindingModel.StartBarcode = string.IsNullOrEmpty(StartBarcode) ? "-1" : StartBarcode;
            bindingModel.EndBarcode = string.IsNullOrEmpty(EndBarcode) ? "-1" : EndBarcode;
            bindingModel.User = user;
            bindingModel.ServiceCompanyList = ServiceCompanyList;
            bindingModel.PermissionLevel = PermissionLevel;
            var response = await _flurlClient.Request("/API/finishing/PendingRegist/RegistPartialRecover")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, bindingModel);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ResultsViewModel>();
        }

        public async Task<ResultsViewModel> RegistDetailRecover(string StartBarcode, string EndBarcode, string user, string ServiceCompanyList, bool PermissionLevel)
        {
            RegistElaborate bindingModel = new RegistElaborate();
            bindingModel.StartBarcode = string.IsNullOrEmpty(StartBarcode) ? "-1" : StartBarcode;
            bindingModel.EndBarcode = string.IsNullOrEmpty(EndBarcode) ? "-1" : EndBarcode;
            bindingModel.User = user;
            bindingModel.ServiceCompanyList = ServiceCompanyList;
            bindingModel.PermissionLevel = PermissionLevel;
            var response = await _flurlClient.Request("/API/finishing/PendingRegist/RegistDetailRecover")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, bindingModel);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ResultsViewModel>();
        }
    }
}
