using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.NotificationServices.Helpers;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json; 
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;  
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration; 

namespace AmazonFarmer.NotificationServices.Services
{
    public class NotificationService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;
        private FcmConfiguration? _fcmConfiguration = new FcmConfiguration();
        private SMSConfiguration? _smsConfiguration = new SMSConfiguration();
        private IRepositoryWrapper _repoWrapper;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IConfiguration _config;
        public NotificationService(
            IOptions<EmailConfiguration> emailConfig,
            IOptions<FcmConfiguration> fcmConfig,
            IOptions<SMSConfiguration> smsConfig,
            IRepositoryWrapper repoWrapper,
            IHostEnvironment hostEnvironment,
            IConfiguration configuration
            )
        {
            var emailOptions = emailConfig.Value;
            var smtpServer = emailOptions.SMTPServer;
            var port = emailOptions.Port;
            var fromAddress = emailOptions.FromEmail;
            var username = emailOptions.UserName;
            var password = emailOptions.Password;
            _hostEnvironment = hostEnvironment;
            _config = configuration;

            _smtpClient = new SmtpClient(smtpServer, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password),
            };

            _fromAddress = fromAddress;

            _fcmConfiguration = fcmConfig.Value;
            _smsConfiguration = smsConfig.Value;
            _repoWrapper = repoWrapper;
        }
        private async Task<NotificationLog> SaveLog(List<string> toAddress, string subject, string body, ENotificationType type)
        {
            string jsonString = JsonConvert.SerializeObject(toAddress);
            NotificationLog log = new NotificationLog
            {
                IsSuccess = false, // In start setting false 
                Message = body,
                Subject = subject,
                Recipient = jsonString,
                SentDate = DateTime.UtcNow,
                Type = type
            };
            log = _repoWrapper.LoggingRepository.AddNoticationLog(log);
            await _repoWrapper.SaveAsync();
            //await _repoWrapper.DisposeAsync();
            return log;
        }

        private async Task SendEmail(List<NotificationRequestRecipient> toAddress, string subject, string body)
        {
            try
            {

                if (IsEmail(toAddress))
                {
                    NotificationLog log = await SaveLog(toAddress.Select(t => t.Email).ToList(), subject, body, ENotificationType.Email);
                    //Console.WriteLine("Thread Starting");
                    //Console.WriteLine(string.Join(",", toAddress));
                    //Console.WriteLine(subject);
                    //Console.WriteLine(body);
                    //Console.WriteLine(_fromAddress);
                    string logoURL = _config["Locations:BaseURL"] + _config["Locations:LogoURL"];
                    foreach (NotificationRequestRecipient to in toAddress)
                    {
                        Thread emailThread = new(async delegate ()
                        {
                            MailMessage mail = new MailMessage(_fromAddress, to.Email)
                            {
                                Subject = subject,
                                IsBodyHtml = true
                            };
                            try
                            {
                                var pathToFile = Path.Combine(_hostEnvironment.ContentRootPath, "Templates", "EmailTemplate.html");

                                var builder = new BodyBuilder();

                                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                                {
                                    builder.HtmlBody = SourceReader.ReadToEnd();

                                    builder.HtmlBody = builder.HtmlBody.Replace("{{name}}", to.Name).Replace("{{content}}", body).Replace("{{logoUrl}}", logoURL);
                                }
                                mail.Body = builder.HtmlBody;

                                Console.WriteLine("Sending Email...");
                                await _smtpClient.SendMailAsync(mail);

                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine(ex.Message);
                                await HandleExceptionAsync(ex, "EmailFailure");
                            }
                        });
                        emailThread.IsBackground = true;
                        emailThread.Start();
                    };

                    Console.WriteLine("Thread End");
                    //Marking it success as no exception is raised
                    log.IsSuccess = true;
                    _repoWrapper.LoggingRepository.UpdateNoticationLog(log);
                    await _repoWrapper.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private async Task SendEmailForScheduledTask(List<NotificationRequestRecipient> toAddress, string subject, string body)
        {
            try
            {


                if (IsEmail(toAddress))
                {
                    NotificationLog log = await SaveLog(toAddress.Select(t => t.Email).ToList(), subject, body, ENotificationType.Email);
                    //Console.WriteLine("Thread Starting");
                    //Console.WriteLine(string.Join(",", toAddress));
                    //Console.WriteLine(subject);
                    //Console.WriteLine(body);
                    //Console.WriteLine(_fromAddress);
                    string logoURL = _config["Locations:BaseURL"] + _config["Locations:LogoURL"];
                    foreach (NotificationRequestRecipient to in toAddress)
                    {
                        Thread emailThread = new(async delegate ()
                        {
                            MailMessage mail = new MailMessage(_fromAddress, to.Email)
                            {
                                Subject = subject,
                                IsBodyHtml = true
                            };
                            try
                            {
                                var pathToFile = Path.Combine(_hostEnvironment.ContentRootPath, "Templates", "EmailTemplate.html");

                                var builder = new BodyBuilder();

                                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                                {
                                    builder.HtmlBody = SourceReader.ReadToEnd();

                                    builder.HtmlBody = builder.HtmlBody.Replace("{{name}}", to.Name).Replace("{{content}}", body).Replace("{{logoUrl}}", logoURL);
                                }
                                mail.Body = builder.HtmlBody;

                                Console.WriteLine("Sending Email...");
                                await _smtpClient.SendMailAsync(mail);

                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine(ex.Message);
                                await HandleExceptionAsync(ex, "EmailFailure");
                            }
                        });
                        emailThread.IsBackground = true;
                        emailThread.Start();
                    };

                    Console.WriteLine("Thread End");
                    //Marking it success as no exception is raised
                    log.IsSuccess = true;
                    _repoWrapper.LoggingRepository.UpdateNoticationLog(log);
                    await _repoWrapper.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task SendSMS(List<string> phoneNumbers, string message)
        {
            NotificationLog log = await SaveLog(phoneNumbers, "", message, ENotificationType.SMS);
            try
            {
                Thread emailThread = new(async delegate ()
                {
                    foreach (string to in phoneNumbers)
                    {
                        await SendSMS(to, message, "");
                    }
                });
                emailThread.IsBackground = true;
                emailThread.Start();

                //Marking it success as no exception is raised
                log.IsSuccess = true;
                _repoWrapper.LoggingRepository.UpdateNoticationLog(log);
                await _repoWrapper.SaveAsync();
            }
            catch (Exception ex)
            {
                //Marking it success as no exception is raised
                log.IsSuccess = false;
                _repoWrapper.LoggingRepository.UpdateNoticationLog(log);
                await _repoWrapper.SaveAsync();

                await HandleExceptionAsync(ex, "SMSFailure");
            }
        }
        public async Task SendSMS(List<string> phoneNumbers, string message, string OTP)
        {
            NotificationLog log = await SaveLog(phoneNumbers, "", message, ENotificationType.SMS);
            try
            {

                Thread emailThread = new(async delegate ()
                {
                    foreach (string to in phoneNumbers)
                    {
                        await SendSMS(to, message, OTP);
                    }
                });
                emailThread.IsBackground = true;
                emailThread.Start();

                //Marking it success as no exception is raised
                log.IsSuccess = true;
                _repoWrapper.LoggingRepository.UpdateNoticationLog(log);
                await _repoWrapper.SaveAsync();
            }
            catch (Exception ex)
            {
                //Marking it success as no exception is raised
                log.IsSuccess = false;
                _repoWrapper.LoggingRepository.UpdateNoticationLog(log);
                await _repoWrapper.SaveAsync();

                await HandleExceptionAsync(ex, "SMSFailure");
            }
        }


        private async Task SendSMS(string mobileNo, string message, string OTP)
        {
            //NotificationLog log = await SaveLog(mobileNo, "", message, ENotificationType.SMS);

            //"(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])([a-zA-Z0-9]{5,})$"

            Regex pattern = new Regex(@"^([0-9]{11,11})$");

            if (!string.IsNullOrEmpty(mobileNo) /*&& pattern.IsMatch(mobileNo)*/)
            {
                string mobilePrefex = new string(mobileNo.TrimStart('+').Take(2).ToArray());
                //if (mobilePrefex == "03")
                if (mobilePrefex == "92")
                {
                    mobileNo = mobileNo.Replace("+", "").Replace("-", "").Replace(" ", "");
                    string apiURL = !string.IsNullOrEmpty(OTP) ? _smsConfiguration.SMSOTPApi.Replace("[OTPCode]", OTP) : _smsConfiguration.SMSApi.Replace("[MessageData]", message);

                    apiURL = apiURL.Replace("[PhoneNumber]", mobileNo)
                            .Replace("[Action]", _smsConfiguration.SMSAction)
                            .Replace("[UserName]", _smsConfiguration.SMSUserName)
                            .Replace("[Password]", _smsConfiguration.SMSPassword)
                            .Replace("[Originator]", _smsConfiguration.SMSOriginator)
                            .Replace("[PhoneNumber]", mobileNo);

                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.GetAsync(apiURL);
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new AmazonFarmerException(_exceptions.smsResponseNot200);
                        }
                        string responseBody = await response.Content.ReadAsStringAsync();

                        //if (!responseBody.Contains("<action>sendmessage</action>"))
                        //    throw new AmazonFarmerException(_exceptions.smsHasNotSent);
                        //if (!response.IsSuccessStatusCode)
                        //{

                        //}

                    }
                    //var builder = new UriBuilder(_smsConfiguration.SMSApi);
                    //var query = HttpUtility.ParseQueryString(builder.Query);
                    //query["action"] = _smsConfiguration.SMSAction;
                    //query["username"] = _smsConfiguration.SMSUserName;
                    //query["password"] = _smsConfiguration.SMSPassword;
                    //query["recipient"] = mobileNo;
                    //query["originator"] = _smsConfiguration.SMSOriginator;
                    //query["messagedata"] = message;
                    //builder.Query = query.ToString();
                    //string url = builder.ToString();
                    //HttpClient client = new HttpClient();
                    //HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                    //var response = client.SendAsync(requestMessage).GetAwaiter().GetResult();
                    //client.Dispose();
                    //var StatusCode = response.StatusCode;

                    //if (StatusCode == HttpStatusCode.OK)
                    //{
                    //    XDocument xdoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
                    //    string responseCode = xdoc.Descendants("statuscode").Select(x => x.Value).FirstOrDefault();
                    //    string responseMessage = xdoc.Descendants("statusmessage").Select(x => x.Value).FirstOrDefault();
                    //    if (responseMessage == null)
                    //    {
                    //        responseCode = xdoc.Descendants("errorcode").Select(x => x.Value).FirstOrDefault();
                    //        responseMessage = xdoc.Descendants("errormessage").Select(x => x.Value).FirstOrDefault();
                    //    }
                    //    //log.IsSuccess = true;
                    //    //_repoWrapper.LoggingRepository.UpdateNoticationLog(log);
                    //    //await _repoWrapper.SaveAsync();
                    //}
                    //else
                    //{
                    //    //log.IsSuccess = true;
                    //    //_repoWrapper.LoggingRepository.UpdateNoticationLog(log);
                    //    //await _repoWrapper.SaveAsync();
                    //}

                }
            }
        }

        public async Task SendFCM(List<string> deviceIds, string subject, string message)
        {
            NotificationLog log = await SaveLog(deviceIds, subject, message, ENotificationType.FCM);
            try
            {

                Thread emailThread = new(async delegate ()
                {
                    // Here you would implement code to send FCM
                    foreach (var deviceId in deviceIds)
                    {
                        await SendNotification(deviceId, subject, message);
                    }
                });
                emailThread.IsBackground = true;
                emailThread.Start();

                log.IsSuccess = true;
                //await SendFCMNotification(deviceId, subject, message);

                //Marking it success as no exception is raised
                _repoWrapper.LoggingRepository.UpdateNoticationLog(log);
                await _repoWrapper.SaveAsync();
            }
            catch (Exception)
            {
                log.IsSuccess = false;

                //Marking it success as no exception is raised
                _repoWrapper.LoggingRepository.UpdateNoticationLog(log);
                await _repoWrapper.SaveAsync();
            }
        }
        private async Task SendDevice(List<string> userIds, NotificationReplacementDTO? notificationReplacement)
        {
            //notificationReplacement = notificationReplacement ?? new NotificationReplacementDTO();
            if (notificationReplacement != null)
            {
                foreach (var userId in userIds)
                {
                    if (!string.IsNullOrEmpty(userId))
                    {

                        tblNotification notification = new tblNotification()
                        {
                            UserID = userId,
                            DeviceNotificationID = (int)notificationReplacement?.NotificationBodyTypeID,
                            PlanID = string.IsNullOrEmpty(notificationReplacement.PlanID) ? null : Convert.ToInt32(notificationReplacement.PlanID.TrimStart('0')),
                            FarmID = string.IsNullOrEmpty(notificationReplacement.FarmID) ? null : Convert.ToInt32(notificationReplacement.FarmID.TrimStart('0')),
                            OrderID = string.IsNullOrEmpty(notificationReplacement.OrderID) ? null : Convert.ToInt64(notificationReplacement.OrderID.TrimStart('0')),
                            AuthorityLetterID = string.IsNullOrEmpty(notificationReplacement.AuthorityLetterId) || notificationReplacement.AuthorityLetterId == "0" ? null : Convert.ToInt32(notificationReplacement.AuthorityLetterId),
                            AuthorityLetterNo = (notificationReplacement.AuthorityLetterNo),
                            FarmApplicationID = string.IsNullOrEmpty(notificationReplacement.ApplicationId) || notificationReplacement.ApplicationId == "0" ? null : Convert.ToInt32(notificationReplacement.ApplicationId.TrimStart('0')),
                            FarmName = (notificationReplacement.FarmName),
                            NewPaymentDueDate = notificationReplacement.NewPaymentDueDate,
                            ConsumerNumber = notificationReplacement.ConsumerNumber,
                            PKRAmount = notificationReplacement.PKRAmount,
                            WarehouseID = string.IsNullOrEmpty(notificationReplacement.WarehouseId) || notificationReplacement.WarehouseId == "0" ? null : Convert.ToInt32(notificationReplacement.WarehouseId.TrimStart('0')),
                            PickUPDate = notificationReplacement.PickupDate,
                            ReasonsDropdownID = string.IsNullOrEmpty(notificationReplacement.ReasonDropDownOptionId) || notificationReplacement.ReasonDropDownOptionId == "0" ? null : Convert.ToInt32(notificationReplacement.ReasonDropDownOptionId.TrimStart('0')),
                            ReasonCommentBox = notificationReplacement.ReasonComment,
                            GoogleMapLinkWithCoordinated = notificationReplacement.GoogleMapLinkWithCoordinated,
                            CreatedOn = DateTime.UtcNow,
                            IsClicked = false
                        };

                        _repoWrapper.NotificationRepo.addDeviceNotification(notification);
                    }
                }
                await _repoWrapper.SaveAsync();
            }
        }

        private async Task<bool> SendNotification(string sendTo, string title, string text)
        {
            try
            {
                var body = new
                {
                    to = sendTo,
                    notification = new
                    {
                        title = title,
                        body = text,
                        sound = "default"
                    }
                };
                var jsonBody = JsonConvert.SerializeObject(body);

                // Retrieve FCM server key from configuration
                string fcmServerKey = _fcmConfiguration.ServerKey;

                using (HttpClient client = new HttpClient())
                {
                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, _fcmConfiguration.FcmURL);
                    requestMessage.Headers.TryAddWithoutValidation("Authorization", "key=" + fcmServerKey); // Add authorization header with FCM server key
                    requestMessage.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    var response = await client.SendAsync(requestMessage);
                    client.Dispose();

                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task SendNotifications(List<NotificationRequest> notifications, NotificationReplacementDTO notificationReplacement)
        {
            bool isOTP = string.IsNullOrEmpty(notificationReplacement.OTP) ? false : true;

            foreach (var notification in notifications)
            {
                notification.Message = notification.Message
                    .Replace("[PlanID]", notificationReplacement.PlanID)
                    .Replace("[OrderID]", notificationReplacement.OrderID)
                    .Replace("[next approver]", notificationReplacement.Status)
                    .Replace("[UserName]", notificationReplacement.UserName)
                    .Replace("[SendToName]", notificationReplacement.UserName)
                    .Replace("[Farmer]", notificationReplacement.UserName)
                    .Replace("[ApplicationId]", notificationReplacement.ApplicationId)
                    .Replace("[OTP]", notificationReplacement.OTP)
                    .Replace("[AuthorityLetterNo]", notificationReplacement.AuthorityLetterNo)
                    .Replace("[FarmID]", notificationReplacement.FarmID)
                    .Replace("[FarmName]", notificationReplacement.FarmName)
                    .Replace("[FarmerName]", notificationReplacement.farmerName)
                    .Replace("[AuthorityLetterQuantity]", notificationReplacement.AuthorityLetterQuantity)
                    .Replace("[ReasonDropDownOption]", notificationReplacement.ReasonDropDownOption)
                    .Replace("[ReasonComment]", notificationReplacement.ReasonComment)
                    .Replace("[NewPaymentDueDate]", notificationReplacement.NewPaymentDueDate)
                    .Replace("[PKRAmount]", notificationReplacement.PKRAmount)
                    .Replace("[WarehouseName]", notificationReplacement.WarehouseName)
                    .Replace("[GoogleMapLinkWithCoordinated]", notificationReplacement.GoogleMapLinkWithCoordinated)
                    .Replace("[PickupDate]", notificationReplacement.PickupDate)
                    .Replace("[ConsumerNumber]", notificationReplacement.ConsumerNumber)
                    .Replace("[project]", notificationReplacement.Project)
                    .Replace("[password]", notificationReplacement.Password)
                    .Replace("[APPID]", notificationReplacement.ApplicationId)
                    .Replace("[Reasons Dropdown Option]", notificationReplacement.ReasonDropDownOption)
                    .Replace("[Reason Comment Box]", notificationReplacement.ReasonComment)
                    .Replace("[Authority Letter ID]", notificationReplacement.AuthorityLetterId)
                    .Replace("<br/>", Environment.NewLine)
                    .Replace("<br>", Environment.NewLine);

                notification.Subject = notification.Subject
                    .Replace("[PlanID]", notificationReplacement.PlanID)
                    .Replace("[OrderID]", notificationReplacement.OrderID)
                    .Replace("[next approver]", notificationReplacement.Status)
                    .Replace("[UserName]", notificationReplacement.UserName)
                    .Replace("[SendToName]", notificationReplacement.UserName)
                    .Replace("[Farmer]", notificationReplacement.UserName)
                    .Replace("[ApplicationId]", notificationReplacement.ApplicationId)
                    .Replace("[OTP]", notificationReplacement.OTP)
                    .Replace("[AuthorityLetterNo]", notificationReplacement.AuthorityLetterNo)
                    .Replace("[FarmID]", notificationReplacement.FarmID)
                    .Replace("[FarmName]", notificationReplacement.FarmName)
                    .Replace("[AuthorityLetterQuantity]", notificationReplacement.AuthorityLetterQuantity)
                    .Replace("[ReasonDropDownOption]", notificationReplacement.ReasonDropDownOption)
                    .Replace("[ReasonComment]", notificationReplacement.ReasonComment)
                    .Replace("[NewPaymentDueDate]", notificationReplacement.NewPaymentDueDate)
                    .Replace("[PKRAmount]", notificationReplacement.PKRAmount)
                    .Replace("[WarehouseName]", notificationReplacement.WarehouseName)
                    .Replace("[GoogleMapLinkWithCoordinated]", notificationReplacement.GoogleMapLinkWithCoordinated)
                    .Replace("[PickupDate]", notificationReplacement.PickupDate)
                    .Replace("[ConsumerNumber]", notificationReplacement.ConsumerNumber)
                    .Replace("[project]", notificationReplacement.Project)
                    .Replace("[password]", notificationReplacement.Password)
                    .Replace("[APPID]", notificationReplacement.ApplicationId)
                    .Replace("[Reasons Dropdown Option]", notificationReplacement.ReasonDropDownOption)
                    .Replace("[Reason Comment Box]", notificationReplacement.ReasonComment)
                    .Replace("[Authority Letter ID]", notificationReplacement.AuthorityLetterId)
                    .Replace("<br/>", Environment.NewLine)
                    .Replace("<br>", Environment.NewLine);

                switch (notification.Type)
                {
                    case ENotificationType.Email:
                        await SendEmail(notification.Recipients, notification.Subject, notification.Message);
                        break;
                    case ENotificationType.SMS:
                        await SendSMS(notification.Recipients.Select(a => a.Email).ToList(), notification.Message, notificationReplacement.OTP);
                        break;
                    case ENotificationType.FCM:
                        await SendFCM(notification.Recipients.Select(a => a.Email).ToList(), notification.Subject, notification.Message);
                        break;
                    case ENotificationType.Device:
                        await SendDevice(notification.Recipients.Select(a => a.Email).ToList(), notificationReplacement);
                        break;
                    default:
                        throw new ArgumentException("Invalid notification type");
                }
            }
        }
        public async Task SendNotificationsForSceduledTask(List<NotificationRequest> notifications)
        {
            foreach (var notification in notifications)
            {
                switch (notification.Type)
                {
                    case ENotificationType.Email:
                        await SendEmailForScheduledTask(notification.Recipients, notification.Subject, notification.Message);
                        break;
                    case ENotificationType.SMS:
                        await SendSMS(notification.Recipients.Select(r => r.Email).ToList(), notification.Message);
                        break;
                    case ENotificationType.FCM:
                        await SendFCM(notification.Recipients.Select(r => r.Email).ToList(), notification.Subject, notification.Message);
                        break;
                    case ENotificationType.Device:
                        await SendDevice(notification.Recipients.Select(a => a.Email).ToList(), null);
                        break;
                    default:
                        throw new ArgumentException("Invalid notification type");
                }
            }
        }

        private static bool IsEmail(List<NotificationRequestRecipient> emailaddress)
        {
            try
            {
                foreach (var email in emailaddress)
                {
                    MailAddress m = new MailAddress(email.Email);
                }

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        //For Exception Logging 
        private async Task HandleExceptionAsync(Exception exception, string requestTypeName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            var exceptionDetails = GetExceptionDetails(exception);
            var json = JsonConvert.SerializeObject(exceptionDetails);
            string filePath = Path.Combine(currentDirectory, $"logs/Exceptions/{requestTypeName}_{DateTime.UtcNow:yyyy-MM-ddThhmmss-7199222}.json");
            await WriteFileAsync(filePath, json);
        }

        private ExceptionDetails GetExceptionDetails(Exception ex)
        {
            if (ex == null) return null;

            return new ExceptionDetails
            {
                Type = ex.GetType().FullName,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Data = ex.Data,
                InnerException = GetExceptionDetails(ex.InnerException)
            };
        }

        private async Task WriteFileAsync(string filePath, string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await stream.WriteAsync(byteData, 0, byteData.Length);
            }
        }

    }

}
