using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using triviaWebASPNET.Models;

using MySqlConnector;
//using System.Net.Http;
//using Microsoft.AspNetCore.Session;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using BCrypt.Net;
// nebo 'using BCrypt = BCrypt.Net.BCrypt;' pak to nebudu muset psat tak dlouhy

using triviaWebASPNET.ViewModels;
using triviaWebASPNET.sqlTools;
using triviaWebASPNET.modelTools;

namespace triviaWebASPNET.Controllers
{
    public class users : Controller
    {
        private readonly ILogger<users> _logger;

        public users(ILogger<users> logger)
        {
            _logger = logger;
        }

        [Route("user/{username}")]
        [HttpGet]
        public IActionResult user()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
