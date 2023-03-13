namespace evolUX.API.Areas.Core.ViewModels
{
    public class RolesModel
    {
        public int ProfileID { get; set; }
        public int ParentProfileID { get; set; }
        public int NrChildren { get; set; }
        public string CompanyServer { get; set; }
        public string Description { get; set; }
    }
}
