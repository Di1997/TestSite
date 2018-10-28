using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSite.Data;

namespace TestSite.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            //using (var context = new ApplicationDbContext(
            //    serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            //{
            //    // Look for any movies.
            //    if (context.UserRoles.Any())
            //    {
            //        return;   // DB has been seeded
            //    }

            //    //context.UserRoles.Add
            //    //    (
            //    //    new Use
            //    //    {
            //    //        Title = "When Harry Met Sally",
            //    //        ReleaseDate = DateTime.Parse("1989-2-12"),
            //    //        Genre = "Romantic Comedy",
            //    //        Price = 7.99M
            //    //    }
            //    //);
            //    context.SaveChanges();
            //}
        }
    }
}
