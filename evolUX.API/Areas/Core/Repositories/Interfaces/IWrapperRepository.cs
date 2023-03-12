using evolUX.API.Areas.evolDP.Repositories.Interfaces;
using evolUX.API.Areas.Finishing.Repositories.Interfaces;

namespace evolUX.API.Areas.Core.Repositories.Interfaces
{
    public interface IWrapperRepository
    {
        IFinishingRepository Finishing { get; }

        IUserRepository User { get; }

        ISidebarRepository Sidebar { get; }

        IDocCodeRepository DocCode { get; }

        IGenericRepository Generic { get; }

        IExpeditionRepository ExpeditionType { get; }

        IConsumablesRepository Consumables { get; }

        IServiceProvisionRepository ServiceProvision { get; }

        IProductionReportRepository ProductionReport { get; }

        ISessionRepository Session { get; }

        IRegistJobRepository RegistJob { get; }

        IPrintedFilesRepository PrintedFiles { get; }

        IFulfiledFilesRepository FullfilledFiles { get; }

        IRecoverRepository Recover { get; }

        IPendingRegistRepository PendingRegist { get; }

        IPostalObjectRepository PostalObject { get; }

        IPendingRecoverRepository PendingRecover { get; }

        IExpeditionReportRepository ExpeditionReport { get; }
    }
}
