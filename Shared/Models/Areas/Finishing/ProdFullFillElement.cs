namespace Shared.Models.Areas.Finishing
{
    public class ProdFullFillElement
    {
        public string FullFillMaterialCode { get; set; }
        public int FullFillCapacity { get; set; }
        public List<ProdFileInfo> FileList { get; set; }
        public ProdFullFillElement()
        {
            FileList = new List<ProdFileInfo>();
            FullFillMaterialCode = string.Empty;
        }
    }
}
