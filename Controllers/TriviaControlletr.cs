using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using triviaWebASPNET.Models;

using MySqlConnector;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

using triviaWebASPNET.ViewModels;
using triviaWebASPNET.sqlTools;

namespace triviaWebASPNET.Controllers
{
    public class TriviaController : Controller
    {
        private readonly ILogger<TriviaController> _logger;

        public TriviaController(ILogger<TriviaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Trivia()
        {
            string name;
            string score;

            if (HttpContext.Session.GetString("username") != null)
            {
                name = HttpContext.Session.GetString("username");
                score = ConnectToDatabase.getScore(name);   
            }
            else
            {
                name = "Guest";
                score = "No score for guest";   
            }

            var model = new UserModel {
                jmeno = name,
                score = score
            };

            return View("~/Views/trivia/trivia.cshtml", model);
        }

        [HttpPost]
        public IActionResult Trivia(string jmeno, string score) // trivia score funkce
        {

            var pripojeni = ConnectToDatabase.Connector();
            string sql = $"UPDATE databazeUzivatelu SET score = score + {score} WHERE jmeno = '{jmeno}';";

            try
            {
                var prikaz = new MySqlCommand(sql, pripojeni);
                prikaz.ExecuteNonQuery();
            }
            catch (MySqlException ex) { Console.WriteLine(ex); }


            pripojeni.Close();
            return View();
        }

        [HttpGet]
        public IActionResult Scoreboard()
        {
            var pripojeni = ConnectToDatabase.Connector();

            string sql = $"SELECT score, jmeno FROM databazeUzivatelu ORDER BY score DESC LIMIT 50;";
            var prikaz = new MySqlCommand(sql, pripojeni);
            var data = prikaz.ExecuteReader();

            // Dictionary<List<string>, string> values = new Dictionary<string, string>();
            List<string> jmenaList = new List<string>();
            List<string> scoreList = new List<string>();
            var modelScoreBoard = new ScoreBoard {
                jmena = jmenaList,
                score = scoreList
            };

            while (data.Read())
            {   
                /*             
                var score = data[0].ToString();
                var jmeno = data[1].ToString();
                Console.WriteLine(jmeno);
                */
                modelScoreBoard.jmena.Add(data[1].ToString());
                modelScoreBoard.score.Add(data[0].ToString());
                //Dictionary<int, string> dataList = new Dictionary<int, string>();
                //var jsonString = JsonConvert.SerializeObject(data);

            }
            //scoreList.ForEach(Console.WriteLine);
            //jmenaList.ForEach(Console.WriteLine);
            return View(modelScoreBoard);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
