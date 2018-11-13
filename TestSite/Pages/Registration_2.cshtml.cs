using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestSite.Data;
using TestSite.Models;

namespace TestSite.Pages.Shared
{
    public class Registration_2Model : PageModel
    {
        private readonly ApplicationDbContext _context;

        public UserManager<IdentityUser> _user;

        public string _ID { get; set; }
        public string _Code { get; set; }

        public Registration_2Model(UserManager<IdentityUser> UserManager, ApplicationDbContext context)
        {
            _context = context;
            _user = UserManager;
        }

        public IActionResult OnGet()
        {
            var CurrentUser = _context.Simple_User.FirstOrDefault(m => m.ID == Guid.Parse(_user.GetUserId(User)));
            if (CurrentUser == null)
            {
                _ID = _user.GetUserId(User);
                _Code = GenerateCode();

                HttpContext.Session.SetString("ID", _ID);
                HttpContext.Session.SetString("Code", _Code);

                return Page();
            } else { return Redirect("/Index"); }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Display(Name = "Address")]
            public string Address { get; set; }
        }


        public Simple_User Simple_User { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

            Simple_User = new Simple_User
            {
                ID = Guid.Parse(HttpContext.Session.GetString("ID")),
                Code = HttpContext.Session.GetString("Code"),
                Validated = false,
                Name = Input.Name,
                Address = Input.Address == null? "" : Input.Address,
                Discount = 0
            };

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Simple_User.Add(Simple_User);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private string GenerateCode()
        {
            Random _random = new Random();
            return $"{_random.Next(1000, 9999).ToString()}-{DateTime.Now.Year}";
        }

    }
}