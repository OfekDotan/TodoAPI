using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI
{
    public class Todo
    {
         public int Id { get;  }
        public string Title{ get; private set; }
        public bool Completed{ get; private set; }
        public Todo(int id, string title,bool completed=false)
        {
            Id = id;
            Title = title;
            Completed = completed;
        }
        public void MarkAsDone()
        {
            Completed = true;
        }
        public void MarkAsNotDone()
        {
            Completed = false;
        }
        public void ChangeTitle(string title)
        {
            Title = title;
        }
    }
}
