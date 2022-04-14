namespace evolUX.API.Data.Interfaces
{
    public interface IFinishingRepository
    {
        public Task<dynamic> GetRunsOngoing();

        public Task<dynamic> GetPendingRegist();
    }
}
