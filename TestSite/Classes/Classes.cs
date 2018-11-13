using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestSite.Data;
using TestSite.Models;
using TestSite.Statics;

namespace TestSite.Classes
{
    public class DBParams
    {
        public ApplicationDbContext Context { get; set; }
        public UserManager<IdentityUser> UserManager { get; set; }
        public ClaimsPrincipal User { get; set; }

        public DBParams(UserManager<IdentityUser> UserManager, ApplicationDbContext Context)
        {
            this.Context = Context;
            this.UserManager = UserManager;
        }

        public Simple_User SimpleUser
        {
            get { return Context.Simple_User.FirstOrDefault(m => m.ID == Guid.Parse(UserManager.GetUserId(User))); }
        }

        public Simple_User GetSimple_User(Guid userID)
        {
            return Context.Simple_User.FirstOrDefault(m => m.ID == userID);
        }

        public async Task<bool> IsUserAdminAsync()
        {
            return await UserManager.IsInRoleAsync(IdentityUser, Roles.Manager);
        }

        public bool IsUserAdmin
        {
            get
            {
                if (IdentityUser != null)
                {
                    IsUserAdminAsync().Wait();
                    return IsUserAdminAsync().Result;
                }
                else return false;
            }
        }

        public ActionResult CanAccessServices
        {
            get {
                if (!IsUserAdmin)
                {
                    if (SimpleUser == null)
                    {
                        return new RedirectResult(Statics.Pages.Registation2);
                    }

                    else if (!SimpleUser.Validated)
                    {
                        return new RedirectResult(Statics.Pages.InvalidUser);
                    }
                }

                return null;
            }
        }

        public IdentityUser IdentityUser
        {
            get
            {
                UserManager.FindByIdAsync(UserManager.GetUserId(User)).Wait();
                return UserManager.FindByIdAsync(UserManager.GetUserId(User)).Result;
            }
        }

        public IdentityUser GetIdentityUser(string id)
        {
            var GetIdentityUser = UserManager.FindByIdAsync(id);
            GetIdentityUser.Wait();
            return GetIdentityUser.Result;
        }

        public ActionResult CanAccessAdmin
        {
            get
            {
                if(!IsUserAdmin)
                {
                    return new RedirectResult(Statics.Pages.NotAdmin);
                }

                return null;
            }
        }

        public string GenerateProductCode()
        {
            Random random = new Random();

            char[] chars = { (char)('A' + random.Next(0, 26)), (char)('A' + random.Next(0, 26)) };
            string letters = new string(chars);
            string code = $"{random.Next(10, 99)}-{random.Next(1000,9999)}-{letters}{random.Next(10,99)}";
            return code;
        }

        public Product GetProduct(Guid id)
        {
            return Context.Product.FirstOrDefault(p => p.ID ==id);
        }

        public Order[] UserOrders
        {
            get
            {
                return Context.Order.Where(o => o.Customer_ID == SimpleUser.ID).ToArray();
            }
        }
    }
}