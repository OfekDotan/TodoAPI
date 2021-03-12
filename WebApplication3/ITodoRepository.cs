using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI
{
    public interface ITodoRepository {
        int GetNextIdentity();
        void Add(Todo todo);
        IEnumerable<Todo> List(int limit);
        Todo FindById(int id);
        bool Remove(int id);
        IEnumerable<Todo> Search(string query);
    }
}
