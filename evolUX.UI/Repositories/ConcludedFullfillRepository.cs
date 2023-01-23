using evolUX.UI.Repositories.Interfaces;
using Flurl.Http;
using Flurl.Http.Configuration;
using Shared.BindingModels.Finishing;
using Shared.ViewModels.General;
using System.Data;
using System.Net;

namespace evolUX.UI.Repositories
{
    public class ConcludedFullfillRepository : RepositoryBase, IConcludedFullfillRepository
    {
        public ConcludedFullfillRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<ResultsViewModel> RegistFullFill(string FileBarcode, string user, string ServiceCompanyList)
        {
            try
            {
                Regist bindingModel = new Regist();
                bindingModel.FileBarcode = FileBarcode;
                bindingModel.User = user;
                bindingModel.ServiceCompanyList = ServiceCompanyList;
                var response = await _flurlClient.Request("/API/finishing/PendingRegist/RegistFullFill")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, bindingModel);

                return await response.GetJsonAsync<ResultsViewModel>();
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
