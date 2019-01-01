using Ebu.Northwind.MvcWebUI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ebu.Northwind.MvcWebUI.Tag_Helpers
{
    [HtmlTargetElement("td",Attributes = "identity-role")]
    public class RoleUsersTagHelper:TagHelper
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public RoleUsersTagHelper(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HtmlAttributeName("Identity-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();
            var role = await _roleManager.FindByIdAsync(Role);
            if (role!=null)
            {
                foreach (var user in _userManager.Users)
                {
                    if (user!=null && await _userManager.IsInRoleAsync(user,role.Name))
                    {
                        names.Add(user.UserName);
                    }
                }
            }
            output.Content.SetContent(names.Count == 0 ? 
                "No User" : 
                string.Join(",", names));
        }
    }
}
