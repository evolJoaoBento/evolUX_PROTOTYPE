namespace evolUX.UI.Areas.Core.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserType { get; set; }
        public string ParentUserID { get; set; }
        public string PlainText { get; set; }
        public string Username { get; set; }
        public List<RolesModel> Roles { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
