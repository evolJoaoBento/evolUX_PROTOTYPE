using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX_dev.Areas.evolDP.Models
{
    public class EnvMediaGroupViewModel
    {
        public int EnvMediaGroupId { get; set; }
        public string EnvMediaGroupDescription { get; set; }

        public string EnvMediaGroupOmission { get; set; }
        public string EnvMediaGroupName { get; set; }
        public int DefaultEnvMediaId { get; set; }
        public int EnvMediaId { get; set; }
        public string EnvMediaName { get; set; }
        public string EnvMediaDescription { get; set; }

        public string FullFillMaterialCode { get; set; }

        public ExpeditionType ExpeditionType { get; set; }

        public string Format { get; set; }
        public Material Material { get; set; }

        public List<DropListItemViewModel> EnvMaterialList { get; set; }

        public List<DropListItemViewModel> EnvMediaList { get; set; }
    }

}
