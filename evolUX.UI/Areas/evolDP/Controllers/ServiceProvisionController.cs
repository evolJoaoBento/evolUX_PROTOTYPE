using evolUX.UI.Areas.evolDP.Services.Interfaces;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
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
                result.Restrictions = await _serviceProvisionService.GetServiceCompanyRestrictions(null);
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

        public async Task<IActionResult> ServiceCompanyConfig(string serviceCompanyJson, int cotDate, int serviceTypeID, int serviceID)
        {
            try
            {
                Company company = JsonConvert.DeserializeObject<Company>(serviceCompanyJson);
                ServiceCompanyConfigViewModel result = new ServiceCompanyConfigViewModel();
                result.ServiceCompany = company;
                result.Configs = await _serviceProvisionService.GetServiceCompanyConfigs(company.ID, cotDate, serviceTypeID, serviceID);
                result.ServiceTypeID = serviceTypeID;
                result.ServiceID = serviceID;
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
        //public async Task<IActionResult> Types()
        //{
        //    try
        //    {
        //        string serviceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
        //        ExpeditionTypeViewModel result = await _serviceProvisionService.GetServiceTypes(serviceCompanyList);
        //        if (result != null && result.Types != null && result.Types.Count() > 0)
        //        {
        //            result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
        //            result.ExpCompanies = JsonConvert.DeserializeObject<List<Company>>(serviceCompanyList);
        //            return View(result);
        //        }
        //        return View(null);
        //    }
        //    catch (FlurlHttpException ex)
        //    {
        //        // For error responses that take a known shape
        //        //TError e = ex.GetResponseJson<TError>();
        //        // For error responses that take an unknown shape
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.RequestID = ex.Source;
        //        viewModel.ErrorResult = new ErrorResult();
        //        viewModel.ErrorResult.Code = (int)ex.StatusCode;
        //        viewModel.ErrorResult.Message = ex.Message;
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpNotFoundException ex)
        //    {
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpUnauthorizedException ex)
        //    {
        //        if (ex.response.Headers.Contains("Token-Expired"))
        //        {
        //            var header = ex.response.Headers.FirstOrDefault("Token-Expired");
        //            var returnUrl = Request.Path.Value;
        //            //var url = Url.RouteUrl("MyAreas", )

        //            return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Auth", new { Area = "Core" });
        //        }
        //    }

        //}

        //public async Task<IActionResult> TypeDetail(string serviceTypeJson)
        //{
        //    try
        //    {
        //        ServiceTypeViewModel result = new ServiceTypeViewModel();
        //        ServiceTypeElement serviceType = JsonConvert.DeserializeObject<ServiceTypeElement>(serviceTypeJson);
        //        List<ServiceTypeElement> types = new List<ServiceTypeElement>();
        //        types.Add(serviceType);
        //        result.Types = types;
        //        string serviceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
        //        result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
        //        result.ServiceCompanies = JsonConvert.DeserializeObject<List<Company>>(serviceCompanyList);

        //        return View("TypeDetail",result);
        //    }
        //    catch (FlurlHttpException ex)
        //    {
        //        // For error responses that take a known shape
        //        //TError e = ex.GetResponseJson<TError>();
        //        // For error responses that take an unknown shape
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.RequestID = ex.Source;
        //        viewModel.ErrorResult = new ErrorResult();
        //        viewModel.ErrorResult.Code = (int)ex.StatusCode;
        //        viewModel.ErrorResult.Message = ex.Message;
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpNotFoundException ex)
        //    {
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpUnauthorizedException ex)
        //    {
        //        if (ex.response.Headers.Contains("Token-Expired"))
        //        {
        //            var header = ex.response.Headers.FirstOrDefault("Token-Expired");
        //            var returnUrl = Request.Path.Value;
        //            //var url = Url.RouteUrl("MyAreas", )

        //            return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Auth", new { Area = "Core" });
        //        }
        //    }

        //}


        //public async Task<IActionResult> ServiceTasks()
        //{
        //    try
        //    {
        //        string serviceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
        //        ServiceTaskViewModel result = new ServiceTaskViewModel();
        //        result.ServiceTasks = await _serviceProvisionService.GetServiceTasks(serviceCompanyList);
        //        return View(result);
        //    }
        //    catch (FlurlHttpException ex)
        //    {
        //        // For error responses that take a known shape
        //        //TError e = ex.GetResponseJson<TError>();
        //        // For error responses that take an unknown shape
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.RequestID = ex.Source;
        //        viewModel.ErrorResult = new ErrorResult();
        //        viewModel.ErrorResult.Code = (int)ex.StatusCode;
        //        viewModel.ErrorResult.Message = ex.Message;
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpNotFoundException ex)
        //    {
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpUnauthorizedException ex)
        //    {
        //        if (ex.response.Headers.Contains("Token-Expired"))
        //        {
        //            var header = ex.response.Headers.FirstOrDefault("Token-Expired");
        //            var returnUrl = Request.Path.Value;
        //            //var url = Url.RouteUrl("MyAreas", )

        //            return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Auth", new { Area = "Core" });
        //        }
        //    }

        //}

        //public async Task<IActionResult> ServiceTaskDetail(string serviceTaskJson)
        //{
        //    try
        //    {
        //        ServiceTaskViewModel result = new ServiceTaskViewModel();
        //        ServiceTask serviceTask = JsonConvert.DeserializeObject<ServiceTask>(serviceTaskJson);
        //        List<ExpeditionZoneElement> zones = new List<ExpeditionZoneElement>();
        //        zones.Add(expeditionZone);
        //        result.Zones = zones;
        //        if (result.ServiceTasks != null && result.ServiceTasks.Count() > 0)
        //        {
        //            result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
        //            result.Services = await _serviceProvisionService.GetServices();
        //            return View(result);
        //        }

        //        string expCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
        //        result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
        //        result.ExpCompanies = JsonConvert.DeserializeObject<List<Company>>(expCompanyList);

        //        return View("ZoneDetail", result);
        //    }
        //    catch (FlurlHttpException ex)
        //    {
        //        // For error responses that take a known shape
        //        //TError e = ex.GetResponseJson<TError>();
        //        // For error responses that take an unknown shape
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.RequestID = ex.Source;
        //        viewModel.ErrorResult = new ErrorResult();
        //        viewModel.ErrorResult.Code = (int)ex.StatusCode;
        //        viewModel.ErrorResult.Message = ex.Message;
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpNotFoundException ex)
        //    {
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpUnauthorizedException ex)
        //    {
        //        if (ex.response.Headers.Contains("Token-Expired"))
        //        {
        //            var header = ex.response.Headers.FirstOrDefault("Token-Expired");
        //            var returnUrl = Request.Path.Value;
        //            //var url = Url.RouteUrl("MyAreas", )

        //            return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Auth", new { Area = "Core" });
        //        }
        //    }

        //}



        //public async Task<IActionResult> ChangeExpCompanyConfig(IFormCollection form, string expCompanyJson, int startDate, int pageExpeditionType, int pageExpeditionZone)
        //{
        //    try
        //    {
        //        Company company = JsonConvert.DeserializeObject<Company>(expCompanyJson);
        //        int value = 0;
        //        string str;
        //        ExpCompanyConfig expCompanyConfig = new ExpCompanyConfig();
        //        expCompanyConfig.ExpCompanyID = 0;
        //        str = form["ExpCompanyID"].ToString();
        //        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
        //            expCompanyConfig.ExpCompanyID = value;

        //        expCompanyConfig.ExpeditionType = 0;
        //        str = form["ExpeditionType"].ToString();
        //        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
        //            expCompanyConfig.ExpeditionType = value;

        //        expCompanyConfig.ExpeditionZone = 0;
        //        str = form["ExpeditionZone"].ToString();
        //        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
        //            expCompanyConfig.ExpeditionZone = value;

        //        expCompanyConfig.ExpCompanyLevel = 0;
        //        str = form["ExpCompanyLevel"].ToString();
        //        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
        //            expCompanyConfig.ExpCompanyLevel = value;

        //        if (startDate == 0)
        //        {
        //            DateTime startDateDT = DateTime.Now;
        //            expCompanyConfig.StartDate = 0;
        //            str = form["StartDate"].ToString();
        //            if (!string.IsNullOrEmpty(str) && DateTime.TryParse(str, out startDateDT))
        //                startDate = Int32.Parse(((DateTime)startDateDT).ToString("yyyyMMdd"));
        //        }
        //        expCompanyConfig.StartDate = startDate;

        //        expCompanyConfig.MaxWeight = 0;
        //        str = form["MaxWeight"].ToString();
        //        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
        //            expCompanyConfig.MaxWeight = value;

        //        double valDouble = 0;
        //        expCompanyConfig.UnitCost = 0;
        //        str = form["UnitCost"].ToString();
        //        if (!string.IsNullOrEmpty(str) && double.TryParse(str, out valDouble))
        //        {
        //            if (valDouble.ToString("F2") != str)
        //            {
        //                if (double.TryParse(str.Replace(".", ","), out valDouble))
        //                    expCompanyConfig.UnitCost = valDouble;
        //            }
        //            else
        //                expCompanyConfig.UnitCost = valDouble;
        //        }
        //        str = form["ExpColumnA"].ToString();
        //        if (!string.IsNullOrEmpty(str))
        //            expCompanyConfig.ExpColumnA = str;

        //        str = form["ExpColumnB"].ToString();
        //        if (!string.IsNullOrEmpty(str))
        //            expCompanyConfig.ExpColumnB = str;

        //        str = form["ExpColumnE"].ToString();
        //        if (!string.IsNullOrEmpty(str))
        //            expCompanyConfig.ExpColumnE = str;

        //        str = form["ExpColumnI"].ToString();
        //        if (!string.IsNullOrEmpty(str))
        //            expCompanyConfig.ExpColumnI = str;

        //        str = form["ExpColumnF"].ToString();
        //        if (!string.IsNullOrEmpty(str))
        //            expCompanyConfig.ExpColumnF = str;


        //        Company Company = JsonConvert.DeserializeObject<Company>(expCompanyJson);
        //        ExpCompanyConfigViewModel result = new ExpCompanyConfigViewModel();
        //        await _serviceProvisionService.SetExpCompanyConfig(expCompanyConfig);
        //        result.Configs = await _serviceProvisionService.GetExpCompanyConfigs(expCompanyConfig.ExpCompanyID, startDate, pageExpeditionType, pageExpeditionZone);
        //        result.ExpCompany = company;
        //        result.ExpeditionZone = pageExpeditionZone;
        //        result.ExpeditionType = pageExpeditionType;
        //        result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
        //        return View("ExpCompanyConfig", result);
        //    }
        //    catch (FlurlHttpException ex)
        //    {
        //        // For error responses that take a known shape
        //        //TError e = ex.GetResponseJson<TError>();
        //        // For error responses that take an unknown shape
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.RequestID = ex.Source;
        //        viewModel.ErrorResult = new ErrorResult();
        //        viewModel.ErrorResult.Code = (int)ex.StatusCode;
        //        viewModel.ErrorResult.Message = ex.Message;
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpNotFoundException ex)
        //    {
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpUnauthorizedException ex)
        //    {
        //        if (ex.response.Headers.Contains("Token-Expired"))
        //        {
        //            var header = ex.response.Headers.FirstOrDefault("Token-Expired");
        //            var returnUrl = Request.Path.Value;
        //            //var url = Url.RouteUrl("MyAreas", )

        //            return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Auth", new { Area = "Core" });
        //        }
        //    }

        //}

        //public async Task<IActionResult> AddExpCompanyConfig(IFormCollection form, string expCompanyJson)
        //{
        //    try
        //    {
        //        Company company = JsonConvert.DeserializeObject<Company>(expCompanyJson);

        //        DateTime startDateDT = DateTime.Now;
        //        int startDate = 0;
        //        string str = form["StartDate"].ToString();
        //        if (!string.IsNullOrEmpty(str) && DateTime.TryParse(str, out startDateDT))
        //            startDate = Int32.Parse(((DateTime)startDateDT).ToString("yyyyMMdd"));


        //        Company Company = JsonConvert.DeserializeObject<Company>(expCompanyJson);
        //        ExpCompanyConfigViewModel result = new ExpCompanyConfigViewModel();
        //        await _serviceProvisionService.NewExpCompanyConfig(Company.ID, startDate);
        //        result.Configs = await _serviceProvisionService.GetExpCompanyConfigs(Company.ID, startDate, 0, 0);
        //        result.ExpCompany = company;
        //        result.ExpeditionZone = 0;
        //        result.ExpeditionType = 0;
        //        ExpeditionZoneViewModel zones = await _serviceProvisionService.GetExpeditionZones(0, "");
        //        result.Zones = zones.Zones.ToList();
        //        result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
        //        return View("ExpCompanyConfig", result);
        //    }
        //    catch (FlurlHttpException ex)
        //    {
        //        // For error responses that take a known shape
        //        //TError e = ex.GetResponseJson<TError>();
        //        // For error responses that take an unknown shape
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.RequestID = ex.Source;
        //        viewModel.ErrorResult = new ErrorResult();
        //        viewModel.ErrorResult.Code = (int)ex.StatusCode;
        //        viewModel.ErrorResult.Message = ex.Message;
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpNotFoundException ex)
        //    {
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpUnauthorizedException ex)
        //    {
        //        if (ex.response.Headers.Contains("Token-Expired"))
        //        {
        //            var header = ex.response.Headers.FirstOrDefault("Token-Expired");
        //            var returnUrl = Request.Path.Value;
        //            //var url = Url.RouteUrl("MyAreas", )

        //            return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Auth", new { Area = "Core" });
        //        }
        //    }

        //}

        //public async Task<IActionResult> ExpRegistRange(string expCompanyJson)
        //{
        //    try
        //    {
        //        ExpeditionRegistViewModel result = new ExpeditionRegistViewModel();
        //        result.Company = JsonConvert.DeserializeObject<Company>(expCompanyJson);
        //        result.ExpeditionRegistIDs = await _serviceProvisionService.GetExpeditionRegistIDs(result.Company.ID);
        //        result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

        //        return View(result);
        //    }
        //    catch (FlurlHttpException ex)
        //    {
        //        // For error responses that take a known shape
        //        //TError e = ex.GetResponseJson<TError>();
        //        // For error responses that take an unknown shape
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.RequestID = ex.Source;
        //        viewModel.ErrorResult = new ErrorResult();
        //        viewModel.ErrorResult.Code = (int)ex.StatusCode;
        //        viewModel.ErrorResult.Message = ex.Message;
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpNotFoundException ex)
        //    {
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpUnauthorizedException ex)
        //    {
        //        if (ex.response.Headers.Contains("Token-Expired"))
        //        {
        //            var header = ex.response.Headers.FirstOrDefault("Token-Expired");
        //            var returnUrl = Request.Path.Value;
        //            //var url = Url.RouteUrl("MyAreas", )

        //            return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Auth", new { Area = "Core" });
        //        }
        //    }

        //}

        //public async Task<IActionResult> ConfigExpRegistRange(IFormCollection form, string expCompanyJson)
        //{
        //    try
        //    {
        //        int value = 0;
        //        string str;
        //        ExpeditionRegistElement expRegist = new ExpeditionRegistElement();
        //        expRegist.ExpCompanyID = 0;
        //        str = form["ExpCompanyID"].ToString();
        //        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
        //            expRegist.ExpCompanyID = value;

        //        expRegist.CompanyRegistCode = 0;
        //        str = form["CompanyRegistCode"].ToString();
        //        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
        //            expRegist.CompanyRegistCode = value;

        //        expRegist.StartExpeditionID = 0;
        //        str = form["StartExpeditionID"].ToString();
        //        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
        //            expRegist.StartExpeditionID = value;

        //        expRegist.EndExpeditionID = 0;
        //        str = form["EndExpeditionID"].ToString();
        //        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
        //            expRegist.EndExpeditionID = value;

        //        expRegist.RegistCodePrefix = form["RegistCodePrefix"].ToString();
        //        expRegist.RegistCodeSuffix = form["RegistCodeSuffix"].ToString();

        //        expRegist.LastExpeditionID = 0;
        //        str = form["LastExpeditionID"].ToString();
        //        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
        //            expRegist.LastExpeditionID = value;

        //        ExpeditionRegistViewModel result = new ExpeditionRegistViewModel();
        //        result.Company = JsonConvert.DeserializeObject<Company>(expCompanyJson);
        //        await _serviceProvisionService.SetExpeditionRegistID(expRegist);
        //        result.ExpeditionRegistIDs = await _serviceProvisionService.GetExpeditionRegistIDs(result.Company.ID);
        //        result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

        //        return View("ExpRegistRange", result);
        //    }
        //    catch (FlurlHttpException ex)
        //    {
        //        // For error responses that take a known shape
        //        //TError e = ex.GetResponseJson<TError>();
        //        // For error responses that take an unknown shape
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.RequestID = ex.Source;
        //        viewModel.ErrorResult = new ErrorResult();
        //        viewModel.ErrorResult.Code = (int)ex.StatusCode;
        //        viewModel.ErrorResult.Message = ex.Message;
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpNotFoundException ex)
        //    {
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpUnauthorizedException ex)
        //    {
        //        if (ex.response.Headers.Contains("Token-Expired"))
        //        {
        //            var header = ex.response.Headers.FirstOrDefault("Token-Expired");
        //            var returnUrl = Request.Path.Value;
        //            //var url = Url.RouteUrl("MyAreas", )

        //            return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Auth", new { Area = "Core" });
        //        }
        //    }

        //}

        //public async Task<IActionResult> ExpContracts(string expCompanyJson)
        //{
        //    try
        //    {
        //        ExpContractViewModel result = new ExpContractViewModel();
        //        result.Company = JsonConvert.DeserializeObject<Company>(expCompanyJson);
        //        result.ExpeditionContracts = await _serviceProvisionService.GetExpContracts(result.Company.ID);
        //        result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

        //        return View(result);
        //    }
        //    catch (FlurlHttpException ex)
        //    {
        //        // For error responses that take a known shape
        //        //TError e = ex.GetResponseJson<TError>();
        //        // For error responses that take an unknown shape
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.RequestID = ex.Source;
        //        viewModel.ErrorResult = new ErrorResult();
        //        viewModel.ErrorResult.Code = (int)ex.StatusCode;
        //        viewModel.ErrorResult.Message = ex.Message;
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpNotFoundException ex)
        //    {
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpUnauthorizedException ex)
        //    {
        //        if (ex.response.Headers.Contains("Token-Expired"))
        //        {
        //            var header = ex.response.Headers.FirstOrDefault("Token-Expired");
        //            var returnUrl = Request.Path.Value;
        //            //var url = Url.RouteUrl("MyAreas", )

        //            return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Auth", new { Area = "Core" });
        //        }
        //    }

        //}

        //public async Task<IActionResult> ConfigExpContract(IFormCollection form, string expCompanyJson)
        //{
        //    try
        //    {
        //        ExpContractElement expContract = new ExpContractElement();

        //        int value = 0;
        //        string str;
        //        expContract.ExpCompanyID = 0;
        //        str = form["ExpCompanyID"].ToString();
        //        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
        //            expContract.ExpCompanyID = value;

        //        expContract.ContractID = 0;
        //        str = form["ContractID"].ToString();
        //        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
        //            expContract.ContractID = value;

        //        expContract.ContractNr = 0;
        //        str = form["ContractNr"].ToString();
        //        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
        //            expContract.ContractNr = value;

        //        expContract.ClientNr = 0;
        //        str = form["ClientNr"].ToString();
        //        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
        //            expContract.ClientNr = value;

        //        expContract.ClientName = form["ClientName"].ToString();
        //        expContract.ClientNIF = form["ClientNIF"].ToString();
        //        expContract.ClientAddress = form["ClientAddress"].ToString();
        //        expContract.ClientPostalCode = form["ClientPostalCode"].ToString();
        //        expContract.ClientPostalCodeDescription = form["ClientPostalCodeDescription"].ToString();
        //        expContract.ClientNIF = form["ClientNIF"].ToString();
        //        expContract.CompanyExpeditionCode = form["CompanyExpeditionCode"].ToString();

        //        expContract.PurchaseOrderNr = 0;
        //        str = form["PurchaseOrderNr"].ToString();
        //        decimal dec = 0;
        //        if (!string.IsNullOrEmpty(str) && decimal.TryParse(str, out dec))
        //            expContract.PurchaseOrderNr = dec;


        //        ExpContractViewModel result = new ExpContractViewModel();
        //        result.Company = JsonConvert.DeserializeObject<Company>(expCompanyJson);
        //        await _serviceProvisionService.SetExpContract(expContract);
        //        result.ExpeditionContracts = await _serviceProvisionService.GetExpContracts(result.Company.ID);
        //        result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

        //        return View("ExpContracts", result);
        //    }
        //    catch (FlurlHttpException ex)
        //    {
        //        // For error responses that take a known shape
        //        //TError e = ex.GetResponseJson<TError>();
        //        // For error responses that take an unknown shape
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.RequestID = ex.Source;
        //        viewModel.ErrorResult = new ErrorResult();
        //        viewModel.ErrorResult.Code = (int)ex.StatusCode;
        //        viewModel.ErrorResult.Message = ex.Message;
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpNotFoundException ex)
        //    {
        //        ErrorViewModel viewModel = new ErrorViewModel();
        //        viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
        //        return View("Error", viewModel);
        //    }
        //    catch (HttpUnauthorizedException ex)
        //    {
        //        if (ex.response.Headers.Contains("Token-Expired"))
        //        {
        //            var header = ex.response.Headers.FirstOrDefault("Token-Expired");
        //            var returnUrl = Request.Path.Value;
        //            //var url = Url.RouteUrl("MyAreas", )

        //            return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Auth", new { Area = "Core" });
        //        }
        //    }

        //}
    }
}
