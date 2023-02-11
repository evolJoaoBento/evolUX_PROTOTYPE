﻿using evolUX.API.Models;
using evolUX.UI.Areas.EvolDP.Repositories.Interfaces;
using evolUX.UI.Exceptions;
using evolUX.UI.Repositories;
using evolUX_dev.Areas.EvolDP.Models;
using Flurl.Http;
using Flurl.Http.Configuration;
using Shared.ViewModels.Areas.evolDP;
using Shared.ViewModels.Areas.Finishing;
using System.Net;

namespace evolUX.UI.Areas.EvolDP.Repositories
{
    public class DocCodeRepository : RepositoryBase, IDocCodeRepository
    {
        public DocCodeRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<DocCodeViewModel> GetDocCodeGroup()
        {
            try
            {
                var response = await _flurlClient.Request("/API/evolDP/DocCode/DocCodeGroup")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .GetAsync();
                if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
                return await response.GetJsonAsync<DocCodeViewModel>();
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
        public async Task<DocCodeViewModel> GetDocCode(string docLayout, string docType)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("DocLayout", docLayout);
                dictionary.Add("DocType", docType);
                var response = await _flurlClient.Request("/API/evolDP/DocCode/DocCode")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, dictionary);
                if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
                return await response.GetJsonAsync<DocCodeViewModel>();
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
