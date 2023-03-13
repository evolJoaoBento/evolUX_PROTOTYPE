using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using System.Data;
using Shared.Models.Areas.evolDP;
using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using Shared.Models.Areas.Core;
using evolUX.API.Areas.Core.Repositories.Interfaces;

namespace evolUX.API.Areas.Finishing.Services
{
    public class PendingRecoverService : IPendingRecoverService
    {
        private readonly IWrapperRepository _repository;
        public PendingRecoverService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Company>> GetServiceCompanies(DataTable ServiceCompanyList)
        {
            IEnumerable<Company> serviceCompanies = await _repository.Generic.GetCompanies(null,ServiceCompanyList);
            if (serviceCompanies == null)
            {

            }

            return serviceCompanies;
        }
        
        public async Task<PendingRecoverDetailViewModel> GetPendingRecoveries(int serviceCompanyID, string serviceCompanyCode)
        {
            PendingRecoverDetailViewModel viewmodel = new PendingRecoverDetailViewModel();

            viewmodel.PendingRecoverDetail.PendingRecoverFiles = (List<PendingRecoverElement>)await _repository.PendingRecover.GetPendingRecoverFiles(serviceCompanyID);
            viewmodel.PendingRecoverDetail.PendingRecoverRegistDetailFiles = (List<PendingRecoverElement>)await _repository.PendingRecover.GetPendingRecoverRegistDetailFiles(serviceCompanyID);

            if (viewmodel.PendingRecoverDetail.PendingRecoverFiles != null && viewmodel.PendingRecoverDetail.PendingRecoverFiles.Count() > 0)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("SERVICECOMPANYCODE", serviceCompanyCode);
                dictionary.Add("TYPE", "RECOVER");

                FlowInfo flowInfo = await _repository.RegistJob.GetFlowByCriteria(dictionary);
                viewmodel.PendingRecoverDetail.PendingRecoverFilesJobs = (List<Job>)await _repository.RegistJob.GetJobs(flowInfo.FlowID);
            }
            if (viewmodel.PendingRecoverDetail.PendingRecoverRegistDetailFiles != null && viewmodel.PendingRecoverDetail.PendingRecoverRegistDetailFiles.Count() > 0)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("SERVICECOMPANYCODE", serviceCompanyCode);
                dictionary.Add("TYPE", "RDRECOVER");

                FlowInfo flowInfo = await _repository.RegistJob.GetFlowByCriteria(dictionary);
                viewmodel.PendingRecoverDetail.PendingRecoverRegistDetailFilesJobs = (List<Job>)await _repository.RegistJob.GetJobs(flowInfo.FlowID);
            }
            return viewmodel;
        }

        public async Task<Result> RegistPendingRecover(int serviceCompanyID, string serviceCompanyCode, string recoverType, int userID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("SERVICECOMPANYCODE", serviceCompanyCode);
            dictionary.Add("TYPE", recoverType);

            FlowInfo flowInfo = await _repository.RegistJob.GetFlowByCriteria(dictionary);
            flowInfo.FlowName = serviceCompanyCode + " [" + flowInfo.FlowName + "]";

            IEnumerable<FlowParameter> flowParameters = await _repository.RegistJob.GetFlowData(flowInfo.FlowID);

            foreach (FlowParameter p in flowParameters)
            {
                p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/SERVICECOMPANYID ", serviceCompanyID.ToString());
            }
            Result viewmodel = await _repository.RegistJob.TryRegistJob(flowParameters, flowInfo, userID);
            if (viewmodel == null)
            {
                throw new NullReferenceException("No result was sent by the Database!");
            }
            return viewmodel;
        }


    }
}
