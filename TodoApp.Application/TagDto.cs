using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Application
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public string Tag { get; set; }
        public Guid TodoId { get; set; }
        public TagDto(Guid Id, string Tag, Guid TodoId)
        {
            this.Id = Id;
            this.Tag = Tag;
            this.TodoId = TodoId;
        }
    }
}
