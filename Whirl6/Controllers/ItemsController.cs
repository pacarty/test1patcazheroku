using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whirl6.Models;

namespace Whirl6.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private TodoContext _context;

        public ItemsController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetItems")]
        public IActionResult GetItems()
        {
            var result = _context.TodoItems.ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("PostItem")]
        public IActionResult PostItem(TodoItem item)
        {
            //TodoItem item = new TodoItem { Name = name, IsComplete = isComplete };

            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return Ok();
        }
    }
}
