namespace evolUX.Interfaces
{
    public interface IRepositoryWrapper
    {
        IEnvelopeMediaRepository EnvelopeMedia { get; }
        IExpeditionTypeRepository ExpeditionType { get; }
        IFinishingRepository Finishing { get; }
    }
}
