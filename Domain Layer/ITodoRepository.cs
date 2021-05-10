using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain_Layer
{
    public interface ITodoRepository
    {
        Task AddAsync(Todo todo);

        Task<Todo> FindByIdAsync(Guid id);

        Task<IReadOnlyList<Todo>> ListAsync(int limit);

        Task<bool> RemoveAsync(Guid id);

        Task<IReadOnlyList<Todo>> SearchAsync(string query);

        Task UpdateAsync(Todo todo);
        Task AddTagAsync(Tag tag);
        Task<IReadOnlyList<Todo>> SearchByTagAsync(string tag);
        Task<bool> RemoveTagAsync(Guid id, string tag);
    }
}