using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace TodoAPI
{
    public class FileTodoRepository : ITodoRepository
    {
       private const string Path = @"todos.json";
        private static int nextId = 1;
        public void Add(Todo todo)

        {
            var todos = ReadAll();
            todos.Add(todo);
            WriteAll(todos);
        }

        public Todo FindById(int id)
        {
            var todos = ReadAll();
            return todos.SingleOrDefault(t => t.Id == id);
        }

        public int GetNextIdentity()
        {
            var todos = ReadAll();
            if (todos.Count == 0)
                return 1;
            return todos.Select(t => t.Id).Max() + 1;
        }

        public IEnumerable<Todo> List(int limit)
        {
            if (limit < 0)
                throw new ArgumentOutOfRangeException();
           
            var todos = ReadAll();
            return todos.Take(limit);
        }

        public bool Remove(int id)
        {
             var todos = ReadAll();
            if (FindById(id) is null)
                return false;
            WriteAll(todos.Where(todo => todo.Id != id).ToList());
            return true;
        }

        public IEnumerable<Todo> Search(string query)
        {
            var todos = ReadAll();

            return todos.Where(todo => todo.Title.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public void Update(Todo todo)
        {
            int id = todo.Id;
            if (FindById(id) is null) 
                return;
            Remove(id);
            Add(todo);
        }
        private ICollection<Todo> ReadAll()
        {
            string todos;
            try
            {
                 todos = File.ReadAllText(Path);
            }
            catch (FileNotFoundException)
            {
                return new HashSet<Todo>();
            }
            return JsonSerializer.Deserialize<ICollection<Todo>>(todos);
        }
         private void WriteAll(ICollection<Todo> todos)
        {
            
            File.WriteAllText(Path, JsonSerializer.Serialize(todos));
        }
    }
}
