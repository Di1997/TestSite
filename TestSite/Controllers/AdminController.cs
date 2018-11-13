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
        private DBParams dBParams;

        public AdminController(UserManager<IdentityUser> UserManager, ApplicationDbContext context)
        {
            dBParams = new DBParams(UserManager, context);
        }

        #region Users

        [HttpPut]
        public void UpdateUser(Simple_User user)
        {
            Simple_User simple_User = dBParams.GetSimple_User(user.ID);

            simple_User.Name = user.Name;
            simple_User.Address = user.Address;
            simple_User.Discount = user.Discount;
            simple_User.Validated = user.Validated;

            dBParams.Context.SaveChanges();
        }

        [HttpDelete]
        public void DeleteUser(string id)
        {
            var UserOrders = dBParams.Context.Order.Where(o => o.Customer_ID == Guid.Parse(id));
            var AllOrderElements = dBParams.Context.Order_Element.Where(o => UserOrders.Select(uo => uo.ID).Contains(o.Order_ID));

            dBParams.Context.RemoveRange(AllOrderElements);
            dBParams.Context.RemoveRange(UserOrders);
            dBParams.Context.Remove(dBParams.GetSimple_User(Guid.Parse(id)));

            dBParams.Context.SaveChanges();
            dBParams.UserManager.DeleteAsync(dBParams.GetIdentityUser(id)).Wait();
        }

        [HttpGet]
        public JsonResult GetUsers()
        {
            return new JsonResult(dBParams.Context.Simple_User);
        }

        #endregion

        #region Products
        [HttpGet]
        public Product GenerateProductInfo()
        {
            return new Product
            {
                ID = Guid.NewGuid(),
                Code = dBParams.GenerateProductCode()
            };
        }

        [HttpPost]
        public void AddProduct(Product product)
        {
            dBParams.Context.Product.Add(new Product {
                ID = product.ID,
                Category = product.Category,
                Code = product.Code,
                Name = product.Name,
                Price = product.Price });

            dBParams.Context.SaveChanges();
        }

        [HttpDelete]
        public void DeleteProduct(Guid id)
        {
            Product product = dBParams.GetProduct(id);
            dBParams.Context.Remove(product);
            dBParams.Context.SaveChanges();
        }

        [HttpPut]
        public void UpdateProduct(Product p)
        {
            Product product = dBParams.GetProduct(p.ID);

            product.Name = p.Name;
            product.Price = p.Price;
            product.Category = p.Category;

            dBParams.Context.SaveChanges();
        }
        #endregion

        #region Orders

        [HttpGet]
        public List<object> GetOrders()
        {
            var Orders = dBParams.Context.Order.Where(o => o.Status != null).OrderByDescending(o => o.Order_Date);

            List<object> FilteredObjects = new List<object>();

            var test = Orders.First();

            foreach(var i in Orders)
            {
                FilteredObjects.Add(new
                {
                    i.ID,
                    i.Customer_ID,
                    Customer_name = dBParams.Context.Simple_User.FirstOrDefault(u => u.ID == i.Customer_ID).Name,
                    Order_Date = i.Order_Date.ToString("dd/MM/yyyy HH:mm:ss"),
                    Shipment_Date = i.Shipment_Date == DateTime.MinValue ? null : i.Shipment_Date.ToString("dd/MM/yyyy HH:mm:ss"),
                    i.Order_Number,
                    i.Status
                });
            };

            return FilteredObjects;
        }

        [HttpGet]
        public List<object> GetOrderElements(string id)
        {
            var Orders = dBParams.Context.Order_Element.Where(o => o.Order_ID == Guid.Parse(id));

            List<object> FilteredObjects = new List<object>();

            var test = Orders.First();

            foreach (var i in Orders)
            {
                FilteredObjects.Add(new
                {
                    i.ID,
                    i.Item_ID,
                    ItemName = dBParams.Context.Product.FirstOrDefault(p => p.ID == i.Item_ID).Name,
                    category = dBParams.Context.Product.FirstOrDefault(p => p.ID == i.Item_ID).Category,
                    i.Items_Count,
                    i.Item_Price,
                    i.Order_ID
                });
            };

            return FilteredObjects;
        }

        [HttpPut]
        public void UpdateOrderStatus(string id)
        {
            var order = dBParams.Context.Order.FirstOrDefault(o => o.ID == Guid.Parse(id));
            
            switch (order.Status)
            {
                case OrderStatus.New:
                    order.Status = OrderStatus.InProgress;
                    break;

                case OrderStatus.InProgress:
                    order.Shipment_Date = DateTime.Now;
                    order.Status = OrderStatus.Completed;
                    break;
            }

            dBParams.Context.SaveChanges();
        }

        #endregion
    }
}