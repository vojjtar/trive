using System;
using System.Collections.Generic;

using MySqlConnector;

using triviaWebASPNET.ViewModels;

namespace triviaWebASPNET.modelTools
{
    class modelCreate
    {
        public static LoginRegisterModel infoMessageModel(string zpravaProUzivatele)
        {
            var modelError = new LoginRegisterModel {
                errorMessage = zpravaProUzivatele
            };

            return modelError;
        }
        public static LoginModel loginMessageModel(string name)
        {
            var modelLogin = new LoginModel {
                jmeno = name
            };

            return modelLogin;
        }
    }
}