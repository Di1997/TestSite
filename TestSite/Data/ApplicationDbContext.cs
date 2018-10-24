using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestSite.Models;

namespace TestSite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Simple_User> Simple_User { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Order_Element> Order_Element { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}
