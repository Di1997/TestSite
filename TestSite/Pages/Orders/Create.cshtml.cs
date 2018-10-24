using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestSite.Data;
using TestSite.Models;

namespace TestSite.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly TestSite.Data.ApplicationDbContext _context;

        public CreateModel(TestSite.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Simple_User Simple_User { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Simple_User.Add(Simple_User);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}