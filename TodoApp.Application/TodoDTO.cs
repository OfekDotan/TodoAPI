using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Application
{
    public class TodoDto
    {
        public bool Completed { get; private set; }
        public Guid Id { get; }
        public string Title { get; private set; }
        public List<string> Tags { get; private set; }

        public TodoDto(Guid id, string title, List<string> tags, bool completed)
        {
            Id = id;
            Title = title;
            Tags = tags;
            Completed = completed;
        }
    }
}
