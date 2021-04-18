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

namespace triviaWebASPNET.Controllers
{
    public class loginRegister : Controller
    {
        private readonly ILogger<loginRegister> _logger;

        public loginRegister(ILogger<loginRegister> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public IActionResult login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult register()
        {
            return View();
        }


        // tady budou posty  // jmeno = Request.Form["jmeno"] atd.

        [HttpPost]
        public IActionResult login(string name, string password)
        {
            var pripojeni = ConnectToDatabase.Connector();

            string hesloDatabaze = String.Empty;
            string scoreDatabaze = String.Empty;
            bool verified = false;

            string sql = $"SELECT heslo FROM databazeUzivatelu where jmeno = '{name}';";
            var prikaz = new MySqlCommand(sql, pripojeni);
            var data = prikaz.ExecuteReader();


            if (data.Read())
            {
                //Console.WriteLine(data["heslo"]);
                hesloDatabaze = data["heslo"].ToString();
                //scoreDatabaze = data["score"].ToString();

                verified = BCrypt.Net.BCrypt.Verify(password, hesloDatabaze);
                Console.WriteLine(verified);

            }
            if (verified)
            {
                HttpContext.Session.SetString("username", name);

                var model = new LoginModel {
                    jmeno = name,
                };

                return View("~/Views/loginRegister/login.cshtml", model);
            }

            else
            {
                var modelError = new LoginRegisterModel {
                    errorMessage = "Spatne jmeno nebo heslo"
                };

                return View("~/Views/loginRegister/login.cshtml", modelError);
            }


        }

        [HttpPost]
        public IActionResult register(string name, string email, string password)
        {
            var pripojeni = ConnectToDatabase.Connector();

            string sql = $"SELECT * FROM databazeUzivatelu where jmeno = '{name}';";
            var prikaz = new MySqlCommand(sql, pripojeni);
            var data = prikaz.ExecuteReader();
            if (!data.Read())
            {
                pripojeni.Close();
                pripojeni.Open();

                string passwordHash =  BCrypt.Net.BCrypt.HashPassword(password);

                sql = $"INSERT INTO databazeUzivatelu (jmeno, heslo, email, score) VALUES ('{name}', '{passwordHash}', '{email}', 0);";
                prikaz = new MySqlCommand(sql, pripojeni);

                prikaz.ExecuteNonQuery();

                var modelError = new LoginRegisterModel {
                    errorMessage = "Uspesna registrace"
                };

                return View("~/Views/loginRegister/register.cshtml", modelError);

            }
            else
            {
                var modelError = new LoginRegisterModel {
                    errorMessage = "Jmeno nebo email uz existuje"
                };

                Console.WriteLine(data["jmeno"]);


                return View("~/Views/loginRegister/register.cshtml", modelError);

            }

           // return View("~/Views/loginRegister/register.cshtml");
        }


        [HttpGet]
        public IActionResult logOut()
        {
            HttpContext.Session.Remove("username");
            return View("~/Views/loginRegister/login.cshtml");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
