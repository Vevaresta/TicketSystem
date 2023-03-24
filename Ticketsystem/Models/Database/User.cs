using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ticketsystem.ViewModels;

namespace Ticketsystem.Models.Database
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual IList<TicketChange> TicketChanges { get; set; }

        public static implicit operator UserViewModel(User user)
        {
            return new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
            };
        }
    }
}