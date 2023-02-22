using System.Xml.Linq;

namespace Shared.Models.General
{
    public class CodeName
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public CodeName(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
    
}
