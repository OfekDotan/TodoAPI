using System;
using TodoApp.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TodoApp.Application
{
    public class TodoService
    {
        private readonly ITodoRepository todoRepository;
        public TodoService(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }
        public async Task<bool> ChangeTitleAsync(int id, string title)
        {
            var todo = await todoRepository.FindByIdAsync(id);
            if (todo is null) 
                return false;
            todo.ChangeTitle(title);
            await todoRepository.UpdateAsync(todo);
            return true;
        }
        public async Task<TodoDTO> CreateAsync(CreateTodoRequest requestedTodo)
        {
            var id = await todoRepository.GetNextIdentityAsync();
            var todo = new Todo(id, requestedTodo.Title);
            await todoRepository.AddAsync(todo);
            return ConvertToDTO(todo);
        }
        public async Task<bool> RemoveAsync(int id)
        {
            return await todoRepository.RemoveAsync(id);
        }
        public async Task<IEnumerable<TodoDTO>> ListAsync(int limit)
        {
            IEnumerable<Todo> todos = await todoRepository.ListAsync(limit);
            List<TodoDTO> returnValue = new List<TodoDTO>();

            foreach (Todo todo in todos)
            {
                TodoDTO temp=ConvertToDTO(todo);
                returnValue.Add(temp);
            }
            return returnValue;

        }
        public async Task<TodoDTO> FindByIdAsync(int id)
        {
            var todo = await todoRepository.FindByIdAsync(id);
            return ConvertToDTO(todo);
        }
        public async Task<bool> MarkAsDoneAsync(int id)
        {
            var todo = await todoRepository.FindByIdAsync(id);
            if (todo is null)
                return false;
            todo.MarkAsDone();
            await todoRepository.UpdateAsync(todo);
            return true;
        }
        public async Task<bool> MarkAsNotDoneAsync(int id)
        {
            var todo = await todoRepository.FindByIdAsync(id);
            if (todo is null)
                return false;
            todo.MarkAsNotDone();
            await todoRepository.UpdateAsync(todo);
            return true;
        }
        public async Task<IEnumerable<TodoDTO>> SearchAsync(string title)
        {
            var todos = await todoRepository.SearchAsync(title);
            List<TodoDTO> returnValue = new List<TodoDTO>();

            foreach (Todo todo in todos)
            {
                TodoDTO temp = ConvertToDTO(todo);
                returnValue.Add(temp);
            }
            return returnValue;
        }

        private TodoDTO ConvertToDTO(Todo todo)
        {
            int id = todo.Id;
            string title = todo.Title;
            bool completed = todo.Completed;
            return new TodoDTO(id, title, completed);
        }

    }
}
