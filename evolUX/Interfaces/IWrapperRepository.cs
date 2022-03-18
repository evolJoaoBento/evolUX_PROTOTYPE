namespace evolUX.Interfaces
{
    public interface IWrapperRepository
    {
        IEnvelopeMediaRepository EnvelopeMedia { get; }
        IExpeditionTypeRepository ExpeditionType { get; }
        IFinishingRepository Finishing { get; }
        IUserRepository User { get; }
    }
}
