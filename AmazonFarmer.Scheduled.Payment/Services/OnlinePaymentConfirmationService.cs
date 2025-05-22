using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.NotificationServices.Helpers;
using AmazonFarmer.Scheduled.Payment.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Scheduled.Payment.Services
{
    public class OnlinePaymentConfirmationService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private Reconfirmation_API_Configuration _reconfirmationConfiguration = new Reconfirmation_API_Configuration();
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public OnlinePaymentConfirmationService(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            IOptions<Reconfirmation_API_Configuration> reconfirmationConfiguration
            )
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _reconfirmationConfiguration = reconfirmationConfiguration.Value;
        }

        public async Task ProcessPendingOnlinePaymentsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var request = new Reconfirmation_API_Request
                {
                    Username = _reconfirmationConfiguration.Username,
                    Password = _reconfirmationConfiguration.Password
                };
                var _repoWrapper = scope.ServiceProvider.GetRequiredService<IRepositoryWrapper>();
                var paymentService = scope.ServiceProvider.GetRequiredService<PaymentService>();
                List<tblTransaction> pendingTransactions = await _repoWrapper.OnlinePaymentRepo.getAllPendingTransactions();
                foreach (var transaction in pendingTransactions.Where(pt => pt.TransactionStatusCheckAttempts <= 3).ToList())
                {
                    request.Consumer_No = transaction.ConsumerCode;
                    request.UCID = "IN" + transaction.Prefix;
                    Reconfirmation_API_Response? Reconfirmation = await ReconfirmAsync(request);
                    if (Reconfirmation!.Response_Code == "00")
                    {
                        if (transaction.TransactionStatus == ETransactionStatus.Pending)
                        {
                            await paymentService.TransactionLedgeUpdate(transaction);
                            await paymentService.TransactionFulfilment(transaction, scope, _configuration);
                        }
                        else if (transaction.TransactionStatus == ETransactionStatus.SapLedgerUpdated)
                        {
                            await paymentService.TransactionFulfilment(transaction, scope, _configuration);
                        }

                    }
                    else
                    {
                        transaction.TransactionStatusCheckAttempts = (transaction.TransactionStatusCheckAttempts ?? 0) + 1;
                        await paymentService.TransactionUpdate(transaction);
                    }
                }
            }
        }

        private async Task<Reconfirmation_API_Response?> ReconfirmAsync(Reconfirmation_API_Request request)
        {
            string url = _reconfirmationConfiguration.URL;// "https://api-dev.engro.com/1-link/v1/ps";
            string apiKey = _reconfirmationConfiguration.APIKey;// "PWKq06wDQP7HDwwqiB0BZAuD4Xv00iBzpO7JDPJHtHFe9vkU";

            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("apikey", apiKey);

            HttpResponseMessage response = await _httpClient.PostAsync(url, jsonContent);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            return System.Text.Json.JsonSerializer.Deserialize<Reconfirmation_API_Response?>(responseBody);
        }
    }
}
