using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3
{
    public interface ITodoRepository
    {
        void Add(Todo value);
        IEnumerable<Todo> List(int limit);
      Todo FindById(int id);
        void Update(int id, Todo value);
        void RemoveInstance(int id);
    }
}
