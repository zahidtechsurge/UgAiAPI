using AmazonFarmer.Core.Application.DTOs;
using System;
using MimeKit;
using MailKit.Net.Smtp;
using System.Security.Authentication;

namespace AmazonFarmer.Administrator.API.Extensions
{
    public static class EmailExtension
    {
        public static async void sendEmail(emailDTO req)
        {
            try
            {
                if (Convert.ToBoolean(ConfigExntension.GetConfigurationValue("EmailConfiguration:isAllowed")))
                {
                    using (var client = new SmtpClient())
                    {
                        try
                        {
                            if (Convert.ToBoolean(ConfigExntension.GetConfigurationValue("EmailConfiguration:isDevMode")))
                            {
                                req.name = ConfigExntension.GetConfigurationValue("EmailConfiguration:devName");
                                req.toUser = ConfigExntension.GetConfigurationValue("EmailConfiguration:devMail");
                            }

                            client.CheckCertificateRevocation = false;
                            client.Connect(ConfigExntension.GetConfigurationValue("EmailConfiguration:host"), 587, false);
                            client.AuthenticationMechanisms.Remove("XOAUTH2");
                            client.Authenticate(
                                ConfigExntension.GetConfigurationValue("EmailConfiguration:username"),
                                ConfigExntension.GetConfigurationValue("EmailConfiguration:password")
                            );
                            client.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Ssl3;
                            var message = new EmailMessage(
                                new MailReceipt
                                {
                                    Name = req.name,
                                    Email = req.toUser
                                },
                                req.subject,
                                req.body,
                                null
                                );
                            var emailMessage = new MimeMessage();
                            emailMessage.From.Add(new MailboxAddress(
                                ConfigExntension.GetConfigurationValue("EmailConfiguration:name"),
                                ConfigExntension.GetConfigurationValue("EmailConfiguration:fromEmail")
                            ));
                            emailMessage.To.AddRange(message.To);
                            emailMessage.Subject = message.Subject;
                            var builder = new BodyBuilder();
                            if (!string.IsNullOrEmpty(message.Attachment))
                            {
                                builder.Attachments.Add(message.Attachment);
                            }
                            builder.HtmlBody = message.Content;
                            emailMessage.Body = builder.ToMessageBody();
                            await client.SendAsync( emailMessage );
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            client.Disconnect(true);
                        }
                        client.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
    public class MailReceipt
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class EmailMessage
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string? Attachment { get; set; }
        public EmailMessage(MailReceipt to, string subject, string content, string image)
        {
            To = new List<MailboxAddress>();
            MailboxAddress mailboxAddress = new MailboxAddress(to.Name, to.Email);
            To.Add(mailboxAddress);
            Subject = subject;
            Content = content;
            Attachment = image;
        }
    }

}
