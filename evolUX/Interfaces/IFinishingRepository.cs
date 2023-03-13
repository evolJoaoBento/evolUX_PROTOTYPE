namespace evolUX.Interfaces
{
    public interface IFinishingRepository
    {
        public Task<dynamic> GetRunsOngoing();

        public Task<dynamic> GetPendingRegist();
    }
}
