﻿namespace VeterinarianApp.ViewModels
{
    public class SMTPEmailObject
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string From { get; set; }

        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }



    }
}
