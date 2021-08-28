using Microsoft.AspNetCore.Mvc;
using Npgsql;
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

        public string getConnectionString()
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };

            return builder.ToString();
        }

        [HttpGet]
        [Route("GetItems")]
        public IActionResult GetItems()
        {
            var result = _context.TodoItems.ToList();

            // TodoItem todoItem = new TodoItem { Id = 1, Name = "testname", IsComplete = true };
            // var x = Environment.GetEnvironmentVariable("DATABASE_URL");

            var x = getConnectionString();

            return Ok(x);
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
