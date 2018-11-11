using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            Simple_User simple_User = dBParams.GetSimple_User(Guid.Parse(id));
            dBParams.Context.Remove(simple_User);
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
    }
}