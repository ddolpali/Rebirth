using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Providers
{
    class Settings
    {
        public static IConfiguration config;

        public Settings()
        {
            config = new ConfigurationBuilder()
                .AddJsonFile("Global.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
