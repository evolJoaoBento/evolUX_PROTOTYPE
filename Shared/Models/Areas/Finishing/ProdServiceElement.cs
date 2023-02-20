using Shared.Models.Areas.evolDP;

namespace Shared.Models.Areas.Finishing
{
    public class ProdServiceElement: ServiceTask
    {
        public List<ProdMaterialElement> MediaMaterialList { get; set; }
        public ProdServiceElement()
        {
            MediaMaterialList = new List<ProdMaterialElement>();
        }
    }
}
