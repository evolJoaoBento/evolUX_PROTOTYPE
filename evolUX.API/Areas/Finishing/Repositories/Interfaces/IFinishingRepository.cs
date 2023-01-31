namespace evolUX.API.Areas.Finishing.Repositories.Interfaces
{
    public interface IFinishingRepository
    {
        public Task<dynamic> GetRunsOngoing();

        public Task<dynamic> GetPendingRegist();
    }
}
