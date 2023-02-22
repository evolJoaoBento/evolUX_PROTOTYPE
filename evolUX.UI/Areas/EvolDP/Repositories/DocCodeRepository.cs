using evolUX.API.Models;
using evolUX.UI.Areas.EvolDP.Repositories.Interfaces;
using evolUX.UI.Exceptions;
using evolUX.UI.Repositories;
using evolUX_dev.Areas.EvolDP.Models;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;
using Shared.ViewModels.Areas.Finishing;
using Shared.ViewModels.General;
using System.Data;
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

        public async Task<DocCodeConfigViewModel> GetDocCodeConfig(int docCodeID)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("DocCodeID", docCodeID);
                var response = await _flurlClient.Request("/API/evolDP/DocCode/DocCodeConfig")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, dictionary);
                if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
                return await response.GetJsonAsync<DocCodeConfigViewModel>();
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

        public async Task<DocCodeConfigOptionsViewModel> GetDocCodeConfigOptions(DocCode? docCode)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                if (docCode != null)
                {
                    string docCodeJSON = JsonConvert.SerializeObject(docCode);
                    dictionary.Add("DocCode", docCodeJSON);
                }
                var response = await _flurlClient.Request("/API/evolDP/DocCode/DocCodeConfigOptions")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, dictionary);
                if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
                return await response.GetJsonAsync<DocCodeConfigOptionsViewModel>();
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

        public async Task<DocCodeConfigViewModel> RegistDocCodeConfig(DocCode docCode)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                string docCodeJSON = JsonConvert.SerializeObject(docCode);
                dictionary.Add("DocCode", docCodeJSON);

                var response = await _flurlClient.Request("/API/evolDP/DocCode/RegistDocCodeConfig")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, dictionary);
                if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
                return await response.GetJsonAsync<DocCodeConfigViewModel>();
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

        public async Task<DocCodeConfigViewModel> ChangeDocCode(DocCode docCode)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                string docCodeJSON = JsonConvert.SerializeObject(docCode);
                dictionary.Add("DocCode", docCodeJSON);

                var response = await _flurlClient.Request("/API/evolDP/DocCode/ChangeDocCode")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, dictionary);
                if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
                return await response.GetJsonAsync<DocCodeConfigViewModel>();
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

        public async Task<ResultsViewModel> DeleteDocCodeConfig(int docCodeID, int startDate)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("DocCodeID", docCodeID);
                dictionary.Add("StartDate", startDate);
                var response = await _flurlClient.Request("/API/evolDP/DocCode/DeleteDocCodeConfig")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, dictionary);
                if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
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

        public async Task<ResultsViewModel> DeleteDocCode(DocCode docCode)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                string docCodeJSON = JsonConvert.SerializeObject(docCode);
                dictionary.Add("DocCode", docCodeJSON);

                var response = await _flurlClient.Request("/API/evolDP/DocCode/DeleteDocCode")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, dictionary);
                if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
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

        public async Task<DocCodeCompatibilityViewModel> GetCompatibility(int docCodeID)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("DocCodeID", docCodeID);
                var response = await _flurlClient.Request("/API/evolDP/DocCode/GetCompatibility")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, dictionary);
                if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
                return await response.GetJsonAsync<DocCodeCompatibilityViewModel>();
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
        public async Task<DocCodeCompatibilityViewModel> ChangeCompatibility(int docCodeID, List<string> docCodeList)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("DocCodeID", docCodeID);
                string ListJSON = JsonConvert.SerializeObject(docCodeList);
                dictionary.Add("DocCodeList", ListJSON);

                var response = await _flurlClient.Request("/API/evolDP/DocCode/ChangeCompatibility")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, dictionary);
                if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
                return await response.GetJsonAsync<DocCodeCompatibilityViewModel>();
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
