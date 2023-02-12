namespace Shared.Models.Areas.evolDP
{
    public class Company
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPostalCode { get; set; }
        public string CompanyPostalCodeDescription { get; set; }
        public string CompanyCountry { get; set; }
        public Company()
        {
            ID = 0;
            CompanyName = string.Empty;
            CompanyCode = string.Empty;
            CompanyAddress = string.Empty;
            CompanyPostalCode = string.Empty;
            CompanyPostalCodeDescription = string.Empty;
            CompanyCountry = string.Empty;
        }
    }
}
