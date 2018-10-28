using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestSite.Data;

namespace TestSite.Pages.Shared
{
    public class _LoginPartial
    {
        private readonly ApplicationDbContext _context;
        public UserManager<IdentityUser> _user;

        public _LoginPartial(UserManager<IdentityUser> UserManager, ApplicationDbContext context)
        {
            _context = context;
            _user = UserManager;
        }

        public void OnGet()
        {
            //var CurrentUser = _context.Simple_User.FirstOrDefault(m => m.ID == Guid.Parse(_user.GetUserId(User)));

        }
    }
}
