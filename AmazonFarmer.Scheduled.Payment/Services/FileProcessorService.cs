using ClosedXML.Excel;

namespace AmazonFarmer.Scheduled.Payment.Services
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using AmazonFarmer.Core.Application;
    using AmazonFarmer.Core.Application.DTOs;
    using AmazonFarmer.Core.Application.Exceptions;
    using AmazonFarmer.Core.Domain.Entities;
    using AmazonFarmer.Infrastructure.Persistence;
    using AmazonFarmer.NotificationServices.Services;
    using AmazonFarmer.Scheduled.Payment.DTOs;
    using Azure.Storage.Files.Shares;
    using Azure.Storage.Files.Shares.Models;
    using FirebaseAdmin.Messaging;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Org.BouncyCastle.Ocsp;
    using Renci.SshNet;

    public class FileProcessorService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public FileProcessorService(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        

        private async Task OrderPaymentReconfirmation()
        {

        }

        public async Task ProcessFilesAsync()
        {

            FileExtractDTO fileExtract = await ExtractFile();


            var localDirectories = Directory.GetDirectories(fileExtract.inputDirectory);

            foreach (var directory in localDirectories)
            {
                string extractedDirectoryName = Path.GetFileName(directory);
                if (extractedDirectoryName != null && extractedDirectoryName.StartsWith(fileExtract.companyFileName))
                {
                    var txtFiles = Directory.GetFiles(directory, fileExtract.extractedMISFileName, SearchOption.AllDirectories);

                    foreach (var txtFile in txtFiles)
                    {
                        await ProcessFileAsync(txtFile);
                    }

                    //Move extracted directories in to archive folder
                    Directory.Move(directory, Path.Combine(fileExtract.archiveDirectory, extractedDirectoryName));
                }

            }
        }

        private async Task<FileExtractDTO> ExtractFile()
        {
            var resp = new FileExtractDTO();
            string currentDirectory = Directory.GetCurrentDirectory();

            var inputDirectory = currentDirectory + _configuration["FileSettings:InputDirectory"];
            var archiveDirectory = currentDirectory + _configuration["FileSettings:ArchiveDirectory"];

            var host = _configuration["FileSettings:host"];
            var port = _configuration["FileSettings:port"];
            var username = _configuration["FileSettings:username"];
            var password = _configuration["FileSettings:password"];
            var remoteDirectory = _configuration["FileSettings:ServerPath"];
            var remoteArchiveDirectory = _configuration["FileSettings:ServerArchive"];
            var companyFileName = _configuration["FileSettings:CompanyFileName"];
            var extractedMISFileName = _configuration["FileSettings:ExtractedMISFileName"];

            using (var sftp = new SftpClient(host, Convert.ToInt32(port), username, password))
            {
                try
                {
                    // Connect to the SFTP server
                    sftp.Connect();
                    // Download a file
                    var remoteFiles = sftp.ListDirectory(remoteDirectory)
                                .Where(file => file.IsRegularFile && file.Name.StartsWith(companyFileName) && file.Name.EndsWith(".zip"))
                                .ToList();

                    foreach (var file in remoteFiles)
                    {
                        string remoteFilePath = $"{remoteDirectory}/{file.Name}";
                        string remoteArchiveFilePath = $"{remoteArchiveDirectory}/{file.Name}";
                        string localFilePath = Path.Combine(inputDirectory, file.Name);
                        string extractDirectory = Path.Combine(inputDirectory, Path.GetFileNameWithoutExtension(file.Name));

                        // Download the file
                        using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                        {
                            sftp.DownloadFile(remoteFilePath, fileStream);
                        }

                        // Move the file to the archive directory
                        sftp.RenameFile(remoteFilePath, remoteArchiveFilePath);
                        //Console.WriteLine($"Downloaded {file.Name} to {localFilePath}");

                        // Extract the file to a directory with the same name as the file
                        Directory.CreateDirectory(extractDirectory);
                        System.IO.Compression.ZipFile.ExtractToDirectory(localFilePath, extractDirectory);

                        //Move ZIp file into local archive directory
                        MoveFileToArchive(localFilePath, archiveDirectory);
                    }

                }
                catch (Exception ex)
                {
                }
                finally
                {
                    // Disconnect from the SFTP server
                    sftp.Disconnect();
                }
            }

            resp = new FileExtractDTO()
            {
                companyFileName = companyFileName,
                inputDirectory = inputDirectory,
                extractedMISFileName = extractedMISFileName,
                archiveDirectory = archiveDirectory,
            };
            return resp;
        }

        private async Task ProcessFileAsync(string filePath)
        {

            string fileName = Path.GetFileName(filePath);

            using (var scope = _serviceProvider.CreateScope())
            {
                var paymentService = scope.ServiceProvider.GetRequiredService<PaymentService>();
                var context = scope.ServiceProvider.GetRequiredService<AmazonFarmerContext>();
                var _repoWrapper = scope.ServiceProvider.GetRequiredService<IRepositoryWrapper>();

                var lines = await File.ReadAllLinesAsync(filePath);
                string processingCompanyName = _configuration["CompanyName"].ToString();
                var acknowledgmentFile = new PaymentAcknowledgmentFile
                {
                    DateReceived = DateTime.UtcNow,
                    Comment = "",
                    FileName = filePath,
                    Status = EPaymentAcknowledgmentFileStatus.Received,
                    RowsCount = lines.Where(l => l.StartsWith(processingCompanyName)).Count()
                };

                acknowledgmentFile = context.PaymentAcknowledgmentFiles.Add(acknowledgmentFile).Entity;
                await context.SaveChangesAsync();

                int rowNumber = 0;
                List<int> processedRows = new List<int>();
                List<int> failedRows = new List<int>();
                int totalRows = acknowledgmentFile.RowsCount;

                PaymentAcknowledgment paymentAcknowledgment = null;

                foreach (var line in lines)
                {
                    rowNumber += 1;

                    if (line.Length == 140)
                    {
                        if (line.StartsWith(processingCompanyName))
                        {
                            try
                            {
                                paymentAcknowledgment = new PaymentAcknowledgment
                                {
                                    CompanyName = line.Substring(0, 8).Trim(),
                                    ConsumerNumber = line.Substring(8, 20).Trim(),
                                    AccountNumber = line.Substring(28, 37).Trim(),
                                    MaskedConsumerNumber = line.Substring(65, 20).Trim(),
                                    Amount = line.Substring(85, 14).Trim(),
                                    DatePaid = line.Substring(99, 8).Trim(),
                                    TimePaid = line.Substring(107, 6).Trim(),
                                    SettlementDate = line.Substring(113, 8).Trim(),
                                    PaymentMode = line.Substring(121, 1).Trim(),
                                    BankName = line.Substring(122, 6).Trim(),
                                    Trans_Auth_ID = line.Substring(128, 6).Trim(),
                                    STAN = line.Substring(134, 6).Trim(),
                                    DateReceived = DateTime.UtcNow,
                                    Status = EPaymentAcknowledgmentStatus.Imported,
                                    FileID = acknowledgmentFile.Id
                                };
                                var request = new PaymentAcknowledgmentRequest
                                {
                                    Tran_Auth_ID = paymentAcknowledgment.Trans_Auth_ID,
                                    ConsumerNumber = paymentAcknowledgment.ConsumerNumber,
                                    Amount = Convert.ToDecimal(paymentAcknowledgment.Amount) / 100
                                };

                                paymentAcknowledgment = context.PaymentAcknowledgments.Add(paymentAcknowledgment).Entity;
                                await context.SaveChangesAsync();

                                await paymentService.PostTransactionAcknowledgmentUpdate(request, scope, _configuration);

                                paymentAcknowledgment.Status = EPaymentAcknowledgmentStatus.Processed;
                                context.PaymentAcknowledgments.Update(paymentAcknowledgment);
                                await context.SaveChangesAsync();

                                processedRows.Add(rowNumber);

                            }
                            catch (Exception ex)
                            {
                                if (paymentAcknowledgment != null)
                                {
                                    paymentAcknowledgment.Comment = ex.Message + "::" + ex.StackTrace;
                                    if (ex is AmazonFarmerException)
                                        paymentAcknowledgment.Status = EPaymentAcknowledgmentStatus.Failed;
                                    else
                                        paymentAcknowledgment.Status = EPaymentAcknowledgmentStatus.FailedReprocessable;
                                    context.PaymentAcknowledgments.Update(paymentAcknowledgment);
                                    await context.SaveChangesAsync();
                                }
                                failedRows.Add(rowNumber);
                            }
                        }
                    }
                    else if (line.Length > 0)
                    {
                        if (paymentAcknowledgment != null)
                        {
                            acknowledgmentFile.Comment += string.Format("Row length not valid on row number: {0}. ", rowNumber);
                            acknowledgmentFile = context.PaymentAcknowledgmentFiles.Update(acknowledgmentFile).Entity;
                            await context.SaveChangesAsync();
                        }
                    }

                }

                if (failedRows.Count() > 0)
                {
                    if (failedRows.Count() == totalRows)
                    {

                        acknowledgmentFile.Comment = "All Rows Failed";
                        acknowledgmentFile.Status = EPaymentAcknowledgmentFileStatus.Failed;
                        acknowledgmentFile = context.PaymentAcknowledgmentFiles.Update(acknowledgmentFile).Entity;
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        acknowledgmentFile.Comment = string.Format("Row length not valid on row numbers: {0}. ", string.Join(",", failedRows));
                        acknowledgmentFile.Status = EPaymentAcknowledgmentFileStatus.PartiallyImported;
                        acknowledgmentFile = context.PaymentAcknowledgmentFiles.Update(acknowledgmentFile).Entity;
                        await context.SaveChangesAsync();

                    }
                }
                else
                {
                    acknowledgmentFile.Comment = "Processed Success";
                    acknowledgmentFile.Status = EPaymentAcknowledgmentFileStatus.Processed;
                    acknowledgmentFile = context.PaymentAcknowledgmentFiles.Update(acknowledgmentFile).Entity;
                    await context.SaveChangesAsync();
                }

                var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
                List<NotificationRequest> notifications = new();
                List<string> recepientEmails = _configuration["EmailConfiguration:toFinanceEmail"].ToString().Split(',').ToList();
                List<NotificationRequestRecipient> recipients = new List<NotificationRequestRecipient>();
                foreach (string recepientEmail in recepientEmails)
                {
                    recipients.Add(new NotificationRequestRecipient { Email = recepientEmail, Name = "Admin" });
                }
                NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.FinancePaymentReport, "EN");

                if (notificationDTO != null)
                {
                    var empEmailNotification = new NotificationRequest
                    {
                        Type = ENotificationType.Email,
                        Recipients = recipients,
                        Subject = notificationDTO.title,
                        Message = notificationDTO.body
                    .Replace("[filename]", fileName)
                    .Replace("[TotalRowsReceived]", totalRows.ToString())
                    .Replace("[TotalRowsProcessed]", processedRows.Count().ToString())
                    .Replace("[TotalRowsFailed]", failedRows.Count().ToString())
                    };
                    notifications.Add(empEmailNotification);
                }
                await notificationService.SendNotificationsForSceduledTask(notifications);
            }
        }

        private void MoveFileToArchive(string filePath, string archiveDirectory)
        {
            var fileName = Path.GetFileName(filePath);
            var archivePath = Path.Combine(archiveDirectory, fileName);
            File.Move(filePath, archivePath);
        }

        public async Task CheckDownloads()
        {

            string currentDirectory = Directory.GetCurrentDirectory();

            var inputDirectory = currentDirectory + _configuration["FileSettings:InputDirectory"];
            var archiveDirectory = currentDirectory + _configuration["FileSettings:ArchiveDirectory"];

            var host = _configuration["FileSettings:host"];
            var port = _configuration["FileSettings:port"];
            var username = _configuration["FileSettings:username"];
            var password = _configuration["FileSettings:password"];
            var remoteDirectory = _configuration["FileSettings:ServerPath"];
            var remoteArchiveDirectory = _configuration["FileSettings:ServerArchive"];


            using (var sftp = new SftpClient(host, Convert.ToInt32(port), username, password))
            {
                try
                {
                    // Connect to the SFTP server
                    sftp.Connect();
                    // Download a file
                    var remoteFiles = sftp.ListDirectory(remoteDirectory)
                                .Where(file => file.IsRegularFile && file.Name.EndsWith(".txt"))
                                .ToList();

                    foreach (var file in remoteFiles)
                    {
                        string remoteFilePath = $"{remoteDirectory}/{file.Name}";
                        string dateTimeFileName = file.Name.Replace(".txt", "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt");
                        string remoteArchiveFilePath = $"{remoteArchiveDirectory}/{dateTimeFileName}";
                        string localFilePath = Path.Combine(inputDirectory, file.Name);

                        // Ensure the local directory exists
                        Directory.CreateDirectory(inputDirectory);

                        // Download the file
                        using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                        {
                            sftp.DownloadFile(remoteFilePath, fileStream);
                            // Move the file to the archive directory
                            sftp.RenameFile(remoteFilePath, remoteArchiveFilePath);
                            //Console.WriteLine($"Downloaded {file.Name} to {localFilePath}");
                        }
                    }

                }
                catch (Exception ex)
                {
                }
                finally
                {
                    // Disconnect from the SFTP server
                    sftp.Disconnect();
                }
            }

            var localFiles = Directory.GetFiles(inputDirectory, "*.txt");

            foreach (var file in localFiles)
            {
                await ProcessFileAsync(file);
                MoveFileToArchive(file, archiveDirectory);
            }
        }

        public async Task xyz()
        {
            var connectionString = _configuration["AzureFileStorage:ConnectionString"];
            var shareName = _configuration["AzureFileStorage:ShareName"];
            var directoryName = _configuration["AzureFileStorage:ServiceReportDirectory"];
            try
            {
                ShareClient shareClient = new ShareClient(connectionString, shareName);
                ShareDirectoryClient directoryClient = shareClient.GetDirectoryClient(directoryName);

                List<getSoilSampleList> lst = new List<getSoilSampleList>();

                lst = await ListFilesAndDirectoriesAsync(directoryClient, "", directoryName);


            }
            catch (Exception)
            {
                throw;
            }
        }
        private static async Task<List<getSoilSampleList>> ListFilesAndDirectoriesAsync
            (ShareDirectoryClient directoryClient, string startsWith, string filePath)
        {
            List<getSoilSampleList> reportFiles = new List<getSoilSampleList>();
            await foreach (ShareFileItem item in directoryClient.GetFilesAndDirectoriesAsync())
            {

                if (!item.IsDirectory)
                {
                    if (item.Name.ToLower().StartsWith(startsWith.ToLower()) && item.Name.ToLower().EndsWith(".pdf"))
                    {
                        getSoilSampleList ServiceReport = new getSoilSampleList()
                        {
                            fileName = item.Name.Replace((startsWith), ""),
                            filePath = string.Concat(filePath, "/", item.Name),
                            fileType = ".pdf",
                            //modifiedOn = await ReturnDateTime(item.Name)
                        };
                        reportFiles.Add(ServiceReport);
                    }
                }
            }
            return reportFiles;
        }




        public async Task ExportTables()
        {

            string connectionString = "Server=66.135.60.203,1433;Database=amazonfarmer_Dev;User ID=farmerdb;password=Heart_109c;Trusted_Connection=false;MultipleActiveResultSets=false;TrustServerCertificate=True";
            string[] queries = new[]
            {
            "Select * from ProductCategory",
            "Select * from ProductCategoryTranslation",
            "Select * from Products",
            "Select * from ProductTranslations",
            "Select * from Crops",
            "Select * from CropTranslation",
            "Select * from Regions",
            "Select * from RegionLanguages",
            "Select * from District",
            "Select * from DistrictLanguages",
            "Select * from Cities",
            "Select * from CityLanguages",
            "Select * from Tehsils",
            "select * from TehsilLanguages",
            "Select * from Warehouse",
            "Select * from WarehouseTranslation",
            "Select * from Service",
            "Select * from ServiceTranslation",
            "Select * from DeviceNotification",
            "Select * from DeviceNotificationTranslations",
            "Select * from EmailNotifications",
            "Select * from WeatherIcon",
            "Select * from WeatherIconTranslations"
        };


            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var workbook = new XLWorkbook())
                {
                    foreach (var query in queries)
                    {
                        var dataTable = new DataTable();
                        using (var command = new SqlCommand(query, connection))
                        {
                            using (var adapter = new SqlDataAdapter(command))
                            {
                                adapter.Fill(dataTable);
                            }
                        }

                        if (dataTable.Rows.Count > 0)
                        {
                            var sheetName = GetSheetNameFromQuery(query);
                            var worksheet = workbook.Worksheets.Add(sheetName);

                            for (int i = 0; i < dataTable.Columns.Count; i++)
                            {
                                worksheet.Cell(1, i + 1).Value = dataTable.Columns[i].ColumnName;
                            }

                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                for (int j = 0; j < dataTable.Columns.Count; j++)
                                {
                                    worksheet.Cell(i + 2, j + 1).Value = Convert.ToString(dataTable.Rows[i][j]);
                                }
                            }

                            worksheet.Columns().AdjustToContents();
                        }
                    }

                    workbook.SaveAs("DatabaseExport.xlsx");
                }
            }
        }

        private static DataTable ToDataTable<T>(IList<T> data)
        {
            var dataTable = new DataTable(typeof(T).Name);

            // Get all the properties by using reflection
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                // Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }

            foreach (var item in data)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    // Insert property values to DataTable rows
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        private static string GetSheetNameFromQuery(string query)
        {
            var fromIndex = query.IndexOf("from", StringComparison.OrdinalIgnoreCase);
            var endIndex = query.IndexOf(" ", fromIndex + 5);
            if (endIndex == -1) endIndex = query.Length;

            return query.Substring(fromIndex + 5, endIndex - fromIndex - 5).Trim();
        }
    }

}
