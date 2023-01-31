using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using System.Data;
using Shared.Models.Areas.evolDP;
using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using Shared.Models.Areas.Core;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using Shared.ViewModels.General;
using evolUX.API.Models;
using System.Reflection;
using System.Collections.Generic;

namespace evolUX.API.Areas.Finishing.Services
{
    public class ExpeditionService : IExpeditionService
    {
        private readonly IWrapperRepository _repository;
        public ExpeditionService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Business>> GetCompanyBusiness(DataTable CompanyBusinessList)
        {
            IEnumerable<Business> companyBusiness = await _repository.Expedition.GetCompanyBusiness(CompanyBusinessList);
            if (companyBusiness == null)
            {

            }

            return companyBusiness;
        }
        
        public async Task<ExpeditionFilesViewModel> GetPendingExpeditionFiles(int BusinessID, DataTable ServiceCompanyList)
        {
            ExpeditionFilesViewModel viewmodel = new ExpeditionFilesViewModel();

            viewmodel.ExpeditionFiles = (List<ExpServiceCompanyFileElement>)await _repository.Expedition.GetPendingExpeditionFiles(BusinessID, ServiceCompanyList);
            if (viewmodel.ExpeditionFiles != null)
            {
                foreach(ExpServiceCompanyFileElement s in viewmodel.ExpeditionFiles)
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("SERVICECOMPANYCODE", s.ServiceCompanyCode);
                    dictionary.Add("TYPE", "EXPEDITION");

                    FlowInfo flowInfo = await _repository.RegistJob.GetFlowByCriteria(dictionary);
                    s.ExpeditionReportsJobs = (List<Job>)await _repository.RegistJob.GetJobs(flowInfo.FlowID);
                }
            }

            return viewmodel;
        }

        public async Task<Result> RegistExpeditionReport(List<RegistExpReportElement> expFiles, string userName, int userID)
        {
            Result viewmodel = null;
            foreach (RegistExpReportElement e in expFiles)
            {
                if (e.ExpFileList.Count > 0)
                {
                    DataTable fTable = new DataTable();
                    fTable.Columns.Add("ID1", typeof(int));
                    fTable.Columns.Add("ID2", typeof(int));

                    string filelist = "";
                    foreach (ExpFileElement f in e.ExpFileList)
                    {
                        DataRow row = fTable.NewRow();
                        row["ID1"] = f.RunID;
                        row["ID2"] = f.FileID;
                        fTable.Rows.Add(row);
                        filelist += string.Format("<FILE R=\"{0}\" F=\"{1}\"/>", f.RunID, f.FileID);
                    }
                    int RequestID = await _repository.Expedition.RegistFileList(fTable, userName);
                    if (RequestID > 0)
                    {
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        dictionary.Add("SERVICECOMPANYCODE", e.ServiceCompanyCode);
                        dictionary.Add("TYPE", "EXPEDITION");

                        FlowInfo flowinfo = await _repository.RegistJob.GetFlowByCriteria(dictionary);
                        flowinfo.FlowName = e.ServiceCompanyCode + " [" + flowinfo.FlowName + "]";

                        List<FlowParameter> flowparameters = (List<FlowParameter>)await _repository.RegistJob.GetFlowData(flowinfo.FlowID);

                        bool hasRequestID = false;
                        foreach (FlowParameter p in flowparameters)
                        {
                            p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/FILELIST/FILE_LIST ", filelist);
                            p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/BUSINESS ", "");
                            p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/SERVICECOMPANYID ", "0");
                            p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/SYSTEM/LOGUSER ", userName);
                            if (p.ParameterName == "REQUESTID")
                            {
                                hasRequestID = true;
                                p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/REQUESTID ", RequestID.ToString());
                            }
                        }
                        if (!hasRequestID)
                            flowparameters.Add(new FlowParameter { 
                                FlowID = flowinfo.FlowID, 
                                ParameterName = "REQUESTID", 
                                ParameterValue = string.Format("SELECT {0}", RequestID.ToString()) });

                        viewmodel = await _repository.RegistJob.TryRegistJob(flowparameters, flowinfo, userID);
                        if (viewmodel == null)
                            throw new NullReferenceException("No result was sent by the Database!");
   
                    }
                }
            }
            if (viewmodel == null)
                throw new NullReferenceException("No result was sent by the Database!");
            return viewmodel;
        }
    }
}
