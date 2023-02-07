namespace Shared.Models.Areas.Finishing
{
    public class ProdServiceElement
    {
        public string ServiceTaskCode { get; set; }
        public string ServiceTaskDec { get; set; }
        public List<ProdMaterialElement> MediaMaterialList { get; set; }
        public ProdServiceElement()
        {
            MediaMaterialList = new List<ProdMaterialElement>();
            ServiceTaskCode = string.Empty;
            ServiceTaskDec = string.Empty;
        }
    }
}
