using System;

using System.Collections.Generic;
using MySqlConnector;


namespace triviaWebASPNET.sqlTools
{
    class ConnectToDatabase
    {
        public static MySqlConnector.MySqlConnection Connector()
        {
            string databaze = "";
            string uzivatelskeJmeno = "";
            string hesloNaPhpAdmin = "";
            string port = "";
            string server = "";

            string conString = @"Server = " + server + @";
                                Port = " + port + @";
                                Database = " + databaze + @";
                                Uid = " + uzivatelskeJmeno + @";
                                Pwd = " + hesloNaPhpAdmin + @";
                                charset = utf8;
                                Allow User Variables = True";

            var pripojeni = new MySqlConnection(conString);

            pripojeni.Open();

            return pripojeni;
        }
    }
}