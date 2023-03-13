using System.Text.Json.Serialization;

namespace evolUX.API.Areas.Core.ViewModels
{
    public class UserModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public List<RolesModel> Roles { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string Language { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

    }
}
