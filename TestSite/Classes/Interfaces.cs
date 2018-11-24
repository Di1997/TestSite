using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSite.Models;

namespace TestSite.Classes
{
    public interface IRepository<T>
        where T : class
    {
        void Add(T param);
        void Remove(T param);
        IEnumerable<T> GetAll();
        void Update(T param);
        T GetSingle(string id);
        void RemoveRange(IEnumerable<T> objects);
    }

    public interface IUnitOfWork
    {
        IRepository<Simple_User> SimpleUser { get; }
        IRepository<Order> Order { get; }
        IRepository<Order_Element> OrderElement { get; }
        IRepository<Product> Product { get; }

        void SaveChanges();
    }
}
