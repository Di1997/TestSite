using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestSite.Statics;
using TestSite.Data;
using TestSite.Classes;
using Microsoft.AspNetCore.Identity;

namespace TestSite.Controllers
{
    [Route(Routes.ControllerRoute)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private DBParams dBParams;

        public UserController(UserManager<IdentityUser> UserManager, ApplicationDbContext context)
        {
            dBParams = new DBParams(UserManager, context);
        }

        [HttpGet]
        public JsonResult GetProducts()
        {
            return new JsonResult(dBParams.Context.Product);
        }

        [HttpGet]
        public int GetUserDiscount()
        {
            dBParams.User = User;

            return dBParams.SimpleUser.Discount;
        }
    }
}