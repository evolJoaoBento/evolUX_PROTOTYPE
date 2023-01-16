namespace evolUX.API.Data.Interfaces
{
    public interface IWrapperRepository
    {
        IEnvelopeMediaRepository EnvelopeMedia { get; }

        IExpeditionCompaniesRepository ExpeditionCompanies { get; }

        IExpeditionTypeRepository ExpeditionType { get; }

        IExpeditionZoneRepository ExpeditionZone { get; }

        IFinishingRepository Finishing { get; }

        IUserRepository User { get; }

        ISidebarRepository Sidebar { get; }

        IDocCodeRepository DocCode { get; }

        IProductionReportRepository ProductionReport { get; }

        ISessionRepository Session { get; }

        IPrintFilesRepository PrintFiles { get; }

        IPrintedFilesRepository PrintedFiles { get; }

        IFullfilledFilesRepository FullfilledFiles { get; }

        IRecoverRepository Recover { get; }

        IPendingRegistriesRepository PendingRegistries { get; }
    }
}
