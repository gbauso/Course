using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Database.Query
{
    public interface IReadDatabase<T> : IDisposable
    {
        Task AddItem(T item);
        Task RemoveItem(string id);

        Task<T> GetItem(string id);
        Task<IEnumerable<T>> GetItems(string[] fields);
    }
}
