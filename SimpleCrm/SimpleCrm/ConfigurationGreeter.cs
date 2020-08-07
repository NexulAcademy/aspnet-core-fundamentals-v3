using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCrm
{
    public class ConfigurationGreeter : IGreeter
    {
        public ConfigurationGreeter(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public string GetGreeting()
        {
            return Configuration["Greeting"];
        }
    }
}
