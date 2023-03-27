using evolUX.API.Models;
using evolUX.UI.Areas.evolDP.Services.Interfaces;
using evolUX.UI.Exceptions;
using evolUX_dev.Areas.evolDP.Models;
using Flurl.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.evolDP;
using System.Data;

namespace evolUX.UI.Areas.evolDP.Controllers
{
    [Area("evolDP")]
    public class ServiceProvisionController : Controller
    {
        private readonly IServiceProvisionService _serviceProvisionService;
        public ServiceProvisionController(IServiceProvisionService serviceProvisionService)
        {
            _serviceProvisionService = serviceProvisionService;
        }
        public async Task<IActionResult> Companies()
        {
            try
            {
                string serviceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
                ServiceCompaniesViewModel result = new ServiceCompaniesViewModel();
                result.ServiceCompanies = await _serviceProvisionService.GetServiceCompanies(serviceCompanyList);
                result.Restrictions = await _serviceProvisionService.GetServiceCompanyRestrictions(0);
                if (result.ServiceCompanies != null && result.ServiceCompanies.Count() > 0)
                {
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                    return View(result);
                }
                return View(null);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }

        }

        public async Task<IActionResult> ServiceCompany(string serviceCompanyJson, string restrictionsJson)
        {
            try
            {
                Company serviceCompany = JsonConvert.DeserializeObject<Company>(serviceCompanyJson);
                List<ServiceCompanyRestriction> restrictions = null;
                if (!string.IsNullOrEmpty(restrictionsJson))
                    JsonConvert.DeserializeObject<List<ServiceCompanyRestriction>>(restrictionsJson);
                ServiceCompanyViewModel result = await _serviceProvisionService.GetServiceCompanyViewModel(serviceCompany.ID, restrictions);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                return View("ServiceCompany", result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )
                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
        }

        public async Task<IActionResult> ChangeServiceCompany(IFormCollection form)
        {
            try
            {
                Company serviceCompany = new Company();

                int value = 0;
                string str;
                serviceCompany.ID = 0;
                str = form["ServiceCompanyID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    serviceCompany.ID = value;

                serviceCompany.CompanyName = form["CompanyName"].ToString();
                serviceCompany.CompanyAddress = form["CompanyAddress"].ToString();
                serviceCompany.CompanyPostalCode = form["CompanyPostalCode"].ToString();
                serviceCompany.CompanyPostalCodeDescription = form["CompanyPostalCodeDescription"].ToString();
                serviceCompany.CompanyCountry = form["CompanyCountry"].ToString();
                serviceCompany.CompanyCode = form["CompanyCode"].ToString();
                serviceCompany.CompanyServer = form["CompanyServer"].ToString();

                serviceCompany = await _serviceProvisionService.SetServiceCompany(serviceCompany);
                ServiceCompanyViewModel result = await _serviceProvisionService.GetServiceCompanyViewModel(serviceCompany, null);

                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ServiceCompany", result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }

        }

        public async Task<IActionResult> ChangeServiceCompanyRestriction(IFormCollection form, string serviceCompanyJson, int materialTypeID)
        {
            try
            {
                int materialPosition = 0;
                int fileSheetsCutoffLevel = 0;
                bool restrictionMode = false;

                string str = form["MaterialPosition"].ToString();
                int value = 0;
                if (!string.IsNullOrEmpty(str) && Int32.TryParse(str, out value))
                    materialPosition = value;

                str = form["FileSheetsCutoffLevel"].ToString();
                value = 0;
                if (!string.IsNullOrEmpty(str) && Int32.TryParse(str, out value))
                    fileSheetsCutoffLevel = value;

                str = form["RestrictionMode"].ToString();
                value = 0;
                if (!string.IsNullOrEmpty(str) && Int32.TryParse(str, out value))
                {
                    restrictionMode = value == 1 ? true : false;
                }

                Company serviceCompany = JsonConvert.DeserializeObject<Company>(serviceCompanyJson);
                await _serviceProvisionService.SetServiceCompanyRestrictions(serviceCompany.ID, materialTypeID, materialPosition, fileSheetsCutoffLevel, restrictionMode);
                ServiceCompanyViewModel result = await _serviceProvisionService.GetServiceCompanyViewModel(serviceCompany, null);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                return View("ServiceCompany", result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }

        }

        public async Task<IActionResult> ServiceCompanyConfig(string serviceCompanyJson, int serviceTypeID, int costDate, int serviceID)
        {
            try
            {
                Company company = JsonConvert.DeserializeObject<Company>(serviceCompanyJson);
                ServiceCompanyConfigViewModel result = new ServiceCompanyConfigViewModel();
                result.ServiceCompany = company;
                result.Configs = await _serviceProvisionService.GetServiceCompanyConfigs(company.ID, costDate, serviceTypeID, serviceID);
                result.CostDate = costDate;
                result.ServiceID = serviceID;
                if (serviceID == 0)
                {
                    result.Services = await _serviceProvisionService.GetServices(serviceTypeID);
                }
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                return View(result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }

        }

        public async Task<IActionResult> ServiceTypes()
        {
            try
            {
                ServiceTypeViewModel result = await _serviceProvisionService.GetServiceTypes();
                if (result.Types != null && result.Types.Count() > 0)
                {
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                    return View(result);
                }
                return View(null);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
        }

        public async Task<IActionResult> SetServiceType(int serviceTypeID, string serviceTypeCode, string serviceTypeDesc)
        {
            try
            {
                await _serviceProvisionService.SetServiceType(serviceTypeID, serviceTypeCode, serviceTypeDesc);
                ServiceTypeViewModel result = await _serviceProvisionService.GetServiceTypes();
                if (result.Types != null && result.Types.Count() > 0)
                {
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                    return View("ServiceTypes", result);
                }
                return View("ServiceTypes", null);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
        }

        public async Task<IActionResult> ServiceTypeDetail(string serviceTypeJson)
        {
            try
            {
                ServiceTypeDetailViewModel result = new ServiceTypeDetailViewModel();
                result.Type = JsonConvert.DeserializeObject<ServiceTypeElement>(serviceTypeJson);
                foreach(ServiceElement service in result.Type.ServicesList)
                {
                    service.CompanyList = await _serviceProvisionService.GetServiceCompanyList(service.ServiceTypeID, service.ServiceID, 0);
                }

                string serviceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
                result.ServiceCompanies = JsonConvert.DeserializeObject<List<Company>>(serviceCompanyList);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ServiceTypeDetail", result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }

        }

        public async Task<IActionResult> ServiceWorkFlow()
        {
            try
            {
                ServiceWorkFlowViewModel result = new ServiceWorkFlowViewModel();
                result.ServiceTasksList = await _serviceProvisionService.GetServiceTasks(null);
                result.ServiceTypesList = await _serviceProvisionService.GetAvailableServiceTypes();
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                return View(result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }

        }

        public async Task<IActionResult> SetServiceTask(int serviceTaskID, string serviceTaskCode, string serviceTaskDesc, int refServiceTaskID, int complementServiceTaskID, int externalExpeditionMode, string stationExceededDesc, string source)
        {
            try
            {
                await _serviceProvisionService.SetServiceTask(serviceTaskID, serviceTaskCode, serviceTaskDesc, refServiceTaskID, complementServiceTaskID, externalExpeditionMode, stationExceededDesc);
                ServiceTaskViewModel result = new ServiceTaskViewModel();
                result.ServiceTasksList = await _serviceProvisionService.GetServiceTasks(null);
                if (result.ServiceTasksList != null && result.ServiceTasksList.Count() > 0)
                {
                    result.ServiceTypesList = await _serviceProvisionService.GetAvailableServiceTypes();
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                    if (source == "ServiceWorkFlowDetail")
                    {
                        result.ServiceTaskID = serviceTaskID;
                        result.ExpCodes = await _serviceProvisionService.GetExpCodes(result.ServiceTaskID, 0, "");
                        string expCompanyList = HttpContext.Session.GetString("evolDP/ExpeditionCompanies");
                        if (!string.IsNullOrEmpty(expCompanyList))
                        {
                            DataTable expCompanies = JsonConvert.DeserializeObject<DataTable>(expCompanyList);
                            List<Company> eList = new List<Company>();
                            foreach (DataRow row in expCompanies.Rows)
                            {
                                eList.Add(new Company
                                {
                                    ID = Int32.Parse(row["ID"].ToString()),
                                    CompanyCode = (string)row["CompanyCode"],
                                    CompanyName = (string)row["CompanyName"]
                                });
                            }
                            result.ExpCompanies = eList;
                        }
                        return View(source, result);
                    }
                    else
                        return View(source, result);
                }
                return View(source, null);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
        }

        public async Task<IActionResult> ServiceWorkFlowDetail(int serviceTaskID)
        {
            try
            {
                ServiceTaskViewModel result = new ServiceTaskViewModel();
                result.ServiceTaskID = serviceTaskID;
                result.ServiceTasksList = await _serviceProvisionService.GetServiceTasks(null);
                result.ServiceTypesList = await _serviceProvisionService.GetAvailableServiceTypes();
                result.ExpCodes = await _serviceProvisionService.GetExpCodes(result.ServiceTaskID, 0, "");
                string expCompanyList = HttpContext.Session.GetString("evolDP/ExpeditionCompanies");
                if (!string.IsNullOrEmpty(expCompanyList))
                {
                    DataTable expCompanies = JsonConvert.DeserializeObject<DataTable>(expCompanyList);
                    List<Company> eList = new List<Company>();
                    foreach (DataRow row in expCompanies.Rows)
                    {
                        eList.Add(new Company
                        {
                            ID = Int32.Parse(row["ID"].ToString()),
                            CompanyCode = (string)row["CompanyCode"],
                            CompanyName = (string)row["CompanyName"]
                        });
                    }
                    result.ExpCompanies = eList;
                }
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View(result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }

        }
 
        public async Task<IActionResult> DeleteServiceType(int serviceTaskID, int serviceTypeID)
        {
            try
            {
                await _serviceProvisionService.DeleteServiceType(serviceTaskID, serviceTypeID);

                ServiceTaskViewModel result = new ServiceTaskViewModel();
                result.ServiceTaskID = serviceTaskID;
                result.ServiceTasksList = await _serviceProvisionService.GetServiceTasks(null);
                result.ServiceTypesList = await _serviceProvisionService.GetAvailableServiceTypes();
                result.ExpCodes = await _serviceProvisionService.GetExpCodes(result.ServiceTaskID, 0, "");
                string expCompanyList = HttpContext.Session.GetString("evolDP/ExpeditionCompanies");
                if (!string.IsNullOrEmpty(expCompanyList))
                {
                    DataTable expCompanies = JsonConvert.DeserializeObject<DataTable>(expCompanyList);
                    List<Company> eList = new List<Company>();
                    foreach (DataRow row in expCompanies.Rows)
                    {
                        eList.Add(new Company
                        {
                            ID = Int32.Parse(row["ID"].ToString()),
                            CompanyCode = (string)row["CompanyCode"],
                            CompanyName = (string)row["CompanyName"]
                        });
                    }
                    result.ExpCompanies = eList;
                }
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ServiceWorkFlowDetail",result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
        }

        public async Task<IActionResult> AddServiceType(int serviceTaskID, int serviceTypeID)
        {
            try
            {
                await _serviceProvisionService.AddServiceType(serviceTaskID, serviceTypeID);

                ServiceTaskViewModel result = new ServiceTaskViewModel();
                result.ServiceTaskID = serviceTaskID;
                result.ServiceTasksList = await _serviceProvisionService.GetServiceTasks(null);
                result.ServiceTypesList = await _serviceProvisionService.GetAvailableServiceTypes();
                result.ExpCodes = await _serviceProvisionService.GetExpCodes(result.ServiceTaskID, 0, "");
                string expCompanyList = HttpContext.Session.GetString("evolDP/ExpeditionCompanies");
                if (!string.IsNullOrEmpty(expCompanyList))
                {
                    DataTable expCompanies = JsonConvert.DeserializeObject<DataTable>(expCompanyList);
                    List<Company> eList = new List<Company>();
                    foreach (DataRow row in expCompanies.Rows)
                    {
                        eList.Add(new Company
                        {
                            ID = Int32.Parse(row["ID"].ToString()),
                            CompanyCode = (string)row["CompanyCode"],
                            CompanyName = (string)row["CompanyName"]
                        });
                    }
                    result.ExpCompanies = eList;
                }
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ServiceWorkFlowDetail", result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
        }

        public async Task<IActionResult> ExpCode(string expCodeJson, string serviceTaskDesc, string expCompanyName, string source)
        {
            try
            {
                TempData["ServiceTaskDesc"] = serviceTaskDesc;
                TempData["ExpCompanyName"] = expCompanyName;
                TempData["Source"] = source;
                ExpCodeViewModel result = new ExpCodeViewModel();
                result.ExpCode = JsonConvert.DeserializeObject<ExpCodeElement>(expCodeJson);

                string serviceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
                if (!string.IsNullOrEmpty(serviceCompanyList))
                {
                    DataTable serviceCompanies = JsonConvert.DeserializeObject<DataTable>(serviceCompanyList);
                    List<Company> sList = new List<Company>();
                    foreach (DataRow row in serviceCompanies.Rows)
                    {
                        sList.Add(new Company
                        {
                            ID = Int32.Parse(row["ID"].ToString()),
                            CompanyCode = (string)row["CompanyCode"],
                            CompanyName = (string)row["CompanyName"]
                        });
                    }
                    result.ServiceCompanies = sList;
                    result.ExpCenters = await _serviceProvisionService.GetExpCenters(result.ExpCode.ExpCode, serviceCompanyList);
                    result.Zones = await _serviceProvisionService.GetExpeditionZones(result.ExpCode.ExpCompanyID);
                }
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ExpCode", result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
        }

        public async Task<IActionResult> ChangeExpCenter(string expCodeJson, string expCenterCode, string source, string serviceTaskDesc, string expCompanyName,
            string description1, string description2, string description3, int serviceCompanyID, string expeditionZone)
        {
            try
            {
                TempData["ServiceTaskDesc"] = serviceTaskDesc;
                TempData["ExpCompanyName"] = expCompanyName;
                TempData["Source"] = source;
                ExpCodeViewModel result = new ExpCodeViewModel();
                result.ExpCode = JsonConvert.DeserializeObject<ExpCodeElement>(expCodeJson);
                await _serviceProvisionService.SetExpCenter(result.ExpCode.ExpCode, expCenterCode, description1, description2, description3, serviceCompanyID, expeditionZone);
                
                string serviceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
                if (!string.IsNullOrEmpty(serviceCompanyList))
                {
                    DataTable serviceCompanies = JsonConvert.DeserializeObject<DataTable>(serviceCompanyList);
                    List<Company> sList = new List<Company>();
                    foreach (DataRow row in serviceCompanies.Rows)
                    {
                        sList.Add(new Company
                        {
                            ID = Int32.Parse(row["ID"].ToString()),
                            CompanyCode = (string)row["CompanyCode"],
                            CompanyName = (string)row["CompanyName"]
                        });
                    }
                    result.ServiceCompanies = sList;
                    result.ExpCenters = await _serviceProvisionService.GetExpCenters(result.ExpCode.ExpCode, serviceCompanyList);
                    result.Zones = await _serviceProvisionService.GetExpeditionZones(result.ExpCode.ExpCompanyID);
                }
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ExpCode", result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
        }

        
        public async Task<IActionResult> ServiceCompanyExpCodes(string serviceCompanyJson)
        {
            try
            {
                ServiceCompanyExpCodesViewModel result = new ServiceCompanyExpCodesViewModel();
                result.ServiceCompany = JsonConvert.DeserializeObject<Company>(serviceCompanyJson);
                string expCompanyList = HttpContext.Session.GetString("evolDP/ExpeditionCompanies");

                result.ServiceCompanyExpCodes = await _serviceProvisionService.GetServiceCompanyExpCodes(result.ServiceCompany.ID, expCompanyList);
                result.ExpCodes = await _serviceProvisionService.GetExpCodes(expCompanyList);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                TempData["Source"] = "ServiceCompany";
                return View("ServiceCompanyExpCodes", result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
        }

        public async Task<IActionResult> ExpCodeConfig(string serviceCompanyJson, string expCodeJson, string expCenterCode, string source, string serviceTaskDesc, string expCompanyName)
        {
            try
            {
                TempData["ServiceTaskDesc"] = serviceTaskDesc;
                TempData["ExpCompanyName"] = expCompanyName;
                TempData["Source"] = source;
                ServiceCompanyExpCodeConfigViewModel result = new ServiceCompanyExpCodeConfigViewModel();
                result.ServiceCompany = JsonConvert.DeserializeObject<Company>(serviceCompanyJson);
                result.ExpCode = JsonConvert.DeserializeObject<ExpCodeElement>(expCodeJson);
                result.ExpCenterCode = expCenterCode;
                result.Configs = await _serviceProvisionService.GetServiceCompanyExpCodeConfigs(result.ExpCode.ExpCode, result.ServiceCompany.ID, result.ExpCenterCode);
                result.FulfillMaterialCodes = await _serviceProvisionService.GetFulfillMaterialCodes();

                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ExpCodeConfig", result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
        }

        public async Task<IActionResult> DeleteServiceCompanyExpLevel(string serviceCompanyJson, string expCodeJson, string expCenterCode, string source, string serviceTaskDesc, string expCompanyName, int expLevel)
        {
            try
            {
                TempData["ServiceTaskDesc"] = serviceTaskDesc;
                TempData["ExpCompanyName"] = expCompanyName;
                TempData["Source"] = source;
                ServiceCompanyExpCodeConfigViewModel result = new ServiceCompanyExpCodeConfigViewModel();
                result.ServiceCompany = JsonConvert.DeserializeObject<Company>(serviceCompanyJson);
                result.ExpCode = JsonConvert.DeserializeObject<ExpCodeElement>(expCodeJson);
                result.ExpCenterCode = expCenterCode;
                await _serviceProvisionService.DeleteServiceCompanyExpCodeConfig(result.ExpCode.ExpCode, result.ServiceCompany.ID, expCenterCode, expLevel);
                result.Configs = await _serviceProvisionService.GetServiceCompanyExpCodeConfigs(result.ExpCode.ExpCode, result.ServiceCompany.ID, result.ExpCenterCode);
                result.FulfillMaterialCodes = await _serviceProvisionService.GetFulfillMaterialCodes();

                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ExpCodeConfig", result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
        }

        public async Task<IActionResult> ChangeServiceCompanyExpLevel(string serviceCompanyJson, string expCodeJson, string expCenterCode, string source, string serviceTaskDesc, string expCompanyName, int expLevel, string fullFillMaterialCode, int docMaxSheets, string barcode)
        {
            try
            {
                TempData["ServiceTaskDesc"] = serviceTaskDesc;
                TempData["ExpCompanyName"] = expCompanyName;
                TempData["Source"] = source;
                ServiceCompanyExpCodeConfigViewModel result = new ServiceCompanyExpCodeConfigViewModel();
                result.ServiceCompany = JsonConvert.DeserializeObject<Company>(serviceCompanyJson);
                result.ExpCode = JsonConvert.DeserializeObject<ExpCodeElement>(expCodeJson);
                result.ExpCenterCode = expCenterCode;
                await _serviceProvisionService.SetServiceCompanyExpCodeConfig(result.ExpCode.ExpCode, result.ServiceCompany.ID, expCenterCode, expLevel, fullFillMaterialCode, docMaxSheets, barcode);
                result.Configs = await _serviceProvisionService.GetServiceCompanyExpCodeConfigs(result.ExpCode.ExpCode, result.ServiceCompany.ID, result.ExpCenterCode);
                result.FulfillMaterialCodes = await _serviceProvisionService.GetFulfillMaterialCodes();

                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ExpCodeConfig", result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
        }
    }
}
