using System;
using Microsoft.AspNetCore.Hosting;

namespace AliceMafia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StartServer();
            Console.WriteLine("Server was started");
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