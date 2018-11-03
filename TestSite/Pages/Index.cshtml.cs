using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestSite.Classes;
using TestSite.Data;

namespace TestSite.Pages
{
    public class IndexModel : PageModel
    {
        public DBParams dBParams;

        public IndexModel(UserManager<IdentityUser> UserManager, ApplicationDbContext context)
        {
            dBParams = new DBParams(UserManager, context);
        }

        public ActionResult OnGet()
        {
            dBParams.User = User;

            return dBParams.CanAccessServices;
        }
    }
}
