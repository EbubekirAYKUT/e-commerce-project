using Ebu.Northwind.MvcWebUI.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ebu.Northwind.MvcWebUI.Infrastructure
{
    public class CustomUserValidator : IUserValidator<ApplicationUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            if (user.Email.ToLower().EndsWith("@gmail.com") || user.Email.ToLower().EndsWith("@hotmail.com"))
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "MailServer",
                    Description = "You can register with just gmail or hotmail "
                }));
            }
        }
    }
}
