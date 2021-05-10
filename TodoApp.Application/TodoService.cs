using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Domain_Layer;

namespace TodoApp.Application
{
    public class TodoService
    {
        private readonly ITodoRepository todoRepository;
        public TodoService(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }
        public async Task<bool> ChangeTitleAsync(Guid id, string title)
        {
            var todo = await todoRepository.FindByIdAsync(id);
            if (todo is null) 
                return false;
            todo.ChangeTitle(title);
            await todoRepository.UpdateAsync(todo);
            return true;
        }
        public async Task<TodoDto> CreateAsync(CreateTodoRequest requestedTodo)
        {
            var id = Guid.NewGuid();
           
            var todo = new Todo(id, requestedTodo.Title);

            foreach (var tag in requestedTodo.Tags)
            {
                todo.AddTag(new Tag(Guid.NewGuid(), tag, id));
            }
            await todoRepository.AddAsync(todo);
            return ConvertToDto(todo);
        }
        public async Task<TagDto> AddTagAsync(Guid id, CreateTagRequest requestTag)
        {
            var tagId = Guid.NewGuid();
            var tag = new Tag(tagId, requestTag.Tag, id);
            await todoRepository.AddTagAsync(tag);
            return ConvertTagToDto(tag);
        }
        public async Task<bool> RemoveAsync(Guid id)
        {
            return await todoRepository.RemoveAsync(id);
        }
        public async Task<IEnumerable<TodoDto>> ListAsync(int limit)
        {

           var todos = await todoRepository.ListAsync(limit);
            return todos.Select(ConvertToDto);

        }
        public async Task<TodoDto> FindByIdAsync(Guid id)
        {
            var todo = await todoRepository.FindByIdAsync(id);
            if (todo is null)
                return null;
            return ConvertToDto(todo);
        }
        public async Task<bool> MarkAsDoneAsync(Guid id)
        {
            var todo = await todoRepository.FindByIdAsync(id);
            if (todo is null)
                return false;
            todo.MarkAsDone();
            await todoRepository.UpdateAsync(todo);
            return true;
        }

        public async Task<bool> removeTag(Guid id, string tag)
        {
            return await todoRepository.RemoveTagAsync(id, tag);
        }

        public async Task<bool> MarkAsNotDoneAsync(Guid id)
        {
            var todo = await todoRepository.FindByIdAsync(id);
            if (todo is null)
                return false;
            todo.MarkAsNotDone();
            await todoRepository.UpdateAsync(todo);
            return true;
        }
        public async Task<IEnumerable<TodoDto>> SearchAsync(string title)
        {
            var todos = await todoRepository.SearchAsync(title);
            return todos.Select(ConvertToDto);
        }
        public async Task<IEnumerable<TodoDto>> SearchTagAsync(string tag)
        {

            var todos = await todoRepository.SearchByTagAsync(tag);
            return todos.Select(ConvertToDto);
        }
        private TodoDto ConvertToDto(Todo todo)
        {
            var id = todo.Id;
            string title = todo.Title;
            bool completed = todo.Completed;
            var tags = new List<string>();
            if(!(todo.ReadTags is null))
            foreach(Tag tag in todo.ReadTags)
            {
                    if(!(tag is null))
                tags.Add(tag.Title);
            }

            return new TodoDto(id, title,tags, completed);
        }
        public TagDto ConvertTagToDto(Tag tag)
        {
            Guid id = tag.Id;
            string title = tag.Title;
            Guid todoId = tag.TodoId;
            return new TagDto(id, title, todoId);
        }
    }
}
