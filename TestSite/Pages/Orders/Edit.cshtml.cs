using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestSite.Data;
using TestSite.Models;

namespace TestSite.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly TestSite.Data.ApplicationDbContext _context;

        public EditModel(TestSite.Data.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Simple_User).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Simple_UserExists(Simple_User.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool Simple_UserExists(Guid id)
        {
            return _context.Simple_User.Any(e => e.ID == id);
        }
    }
}
