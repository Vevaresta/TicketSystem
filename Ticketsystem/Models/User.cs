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

        public virtual IList<TicketUsers> TicketUsers { get; set; }
        public virtual IList<TicketChanges> TicketChanges { get; set; }
        public virtual IList<PermissionsTriggered> PermissionsTriggered { get; set; }
    }
}