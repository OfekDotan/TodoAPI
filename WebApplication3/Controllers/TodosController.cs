using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // GET: api/<TodosControllers>
        [HttpGet]
        public ActionResult<IEnumerable<Todo>> GetAll ([FromQuery]int limit)
        {
            if (limit > 100) return BadRequest();
            return Ok(todoRepository.List(limit));

        }

        // GET api/<TodosControllers>/5
        [HttpGet("{id}")]
        public ActionResult<Todo> GetById(int id)
        {
            ActionResult<Todo> todo = todoRepository.FindById(id);
            if (todo == null) 
                return NotFound();
            return todo;
        }
        
        [HttpGet("search")]
        public ActionResult<IEnumerable<Todo>> SearchByTitle([FromQuery] string query)
        {
            return Ok(todoRepository.Search(query));
        }

        // POST api/<TodosControllers>
        [HttpPost]
        public IActionResult Create([FromBody] CreateTodoRequest RequestedTodo)
        {
            int id = todoRepository.GetNextIdentity();
            Todo todo = new Todo(id, RequestedTodo.Title);
            todoRepository.Add(todo);
            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }


        // DELETE api/<TodosControllers>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) 
        {
            bool success=todoRepository.Remove(id);
            if (success) 
                return Ok();
            return NotFound();
        }
        
        [HttpPut("{id}/title")]
        public IActionResult ChangeTitle(int id, [FromBody] string title)
        {
            var todo = todoRepository.FindById(id);
            if (todo is null) 
                return NotFound();
            todo.ChangeTitle(title);

            return NoContent();
        }

        [HttpPut("{id}/completeness")] 
        public IActionResult MarkAsDone(int id)
        {
            var todo = todoRepository.FindById(id);
            if (todo is null) 
                return NotFound();
            todo.MarkAsDone();

            return NoContent();
        }

        [HttpDelete("{id}/completeness")]
        public IActionResult MarkAsNotDone(int id)
        {
            var todo = todoRepository.FindById(id);
            if (todo is null) 
                return NotFound();
            todo.MarkAsNotDone();

            return NoContent();
        }
        
    }
}
