using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using evolUX.API.Models;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Services
{
    public class ExpeditionService : IExpeditionService
    {
        private readonly IWrapperRepository _repository;
        public ExpeditionService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExpeditionTypeViewModel> GetExpeditionCompanies(DataTable expCompanyList)
        {
            ExpeditionTypeViewModel viewModel = new ExpeditionTypeViewModel();
            viewModel.ExpCompanies = await _repository.Generic.GetCompanies(null, expCompanyList);
            viewModel.Types = await _repository.ExpeditionType.GetExpeditionTypes(null);
            if (viewModel.Types != null)
            {
                foreach (ExpeditionTypeElement e in viewModel.Types.ToList())
                {
                    e.ExpCompanyTypesList = await _repository.ExpeditionType.GetExpCompanyTypes(e.ExpeditionType, null, expCompanyList);
                }
            }
            return viewModel;
        }

        public async Task<ExpeditionTypeViewModel> GetExpeditionTypes(int? expeditionType, DataTable? expCompanyList)
        {
            ExpeditionTypeViewModel viewModel = new ExpeditionTypeViewModel();
            viewModel.Types = await _repository.ExpeditionType.GetExpeditionTypes(expeditionType);
            if (viewModel.Types != null && expeditionType == null && expCompanyList != null)
            {
                foreach(ExpeditionTypeElement e in viewModel.Types.ToList())
                {
                    e.ExpCompanyTypesList = await _repository.ExpeditionType.GetExpCompanyTypes(e.ExpeditionType, null, expCompanyList);
                }
            }
            return viewModel;
        }
        public async Task<ExpeditionTypeViewModel> GetExpCompanyTypes(int? expeditionType, int? expCompanyID)
        {
            ExpeditionTypeViewModel viewModel = new ExpeditionTypeViewModel();
            IEnumerable<ExpCompanyType> expCompanyTypes = await _repository.ExpeditionType.GetExpCompanyTypes(expeditionType, expCompanyID, null);
            List<ExpeditionTypeElement> expList = new List<ExpeditionTypeElement>();
            if (expCompanyTypes != null)
            {
                viewModel.Types = await _repository.ExpeditionType.GetExpeditionTypes(expeditionType);
                foreach (ExpeditionTypeElement e in viewModel.Types.ToList())
                {
                    e.ExpCompanyTypesList = expCompanyTypes.Where(x => x.ExpeditionType == e.ExpeditionType);
                }
                
            }

            return viewModel;
        }
        public async Task<ExpeditionTypeViewModel> SetExpCompanyType(int expeditionType, int expCompanyID, bool registMode, bool separationMode, bool barcodeRegistMode, bool returnAll)
        {
            await _repository.ExpeditionType.SetExpCompanyType(expeditionType, expCompanyID, registMode, separationMode, barcodeRegistMode);
            ExpeditionTypeViewModel viewModel = await GetExpCompanyTypes(returnAll ? null : expeditionType, expCompanyID);
            return viewModel;
        }
        public async Task<ExpeditionZoneViewModel> GetExpeditionZones(int? expeditionZone, DataTable? expCompanyList)
        {
            ExpeditionZoneViewModel viewModel = new ExpeditionZoneViewModel();
            viewModel.Zones = await _repository.ExpeditionType.GetExpeditionZones(expeditionZone);
            if (viewModel.Zones != null && expeditionZone == null && expCompanyList != null)
            {
                foreach (ExpeditionZoneElement e in viewModel.Zones.ToList())
                {
                    e.ExpCompanyZonesList = await _repository.ExpeditionType.GetExpCompanyZones(e.ExpeditionZone, null, expCompanyList);
                }
            }
            return viewModel;
        }

        public async Task<IEnumerable<Company>> GetExpeditionCompanies(int? expCompanyID, DataTable? expCompanyList)
        {
            var expeditionCompaniesList = await _repository.Generic.GetCompanies(expCompanyID, expCompanyList);
            if (expeditionCompaniesList == null)
            {

            }
            return expeditionCompaniesList;
        }
        public async Task<IEnumerable<ExpeditionRegistElement>> GetExpeditionRegistIDs(int expCompanyID)
        {
            var expeditionRegistIDs = await _repository.ExpeditionType.GetExpeditionRegistIDs(expCompanyID);
            if (expeditionRegistIDs == null)
            {

            }
            return expeditionRegistIDs;
        }
        public async Task<List<dynamic>> GetExpeditionCompanyConfigs(dynamic data)
        {
            var expeditionCompanyConfigsList = await _repository.ExpeditionType.GetExpeditionCompanyConfigs(data);
            if (expeditionCompanyConfigsList == null)
            {

            }
            return expeditionCompanyConfigsList;
        }

        public async Task<List<dynamic>> GetExpeditionCompanyConfigCharacteristics(dynamic data)
        {
            var envelopeMediaGroupList = await _repository.ExpeditionType.GetExpeditionCompanyConfigCharacteristics(data);
            if (envelopeMediaGroupList == null)
            {

            }
            return envelopeMediaGroupList;
        }
    }
}
