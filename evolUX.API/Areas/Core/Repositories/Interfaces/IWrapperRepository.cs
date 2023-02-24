using evolUX.API.Areas.evolDP.Repositories.Interfaces;
using evolUX.API.Areas.Finishing.Repositories.Interfaces;

namespace evolUX.API.Areas.Core.Repositories.Interfaces
{
    public interface IWrapperRepository
    {
        IEnvelopeMediaRepository EnvelopeMedia { get; }

        IFinishingRepository Finishing { get; }

        IUserRepository User { get; }

        ISidebarRepository Sidebar { get; }

        IDocCodeRepository DocCode { get; }

        IClientRepository Client { get; }

        IExpeditionRepository Expedition { get; }

        IProductionReportRepository ProductionReport { get; }

        ISessionRepository Session { get; }

        IRegistJobRepository RegistJob { get; }

        IPrintedFilesRepository PrintedFiles { get; }

        IFullfilledFilesRepository FullfilledFiles { get; }

        IRecoverRepository Recover { get; }

        IPendingRegistRepository PendingRegist { get; }

        IPostalObjectRepository PostalObject { get; }

        IPendingRecoverRepository PendingRecover { get; }

        IExpeditionReportRepository ExpeditionReport { get; }
    }
}
