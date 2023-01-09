using System.Data.SqlTypes;

namespace Shared.Models.Areas.Core
{
    public class SideBarAction
    {
        public int ActionID { get; set; }
        public string Description { get; set; }
        public string LocalizationKey { get; set; }
        public string URL { get; set; }
        public List<SideBarAction> ChildActions { get; set; }

        public SideBarAction(int actionID, string description, string localizationKey, string url)
        {
            ActionID = actionID;
            Description = description;
            LocalizationKey = localizationKey;
            URL = url;
            ChildActions = new List<SideBarAction>();
        }
    }
}
