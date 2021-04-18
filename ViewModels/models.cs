using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Web;  
  
namespace triviaWebASPNET.ViewModels
{
    public class UserModel  
    {
        public String jmeno { get; set; }
        public String score { get; set; }
    }
    public class LoginRegisterModel  
    {
        public String errorMessage { get; set; }

        /*
        public bool Empty
        {
            get { return (string.IsNullOrWhiteSpace(errorMessage)); }
        }
        */
    }
    public class ScoreBoard
    {
        public List<string> jmena { get; set; }
        public List<string> score { get; set; }
    }
}