namespace evolUX.API.Areas.EvolDP.Services.Interfaces
{
    public interface IDocCodeService
    {
        public Task<List<dynamic>> GetDocCode();
        public Task<List<dynamic>> GetDocCodeLevel1(dynamic data);
        public Task<List<dynamic>> GetDocCodeLevel2(dynamic data);
        public Task<List<dynamic>> GetDocCodeConfig(dynamic data);
        public Task<List<dynamic>> GetDocCodeExceptionOptions(dynamic data);
    }
}
