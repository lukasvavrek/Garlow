using System;
using System.Threading;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using CommandLine;

namespace Garlow.Generator
{
    class Program
    {
        public class Options
        {
            [Option('p', "publicId", Required = true, HelpText = "PublicId of Location.")]
            public string PublicId { get; set; }

            [Option('s', "secretKey", Required = true, HelpText = "SecretKey of Location.")]
            public string SecretKey { get; set; }
        }

        public class Auth
        {
            public string PublicId { get; set; }
            public string SecretKey { get; set; }
        }


        static void Main(string[] args)
        {
            var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            var client = new HttpClient(clientHandler);

            var authJson = string.Empty;

            Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed<Options>(options => {
                    authJson = JsonConvert.SerializeObject(new Auth {
                        PublicId = options.PublicId,
                        SecretKey = options.SecretKey
                    });
                })
                .WithNotParsed<Options>(_ => Environment.Exit(0));

            Console.WriteLine($"Serialized auth: {authJson}");

            var count = 0;
            var trend = 1;
            const int minCount = 0;
            const int maxCount = 100;

            Console.WriteLine($"Garlow data generator:");
            Console.WriteLine($"\tstart: {count}");
            Console.WriteLine($"\tmin: {minCount}");
            Console.WriteLine($"\tmax: {maxCount}");
            Console.WriteLine(new string('-', 10));

            while(true)
            {
                // 1/3 chanse of changing trend
                if (new Random().Next(0, 3) == 0)
                    trend = -trend;

                // check for boundaries
                if (count == minCount)
                    trend = 1;
                else if (count == maxCount)
                    trend = -1;

                count += trend;

                var trendApi = trend > 0 ? "in" : "out";
                var url = $"https://localhost:5001/api/movements/{trendApi}";
                // send trend information to the server
                var content = new StringContent(authJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(url, content).Result;

                Console.WriteLine(count);
                Thread.Sleep(1000);
            }
        }
    }
}
