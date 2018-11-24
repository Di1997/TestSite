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
using TestSite.Models;
using Newtonsoft.Json;

namespace TestSite.Controllers
{
    [Route(Routes.ControllerRoute)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private DBParams dBParams;
        private UserControllerLogic logic;
        UserManager<IdentityUser> UserManager;

        public UserController(UserManager<IdentityUser> UserManager, ApplicationDbContext context)
        {
            dBParams = new DBParams(UserManager, context);
            logic = new UserControllerLogic(new UnitOfWork(context));
            this.UserManager = UserManager;
        }

        #region Shop

        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return logic.GetProducts();
        }

        [HttpGet]
        public int GetUserDiscount()
        {
            return logic.GetUserDiscount(UserManager.GetUserId(User));
        }

        [HttpGet]
        public Order GetUnfinishedOrder()
        {
            return logic.GetUnfinishedOrder(UserManager.GetUserId(User));
        }

        [HttpGet]
        public IEnumerable<Order_Element> GetOrderElementsByID(string id)
        {
            return logic.GetOrderElementsByID(id);
        }

        [HttpPost]
        public void AddItemToCart(Order_Element order)
        {
            logic.AddItemToCart(order, UserManager.GetUserId(User));
        }

        [HttpDelete]
        public void DeleteCartItemByID (string id)
        {
            logic.DeleteCartItemByID(id);
        }

        [HttpPut]
        public void UpdateCart (IEnumerable<Order_Element> orders)
        {
            logic.UpdateCart(orders);
        }

        [HttpPut]
        public void SubmitOrder()
        {
            logic.SubmitOrder(UserManager.GetUserId(User));
        }
        #endregion

        #region Orders

        public List<object> GetUserOrders()
        {
            return logic.GetUserOrders(UserManager.GetUserId(User));
        }

        public List<object> GetUserOrderElements(string id)
        {
            return logic.GetUserOrderElements(id);
        }

        [HttpDelete]
        public void DeleteUserOrder(string id)
        {
            logic.DeleteUserOrder(id);
        }

        #endregion
    }
}