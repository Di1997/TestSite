using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestSite.Classes;
using TestSite.Data;
using TestSite.Models;

namespace TestSite.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private DBParams dBParams;

        public TestController(UserManager<IdentityUser> UserManager, ApplicationDbContext context)
        {
            dBParams = new DBParams(UserManager, context);
        }

        public string Action(string value, string value2)
        {
            return value + value2;
        }

        public JsonResult SU()
        {
            dBParams.User = User;
            return new JsonResult(dBParams.SimpleUser);
        }
    }
}
