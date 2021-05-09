using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TodoApp.Application;
using Domain_Layer;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoService todoService;

        public TodosController(TodoService todoService)
        {

            this.todoService = todoService;
        }

        [HttpPut("{id}/title")]
        public async Task<IActionResult> ChangeTitleAsync(Guid id, [FromBody] string title)
        {
            var success = await todoService.ChangeTitleAsync(id, title);
            if (success)
                return NoContent();

            return NotFound();

        }
        
        // POST api/<TodosControllers>
        [HttpPost]
        public async Task<ActionResult<Todo>> CreateAsync([FromBody] CreateTodoRequest requestedTodo)
        {
            TodoDto todo = await todoService.CreateAsync(requestedTodo);
            return CreatedAtAction("GetById", new { id = todo.Id }, todo);
        }
        [HttpPost("{id}")]
        public async Task<ActionResult<Tag>> AddTagAsync(Guid id, [FromBody] CreateTagRequest requestTag)
        {
            var todoDto = await todoService.FindByIdAsync(id);
            if (todoDto is null)
                return NotFound();
            TagDto tag = await todoService.AddTagAsync(id, requestTag);
            return CreatedAtAction("GetById", new { id = tag.Id }, tag);
        } 

        // DELETE api/<TodosControllers>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
           var success = await todoService.RemoveAsync(id);
            if (success)
                return NoContent();
            return NotFound();
        }
        [HttpDelete("{id}/tag")]
        public async Task<IActionResult> RemoveTagAsync(Guid id, string tag)
        {
            var success = await todoService.removeTag(id, tag);
            if (success)
                return NoContent();
            return NotFound();
        }
        
        // GET: api/<TodosControllers>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoDto>>> GetAllAsync([FromQuery] int limit = 20)
        {
            if (limit > 100)
                return BadRequest();
            return Ok(await todoService.ListAsync(limit));
        }

        // GET api/<TodosControllers>/
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDto>> GetByIdAsync(Guid id)
        {
            var todoDTO = await todoService.FindByIdAsync(id);
            if (todoDTO is null)
                return NotFound();
            return todoDTO;
        }

        [HttpPut("{id}/completeness")]
        public async Task<IActionResult> MarkAsDoneAsync(Guid id)
        {
            var success= await todoService.MarkAsDoneAsync(id);
            if (!success)
                return NotFound();
           
            return NoContent();
        }

        [HttpDelete("{id}/completeness")]
        public async Task<IActionResult> MarkAsNotDoneAsync(Guid id)
        {
            var success = await todoService.MarkAsNotDoneAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TodoDto>>> SearchByTitleAsync([Required, FromQuery] string query)
        {
            return Ok(await todoService.SearchAsync(query));
        }

        [HttpGet("searchTags")]
        public async Task<ActionResult<IEnumerable<TodoDto>>> SearchByTagAsync([Required, FromQuery] string tag)
        {
            return Ok(await todoService.SearchTagAsync(tag));
        }
        
    }
}