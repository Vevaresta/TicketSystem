using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Ticketsystem.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<TicketUsers> TicketUsers { get; set; }
        public virtual ICollection<TicketChanges> TicketChanges { get; set; }
        public virtual ICollection<PermissionsTriggered> PermissionsTriggered { get; set; }
    }
}