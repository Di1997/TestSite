using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestSite.Data;
using TestSite.Models;

namespace TestSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public UserManager<IdentityUser> _user;
        public IndexModel(UserManager<IdentityUser> UserManager, ApplicationDbContext context)
        {
            _user = UserManager;
            _context = context;
        }

        Simple_User _Simple_User { get; set; }





        public ActionResult OnGet()
        {
            _Simple_User = _context.Simple_User.FirstOrDefault(m => m.ID == Guid.Parse(_user.GetUserId(User)));
            if(_Simple_User == null)
            { return Redirect("/Registration_2"); }
            return null;
        }
    }
}
