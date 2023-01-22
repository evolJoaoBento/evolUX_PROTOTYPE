using evolUX.UI.Repositories.Interfaces;
using Flurl.Http;
using Flurl.Http.Configuration;
using Shared.BindingModels.Finishing;
using System.Data;
using System.Net;

namespace evolUX.UI.Repositories
{
    public class ConcludedPrintRepository : RepositoryBase, IConcludedPrintRepository
    {
        public ConcludedPrintRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        //public async Task<IFlurlResponse> RegistPrint(string FileBarcode, string user, string ServiceCompanyList)
        //{
        //    try
        //    {
        //       var response = await _flurlClient.Request("/api/finishing/ConcludedPrint/RegistPrint")
        //       //var response = await _flurlClient.Request("/api/finishing/PostalObject/RegistPrint")
        //            .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
        //            .SetQueryParam("FileBarcode", FileBarcode)
        //            .SendJsonAsync(HttpMethod.Get, ServiceCompanyList);
        //        return response;
        //    }
        //    catch (FlurlHttpException ex)
        //    {
        //        // For error responses that take a known shape
        //        //TError e = ex.GetResponseJson<TError>();
        //        // For error responses that take an unknown shape
        //        dynamic d = ex.GetResponseJsonAsync();
        //        return d;
        //    }
        //}
        public async Task<IFlurlResponse> RegistPrint(string FileBarcode, string user, string ServiceCompanyList)
        {
            try
            {
                Regist bindingModel = new Regist();
                bindingModel.FileBarcode = FileBarcode;
                bindingModel.User = user;
                bindingModel.ServiceCompanyList = ServiceCompanyList;
                var response = await _flurlClient.Request("/api/finishing/PendingRegist/RegistPrint")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, bindingModel);
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
