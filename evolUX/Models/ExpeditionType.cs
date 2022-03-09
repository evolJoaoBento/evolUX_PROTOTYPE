using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX.Models
{
    public class ExpeditionType
    {
        
        public int Id { get; set; }
        public int Priority { get; set; }
        public string? Description { get; set; }
    }
}
