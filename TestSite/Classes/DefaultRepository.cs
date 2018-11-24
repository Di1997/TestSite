using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestSite.Classes;
using TestSite.Data;
using TestSite.Models;

namespace TestSite.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepository<Simple_User> SimpleUser { get; }
        public IRepository<Order> Order { get; }
        public IRepository<Order_Element> OrderElement { get; }
        public IRepository<Product> Product { get; }
        private readonly ApplicationDbContext Context;

        public UnitOfWork(ApplicationDbContext context) {
            SimpleUser = new SimpleUserRepository(context);
            Order = new OrderRepository(context);
            OrderElement = new OrderElementRepository(context);
            Product = new ProductRepository(context);
            Context = context;
        }

        public void SaveChanges() { Context.SaveChanges(); }
    }
}
