using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Application
{
    public class TodoDTO
    {
        private bool Completed { get; set; }
        public int Id { get; }
        public string Title { get; private set; }

        public TodoDTO(int id, string title, bool completed = false)
        {
            Id = id;
            Title = title;
            Completed = completed;
        }
    }
}
