namespace Shared.Models.Areas.evolDP
{
    public class DocCode
    {
        public int DocCodeID { get; set; }
        public string DocLayout { get; set; }
        public string DocType { get; set; }
        public string DocDescription { get; set; }
        public ExceptionLevel ExceptionLevel1 { get; set; }
        public ExceptionLevel ExceptionLevel2 { get; set; }
        public ExceptionLevel ExceptionLevel3 { get; set; }
        public string PrintMatchCode { get; set; }
    }
}
