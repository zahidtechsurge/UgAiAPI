using AmazonFarmer.NotificationServices.Helpers;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AmazonFarmer.NotificationServices.Services
{
    public class NotificationService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;
        private readonly FcmConfiguration _fcmConfiguration;

        public NotificationService(
            IOptions<EmailConfiguration> emailConfig,
            IOptions<FcmConfiguration> fcmConfig)
        {
            var emailOptions = emailConfig.Value;
            var smtpServer = emailOptions.SMTPServer;
            var port = emailOptions.Port;
            var fromAddress = emailOptions.FromEmail;
            var username = emailOptions.UserName;
            var password = emailOptions.Password;

            _smtpClient = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            _fromAddress = fromAddress;

            _fcmConfiguration = fcmConfig.Value;
        }

        public async Task SendEmail(string toAddress, string subject, string body)
        {
            MailMessage mail = new MailMessage(_fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(mail);
        }

        public async Task SendSMS(string phoneNumber, string message)
        {
            // Here you would implement code to send SMS
            Console.WriteLine($"Sending SMS to {phoneNumber}: {message}");
        }

        public async Task SendFCM(string deviceId, string message)
        {
            // Here you would implement code to send FCM
            var messageBody = new Message
            {
                Data = new Dictionary<string, string>() { { "message", message } },
                Token = deviceId
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(messageBody);
        }

        public async Task SendNotifications(List<NotificationRequest> notifications)
        {
            foreach (var notification in notifications)
            {
                switch (notification.Type)
                {
                    case NotificationType.Email:
                        await SendEmail(notification.Recipient, notification.Subject, notification.Message);
                        break;
                    case NotificationType.SMS:
                        await SendSMS(notification.Recipient, notification.Message);
                        break;
                    case NotificationType.FCM:
                        await SendFCM(notification.Recipient, notification.Message);
                        break;
                    default:
                        throw new ArgumentException("Invalid notification type");
                }
            }
        }
    }

    public enum NotificationType
    {
        Email,
        SMS,
        FCM
    }

    public class NotificationRequest
    {
        public NotificationType Type { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}