using System;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace AliceMafia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StartServer();
            
        }

        private static void StartServer()
        {
            new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}