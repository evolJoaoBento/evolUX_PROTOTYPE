using System.Reflection.Metadata.Ecma335;

namespace Shared.Models.Areas.evolDP
{
    public class ConstantParameter
    {
        public int ParameterID { get; set; }
        public string ParameterRef { get; set; }
        public int ParameterValue { get; set; }
        public string ParameterDescription { get { return _parameterDescription; } set { _parameterDescription = value.Trim(); } }
        private string _parameterDescription;
    }
}
