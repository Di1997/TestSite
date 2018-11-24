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
using Newtonsoft.Json;
using TestSite.Statics;

namespace TestSite.Tests
{
    public class AdminTests
    {
        [Fact]
        public void ShouldUpdateUserWithoutChangingCode()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new AdminControllerLogic(DB);
            Simple_User user = new Simple_User
            {
                ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "123",
                Code = "Some Code",
                Address = "NewAddress",
                Discount = 0,
                Validated = false
            };
            Simple_User ShouldBeUser = new Simple_User
            {
                ID = user.ID,
                Name = user.Name,
                Code = DB.SimpleUser.GetSingle(user.ID.ToString()).Code,
                Address = user.Address,
                Discount = user.Discount,
                Validated = user.Validated
            };

            Logic.UpdateUser(user);

            string ShouldBe = JsonConvert.SerializeObject(ShouldBeUser);
            string changed = JsonConvert.SerializeObject(DB.SimpleUser.GetSingle(user.ID.ToString()));
            Assert.Equal(ShouldBe, changed);
        }

        [Fact]
        public void ShouldDeleteTestUserAndItsOrders()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new AdminControllerLogic(DB);

            Logic.DeleteUser("00000000-0000-0000-0000-000000000000");

            Assert.Null(DB.SimpleUser.GetSingle("00000000-0000-0000-0000-000000000000"));
            Assert.Null(DB.Order.GetSingle("00000000-0000-0000-0000-000000000000"));
            Assert.Null(DB.Order.GetSingle("00000000-0000-0000-0000-000000000001"));
            Assert.Null(DB.Order.GetSingle("00000000-0000-0000-0000-000000000002"));
            Assert.Null(DB.OrderElement.GetSingle("00000000-0000-0000-0000-000000000000"));
        }

        [Fact]
        public void ShouldReturnAllUsers()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new AdminControllerLogic(DB);

            var AllUsers = Logic.GetUsers();

            Assert.Equal(DB.SimpleUser.GetAll().Count(), AllUsers.Count());
        }

        [Fact]
        public void ShouldGenerateNewProductInfo()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new AdminControllerLogic(DB);

            var NewInfo = Logic.GenerateProductInfo();

            Assert.NotNull(NewInfo);
            Assert.Matches(@"\d{2}-\d{4}-\w\w\d\d", NewInfo.Code);
        }

        [Fact]
        public void ShouldCreateProduct()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new AdminControllerLogic(DB);
            var NewProduct = new Product
            {
                ID = Guid.NewGuid(),
                Code = "00-1111-AA22",
                Name = "New Name",
                Price = 15,
                Category = "Test"
            };

            Logic.AddProduct(NewProduct);

            Assert.NotNull(DB.Product.GetSingle(NewProduct.ID.ToString()));
        }

        [Fact]
        public void ShouldDeleteProduct()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new AdminControllerLogic(DB);
            var ProductToDelete = DB.Product.GetAll().First();

            Logic.DeleteProduct(ProductToDelete.ID.ToString());

            Assert.Null(DB.Product.GetSingle(ProductToDelete.ID.ToString()));
        }

        [Fact]
        public void ShouldUpdateProductWithoutChangingCode()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new AdminControllerLogic(DB);
            var NewProductInfo = new Product
            {
                ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Code = "Asd",
                Name = "Old Product",
                Price = 150,
                Category = "New category"
            };
            var ShouldBeProduct = new Product
            {
                ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Code = DB.Product.GetSingle("00000000-0000-0000-0000-000000000000").Code,
                Name = "Old Product",
                Price = 150,
                Category = "New category"
            };

            Logic.UpdateProduct(NewProductInfo);

            var ShouldBe = JsonConvert.SerializeObject(ShouldBeProduct);
            var UpdatedProduct = JsonConvert.SerializeObject(DB.Product.GetSingle("00000000-0000-0000-0000-000000000000"));
            Assert.Equal(ShouldBe, UpdatedProduct);
        }

        [Fact]
        public void ShouldGiveAllOrdersWithStatus()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new AdminControllerLogic(DB);

            var AllOrders = Logic.GetOrders();

            Assert.Equal(DB.Order.GetAll().Where(o => o.Status != null).Count(), AllOrders.Count());
        }

        [Fact]
        public void ShouldReturnOrderElements()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new AdminControllerLogic(DB);
            var OrderWithElements = DB.Order.GetSingle("00000000-0000-0000-0000-000000000000");

            var AllOE = Logic.GetOrderElements(OrderWithElements.ID.ToString());

            Assert.Equal(DB.OrderElement.GetAll().Where(o => o.Order_ID == OrderWithElements.ID).Count(), AllOE.Count());
        }

        [Fact]
        public void ShouldNotUpdateUnfinishedOrder()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new AdminControllerLogic(DB);
            var UnfinishedOrder = DB.Order.GetAll().Where(o => o.Status == null).First();

            Logic.UpdateOrderStatus(UnfinishedOrder.ID.ToString());

            Assert.Null(UnfinishedOrder.Status);
        }

        [Fact]
        public void ShouldNotUpdateFinishedOrder()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new AdminControllerLogic(DB);
            var FinishedOrder = DB.Order.GetAll().Where(o => o.Status == OrderStatus.Completed).First();

            Logic.UpdateOrderStatus(FinishedOrder.ID.ToString());

            Assert.Equal(FinishedOrder.Status, OrderStatus.Completed);
        }

        [Fact]
        public void ShouldProgressStatus()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new AdminControllerLogic(DB);
            var NewOrder = DB.Order.GetAll().First(o => o.Status == OrderStatus.New);

            Logic.UpdateOrderStatus(NewOrder.ID.ToString());
            var Progress = NewOrder.Status;

            Logic.UpdateOrderStatus(NewOrder.ID.ToString());
            var Finished = NewOrder.Status;

            Assert.Equal(OrderStatus.InProgress, Progress);
            Assert.Equal(OrderStatus.Completed, Finished);
        }
    }
}
