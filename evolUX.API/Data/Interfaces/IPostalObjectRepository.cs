

using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface IPostalObjectRepository
    {
        public Task<PostalObjectInfo> GetPostalObjectInfo(DataTable serviceCompanyList, string postObjBarcode);
    }
}
