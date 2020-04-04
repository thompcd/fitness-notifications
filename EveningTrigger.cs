using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace OrangeAlligator
{
    public static class EveningTrigger
    {
        [FunctionName("EveningTrigger")]
        public static void Run([TimerTrigger("0 0 19 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var props = System.Environment.GetEnvironmentVariables();

            var sender = (string)props["SENDER_NUMBER"];
            var recipient = (string)props["RECIPIENT_NUMBER"];
            var accountSid = (string)props["TWILIO_SID"];
            var authToken = (string)props["TWILIO_TOKEN"];

            try
            {
                TwilioClient.Init(accountSid, authToken);

                var messageOptions = new CreateMessageOptions(
                    new PhoneNumber(recipient)
                ); 

                messageOptions.From = new PhoneNumber(sender);    
                messageOptions.Body = "You got this message from azure.";
                var message = MessageResource.Create(messageOptions); 
            }
            catch (System.Exception ex)
            {
                log.LogInformation($"{ex.Message}");
                log.LogInformation($"{ex.InnerException}");
            }
        }
    }
}
