using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using SCCB.Core.DTO;
using SCCB.Core.Settings;

namespace SCCB.Services.EmailService
{
    /// <summary>
    /// Email service.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailSetting _emailSetting;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="emailSetting">EmailSetting instance.</param>
        public EmailService(IOptions<EmailSetting> emailSetting)
        {
            _emailSetting = emailSetting.Value ?? throw new ArgumentException(nameof(emailSetting));
        }

        /// <inheritdoc/>
        public void SendChangePasswordEmail(EmailWithToken email)
        {
            var replaceDictionary = new Dictionary<string, string>
            {
                { "{_gatewayUrl}", _emailSetting.GatewayUrl },
                { "{email.ResetToken}", email.Token },
            };

            var template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Templates", "ChangePasswordTemplate.html"));
            var messageText = MessageBuilder(template, replaceDictionary);

            var message = new MailMessage(_emailSetting.Email, email.EmailAddress)
            {
                Subject = "Password change",
                IsBodyHtml = true,
                Body = messageText,
            };

            using var smtp = GetSmtpClient();
            smtp.Send(message);
        }

        private SmtpClient GetSmtpClient()
        {
            return new SmtpClient
            {
                Host = _emailSetting.Host,
                Port = _emailSetting.Port,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSetting.Email, _emailSetting.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
            };
        }

        private string MessageBuilder(string template, Dictionary<string, string> replaceDictionary)
        {
            foreach (var item in replaceDictionary)
            {
                template = template.Replace(item.Key, item.Value);
            }

            return template;
        }
    }
}
