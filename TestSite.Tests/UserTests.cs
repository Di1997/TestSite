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
    public class UserTests
    {
        [Fact]
        public void ShouldReturnAllProducts()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new UserControllerLogic(DB);

            var AllUsers = Logic.GetProducts();

            Assert.Equal(DB.Product.GetAll().Count(), AllUsers.Count());
        }

        [Fact]
        public void ShouldReturnUserDiscount()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new UserControllerLogic(DB);
            var User = DB.SimpleUser.GetAll().First();

            var Discount = Logic.GetUserDiscount(User.ID.ToString());

            Assert.Equal(User.Discount, Discount);
        }

        [Fact]
        public void ShouldReturnUserUnfinishedOrder()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new UserControllerLogic(DB);
            var User = DB.SimpleUser.GetAll().First();

            var Order = Logic.GetUnfinishedOrder(User.ID.ToString());

            Assert.Equal("00000000-0000-0000-0000-000000000000", Order.ID.ToString());
        }

        [Fact]
        public void ShouldReturnAllOrderElements()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new UserControllerLogic(DB);
            var OrderElements = DB.OrderElement.GetAll().Where(oe => oe.Order_ID == Guid.Parse("00000000-0000-0000-0000-000000000000"));

            var ReturnedOE = Logic.GetUserOrderElements("00000000-0000-0000-0000-000000000000");

            Assert.Equal(OrderElements.Count(), ReturnedOE.Count());
        }

        [Fact]
        public void ShouldAddProductToCart()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new UserControllerLogic(DB);
            var OrderElementToAdd = new Order_Element
            {
                Order_ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Item_ID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Items_Count = 1
            };

            Logic.AddItemToCart(OrderElementToAdd, "00000000-0000-0000-0000-000000000000");

            Assert.NotNull(DB.OrderElement.GetAll().First(oe => oe.Item_ID == OrderElementToAdd.Item_ID));
        }

        [Fact]
        public void ShouldIncreaseItemsAmountIfItemExists()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new UserControllerLogic(DB);
            var OrderElementToAdd = new Order_Element
            {
                Order_ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Item_ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Items_Count = 13
            };
            var CurrentOrderElementCount = DB.OrderElement.GetSingle("00000000-0000-0000-0000-000000000000").Items_Count;

            Logic.AddItemToCart(OrderElementToAdd, "00000000-0000-0000-0000-000000000000");

            Assert.True(DB.OrderElement.GetSingle("00000000-0000-0000-0000-000000000000").Items_Count == CurrentOrderElementCount + OrderElementToAdd.Items_Count);
        }

        [Fact]
        public void ShouldRemoveCartItem()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new UserControllerLogic(DB);
            var CartItemID = "00000000-0000-0000-0000-000000000000";

            Logic.DeleteCartItemByID(CartItemID);

            Assert.Null(DB.OrderElement.GetSingle(CartItemID));
        }

        [Fact]
        public void ShouldUpdateMultipleOrderElements()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new UserControllerLogic(DB);
            var OrderChanges = new List<Order_Element>
            {
                new Order_Element
                {
                    ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    Items_Count = 30
                },
                new Order_Element
                {
                    ID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Items_Count = 30
                }
            };

            Logic.UpdateCart(OrderChanges);

            Assert.True(DB.OrderElement.GetSingle(OrderChanges[0].ID.ToString()).Items_Count == 30);
            Assert.True(DB.OrderElement.GetSingle(OrderChanges[1].ID.ToString()).Items_Count == 30);
        }

        [Fact]
        public void ShouldFinishUnfinishedOrderAndSubmit()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new UserControllerLogic(DB);
            var UnfinishedOrder = DB.Order.GetAll().First(O => O.Status == null && O.Customer_ID == Guid.Parse("00000000-0000-0000-0000-000000000000"));

            Logic.SubmitOrder("00000000-0000-0000-0000-000000000000");

            Assert.Null(DB.Order.GetAll().FirstOrDefault(O => O.Customer_ID == Guid.Parse("00000000-0000-0000-0000-000000000000") && O.Status == null));
            Assert.True(UnfinishedOrder.Order_Date != DateTime.MinValue && UnfinishedOrder.Status == OrderStatus.New);
        }

        [Fact]
        public void ShouldReturnUserOrders()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new UserControllerLogic(DB);
            var UserID = "00000000-0000-0000-0000-000000000000";

            var output = Logic.GetUserOrders(UserID);

            Assert.Equal(DB.Order.GetAll().Where(o => o.Customer_ID == Guid.Parse(UserID) && o.Status != null).Count(), output.Count());
        }

        [Fact]
        public void ShouldGetUserOrderElements()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new UserControllerLogic(DB);
            var Order = DB.Order.GetSingle("00000000-0000-0000-0000-000000000000");

            var output = Logic.GetUserOrderElements(Order.ID.ToString());

            Assert.Equal(DB.OrderElement.GetAll().Where(OE => OE.Order_ID == Order.ID).Count(), output.Count());
        }

        [Fact]
        public void ShouldDeleteUserOrder()
        {
            var DB = TestClass.CreateNewDB();
            var Logic = new UserControllerLogic(DB);
            var OrderToDelete = DB.Order.GetSingle("00000000-0000-0000-0000-000000000001");

            Logic.DeleteUserOrder(OrderToDelete.ID.ToString());

            Assert.Null(DB.Order.GetSingle(OrderToDelete.ID.ToString()));
        }
    }
}
