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
    public class DetailsModel : PageModel
    {
        private readonly TestSite.Data.ApplicationDbContext _context;

        public DetailsModel(TestSite.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Simple_User Simple_User { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Simple_User = await _context.Simple_User.FirstOrDefaultAsync(m => m.ID == id);

            if (Simple_User == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
