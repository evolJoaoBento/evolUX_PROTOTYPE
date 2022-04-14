namespace evolUX.API.Data.Interfaces
{
    public interface IWrapperRepository
    {
        IEnvelopeMediaRepository EnvelopeMedia { get; }
        IExpeditionTypeRepository ExpeditionType { get; }
        IFinishingRepository Finishing { get; }
        IUserRepository User { get; }

        ISidebarRepository Sidebar { get; }

        IDocCodeRepository DocCode { get; }
    }
}
