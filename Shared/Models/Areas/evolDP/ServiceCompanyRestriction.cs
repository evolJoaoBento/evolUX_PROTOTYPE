using System.Text;

namespace Shared.Models.Areas.evolDP
{
    public class ServiceCompanyRestriction
    {
        public int ServiceCompanyID { get; set; }
        public int MaterialTypeID { get; set; }
        public string MaterialTypeDesc { get; set; }
        public string MaterialTypeCode { get; set; }
        public int MaterialPosition { get; set; }
        public int FileSheetsCutoffLevel { get; set; }
        public bool RestrictionMode { get; set; } //CASE WHEN mt.MaterialTypeCode = 'STATION' THEN CAST(scr.RestrictionMode as varchar) ELSE 'NA' END -- Ação em caso de exceder o nº máximo de estações, '0' ==> 'Impede Produção do objecto postal', 1 ==> 'Obriga a envelopagem manual do objecto postal'
    }
}
