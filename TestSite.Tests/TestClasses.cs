using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TestSite.Classes;
using TestSite.Data;
using TestSite.Models;
using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TestSite.Tests
{
    static class TestClass
    {
        public static IUnitOfWork CreateNewDB()
        {
            var SimpleUsers = new List<Simple_User>
            {
                new Simple_User
                {
                ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "Test Name",
                Code = "NoCode",
                Address = "Test Address",
                Discount = 10,
                Validated = true
                }
            };

            var products = new List<Product>
            {
                new Product
                {
                    ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    Code = "",
                    Name = "New Product",
                    Price = 100,
                    Category = "Test"
                },
                new Product
                {
                    ID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Code = "",
                    Name = "Test Product",
                    Price = 101,
                    Category = "Test2"
                },
                new Product
                {
                    ID = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Code = "",
                    Name = "Test Product02",
                    Price = 102,
                    Category = "Test02"
                }
            };

            var orders = new List<Order>
            {
                new Order
                {
                    ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    Customer_ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    Order_Date = DateTime.MinValue,
                    Shipment_Date = DateTime.MinValue,
                    Status = null
                },
                new Order
                {
                    ID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Customer_ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    Order_Date = DateTime.Now,
                    Shipment_Date = DateTime.MinValue,
                    Status = Statics.OrderStatus.New
                },
                new Order
                {
                    ID = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Customer_ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    Order_Date = DateTime.Now,
                    Shipment_Date = DateTime.MinValue,
                    Status = Statics.OrderStatus.Completed
                },
            };

            var order_Elements = new List<Order_Element>
            {
                new Order_Element
                {
                    ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    Order_ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    Item_ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    Items_Count = 5,
                    Item_Price = 100
                },
                new Order_Element
                {
                    ID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Order_ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    Item_ID = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Items_Count = 1,
                    Item_Price = 102
                }
            };

            var uow = new TestUnitOfWork(SimpleUsers, orders, order_Elements, products);

            return uow;
        }
    }


    public abstract class TestRepository<T> : IRepository<T>
        where T : class
    {
        public List<T> data;

        public TestRepository(List<T> data)
        {
            this.data = data;
        }

        public void Add(T param)
        {
            data.Add(param);
        }
        public IEnumerable<T> GetAll()
        {
            return data;
        }
        public void Remove(T param)
        {
            data.Remove(param);
        }

        public void RemoveRange(IEnumerable<T> objects)
        {
            data.RemoveAll(d => objects.Contains(d));
        }

        public abstract T GetSingle(string id);
        public abstract void Update(T param);
    }

    public class TestUnitOfWork : IUnitOfWork
    {
        public TestUnitOfWork(List<Simple_User> simpleUser, List<Order> order, List<Order_Element> orderElement, List<Product> product)
        {
            SimpleUser = new MockSU(simpleUser);
            Order = new MockO(order);
            OrderElement = new MockOE(orderElement);
            Product = new MockP(product);
        }

        public IRepository<Simple_User> SimpleUser { get; }

        public IRepository<Order> Order { get; }

        public IRepository<Order_Element> OrderElement { get; }

        public IRepository<Product> Product { get; }

        public void SaveChanges()
        {
        }
    }

    public class MockSU : TestRepository<Simple_User>
    {
        public MockSU(List<Simple_User> data) : base(data)
        {
        }

        public override Simple_User GetSingle(string id)
        {
            return data.FirstOrDefault(d => d.ID == Guid.Parse(id));
        }

        public override void Update(Simple_User param)
        {
            Simple_User user = data.FirstOrDefault(d => d.ID == param.ID);

            user.Name = param.Name;
            user.Address = param.Address;
            user.Discount = param.Discount;
            user.Validated = param.Validated;
        }
    }

    public class MockO : TestRepository<Order>
    {
        public MockO(List<Order> data) : base(data)
        {
        }

        public override Order GetSingle(string id)
        {
            return data.FirstOrDefault(d => d.ID == Guid.Parse(id));
        }

        public override void Update(Order param)
        {
            Order order = data.FirstOrDefault(d => d.ID == param.ID);

            order.Order_Date = param.Order_Date;
            order.Shipment_Date = param.Shipment_Date;
            order.Order_Number = param.Order_Number;
            order.Status = param.Status;
        }
    }

    public class MockOE : TestRepository<Order_Element>
    {
        public MockOE(List<Order_Element> data) : base(data)
        {
        }

        public override Order_Element GetSingle(string id)
        {
            return data.FirstOrDefault(d => d.ID == Guid.Parse(id));
        }

        public override void Update(Order_Element param)
        {
            Order_Element orderElement = data.FirstOrDefault(d => d.ID == param.ID);

            orderElement.Items_Count = param.Items_Count;
            orderElement.Item_Price = param.Item_Price;
        }
    }

    public class MockP : TestRepository<Product>
    {
        public MockP(List<Product> data) : base(data)
        {
        }

        public override Product GetSingle(string id)
        {
            return data.FirstOrDefault(d => d.ID == Guid.Parse(id));
        }

        public override void Update(Product param)
        {
            Product product = data.FirstOrDefault(d => d.ID == param.ID);

            product.Name = param.Name;
            product.Price = param.Price;
            product.Category = param.Category;
        }
    }
}
