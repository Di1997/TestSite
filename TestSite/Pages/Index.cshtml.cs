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
using TestSite.Statics;

namespace TestSite.Pages
{
    public class IndexModel : PageModel
    {
        DBParams dBParams;

        public string[] Categories;

        public IndexModel(UserManager<IdentityUser> UserManager, ApplicationDbContext context)
        {
            dBParams = new DBParams(UserManager, context);
        }

        public ActionResult OnGet()
        {
            Categories = (from p in dBParams.Context.Product
                          select p.Category).Distinct().ToArray();

            dBParams.User = User;

            if(dBParams.IsUserAdmin)
            {
                return new RedirectResult(Statics.Pages.ASPAdminOrdersPage);
            }
            return dBParams.CanAccessServices;
        }
    }
}
