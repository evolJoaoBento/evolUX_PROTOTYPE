﻿using evolUX.UI.Repositories.Interfaces;
using Flurl.Http;
using Flurl.Http.Configuration;
using Shared.BindingModels.Finishing;
using System.Data;
using System.Net;

namespace evolUX.UI.Repositories
{
    public class ConcludedFullfillRepository : RepositoryBase, IConcludedFullfillRepository
    {
        public ConcludedFullfillRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<IFlurlResponse> RegistFullFill(string FileBarcode, string user, DataTable ServiceCompanyList)
        {
            try
            {
                Regist bindingModel = new Regist();
                bindingModel.FileBarcode = FileBarcode;
                bindingModel.User = user;
                bindingModel.ServiceCompanyList = ServiceCompanyList;
                var response = await _flurlClient.Request("/API/finishing/ConcludedFullfill/RegistFullFill")
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
    }
}
