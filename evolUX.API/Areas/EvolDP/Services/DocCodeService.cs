using evolUX.API.Areas.EvolDP.Services.Interfaces;
using evolUX.API.Data.Interfaces;

namespace evolUX.API.Areas.EvolDP.Services
{
    public class DocCodeService : IDocCodeService
    {
        private readonly IWrapperRepository _repository;
        public DocCodeService(IWrapperRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<dynamic>> GetDocCode()
        {
            var docCodeList = await _repository.DocCode.GetDocCode();
            if(docCodeList == null)
            {

            }
            return docCodeList;
        }

        public async Task<List<dynamic>> GetDocCodeLevel1(dynamic data)
        {
            var docList = await _repository.DocCode.GetDocCodeLevel1(data);
            if (docList == null)
            {

            }
            return docList;
        }        
        public async Task<List<dynamic>> GetDocCodeLevel2(dynamic data)
        {
            var docList = await _repository.DocCode.GetDocCodeLevel2(data);
            if (docList == null)
            {

            }
            return docList;
        }        

        public async Task<List<dynamic>> GetDocCodeConfig(dynamic data)
        {
            var docList = await _repository.DocCode.GetDocCodeConfig(data);
            if (docList == null)
            {

            }
            return docList;
        }
        public async Task<List<dynamic>> GetDocCodeExceptionOptions(dynamic data)
        {
            var docList = await _repository.DocCode.GetDocCodeExceptionOptions(data);
            if (docList == null)
            {

            }
            return docList;
        }
    }
}
