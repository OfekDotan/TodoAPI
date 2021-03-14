using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Reflection;

namespace TodoAPI
{
    public class FileTodoRepository : ITodoRepository
    {
        private static readonly string TodosPath;
        static FileTodoRepository()
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            TodosPath = directory+@"/todos.json";
                }
        public async Task AddAsync(Todo todo)
        {
            var todos = await ReadAllAsync();
            todos.Add(todo);
            await WriteAllAsync(todos);
        }

        public async Task<Todo> FindByIdAsync(int id)
        {
            var todos = await ReadAllAsync();
            return todos.SingleOrDefault(t => t.Id == id);
        }

        public async Task<int> GetNextIdentityAsync()
        {
            var todos =await ReadAllAsync();
            if (todos.Count is 0)
                return 1;
            return todos.Select(t => t.Id).Max() + 1;
        }

        public async Task<IEnumerable<Todo>> ListAsync(int limit)
        {
            if (limit < 0)
                throw new ArgumentOutOfRangeException();

            return (await ReadAllAsync()).Take(limit);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var todos =await ReadAllAsync();
            if (FindByIdAsync(id) is null)
                return false;
           await WriteAllAsync(todos.Where(todo => todo.Id != id));
            return true;
        }

        public async Task<IEnumerable<Todo>> SearchAsync(string query)
        {
            var todos = await ReadAllAsync();

            return todos.Where(todo => todo.Title.Contains(query, StringComparison.OrdinalIgnoreCase));
        }

        public async Task UpdateAsync(Todo todo)
        {
                var removed = await RemoveAsync(todo.Id);
                if (removed)
                   await AddAsync(todo);
           
        }
        private async Task<ICollection<Todo>> ReadAllAsync()
        {
            string todos;
            try
            {
                todos=await File.ReadAllTextAsync(TodosPath);
            }
            catch (FileNotFoundException)
            {
                return new HashSet<Todo>();
            }
            return JsonSerializer.Deserialize<ICollection<Todo>>(todos);
        }
        private async Task WriteAllAsync(IEnumerable<Todo> todos)
        {
           await File.WriteAllTextAsync(TodosPath, JsonSerializer.Serialize(todos));
        }
    }
}
