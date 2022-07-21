using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX_dev.Areas.EvolDP.Models
{
    public class MaterialGroup
    {
        public string MaterialGroupId { get; set; }
        public string MaterialGroupCode { get; set; }
        public string MaterialGroupDescription { get; set; }
        public Material Material { get; set; }
    }
}
