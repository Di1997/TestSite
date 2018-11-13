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

        public UserController(UserManager<IdentityUser> UserManager, ApplicationDbContext context)
        {
            dBParams = new DBParams(UserManager, context);
        }

        #region Shop

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

        [HttpGet]
        public Order GetUnfinishedOrder()
        {
            dBParams.User = User;

            Order UnfinishedOrder = dBParams.Context.Order.FirstOrDefault(o => o.Customer_ID == dBParams.SimpleUser.ID && o.Status == null);

            if(UnfinishedOrder == null)
            {
                UnfinishedOrder = CreateNewOrder(); //Users must have 1 blank order, which will work like a cart
            }

            return UnfinishedOrder;
        }

        [HttpGet]
        public JsonResult GetOrderElementsByID(string id)
        {
            return new JsonResult(dBParams.Context.Order_Element.Where(oe => oe.Order_ID == Guid.Parse(id)));
        }

        [HttpPost]
        public void AddItemToCart(Order_Element order)
        {
            dBParams.User = User;

            Order_Element NewOrder = new Order_Element { Items_Count = order.Items_Count, Item_ID = order.Item_ID, Order_ID = order.Order_ID };

            var ExistingOrderElement = dBParams.Context.Order_Element.Where(o => o.Item_ID == NewOrder.Item_ID && o.Order_ID == NewOrder.Order_ID);

            if (ExistingOrderElement.Count() > 0)
            {
                ExistingOrderElement.First().Items_Count += NewOrder.Items_Count;
                dBParams.Context.SaveChanges();
                return;
            }
            else
            {
                NewOrder.ID = Guid.NewGuid();
                NewOrder.Item_Price = Math.Ceiling((decimal)dBParams.GetProduct(NewOrder.Item_ID).Price / 100 * (100 - dBParams.SimpleUser.Discount));

                dBParams.Context.Add(NewOrder);
                dBParams.Context.SaveChanges();
            }
        }

        [HttpDelete]
        public void DeleteCartItemByID (string id)
        {
            var item = dBParams.Context.Order_Element.FirstOrDefault(o => o.ID == Guid.Parse(id));

            dBParams.Context.Order_Element.Remove(item);
            dBParams.Context.SaveChanges();
        }

        [HttpPut]
        public void UpdateCart (IEnumerable<Order_Element> orders)
        {
            foreach(var i in orders)
            {
                dBParams.Context.Order_Element.FirstOrDefault(o => o.ID == i.ID).Items_Count = i.Items_Count;
            }
            dBParams.Context.SaveChanges();
        }

        [HttpPut]
        public void SubmitOrder()
        {
            dBParams.User = User;
            Order UnfinishedOrder = dBParams.Context.Order.FirstOrDefault(o => o.Customer_ID == dBParams.SimpleUser.ID && o.Status == null);

            UnfinishedOrder.Status = OrderStatus.New;
            UnfinishedOrder.Order_Date = DateTime.Now;

            dBParams.Context.SaveChanges();
        }

        Order CreateNewOrder()
        {
            dBParams.User = User;

            var LastOrder = dBParams.UserOrders.OrderByDescending(o => o.Order_Number).FirstOrDefault();

            Order order = new Order
            {
                ID = Guid.NewGuid(),
                Customer_ID = dBParams.SimpleUser.ID,
                Order_Number = LastOrder == null ? 0 : LastOrder.Order_Number + 1
        };

            dBParams.Context.Add(order);
            dBParams.Context.SaveChanges();

            return order;
        }
        #endregion

        #region Orders

        public List<object> GetUserOrders()
        {
            dBParams.User = User;

            IEnumerable<Order> TempOrder = dBParams.Context.Order.Where(o => o.Customer_ID == dBParams.SimpleUser.ID && o.Status != null).OrderByDescending(o => o.Order_Number);

            List<object> FilteredOrder = new List<object>();

            foreach(var i in TempOrder)
            {
                FilteredOrder.Add(new
                {
                    i.ID,
                    Order_Date = i.Order_Date.ToString("dd/MM/yyyy HH:mm:ss"),
                    Shipment_Date = i.Shipment_Date == DateTime.MinValue ? "Is not shipped yet" : i.Shipment_Date.ToString("dd/MM/yyyy HH:mm:ss"),
                    i.Order_Number,
                    i.Status
                });
            }

            return FilteredOrder;
        }

        public List<object> GetUserOrderElements(string id)
        {
            dBParams.User = User;
            List<object> FilteredOrderElements = new List<object>();

            IEnumerable<Order_Element> TempElements = dBParams.Context.Order_Element.Where(o => o.Order_ID == Guid.Parse(id));

            foreach (var i in TempElements)
            {
                FilteredOrderElements.Add(new
                {
                    ItemCategory = dBParams.Context.Product.FirstOrDefault(p => p.ID == i.Item_ID).Category,
                    ItemName = dBParams.Context.Product.FirstOrDefault(p => p.ID == i.Item_ID).Name,
                    i.Items_Count,
                    i.Item_Price
                });
            }

            return FilteredOrderElements;
        }

        [HttpDelete]
        public void DeleteUserOrder(string id)
        {
            var Order = dBParams.Context.Order.FirstOrDefault(o => o.ID == Guid.Parse(id));
            dBParams.Context.Order.Remove(Order);

            dBParams.Context.SaveChanges();
        }

        #endregion
    }
}