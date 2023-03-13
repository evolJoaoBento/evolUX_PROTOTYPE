using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX_dev.Areas.evolDP.Models
{
    public class Material
    {
        public int MaterialID { get; set; }
        public int MaterialTypeID { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public string MaterialTypeDescription { get; set; }
        public float MaterialWeight { get; set; }
        public string MaterialRef { get; set; }
        public int FullFillSheets { get; set; }
        public string FullFillMaterialCode { get; set; }
        public float ExpeditionMinWeight { get; set; }
        public int GroupID { get; set; }
        public string EnvelopeMinFormat { get; set; }
        public string MaterialGroup { get; set; }
        public string MaterialGroupID{ get; set; }
        public int MaterialProviderID { get; set; }
        public string MaterialProviderName { get; set; }
        public int MaterialPage { get; set; }
        public string MaterialCost { get; set; }
        public string MaterialBinPos { get; set; }

        public List<DropListItemViewModel> FullFillMaterialCodeList { get; set; }
        public List<DropListItemViewModel> MaterialProviderList { get; set; }
        public List<DropListItemViewModel> MaterialTypeList { get; set; }
        public List<DropListItemViewModel> MaterialBinPosList { get; set; }


    }
}