using Microsoft.AspNetCore.Mvc;
using System.Data;
using Flurl.Http;
using evolUX.UI.Exceptions;
using Newtonsoft.Json;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using Shared.Models.Areas.evolDP;
using Microsoft.Extensions.Localization;
using Shared.ViewModels.Areas.Reports;
using evolUX.UI.Areas.Reports.Services.Interfaces;

namespace evolUX.UI.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class RetentionReportController : Controller
    {
        private readonly IRetentionReportService _retentionReportService;
        private readonly IStringLocalizer<RetentionReportController> _localizer;
        public RetentionReportController(IRetentionReportService retentionReportService, IStringLocalizer<RetentionReportController> localizer)
        {
            _retentionReportService = retentionReportService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            string BusinessAreaList = HttpContext.Session.GetString("evolDP/CompanyBusiness");
            try
            {
                if (string.IsNullOrEmpty(BusinessAreaList))
                    return View(null);
                DataTable BusinessAreas = JsonConvert.DeserializeObject<DataTable>(BusinessAreaList);
                if (BusinessAreas.Rows.Count > 1)
                {
                    BusinessAreasViewModel result = new BusinessAreasViewModel();
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                    List<Business> sList = new List<Business>();
                    foreach (DataRow row in BusinessAreas.Rows)
                    {
                        sList.Add(new Business
                        {
                            BusinessID = Int32.Parse(row["ID"].ToString()),
                            BusinessCode = (string)row["BusinessCode"],
                            Description = (string)row["Description"],
                            CompanyID = Int32.Parse(row["CompanyID"].ToString())
                        });
                    }
                    result.BusinessAreas = sList;
                    return View(result);
                }
                else
                {
                    string baValues = BusinessAreas.Rows[0]["ID"].ToString() + "|" + BusinessAreas.Rows[0]["CompanyCode"].ToString() + " | " + BusinessAreas.Rows[0]["CompanyName"].ToString();
                    return RedirectToAction("RetentionRunReport", "RetentionReport", new { Area = "Reports", businessAreaValues = baValues });
                }
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

        public async Task<IActionResult> RetentionRunReport(/*string businessAreaValues, string source*/ int BusinessAreaID, int RefDate, string BusinessID, int BusinessCode, string Description)
        {
            //ViewBag.Source = source;
            //string[] businessAreaValue = businessAreaValues.Split('|');
            //int BusinessAreaID = Convert.ToInt32(businessAreaValue[0]);
            //string BusinessAreaCode = businessAreaValue.Length > 1 ? businessAreaValue[1] : "";
            //string BusinessAreaName = businessAreaValue.Length > 2 ? businessAreaValue[2] : "";

            //TempData["BusinessAreaID"] = businessAreaValue[0];
            //TempData["BusinessAreaCode"] = BusinessAreaCode;
            TempData["DateRef"] = RefDate;
            TempData["BusinessAreaName"] = Description;
            TempData["BusinessID"] = BusinessID;
            TempData["BusinessCode"] = BusinessCode;
            try
            {

                RetentionRunReportViewModel result = await _retentionReportService.GetRetentionRunReport(BusinessAreaID, RefDate);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                //DataTable BusinessAreaDT = JsonConvert.DeserializeObject<DataTable>(HttpContext.Session.GetString("evolDP/BusinessAreas"));
                //if (BusinessAreaDT.Rows.Count > 1)
                //{
                //    ViewBag.hasMultipleBusinessAreas = true;
                //}
                //else
                //{
                //    ViewBag.hasMultipleBusinessAreas = false;
                //}
                return View(result);
            }
            catch (ErrorViewModelException ex)
            {
                return View("Error", ex.ViewModel);
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

        public async Task<IActionResult> RetentionReport(string RunIDList, int BusinessAreaID)
        {
            try
            {
                List<int> runIDList = new List<int>();
                string[] runIDListStr = RunIDList.Split('|');
                if (string.IsNullOrEmpty(RunIDList) || runIDListStr.Length == 0)
                {
                    return PartialView("MessageView", new MessageViewModel(_localizer["Missing Runs"]));
                }
                foreach (string r in runIDListStr)
                    runIDList.Add(int.Parse(r));

                string profileList = HttpContext.Session.GetString("evolUX/Profiles");
                RetentionReportViewModel result = await _retentionReportService.GetRetentionReport(runIDList, BusinessAreaID);

                if (result != null && result.RetentionReport != null && result.RetentionReport.Count() > 0)
                {
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                    //TempData["ServiceCompanyCode"] = result.ProductionReport.First().ServiceCompanyCode;C:\Users\ZivileSink\Documents\evolUX_PROTOTYPE\evolUX.UI\Areas\Reports\Views\RetentionReport\RetentionReport.cshtml
                }
                return View(result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape

                var resultError = await ex.GetResponseJsonAsync<ErrorResult>();
                return View("Error", resultError);
            }
            catch (HttpNotFoundException ex)
            {
                var resultError = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", resultError);
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

        public async Task<IActionResult> RetentionInfoReport(int RunID, int FileID, int SetID, int DocID)
        {
            try
            {
                RetentionInfoReportViewModel result = await _retentionReportService.GetRetentionInfoReport(RunID, FileID, SetID, DocID);

                if (result != null && result.RetentionInfo != null)
                {
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                }
                return View(result);
            }

            catch (ErrorViewModelException ex)
            {
                return View("Error", ex.ViewModel);
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
