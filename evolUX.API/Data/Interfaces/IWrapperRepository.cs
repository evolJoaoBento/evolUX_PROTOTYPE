﻿namespace evolUX.API.Data.Interfaces
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

    }
}
