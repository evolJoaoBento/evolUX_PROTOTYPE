using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using System.Data;

namespace evolUX.API.Areas.Finishing.Repositories.Interfaces
{
    public interface IPostalObjectRepository
    {
        public Task<PostalObjectInfo> GetPostalObjectInfo(DataTable serviceCompanyList, string postObjBarcode);
    }
}
