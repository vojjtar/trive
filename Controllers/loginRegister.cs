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
                    return View("~/Views/loginRegister/register.cshtml", modelError);
                }
                catch
                {
                    var modelError = modelCreate.infoMessageModel("Registrace se nezdarila");
                    return View("~/Views/loginRegister/register.cshtml", modelError);
                }
            }
            else
            {
                //Console.WriteLine(data["jmeno"]);
                var modelError = modelCreate.infoMessageModel("Jmeno nebo email uz existuje");
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
