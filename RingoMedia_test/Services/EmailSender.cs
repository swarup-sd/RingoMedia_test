using Newtonsoft.Json.Linq;
using RingoMedia_test.Models;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System.Diagnostics;

namespace RingoMedia_test.Services
{
    public interface IEmailSender
    {
        System.Threading.Tasks.Task SendEmailAsync(string email, string subject, string message);
    }

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async System.Threading.Tasks.Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();

            
            Configuration.Default.ApiKey["api-key"] = smtpSettings.Key;

            var apiInstance = new TransactionalEmailsApi();
            string SenderName = "Ringo Test";
            SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, smtpSettings.From);
            
            var To = new List<SendSmtpEmailTo>() {
                new SendSmtpEmailTo(email, email.Split("@")[0])
            };

            try
            {
                var mail = new SendSmtpEmail(sender: Email, to: To, subject: subject, htmlContent: message);
                CreateSmtpEmail result = await apiInstance.SendTransacEmailAsync(mail);
                Debug.WriteLine(result.ToJson());

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }

}
