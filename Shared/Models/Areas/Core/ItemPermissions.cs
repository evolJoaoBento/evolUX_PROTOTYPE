using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Shared.Models.Areas.Core
{
    public class ItemPermissions
    {
        private List<string> _permissions = new List<string>();
        public List<string> Permissions { 
            get { return _permissions; } 
        }
        public void SetPermissions(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                List<string> permissions = JsonConvert.DeserializeObject<List<string>>(json);
                if (permissions != null)
                {
                    _permissions = permissions.ToList();
                }
                else
                    _permissions = new List<string>();
            }
        }
    }
}
