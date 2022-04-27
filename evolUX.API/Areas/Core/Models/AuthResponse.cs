

namespace evolUX.API.Areas.Core.Models
{
    public class AuthResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public List<RolesModel> Roles { get; set; }


        public AuthResponse(UserModel user, string accessToken)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            Roles = user.Roles;
            AccessToken = accessToken;
            RefreshToken = user.RefreshToken;
        }
    }
}
