using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestSite.Classes;
using TestSite.Data;

namespace TestSite.Pages.Admin
{
    public class UsersModel : PageModel
    {
        DBParams dBParams;

        public UsersModel(UserManager<IdentityUser> UserManager, ApplicationDbContext context)
        {
            dBParams = new DBParams(UserManager, context);
        }

        public ActionResult OnGet()
        {
            dBParams.User = User;

            return dBParams.CanAccessAdmin;
        }
    }
}