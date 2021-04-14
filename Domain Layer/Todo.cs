namespace TodoApp.Domain
{
    public class Todo
    {
        public Todo(int id, string title, bool completed = false)
        {
            Id = id;
            Title = title;
            Completed = completed;
        }

        public bool Completed { get;  private set; }
        public int Id { get; }
        public string Title { get; private set; }

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
    }
}