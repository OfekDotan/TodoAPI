using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication3.Controllers
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
        public ActionResult Get()
        {
            return Ok(todoRepository.List(20));

        }

        // GET api/<TodosControllers>/5
        [HttpGet("{id}")]
        public ActionResult<Todo> Get(int id)
        {
            ActionResult<Todo> t = todoRepository.FindById(id);
            if (t == null) return NotFound();
            return t;
        }

        [HttpGet("{id}")]
        public ActionResult Get(string text)
        {
            return Ok();
        }

        // POST api/<TodosControllers>
        [HttpPost]
        public void Create([FromBody] Todo value)
        {
            todoRepository.Add(value);
        }

        // PUT api/<TodosControllers>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Todo value)
        {
            todoRepository.Update(id, value);

        }

        // DELETE api/<TodosControllers>/5
        [HttpDelete("{id}")]
        public void Delete(int id) //remove a todo with removeWhere(LAMZA) from the list
        {
            todoRepository.RemoveInstance(id);
        }
        
        [HttpPut("{id}/title")]
        public IActionResult ChangeTitle(int id, [FromBody] string title)
        {
            var todo = todoRepository.FindById(id);
            if (todo is null) return NotFound();
            todo.ChangeTitle(title);

            todoRepository.Update(todo.Id,todo);
            return NoContent();
        }

        [HttpPut("{id}/completeness")] 
        public IActionResult MarkAsDone(int id)
        {
            var todo = todoRepository.FindById(id);
            if (todo is null) return NotFound();
            todo.MarkAsDone();

            todoRepository.Update(todo.Id,todo);
            return NoContent();

        }

        [HttpDelete("{id}/completeness")]
        public IActionResult MarkAsNotDone(int id)
        {
            var todo = todoRepository.FindById(id);
            if (todo is null) return NotFound();
            todo.MarkAsNotDone();

            todoRepository.Update(todo.Id,todo);
            return NoContent();
                
        }
        
    }
}
