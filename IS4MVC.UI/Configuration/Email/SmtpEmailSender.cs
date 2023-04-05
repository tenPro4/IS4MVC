﻿using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IS4MVC.UI.Configuration.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpConfiguration _configuration;
        private readonly SmtpClient _client;

        public SmtpEmailSender(SmtpConfiguration configuration, SmtpClient client)
        {
            _client = new SmtpClient
            {
                Host = _configuration.Host,
                Port = _configuration.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = _configuration.UseSSL
            };

            if (!string.IsNullOrEmpty(_configuration.Password))
                _client.Credentials = new System.Net.NetworkCredential(_configuration.Login, _configuration.Password);
            else
                _client.UseDefaultCredentials = true;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //_logger.LogInformation($"Sending email: {email}, subject: {subject}, message: {htmlMessage}");
            try
            {
                var from = string.IsNullOrEmpty(_configuration.From) ? _configuration.Login : _configuration.From;
                var mail = new MailMessage(from, email);
                mail.IsBodyHtml = true;
                mail.Subject = subject;
                mail.Body = htmlMessage;

                _client.Send(mail);
                //_logger.LogInformation($"Email: {email}, subject: {subject}, message: {htmlMessage} successfully sent");

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Exception {ex} during sending email: {email}, subject: {subject}");
                throw;
            }
        }
    }
}
