using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSite.Data;
using TestSite.Models;
using TestSite.Statics;

namespace TestSite.Classes
{
    public class AdminControllerLogic
    {
        private IUnitOfWork repo;

        public AdminControllerLogic(IUnitOfWork repo)
        {
            this.repo = repo;
        }

        #region Users

        public void UpdateUser(Simple_User user)
        {
            repo.SimpleUser.Update(user);

            repo.SaveChanges();
        }

        public void DeleteUser(string id)
        {
            var UserOrders = repo.Order.GetAll().Where(o => o.Customer_ID == Guid.Parse(id));
            var AllOrderElements = repo.OrderElement.GetAll().Where(o => UserOrders.Select(uo => uo.ID).Contains(o.Order_ID));

            repo.OrderElement.RemoveRange(AllOrderElements);
            repo.Order.RemoveRange(UserOrders);
            repo.SimpleUser.Remove(repo.SimpleUser.GetSingle(id));

            repo.SaveChanges();
        }

        public IEnumerable<Simple_User> GetUsers()
        {
            return repo.SimpleUser.GetAll();
        }

        #endregion

        #region Products
        public Product GenerateProductInfo()
        {
            return new Product
            {
                ID = Guid.NewGuid(),
                Code = GenerateProductCode()
            };
        }

        public void AddProduct(Product product)
        {
            repo.Product.Add(new Product
            {
                ID = product.ID,
                Category = product.Category,
                Code = product.Code,
                Name = product.Name,
                Price = product.Price
            });

            repo.SaveChanges();
        }

        public void DeleteProduct(string id)
        {
            repo.Product.Remove(repo.Product.GetSingle(id));
            repo.SaveChanges();
        }

        public void UpdateProduct(Product p)
        {
            repo.Product.Update(p);

            repo.SaveChanges();
        }
        #endregion

        #region Orders

        public List<object> GetOrders()
        {
            var Orders = repo.Order.GetAll().Where(o => o.Status != null).OrderByDescending(o => o.Order_Date);

            List<object> FilteredObjects = new List<object>();

            var test = Orders.First();

            foreach (var i in Orders)
            {
                FilteredObjects.Add(new
                {
                    i.ID,
                    i.Customer_ID,
                    Customer_name = repo.SimpleUser.GetSingle(i.Customer_ID.ToString()).Name,
                    Order_Date = i.Order_Date.ToString("dd/MM/yyyy HH:mm:ss"),
                    Shipment_Date = i.Shipment_Date == DateTime.MinValue ? null : i.Shipment_Date.ToString("dd/MM/yyyy HH:mm:ss"),
                    i.Order_Number,
                    i.Status
                });
            };

            return FilteredObjects;
        }

        public List<object> GetOrderElements(string id)
        {
            var Orders = repo.OrderElement.GetAll().Where(o => o.Order_ID == Guid.Parse(id));

            List<object> FilteredObjects = new List<object>();

            var test = Orders.First();

            foreach (var i in Orders)
            {
                FilteredObjects.Add(new
                {
                    i.ID,
                    i.Item_ID,
                    ItemName = repo.Product.GetSingle(i.Item_ID.ToString()).Name,
                    category = repo.Product.GetSingle(i.Item_ID.ToString()).Category,
                    i.Items_Count,
                    i.Item_Price,
                    i.Order_ID
                });
            };

            return FilteredObjects;
        }

        public void UpdateOrderStatus(string id)
        {
            var order = repo.Order.GetSingle(id);

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

            repo.SaveChanges();
        }

        #endregion

        #region Rest of the functions

        string GenerateProductCode()
        {
            Random random = new Random();

            char[] chars = { (char)('A' + random.Next(0, 26)), (char)('A' + random.Next(0, 26)) };
            string letters = new string(chars);
            string code = $"{random.Next(10, 99)}-{random.Next(1000, 9999)}-{letters}{random.Next(10, 99)}";
            return code;
        }

        #endregion
    }
}
