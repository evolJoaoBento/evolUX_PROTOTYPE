using evolUX.UI.Exceptions;
using Flurl.Http;
using Flurl.Http.Configuration;
using System.Net;
using evolUX.UI.Repositories;
using Shared.ViewModels.Areas.evolDP;
using evolUX.UI.Areas.evolDP.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Reflection.Metadata;
using System.Collections.Generic;

namespace evolUX.UI.Areas.evolDP.Repositories
{
    public class GenericRepository : RepositoryBase, IGenericRepository
    {
        public GenericRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<BusinessViewModel> GetCompanyBusiness(string companyBusinessList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("CompanyBusinessList", companyBusinessList);
            var response = await _flurlClient.Request("/API/evolDP/Generic/GetCompanyBusiness")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<BusinessViewModel>();
        }
        public async Task<BusinessViewModel> GetCompanyBusiness(int companyID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("CompanyID", companyID);
            var response = await _flurlClient.Request("/API/evolDP/Generic/GetCompanyBusiness")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<BusinessViewModel>();

        }
        public async Task<ProjectListViewModel> GetProjects(string companyBusinessList)
        {
            var response = await _flurlClient.Request("/API/evolDP/Generic/GetProjects")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, companyBusinessList);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ProjectListViewModel>();

        }
        public async Task<ConstantParameterViewModel> GetParameters()
        {
            try
            {
                var response = await _flurlClient.Request("/API/evolDP/Generic/GetParameters")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .GetAsync();
                if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
                return await response.GetJsonAsync<ConstantParameterViewModel>();
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

        public async Task<ConstantParameterViewModel> SetParameter(int parameterID, string parameterRef, int parameterValue, string parameterDescription)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("ParameterID", parameterID);
                dictionary.Add("ParameterRef", parameterRef);
                dictionary.Add("ParameterValue", parameterValue);
                dictionary.Add("ParameterDescription", parameterDescription);
                var response = await _flurlClient.Request("/API/evolDP/Generic/SetParameter")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, dictionary);
                if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
                return await response.GetJsonAsync<ConstantParameterViewModel>();
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

        public async Task<ConstantParameterViewModel> DeleteParameter(int parameterID)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("ParameterID", parameterID);
                var response = await _flurlClient.Request("/API/evolDP/Generic/DeleteParameter")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, dictionary);
                if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
                return await response.GetJsonAsync<ConstantParameterViewModel>();
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
