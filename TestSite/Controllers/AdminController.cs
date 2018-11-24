using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console.Internal;
using TestSite.Classes;
using TestSite.Data;
using TestSite.Models;
using TestSite.Statics;

namespace TestSite.Controllers
{
    [Route(Routes.ControllerRoute)]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private AdminControllerLogic logic;
        private DBParams dBParams;

        public AdminController(UserManager<IdentityUser> UserManager, ApplicationDbContext context)
        {
            logic = new AdminControllerLogic(new UnitOfWork(context));
            dBParams = new DBParams(UserManager, context);
        }

        #region Users

        [HttpPut]
        public void UpdateUser(Simple_User user)
        {
            logic.UpdateUser(user);
        }

        [HttpDelete]
        public void DeleteUser(string id)
        {
            logic.DeleteUser(id);

            dBParams.UserManager.DeleteAsync(dBParams.GetIdentityUser(id)).Wait();
        }

        [HttpGet]
        public IEnumerable<Simple_User> GetUsers()
        {
            return logic.GetUsers();
        }

        #endregion

        #region Products
        [HttpGet]
        public Product GenerateProductInfo()
        {
            return logic.GenerateProductInfo();
        }

        [HttpPost]
        public void AddProduct(Product product)
        {
            logic.AddProduct(product);
        }

        [HttpDelete]
        public void DeleteProduct(string id)
        {
            logic.DeleteProduct(id);
        }

        [HttpPut]
        public void UpdateProduct(Product p)
        {
            logic.UpdateProduct(p);
        }
        #endregion

        #region Orders

        [HttpGet]
        public List<object> GetOrders()
        {
            return logic.GetOrders();
        }

        [HttpGet]
        public List<object> GetOrderElements(string id)
        {
            return logic.GetOrderElements(id);
        }

        [HttpPut]
        public void UpdateOrderStatus(string id)
        {
            logic.UpdateOrderStatus(id);
        }

        #endregion
    }
}