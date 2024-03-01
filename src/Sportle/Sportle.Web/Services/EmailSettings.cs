﻿namespace Sportle.Web.Services
{
    public class EmailSettings
    {
        public string Host { get; set; } = string.Empty;

        public int Port { get; set; }

        public bool EnableSsl { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Sender { get; set; } = string.Empty;
    }
}
