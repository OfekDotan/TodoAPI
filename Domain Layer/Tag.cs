using System;
using System.Collections.Generic;
using System.Text;

namespace Domain_Layer
{
  public class Tag
    {
        public Tag(Guid TagId, string Tag, Guid TodoId)
        {
            Id = TagId;
            Title = Tag;
            this.TodoId = TodoId;
        }
        public Guid Id { get; set; }
        public string Title{ get; private set; }
        public Guid TodoId{ get; private set; }

        public void ChangeTitle(string title)
        {
            Title = title;
        }
    }
}
