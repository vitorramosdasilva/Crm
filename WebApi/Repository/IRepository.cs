using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        long Add(T item);
        void Remove(long id);
        void Update(T item);
        T FindByID(long id);
        IEnumerable<T> FindAll();
    }
}
