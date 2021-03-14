using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoRepository todoRepository;

        public TodosController(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        [HttpPut("{id}/title")]
        public async Task<IActionResult> ChangeTitleAsync(int id, [FromBody] string title)
        {
            var todo = await todoRepository.FindByIdAsync(id);
            if (todo is null)
                return NotFound();
            todo.ChangeTitle(title);
            await todoRepository.UpdateAsync(todo);
            return NoContent();
        }

        // POST api/<TodosControllers>
        [HttpPost]
        public async Task<ActionResult<Todo>> CreateAsync([FromBody] CreateTodoRequest RequestedTodo)
        {
            var id = await todoRepository.GetNextIdentityAsync();
            var todo = new Todo(id, RequestedTodo.Title);
            await todoRepository.AddAsync(todo);
            return CreatedAtAction("GetById", new { id = todo.Id }, todo);
        }

        // DELETE api/<TodosControllers>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await todoRepository.RemoveAsync(id);
            if (success)
                return NoContent();
            return NotFound();
        }

        // GET: api/<TodosControllers>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAllAsync([FromQuery] int limit = 20)
        {
            if (limit > 100)
                return BadRequest();
            return Ok(await todoRepository.ListAsync(limit));
        }

        // GET api/<TodosControllers>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetByIdAsync(int id)
        {
            var todo = await todoRepository.FindByIdAsync(id);
            if (todo is null)
                return NotFound();
            return todo;
        }

        [HttpPut("{id}/completeness")]
        public async Task<IActionResult> MarkAsDoneAsync(int id)
        {
            var todo = await todoRepository.FindByIdAsync(id);
            if (todo is null)
                return NotFound();
            todo.MarkAsDone();
            await todoRepository.UpdateAsync(todo);
            return NoContent();
        }

        [HttpDelete("{id}/completeness")]
        public async Task<IActionResult> MarkAsNotDoneAsync(int id)
        {
            var todo = await todoRepository.FindByIdAsync(id);
            if (todo is null)
                return NotFound();
            todo.MarkAsNotDone();
            await todoRepository.UpdateAsync(todo);

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Todo>>> SearchByTitleAsync([Required, FromQuery] string query)
        {
            return Ok(await todoRepository.SearchAsync(query));
        }
    }
}