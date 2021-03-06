using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3
{
    public class Todo
    {
         public int Id { get; set; }
        public string Title{ get; set; }
        public bool Completed{ get; set; }

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
