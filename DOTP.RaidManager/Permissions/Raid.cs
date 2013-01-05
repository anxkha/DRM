using DOTP.Users;
using System.Web;

namespace DOTP.RaidManager.Permissions
{
    public class Raid
    {
        public static bool CanModifySignup(string characterName)
        {
            if (null == Manager.GetCurrentUser())
                return false;

            var character = Character.Store.ReadOneOrDefault(c => c.Name == characterName);
            return Manager.GetCurrentUser().ID == character.AccountID;
        }
    }
}
