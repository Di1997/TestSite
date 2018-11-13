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
    public class OrdersModel : PageModel
    {
        DBParams dBParams;

        public OrdersModel(UserManager<IdentityUser> UserManager, ApplicationDbContext context)
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