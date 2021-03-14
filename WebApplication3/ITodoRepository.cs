using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI
{
    public interface ITodoRepository {
        Task<int> GetNextIdentityAsync();
        Task AddAsync(Todo todo);
        Task<IEnumerable<Todo>> ListAsync(int limit);
        Task<Todo> FindByIdAsync(int id);
        Task<bool> RemoveAsync(int id);
        Task<IEnumerable<Todo>> SearchAsync(string query);
        Task UpdateAsync(Todo todo);

    }
}
