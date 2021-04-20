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
    public class loginRegister : Controller
    {
        private readonly ILogger<loginRegister> _logger;

        public loginRegister(ILogger<loginRegister> logger)
        {
            _logger = logger;
        }

        [Route("login")]
        [HttpGet]
        public IActionResult login()
        {
            return View();
        }

        [Route("register")]
        [HttpGet]
        public IActionResult register()
        {
            return View();
        }


        // tady budou posty  // jmeno = Request.Form["jmeno"] atd.

        [Route("login")]
        [HttpPost]
        public IActionResult login(string name, string password)
        {
            var pripojeni = ConnectToDatabase.Connector();

            string hesloDatabaze = String.Empty;
            string scoreDatabaze = String.Empty;
            bool verified = false;

            var data = ConnectToDatabase.LoginRegisterNameCheck(name, pripojeni);

            if (data.Read())
            {
                hesloDatabaze = data["heslo"].ToString();
                verified = BCrypt.Net.BCrypt.Verify(password, hesloDatabaze);
                Console.WriteLine(verified);
            }
            if (verified)
            {
                HttpContext.Session.SetString("username", name);

                var model = modelCreate.infoMessageModel(name);

                //Response.Cookies.Append("jmeno", name);
                //string cookie = Request.Cookies["jmeno"];
                //Console.WriteLine(cookie);

                return View("login", model);
            }

            else
            {
                var modelError = new LoginRegisterModel {
                    errorMessage = "Spatne jmeno nebo heslo"
                };

                return View("login", modelError);
            }

        }

        [Route("register")]
        [HttpPost]
        public IActionResult register(string name, string email, string password)
        {
            string zpravaProUzivatele = String.Empty;

            var pripojeni = ConnectToDatabase.Connector();
            var data = ConnectToDatabase.LoginRegisterNameCheck(name, pripojeni);

            //Console.WriteLine(modelError.GetType());

            if (!data.Read())
            {
                pripojeni.Close();
                pripojeni.Open();

                try
                {
                    ConnectToDatabase.RegisterNewUser(name, email, password, pripojeni);
                    var modelError = modelCreate.infoMessageModel("Uspesna registrace");
                    return View("register", modelError);
                }
                catch
                {
                    var modelError = modelCreate.infoMessageModel("Registrace se nezdarila");
                    return View("register", modelError);
                }
            }
            else
            {
                //Console.WriteLine(data["jmeno"]);
                var modelError = modelCreate.infoMessageModel("Jmeno nebo email uz existuje");
                return View("register", modelError);
            }

           // return View("~/Views/loginRegister/register.cshtml");
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult logOut()
        {
            HttpContext.Session.Remove("username");
            return View("login");
        }

        /*
        public IActionResult pageLeave()
        {
            Console.WriteLine("user left");
            HttpContext.Session.Remove("username");
            return new EmptyResult();
        }
        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
