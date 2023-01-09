using Flurl.Http;
using Flurl.Http.Configuration;
using Shared.BindingModels.Finishing;
using System.Data;
using System.Net;

namespace evolUX.UI.Repositories
{
    public class RecoverRepository : RepositoryBase, IRecoverRepository
    {
        public RecoverRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<IFlurlResponse> RegistTotalRecover(string FileBarcode, string user, DataTable ServiceCompanyList, bool PermissionLevel)
        {
            try
            {
                RegistPermissionLevel bindingModel = new RegistPermissionLevel();
                bindingModel.FileBarcode = FileBarcode;
                bindingModel.User = user;
                bindingModel.ServiceCompanyList = ServiceCompanyList;
                bindingModel.PermissionLevel = PermissionLevel;
                var response = await _flurlClient.Request("/API/finishing/Recover/RegistTotalRecover")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .PostJsonAsync(bindingModel);
                //var response = await BaseUrl
                //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
                //     .GetAsync();

                return response;
            }

            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                dynamic d = ex.GetResponseJsonAsync();
                return d;
            }
        }
        public async Task<IFlurlResponse> RegistPartialRecover(string StartBarcode, string EndBarcode, string user, DataTable ServiceCompanyList, bool PermissionLevel)
        {
            try
            {
                RegistElaboratePermissionLevel bindingModel = new RegistElaboratePermissionLevel();
                bindingModel.StartBarcode = StartBarcode;
                bindingModel.EndBarcode = EndBarcode;
                bindingModel.User = user;
                bindingModel.ServiceCompanyList = ServiceCompanyList;
                bindingModel.PermissionLevel = PermissionLevel;
                var response = await _flurlClient.Request("/API/finishing/Recover/RegistPartialRecover")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .PostJsonAsync(bindingModel);
                //var response = await BaseUrl
                //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
                //     .GetAsync();

                return response;
            }

            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                dynamic d = ex.GetResponseJsonAsync();
                return d;
            }
        }

        public async Task<IFlurlResponse> RegistDetailRecover(string StartBarcode, string EndBarcode, string user, DataTable ServiceCompanyList, bool PermissionLevel)
        {
            try
            {
                RegistElaboratePermissionLevel bindingModel = new RegistElaboratePermissionLevel();
                bindingModel.StartBarcode = StartBarcode;
                bindingModel.EndBarcode = EndBarcode;
                bindingModel.User = user;
                bindingModel.ServiceCompanyList = ServiceCompanyList;
                bindingModel.PermissionLevel = PermissionLevel;
                var response = await _flurlClient.Request("/API/finishing/Recover/RegistDetailRecover")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .PostJsonAsync(bindingModel);
                //var response = await BaseUrl
                //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
                //     .GetAsync();

                return response;
            }

            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                dynamic d = ex.GetResponseJsonAsync();
                return d;
            }
        }

        public async Task<IFlurlResponse> GetPendingRecoveries(int ServiceCompanyID)
        {
            try
            {
                var response = await _flurlClient.Request("/API/finishing/Recover/PendingRecoveries")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SetQueryParam("ServiceCompanyID", ServiceCompanyID)
                    .GetAsync();
                //var response = await BaseUrl
                //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
                //     .GetAsync();

                return response;
            }

            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                dynamic d = ex.GetResponseJsonAsync();
                return d;
            }
        }
        public async Task<IFlurlResponse> GetPendingRecoveriesRegistDetail(int ServiceCompanyID)
        {
            try
            {
                var response = await _flurlClient.Request("/API/finishing/Recover/PendingRecoveriesRegistDetail")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SetQueryParam("ServiceCompanyID", ServiceCompanyID)
                    .GetAsync();
                //var response = await BaseUrl
                //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
                //     .GetAsync();

                return response;
            }

            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                dynamic d = ex.GetResponseJsonAsync();
                return d;
            }
        }



    }
}
