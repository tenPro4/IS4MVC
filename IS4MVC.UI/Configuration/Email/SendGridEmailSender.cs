using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS4MVC.UI.Configuration.Email
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridConfiguration _configuration;
        private readonly ISendGridClient _client;

        public SendGridEmailSender(SendGridConfiguration configuration, ISendGridClient client)
        {
            _configuration = configuration;
            _client = client;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = SendGrid.Helpers.Mail.MailHelper.CreateSingleEmail(
                 new SendGrid.Helpers.Mail.EmailAddress(_configuration.SourceEmail, _configuration.SourceName),
                 new SendGrid.Helpers.Mail.EmailAddress(email),
                  subject,
                  null,
                  htmlMessage
             );

            // More information about click tracking: https://sendgrid.com/docs/ui/account-and-settings/tracking/
            message.SetClickTracking(_configuration.EnableClickTracking, _configuration.EnableClickTracking);

            var response = await _client.SendEmailAsync(message);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                case System.Net.HttpStatusCode.Created:
                case System.Net.HttpStatusCode.Accepted:
                    break;
                default:
                    {
                        var errorMessage = await response.Body.ReadAsStringAsync();
                        //_logger.LogError($"Response with code {response.StatusCode} and body {errorMessage} after sending email: {email}, subject: {subject}");
                        break;
                    }
            }
        }
    }
}
