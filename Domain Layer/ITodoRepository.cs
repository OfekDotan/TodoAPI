using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain_Layer
{
    public interface ITodoRepository
    {
        Task AddAsync(Todo todo);

        Task<Todo> FindByIdAsync(Guid id);

        Task<IEnumerable<Todo>> ListAsync(int limit);

        Task<bool> RemoveAsync(Guid id);

        Task<IEnumerable<Todo>> SearchAsync(string query);

        Task UpdateAsync(Todo todo);
        Task AddTag(Tag tag);
        Task<IEnumerable<Todo>> SearchTagAsync(string tag);
        Task<bool> RemoveTag(Guid id, string tag);
    }
}