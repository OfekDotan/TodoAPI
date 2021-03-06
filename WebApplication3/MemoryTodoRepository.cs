using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3
{
    public class MemoryTodoRepository : ITodoRepository
    {
        private static readonly HashSet<Todo> todos=new HashSet<Todo>();
        private static int nextId = 1;
        public void Add(Todo value)
        {
            value.Id = nextId++;
            value.Completed = false;
            todos.Add(value);
        }

        public void Update(int id, Todo value)
        {
            if (FindById(id) is null) return;
            RemoveInstance(id);
            value.Id = id;
            todos.Add(value);
            
        }

        public Todo FindById(int id)
        {
            foreach (Todo t in todos)
                if (t.Id == id) return t;
            return null;
        }

        public IEnumerable<Todo> List(int limit)
        {
            if (limit <= 0) return null;
          
        
            IEnumerable<Todo> returnValue = todos.Take(limit);
            
                
            return returnValue;
        }

        public void RemoveInstance(int id)
        {
            todos.RemoveWhere(t => t.Id == id);
            
        }
    }
}
