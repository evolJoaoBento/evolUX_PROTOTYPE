using Shared.ViewModels.Areas.Reports;
using Shared.Models.Areas.Reports;
using System.Data;
using evolUX.API.Areas.Core.Repositories.Interfaces;

namespace evolUX.API.Areas.Reports.Services
{
    //public class DependentProductionService
    //{
    //    private readonly IWrapperRepository _repository;
    //    public DependentProductionService(IWrapperRepository repository)
    //    {
    //        _repository = repository;
    //    }
        
    //    public async Task<DependentProductionViewModel> GetDependentPrintsProduction(DataTable ServiceCompanyList)
    //    {
    //        IEnumerable<DependentPrintsInfo> dependentPrintProduction = await _repository.DependentProduction.GetDependentPrintsProduction(ServiceCompanyList);
    //        IEnumerable<DependentFullfillInfo> dependentFullfillProduction = await _repository.DependentProduction.GetDependentFullfillProduction(ServiceCompanyList);
    //        DependentProductionViewModel viewmodel = new DependentProductionViewModel();

    //        viewmodel.DependentPrintProduction = dependentPrintProduction;
    //        viewmodel.DependentFullfillProduction = dependentFullfillProduction;

    //        if (viewmodel.DependentPrintProduction == null)
    //        {
    //            throw new Exception();//MAKE BETTER EXCEPTION
    //        }

    //        if (viewmodel.DependentFullfillProduction == null)
    //        {
    //            throw new Exception();//MAKE BETTER EXCEPTION
    //        }

    //        return viewmodel;
    //    }
    //}
}
