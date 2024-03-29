﻿using evolUX.API.Models;
using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using evolUX.UI.Exceptions;
using evolUX.UI.Repositories;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using Shared.ViewModels.Areas.Finishing;
using System.Data;
using System.Net;
using System.Reflection;

namespace evolUX.UI.Areas.Finishing.Repositories
{
    public class PostalObjectRepository : RepositoryBase, IPostalObjectRepository
    {
        public PostalObjectRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<PostalObjectViewModel> GetPostalObjectInfo(string ServiceCompanyList, string PostObjBarCode)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyList", ServiceCompanyList);
            dictionary.Add("PostObjBarCode", PostObjBarCode);
            var response = await _flurlClient.Request("/API/finishing/PostalObject/GetPostalObjectInfo")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<PostalObjectViewModel>();
        }
    }
}
