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
            [Option('p', "publicId", Default = "2790f8df-7508-4d7d-80bd-a16943f361ca", Required = true, HelpText = "PublicId of Location.")]
            public string PublicId { get; set; }

            [Option('k', "secretKey", Default = "52KNGLBH2XGZG1YIKFZ8IWDDMNKIOLXI", Required = true, HelpText = "SecretKey of Location.")]
            public string SecretKey { get; set; }

            [Option('s', "start", Default = 0, Required = true, HelpText = "SecretKey of Location.")]
            public int Start { get; set; }

            [Option('e', "environment", Default = "dev", Required = true, HelpText = "Environment.")]
            public string Environment { get; set; }
        }

        public class Auth
        {
            public string PublicId { get; set; }
            public string SecretKey { get; set; }
        }


        static void Main(string[] args)
        {
            var random = new Random();
            var urlFormat = "https://localhost:5001/api/movements/{0}";

            var count = 0;
            var trend = 1;
            const int minCount = 0;
            const int maxCount = 20;

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
                    count = options.Start;

                    if (options.Environment == "prod")
                        urlFormat = "https://garlow.azurewebsites.net/api/movements/{0}";
                })
                .WithNotParsed<Options>(_ => Environment.Exit(0));

            Console.WriteLine($"Url format: {urlFormat}");

            Console.WriteLine($"Serialized auth: {authJson}");

            Console.WriteLine($"Garlow data generator:");
            Console.WriteLine($"\tstart: {count}");
            Console.WriteLine($"\tmin: {minCount}");
            Console.WriteLine($"\tmax: {maxCount}");
            Console.WriteLine(new string('-', 10));

            while(true)
            {
                // 1/3 chanse of changing trend
                if (random.Next(0, 3) == 0)
                    trend = -trend;

                // check for boundaries
                if (count <= minCount)
                    trend = 1;
                else if (count >= maxCount)
                    trend = -1;

                count += trend;

                var trendApi = trend > 0 ? "in" : "out";
                // send trend information to the server
                var url = string.Format(urlFormat, trendApi);
                var content = new StringContent(authJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(url, content).Result;

                Console.WriteLine(count);
                Thread.Sleep(random.Next(1, 4) * 1000);
            }
        }
    }
}
