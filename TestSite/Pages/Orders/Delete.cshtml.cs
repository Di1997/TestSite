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
    public class DeleteModel : PageModel
    {
        private readonly TestSite.Data.ApplicationDbContext _context;

        public DeleteModel(TestSite.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Simple_User = await _context.Simple_User.FindAsync(id);

            if (Simple_User != null)
            {
                _context.Simple_User.Remove(Simple_User);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
