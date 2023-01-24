﻿using evolUX.UI.Repositories.Interfaces;
using Flurl.Http;
using Flurl.Http.Configuration;
using Shared.BindingModels.Finishing;
using Shared.ViewModels.Areas.Finishing;
using Shared.ViewModels.General;
using System.Data;
using System.Net;

namespace evolUX.UI.Repositories
{
    public class RecoverRepository : RepositoryBase, IRecoverRepository
    {
        public RecoverRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<ResultsViewModel> RegistTotalRecover(string FileBarcode, string user, string ServiceCompanyList, bool PermissionLevel)
        {
            try
            {
                Regist bindingModel = new Regist();
                bindingModel.FileBarcode = string.IsNullOrEmpty(FileBarcode) ? "-1" : FileBarcode;
                bindingModel.User = user;
                bindingModel.ServiceCompanyList = ServiceCompanyList;
                bindingModel.PermissionLevel = PermissionLevel;
                var response = await _flurlClient.Request("/API/finishing/PendingRegist/RegistTotalRecover")
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
        public async Task<ResultsViewModel> RegistPartialRecover(string StartBarcode, string EndBarcode, string user, string ServiceCompanyList, bool PermissionLevel)
        {
            try
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

        public async Task<ResultsViewModel> RegistDetailRecover(string StartBarcode, string EndBarcode, string user, string ServiceCompanyList, bool PermissionLevel)
        {
            try
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

        //public async Task<IFlurlResponse> GetPendingRecoveries(int ServiceCompanyID)
        //{
        //    try
        //    {
        //        var response = await _flurlClient.Request("/API/finishing/PendingRegist/PendingRecoveries")
        //            .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
        //            .SetQueryParam("ServiceCompanyID", ServiceCompanyID)
        //            .GetAsync();
        //        //var response = await BaseUrl
        //        //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
        //        //     .GetAsync();

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
        //public async Task<IFlurlResponse> GetPendingRecoveriesRegistDetail(int ServiceCompanyID)
        //{
        //    try
        //    {
        //        var response = await _flurlClient.Request("/API/finishing/PendingRegist/PendingRecoveriesRegistDetail")
        //            .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
        //            .SetQueryParam("ServiceCompanyID", ServiceCompanyID)
        //            .GetAsync();
        //        //var response = await BaseUrl
        //        //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
        //        //     .GetAsync();

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

    }
}
