using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alert.API.Repositories
{
    public interface IRepository<T> : IDisposable where T : class
    {
        void Add(T item);

        IEnumerable<T> GetAll();

        IQueryable<T> GetFiltered(string query);

        T GetSingle();

    };
}
