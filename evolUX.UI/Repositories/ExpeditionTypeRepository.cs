﻿using Flurl.Http;
using Flurl.Http.Configuration;
using System.Net;

namespace evolUX.UI.Repositories
{
    public class ExpeditionTypeRepository : RepositoryBase, IExpeditionTypeRepository
    {
        public ExpeditionTypeRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor) : base(flurlClientFactory, httpContextAccessor)
        {
        }

        public async Task<IFlurlResponse> GetExpeditionTypes()
        {
            try
            {
                var response = await _flurlClient.Request("/evoldp/expeditiontype/GetExpeditionTypes")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .GetAsync();
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
