using System;
using System.Collections.Generic;

namespace Domain_Layer
{
    public class Todo
    {
       /* public Todo(Guid id, string title, List<Tag> tags, bool completed = false)
        {
            Id = id;
            Title = title;
            Tags = tags;
            Completed = completed;
        }*/
        public Todo(Guid TodoId, string Title, bool Completed = false)
        {
            
            this.Id = TodoId;
            this.Title = Title;
            this.Completed = Completed;
            Tags = new HashSet<Tag>();
        }
        public bool Completed { get;  private set; }
        public Guid Id { get; /*private set;*/ }
        public string Title { get; private set; }
        private HashSet<Tag> Tags { get; set; }

        public IReadOnlyCollection<Tag> ReadTags => Tags;
        public void ChangeTitle(string title)
        {
            Title = title;
        }
        
        public void MarkAsDone()
        {
            Completed = true;
        }

        public void MarkAsNotDone()
        {
            Completed = false;
        }
        public void AddTag(Tag tag)
        {
            Tags.Add(tag);
        }
    }
}