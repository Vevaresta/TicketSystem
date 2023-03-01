﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Ticketsystem.Areas.Identity.Data;

namespace Ticketsystem.Areas.Identity.Services
{
    public class RolesService
    {
        private readonly RoleManager<EnhancedIdentityRole> _roleManager;
        private readonly UserManager<TicketsystemUser> _userManager;

        public RolesService(RoleManager<EnhancedIdentityRole> roleManager, UserManager<TicketsystemUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public List<EnhancedIdentityRole> GetAllRolesFromDb()
        {
            return _roleManager.Roles.Include(r => r.Permissions).ToList();
        }

        public async Task<EnhancedIdentityRole> GetRoleByNameAsync(string role)
        {
            EnhancedIdentityRole roleInDB = await _roleManager.Roles.Include(r => r.Permissions).FirstOrDefaultAsync(r => r.Name == role);
            if (roleInDB == null)
            {
                throw new Exception("Role not found in DB");
            }
            else
            {
                return roleInDB;
            }
        }

        public async Task<EnhancedIdentityRole> GetUserRoleAsync(TicketsystemUser user)
        {
            var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return await GetRoleByNameAsync(userRole);
        }
    }
}
