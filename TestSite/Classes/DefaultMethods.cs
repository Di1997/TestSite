using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSite.Data;

namespace TestSite.Classes
{
    public abstract class RepositoryMethods<T> : IRepository<T>
        where T : class
    {
        protected ApplicationDbContext context;

        public RepositoryMethods(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(T param) {
            context.Add(param);
        }

        public void Remove(T param) {
            context.Remove(param);
        }

        public void RemoveRange(IEnumerable<T> objects)
        {
            context.RemoveRange(objects);
        }

        public abstract IEnumerable<T> GetAll();
        public abstract void Update(T param);
        public abstract T GetSingle(string id);
    }
}
