using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoAPI
{
    public interface ITodoRepository
    {
        Task AddAsync(Todo todo);

        Task<Todo> FindByIdAsync(int id);

        Task<int> GetNextIdentityAsync();

        Task<IEnumerable<Todo>> ListAsync(int limit);

        Task<bool> RemoveAsync(int id);

        Task<IEnumerable<Todo>> SearchAsync(string query);

        Task UpdateAsync(Todo todo);
    }
}