using Microsoft.AspNetCore.Identity;
using Ticketsystem.Enums;
using Ticketsystem.Models.Database;

namespace Ticketsystem.DbAccess
{
    public class RolesToDisplayDbAccess
    {
        private readonly RoleManager<Role> _roleManager;

        public RolesToDisplayDbAccess(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public List<string> GetList()
        {
            List<string> rolesToDisplay = new()
            {
                DefaultRoles.Mitarbeiter.ToString(),
                DefaultRoles.Abteilungsleiter.ToString(),
                DefaultRoles.Administrator.ToString()
            };

            foreach (var role in _roleManager.Roles)
            {
                if (!rolesToDisplay.Contains(role.Name) && !(role.Name == DefaultRoles.Fallback.ToString()))
                {
                    rolesToDisplay.Add(role.Name);
                }
            }

            return rolesToDisplay;
        }
    }
}
