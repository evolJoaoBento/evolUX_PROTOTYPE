using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using Shared.ViewModels.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Services
{
    public class ServiceProvisionService : IServiceProvisionService
    {
        private readonly IWrapperRepository _repository;

        public ServiceProvisionService(IWrapperRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Company>> GetServiceCompanies(DataTable serviceCompanyList)
        {
            IEnumerable<Company> result = await _repository.Generic.GetCompanies(null, serviceCompanyList);
            return result;
        }
        public async Task<IEnumerable<Company>> GetServiceCompanies(int serviceCompanyID)
        {
            IEnumerable<Company> result = await _repository.Generic.GetCompanies(serviceCompanyID, null);
            return result;
        }
        public async Task<IEnumerable<ServiceCompanyRestriction>> GetServiceCompanyRestrictions(int? serviceCompanyID)
        {
            IEnumerable<ServiceCompanyRestriction> result = await _repository.ServiceProvision.GetServiceCompanyRestrictions(serviceCompanyID);
            return result;
        }
        public async Task SetServiceCompanyRestriction(int serviceCompanyID, int materialTypeID, int materialPosition, int fileSheetsCutoffLevel, bool restrictionMode)
        {
            await SetServiceCompanyRestriction(serviceCompanyID, materialTypeID, materialPosition, fileSheetsCutoffLevel, restrictionMode);
            return;
        }

        public async Task<IEnumerable<ServiceCompanyServiceResume>> GetServiceCompanyConfigsResume(int? serviceCompanyID)
        {
            IEnumerable<ServiceCompanyServiceResume> result = await _repository.ServiceProvision.GetServiceCompanyConfigsResume(serviceCompanyID);
            return result;
        }

        public async Task<IEnumerable<ServiceCompanyService>> GetServiceCompanyConfigs(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID)
        {
            IEnumerable<ServiceCompanyService> result = await _repository.ServiceProvision.GetServiceCompanyConfigs(serviceCompanyID, costDate, serviceTypeID, serviceID);
            if (result == null)
            {

            }
            return result;
        }
        public async Task SetService(ServiceElement service)
        {
            await _repository.ServiceProvision.SetService(service);
        }

        public async Task<IEnumerable<ServiceElement>> GetServices(int serviceTypeID)
        {
            IEnumerable<ServiceElement> result = await _repository.ServiceProvision.GetServices(serviceTypeID);
            if (result == null)
            {

            }
            return result;
        }
        public async Task SetServiceCompanyConfig(ServiceCompanyService serviceCompanyConfig)
        {
            await _repository.ServiceProvision.SetServiceCompanyConfig(serviceCompanyConfig);
        }       //public async Task<ExpeditionTypeViewModel> GetExpeditionTypes(int? expeditionType, DataTable? expCompanyList)
        //{
        //    ExpeditionTypeViewModel viewModel = new ExpeditionTypeViewModel();
        //    viewModel.Types = await _repository.ExpeditionType.GetExpeditionTypes(expeditionType);
        //    if (viewModel.Types != null && expCompanyList != null)
        //    {
        //        foreach (ExpeditionTypeElement e in viewModel.Types.ToList())
        //        {
        //            e.ExpCompanyTypesList = await _repository.ExpeditionType.GetExpCompanyTypes(e.ExpeditionType, null, expCompanyList);
        //        }
        //    }
        //    return viewModel;
        //}
        //public async Task<IEnumerable<ExpCompanyType>> GetExpCompanyTypes(int? expeditionType, int? expCompanyID)
        //{
        //    return await _repository.ExpeditionType.GetExpCompanyTypes(expeditionType, expCompanyID, null);
        //}

        //public async Task<Result> SetExpCompanyType(int expeditionType, int expCompanyID, bool registMode, bool separationMode, bool barcodeRegistMode)
        //{
        //    Result results = await _repository.ExpeditionType.SetExpCompanyType(expeditionType, expCompanyID, registMode, separationMode, barcodeRegistMode);
        //    if (results == null)
        //    {

        //    }
        //    return results;
        //}


        //public async Task<IEnumerable<Company>> GetExpeditionCompanies(int? expCompanyID, DataTable? expCompanyList)
        //{
        //    var expeditionCompaniesList = await _repository.Generic.GetCompanies(expCompanyID, expCompanyList);
        //    if (expeditionCompaniesList == null)
        //    {

        //    }
        //    return expeditionCompaniesList;
        //}
        //public async Task<IEnumerable<ExpeditionRegistElement>> GetExpeditionRegistIDs(int expCompanyID)
        //{
        //    var expeditionRegistIDs = await _repository.ExpeditionType.GetExpeditionRegistIDs(expCompanyID);
        //    if (expeditionRegistIDs == null)
        //    {

        //    }
        //    return expeditionRegistIDs;
        //}
        //public async Task SetExpeditionRegistID(ExpeditionRegistElement expRegist)
        //{
        //    await _repository.ExpeditionType.SetExpeditionRegistID(expRegist);
        //    return;
        //}
        //public async Task<IEnumerable<ExpContractElement>> GetExpContracts(int expCompanyID)
        //{
        //    var expeditionRegistIDs = await _repository.ExpeditionType.GetExpContracts(expCompanyID);
        //    if (expeditionRegistIDs == null)
        //    {

        //    }
        //    return expeditionRegistIDs;
        //}
        //public async Task SetExpContract(ExpContractElement expContract)
        //{
        //    await _repository.ExpeditionType.SetExpContract(expContract);
        //    return;
        //}

        //public async Task NewExpCompanyConfig(int expCompanyID, int startDate)
        //{
        //    await _repository.ExpeditionType.NewExpCompanyConfig(expCompanyID, startDate);
        //}
        //public async Task<IEnumerable<ExpCompanyConfigResume>> GetExpCompanyConfigsResume(int expCompanyID)
        //{
        //    IEnumerable<ExpCompanyConfigResume> result = await _repository.ExpeditionType.GetExpCompanyConfigsResume(expCompanyID);
        //    if (result == null)
        //    {

        //    }
        //    return result;
        //}
    }
}
