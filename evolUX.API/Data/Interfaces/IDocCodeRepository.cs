namespace evolUX.API.Data.Interfaces
{
    public interface IDocCodeRepository
    {
        public Task<List<dynamic>> GetDocCode();
        public Task<List<dynamic>> GetDocCodeLevel1(dynamic data);
        public  Task<List<dynamic>> GetDocCodeLevel2(dynamic data);
        public  Task<dynamic> GetDocCodeConfig(dynamic data);
        public  Task<dynamic> GetDocCodeExceptionOptions(dynamic data);
    }
}
