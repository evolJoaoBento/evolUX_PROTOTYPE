namespace evolUX.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public string Roles { get; set; }


        public AuthenticateResponse(UserModel user, string accessToken)
        {
            Id = user.UserID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.UserName;
            Roles = user.Roles;
            AccessToken = accessToken;
            RefreshToken = user.RefreshToken;
        }
    }
}
