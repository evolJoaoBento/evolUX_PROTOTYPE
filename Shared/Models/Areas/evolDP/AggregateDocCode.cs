namespace Shared.Models.Areas.evolDP
{
    public class AggregateDocCode
    {
        public int DocCodeID { get; set; }
        public string DocLayout { get; set; }
        public string DocType { get; set; }
        public string DocDescription { get; set; }
        public ExceptionLevel DocExceptionLevel1 { get; set; }
        public ExceptionLevel DocExceptionLevel2 { get; set; }
        public ExceptionLevel DocExceptionLevel3 { get; set; }
        public string Campatible { get; set; }
        public string CheckStatus { get; set; }
        public string AggrCompatibility { get; set; }
    }
}
