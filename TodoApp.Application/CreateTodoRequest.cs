using System.Collections.Generic;

namespace TodoApp.Application
{
    public class CreateTodoRequest
    {
        public string Title { get; set; }
        public List<string> Tags { get; set; }      
    }
}