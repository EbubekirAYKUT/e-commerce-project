using Ebu.Northwind.MvcWebUI.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ebu.Northwind.MvcWebUI.Models
{
    public class RoleDetails
    {
        public IdentityRole Role { get; set; }

        public IEnumerable<ApplicationUser> Members { get; set; }

        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }

    public class RoleEditModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public string[] IdToAdd { get; set; }
        public string[] IdToDelete { get; set; }
    }
}
