using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestSite.Data;
using TestSite.Models;

namespace TestSite.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly TestSite.Data.ApplicationDbContext _context;

        public IndexModel(TestSite.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Simple_User> Simple_User { get;set; }

        public async Task OnGetAsync()
        {
            Simple_User = await _context.Simple_User.ToListAsync();
        }
    }
}
