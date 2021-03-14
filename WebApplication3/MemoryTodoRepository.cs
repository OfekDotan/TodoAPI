/**
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI
{
    public class MemoryTodoRepository : ITodoRepository
    {
        private static readonly HashSet<Todo> todos=new HashSet<Todo>();
        private static int nextId = 1;
        public void 
( Todo todo)
        {
            int id = todo.Id;
            if (FindById(id) is null) return;
            Remove(id);
            todos.Add(todo);
        }
        public int GetNextIdentity()
        {
            return nextId++;
        }
        public void Add(Todo todo)
        {

            todos.Add(todo);
        }

        public Todo FindById(int id)
        {
            return todos.SingleOrDefault(t => t.Id == id);
        }

        public IEnumerable<Todo> List(int limit)
        {
           
            if (limit < 0)
                throw new ArgumentOutOfRangeException();

             return todos.Take(limit);
            
        }

        public bool Remove(int id)
        {
            if (FindById(id) is null) 
                return false;
            todos.RemoveWhere(todo => todo.Id == id);
            return true;    
        }

        public IEnumerable<Todo> Search(string query)
        {
            return todos.Where(t => t.Title.Contains(query)).ToList();
        }

       
    }
}
*/