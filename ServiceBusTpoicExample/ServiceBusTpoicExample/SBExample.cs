using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ServiceBusTpoicExample
{
    public class SBExample
    {
        private readonly ILogger<SBExample> _logger;

        public SBExample(ILogger<SBExample> log)
        {
            _logger = log;
        }

        private static IConfigurationRoot iConfig;

        public static IConfigurationRoot GetConfiguration()
        {
            var localRoot = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot");
            var azureWebJobsScriptRoot = @"%HOME%\site\wwwroot";
            var azureRoot = Environment.ExpandEnvironmentVariables(azureWebJobsScriptRoot);
            iConfig = new ConfigurationBuilder()
                .SetBasePath(localRoot ?? azureRoot)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            return iConfig;
        }

        [FunctionName("SBTOPIC")]
        public async Task Run([ServiceBusTrigger("sampletopic", subscriptionName: "sampleSUBS",
                Connection = "Connection")]string mySbMsg)
        {
            _logger.LogInformation($"deserilize the message");
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<sampledeserialize>(mySbMsg);
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            //


            // Processor your message here ..

            //
            var config = iConfig ?? GetConfiguration();
            var Connection = config["Connection"];
            var Topic = config["Topic"];

            //method to send message in topic 
            //await SendMessagetoTopic.sendmessage(Topic, Connection, mySbMsg);

        }
    }
}
