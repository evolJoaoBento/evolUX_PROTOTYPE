namespace evolUX.API.Areas.EvolDP.Services.Interfaces
{
    public interface IExpeditionTypeService
    {
        public Task<List<dynamic>> GetExpeditionTypes();
    }
}
