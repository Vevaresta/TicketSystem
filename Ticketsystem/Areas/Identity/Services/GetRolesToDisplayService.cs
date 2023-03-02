using Microsoft.AspNetCore.Identity;
using Ticketsystem.Areas.Identity.Data;
using Ticketsystem.Areas.Identity.Enums;

namespace Ticketsystem.Areas.Identity.Services
{
    public class GetRolesToDisplayService
    {
        private readonly RoleManager<EnhancedIdentityRole> _roleManager;

        public GetRolesToDisplayService(RoleManager<EnhancedIdentityRole> roleManager)
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
