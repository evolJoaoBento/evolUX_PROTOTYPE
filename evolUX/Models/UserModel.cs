using System.Text.Json.Serialization;

namespace evolUX.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Roles { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

    }
}
