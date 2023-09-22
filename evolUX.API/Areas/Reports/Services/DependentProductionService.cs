using Shared.ViewModels.Areas.Reports;
using Shared.Models.Areas.Reports;
using System.Data;
using evolUX.API.Areas.Core.Repositories.Interfaces;

namespace evolUX.API.Areas.Reports.Services
{
    public class DependentProductionService
    {
        private readonly IWrapperRepository _repository;
        public DependentProductionService(IWrapperRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<DependentProductionViewModel> GetDependentPrintsProduction(int RunID, DataTable ServiceCompanyList)
        {
            IEnumerable<DependentPrintsInfo> dependentProduction = await _repository.DependentProduction.GetDependentPrintsProduction(RunID, ServiceCompanyList);
            DependentProductionViewModel viewmodel = new DependentProductionViewModel();
            IEnumerable<DependentFullfillInfo> FileList = await _repository.DependentProduction.GetDependentFullfillProduction(RunID, ServiceCompanyList);

            viewmodel.DependentPrintProduction;
            viewmodel.DependentFullFillProduction;

            if (viewmodel.DependentPrintProduction == null)
            {
                throw new Exception();//MAKE BETTER EXCEPTION
            }

            return viewmodel;

            if (viewmodel.DependentFullfillProduction == null)
            {
                throw new Exception();//MAKE BETTER EXCEPTION
            }
        }
    }
}
