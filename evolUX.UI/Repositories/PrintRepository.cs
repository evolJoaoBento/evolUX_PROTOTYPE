using Dapper;
using Shared.ViewModels.Areas.Finishing;
using evolUX.API.Extensions;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using Shared.ViewModels.General;
using Shared.Models.General;
using evolUX.UI.Repositories.Interfaces;

namespace evolUX.UI.Repositories
{
    public class PrintRepository : RepositoryBase, IPrintRepository
    {
        public PrintRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<ResoursesViewModel> GetPrinters(string profileList, string filesSpecs, bool ignoreProfiles)
        {
            List<string> list = new List<string>();
            list.Add(profileList);
            list.Add(filesSpecs);
            string ListJSON = JsonConvert.SerializeObject(list);
            var response = await _flurlClient.Request("/API/Finishing/Print/Printers")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SetQueryParam("ignoreProfiles",ignoreProfiles)
                .SendJsonAsync(HttpMethod.Get, ListJSON);
            //var response = await BaseUrl
            //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
            //     .GetAsync();
            if (response.StatusCode == ((int)HttpStatusCode.NotFound)) throw new HttpNotFoundException(response);
            if (response.StatusCode == ((int)HttpStatusCode.Unauthorized)) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ResoursesViewModel>();
        }
        
        public async Task<Result> Print(int runID, int fileID, string printer, string serviceCompanyCode, 
            string username, int userID, string filePath, string fileName, string shortFileName)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("Username", username);
            dictionary.Add("UserID", userID);
            dictionary.Add("FilePath", filePath);
            dictionary.Add("FileID", fileID);
            dictionary.Add("RunID", runID);
            dictionary.Add("Printer", printer);
            dictionary.Add("ServiceCompanyCode", serviceCompanyCode);
            dictionary.Add("FileName", fileName);
            dictionary.Add("ShortFileName", shortFileName);

            string ListJSON = JsonConvert.SerializeObject(dictionary);

            var response = await _flurlClient.Request("/API/Finishing/Print/Print")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            //var response = await BaseUrl
            //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
            //     .GetAsync();
            if (response.StatusCode == ((int)HttpStatusCode.NotFound)) throw new HttpNotFoundException(response);
            if (response.StatusCode == ((int)HttpStatusCode.Unauthorized)) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<Result>();
        }


    }
}
