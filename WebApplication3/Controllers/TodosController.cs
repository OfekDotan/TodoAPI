using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TodoApp.Application;
using TodoApp.Domain;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoService todoService;

        public TodosController(ITodoRepository todoRepository)
        {
            
            this.todoService = new TodoService(todoRepository);
        }

        [HttpPut("{id}/title")]
        public async Task<IActionResult> ChangeTitleAsync(int id, [FromBody] string title)
        {
            var success=await todoService.ChangeTitleAsync(id, title);
            if (success)
                return NoContent();
            
                return NotFound();

        }

        // POST api/<TodosControllers>
        [HttpPost]
        public async Task<ActionResult<Todo>> CreateAsync([FromBody] CreateTodoRequest requestedTodo)
        {
            TodoDTO todo = await todoService.CreateAsync(requestedTodo);
            return CreatedAtAction("GetById", new { id = todo.Id }, todo);
        }

        // DELETE api/<TodosControllers>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
           var success = await todoService.RemoveAsync(id);
            if (success)
                return NoContent();
            return NotFound();
        }

        // GET: api/<TodosControllers>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoDTO>>> GetAllAsync([FromQuery] int limit = 20)
        {
            if (limit > 100)
                return BadRequest();
            return Ok(await todoService.ListAsync(limit));
        }

        // GET api/<TodosControllers>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDTO>> GetByIdAsync(int id)
        {
            var todoDTO = await todoService.FindByIdAsync(id);
            if (todoDTO is null)
                return NotFound();
            return todoDTO;
        }

        [HttpPut("{id}/completeness")]
        public async Task<IActionResult> MarkAsDoneAsync(int id)
        {
            var success= await todoService.MarkAsDoneAsync(id);
            if (!success)
                return NotFound();
           
            return NoContent();
        }

        [HttpDelete("{id}/completeness")]
        public async Task<IActionResult> MarkAsNotDoneAsync(int id)
        {
            var success = await todoService.MarkAsNotDoneAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Todo>>> SearchByTitleAsync([Required, FromQuery] string query)
        {
            return Ok(await todoService.SearchAsync(query));
        }
    }
}