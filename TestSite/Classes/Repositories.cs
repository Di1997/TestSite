using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSite.Data;
using TestSite.Models;

namespace TestSite.Classes
{
    public class SimpleUserRepository : RepositoryMethods<Simple_User>
    {
        public SimpleUserRepository(ApplicationDbContext context) : base(context) { }

        public override IEnumerable<Simple_User> GetAll()
        {
            return context.Simple_User;
        }

        public override Simple_User GetSingle(string id)
        {
            return context.Simple_User.FirstOrDefault(o => o.ID == Guid.Parse(id));
        }

        public override void Update(Simple_User param)
        {
            Simple_User user = context.Simple_User.FirstOrDefault(o => o.ID == param.ID);

            user.Name = param.Name;
            user.Address = param.Address;
            user.Discount = param.Discount;
            user.Validated = param.Validated;
        }
    }

    public class OrderRepository : RepositoryMethods<Order>
    {
        public OrderRepository(ApplicationDbContext context) : base(context) { }

        public override IEnumerable<Order> GetAll()
        {
            return context.Order;
        }

        public override Order GetSingle(string id)
        {
            return context.Order.FirstOrDefault(o => o.ID == Guid.Parse(id));
        }

        public override void Update(Order param)
        {
            Order order = context.Order.FirstOrDefault(o => o.ID == param.ID);

            order.Order_Date = param.Order_Date;
            order.Shipment_Date = param.Shipment_Date;
            order.Order_Number = param.Order_Number;
            order.Status = param.Status;
        }
    }

    public class OrderElementRepository : RepositoryMethods<Order_Element>
    {
        public OrderElementRepository(ApplicationDbContext context) : base(context) { }

        public override IEnumerable<Order_Element> GetAll()
        {
            return context.Order_Element;
        }

        public override Order_Element GetSingle(string id)
        {
            return context.Order_Element.FirstOrDefault(o => o.ID == Guid.Parse(id));
        }

        public override void Update(Order_Element param)
        {
            Order_Element orderElement = context.Order_Element.FirstOrDefault(o => o.ID == param.ID);

            orderElement.Items_Count = param.Items_Count;
            orderElement.Item_Price = param.Item_Price;
        }
    }

    public class ProductRepository : RepositoryMethods<Product>
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public override IEnumerable<Product> GetAll()
        {
            return context.Product;
        }

        public override Product GetSingle(string id)
        {
            return context.Product.FirstOrDefault(o => o.ID == Guid.Parse(id));
        }

        public override void Update(Product param)
        {
            Product product = context.Product.FirstOrDefault(o => o.ID == param.ID);

            product.Name = param.Name;
            product.Price = param.Price;
            product.Category = param.Category;
        }
    }
}
