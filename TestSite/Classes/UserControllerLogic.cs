using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSite.Data;
using TestSite.Models;
using TestSite.Statics;

namespace TestSite.Classes
{
    public class UserControllerLogic
    {
        private IUnitOfWork repo;

        public UserControllerLogic(IUnitOfWork repo)
        {
            this.repo = repo;
        }

        #region Shop

        public IEnumerable<Product> GetProducts()
        {
            return repo.Product.GetAll();
        }

        public int GetUserDiscount(string id)
        {
            return repo.SimpleUser.GetSingle(id).Discount;
        }

        public Order GetUnfinishedOrder(string userid)
        {
            Order UnfinishedOrder = repo.Order.GetAll().FirstOrDefault(o => o.Customer_ID == Guid.Parse(userid) && o.Status == null);

            if (UnfinishedOrder == null)
            {
                UnfinishedOrder = CreateNewOrder(userid); //Users must have 1 blank order, which will work like a cart
            }

            return UnfinishedOrder;
        }

        public IEnumerable<Order_Element> GetOrderElementsByID(string orderid)
        {
            return repo.OrderElement.GetAll().Where(oe => oe.Order_ID == Guid.Parse(orderid));
        }

        public void AddItemToCart(Order_Element order, string userid)
        {
            Order_Element NewOrder = new Order_Element { Items_Count = order.Items_Count, Item_ID = order.Item_ID, Order_ID = order.Order_ID };

            var ExistingOrderElement = repo.OrderElement.GetAll().Where(o => o.Item_ID == NewOrder.Item_ID && o.Order_ID == NewOrder.Order_ID);

            if (ExistingOrderElement.Count() > 0)
            {
                ExistingOrderElement.First().Items_Count += NewOrder.Items_Count;
                repo.SaveChanges();
                return;
            }
            else
            {
                NewOrder.ID = Guid.NewGuid();
                NewOrder.Item_Price = Math.Ceiling((decimal)repo.Product.GetSingle(NewOrder.Item_ID.ToString()).Price / 100 * (100 - GetUserDiscount(userid)));

                repo.OrderElement.Add(NewOrder);
                repo.SaveChanges();
            }
        }

        public void DeleteCartItemByID(string id)
        {
            repo.OrderElement.Remove(repo.OrderElement.GetSingle(id));
        }

        public void UpdateCart(IEnumerable<Order_Element> orders)
        {
            foreach (var i in orders)
            {
                repo.OrderElement.GetAll().FirstOrDefault(o => o.ID == i.ID).Items_Count = i.Items_Count;
            }
            repo.SaveChanges();
        }

        public void SubmitOrder(string userid)
        {
            Order UnfinishedOrder = repo.Order.GetAll().FirstOrDefault(o => o.Customer_ID == Guid.Parse(userid) && o.Status == null);

            UnfinishedOrder.Status = OrderStatus.New;
            UnfinishedOrder.Order_Date = DateTime.Now;

            repo.SaveChanges();
        }

        Order CreateNewOrder(string userid)
        {
            var LastOrder = repo.Order.GetAll().Where(o => o.Customer_ID == Guid.Parse(userid)).OrderByDescending(o => o.Order_Number).FirstOrDefault();

            Order order = new Order
            {
                ID = Guid.NewGuid(),
                Customer_ID = Guid.Parse(userid),
                Order_Number = LastOrder == null ? 0 : LastOrder.Order_Number + 1
            };

            repo.Order.Add(order);
            repo.SaveChanges();

            return order;
        }
        #endregion

        #region Orders

        public List<object> GetUserOrders(string userid)
        {
            IEnumerable<Order> TempOrder = repo.Order.GetAll().Where(o => o.Customer_ID == Guid.Parse(userid) && o.Status != null).OrderByDescending(o => o.Order_Number);

            List<object> FilteredOrder = new List<object>();

            foreach (var i in TempOrder)
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
            List<object> FilteredOrderElements = new List<object>();

            IEnumerable<Order_Element> TempElements = repo.OrderElement.GetAll().Where(o => o.Order_ID == Guid.Parse(id));

            foreach (var i in TempElements)
            {
                FilteredOrderElements.Add(new
                {
                    ItemCategory = repo.Product.GetSingle(i.Item_ID.ToString()).Category,
                    ItemName = repo.Product.GetSingle(i.Item_ID.ToString()).Name,
                    i.Items_Count,
                    i.Item_Price
                });
            }

            return FilteredOrderElements;
        }

        public void DeleteUserOrder(string id)
        {
            repo.Order.Remove(repo.Order.GetSingle(id));
        }

        #endregion
    }
}
