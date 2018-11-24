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

        public virtual DbSet<Simple_User> Simple_User { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Order_Element> Order_Element { get; set; }
        public virtual DbSet<Product> Product { get; set; }
    }
}
