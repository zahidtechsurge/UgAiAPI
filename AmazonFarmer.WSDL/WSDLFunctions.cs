using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using AmazonFarmer.WSDL.Helpers;
using AmazonFarmer.Core.Application.DTOs;
using CreateCustomer;
using ChangeCustomer;
using AmazonFarmer.Infrastructure.Persistence;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Core.Application;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SimulatePrice;
using BalanceCustomer;
using PaymentCustomer;
using System.Diagnostics.Contracts;
using CreateOrder;
using ChangeCustomerPayment;
using ChangeSaleOrder;
using Microsoft.EntityFrameworkCore;
using DetailsInvoice;
using Microsoft.Extensions.Options;

namespace AmazonFarmer.WSDL
{
    public class WSDLFunctions
    {
        private IRepositoryWrapper _repoWrapper;
        private IConfiguration _configuration;
        private WsdlConfig _wsdlConfig;

        public static string WSDLDateFormat { get { return "yyyy-MM-dd"; } }
        // Constructor injection of IRepositoryWrapper.
        public WSDLFunctions(IRepositoryWrapper repoWrapper, WsdlConfig wsdlConfig)
        {
            _repoWrapper = repoWrapper;
            _wsdlConfig = wsdlConfig;

        }
        public async Task<CreateCustomer.ResponseType?> CreateCustomerWSDLAsync(CreateCustomer.RequestType request)
        {
            CustomerCreateClient OutClient = new CustomerCreateClient(CustomerCreateClient.EndpointConfiguration.CustomerCreateSOAP);


            string WSDLUserName = _wsdlConfig.UserName;
            string WSDLPassword = _wsdlConfig.Password;
            string WSDLEndPoint = _wsdlConfig.EndPoints.Where(x => x.EndpointConfiguration == "CreateCustomerWSDLAsync").First().Url;

            OutClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(_wsdlConfig.BaseUrl + WSDLEndPoint);
            OutClient.ClientCredentials.UserName.UserName = WSDLUserName;
            OutClient.ClientCredentials.UserName.Password = WSDLPassword;

            WSDLLog log = await LogRequestWsdl(request, OutClient.Endpoint.Address.Uri.ToString(), "SOAP");

            using (OperationContextScope scope = new OperationContextScope(OutClient.InnerChannel))
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] =
                    "Basic " +
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            OutClient.ClientCredentials.UserName.UserName + ":" +
                            OutClient.ClientCredentials.UserName.Password
                        )
                    );
                Message[] Messages = new Message[10];
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                string json = @"{
                  ""custNum"": ""0045000116"",
                  ""Messages"": [
                    {
                      ""Message"": {
                        ""msgNo"": ""999"",
                        ""msgTyp"": ""S"",
                        ""msg"": ""Customer Created Successfully"",
                        ""msgCat"": """"
                      }
                    }
                  ]
                }";
                var resp = JsonConvert.DeserializeObject<CreateCustomer.ResponseType>(json); ;
                if (!_wsdlConfig.IsDevMode)
                    resp = OutClient.Create(request);

                await LogResponseWsdl(log, resp);
                return resp;

            }

            return null;

        }
        public async Task<ChangeCustomer.ResponseType?> ChangeCustomerWSDLAsync(ChangeCustomer.RequestType request)
        {

            CustomerChangeClient OutClient = new ChangeCustomer.CustomerChangeClient(ChangeCustomer.CustomerChangeClient.EndpointConfiguration.CustomerChangeSOAP);


            string WSDLUserName = _wsdlConfig.UserName;
            string WSDLPassword = _wsdlConfig.Password;
            string WSDLEndPoint = _wsdlConfig.EndPoints.Where(x => x.EndpointConfiguration == "ChangeCustomerWSDLAsync").First().Url;

            OutClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(_wsdlConfig.BaseUrl + WSDLEndPoint);
            OutClient.ClientCredentials.UserName.UserName = WSDLUserName;
            OutClient.ClientCredentials.UserName.Password = WSDLPassword;

            WSDLLog log = await LogRequestWsdl(request, OutClient.Endpoint.Address.Uri.ToString(), "SOAP");
            using (OperationContextScope scope = new OperationContextScope(OutClient.InnerChannel))
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] =
                    "Basic " +
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            OutClient.ClientCredentials.UserName.UserName + ":" +
                            OutClient.ClientCredentials.UserName.Password
                        )
                    );
                Message[] Messages = new Message[10];
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                string json = @"{
                  ""custNum"": ""0045000099"",
                  ""Messages"": [
                    {
                      ""Message"": {
                        ""msgNo"": ""999"",
                        ""msgTyp"": ""S"",
                        ""msg"": ""Customer Updated Successfully"",
                        ""msgCat"": """"
                      }
                    }
                  ]
                }";
                var resp = JsonConvert.DeserializeObject<ChangeCustomer.ResponseType>(json); ;
                if (!_wsdlConfig.IsDevMode)
                    resp = OutClient.Change(request);
                await LogResponseWsdl(log, resp);
                return resp;

            }

            return null;

        }
        public async Task<SimulatePrice.ResponseType> PriceSimluate(SimulatePrice.RequestType request)
        {

            PriceSimulateClient OutClient = new PriceSimulateClient(PriceSimulateClient.EndpointConfiguration.PriceSimulateSOAP);

            string WSDLUserName = _wsdlConfig.UserName;
            string WSDLPassword = _wsdlConfig.Password;
            string WSDLEndPoint = _wsdlConfig.EndPoints.Where(x => x.EndpointConfiguration == "PriceSimluate").First().Url;

            OutClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(_wsdlConfig.BaseUrl+ WSDLEndPoint);
            OutClient.ClientCredentials.UserName.UserName = WSDLUserName;
            OutClient.ClientCredentials.UserName.Password = WSDLPassword;

            WSDLLog log = await LogRequestWsdl(request, OutClient.Endpoint.Address.Uri.ToString(), "SOAP");
            //WSDLLog log = await LogRequestWsdl(request, OutClient.Endpoint.Address.Uri.AbsolutePath, "SOAP");
            using (OperationContextScope scope = new OperationContextScope(OutClient.InnerChannel))
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] =
                    "Basic " +
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            OutClient.ClientCredentials.UserName.UserName + ":" +
                            OutClient.ClientCredentials.UserName.Password
                        )
                    );
                Message[] Messages = new Message[10];
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                string json = @"
                    {
                        ""itemNum"": ""000010"",
                        ""matNum"": ""000000000010000001"",
                        ""matDesc"": ""ENGRO UREA"",
                        ""reqQty"": ""         1.000"",
                        ""plant"": ""F078"",
                        ""uom"": ""BAG"",
                        ""netVal"": ""      10676.87"",
                        ""taxVal"": ""       1111.79"",
                        ""curr"": ""PKR"",
                        ""Messages"": [
                            {
                                ""Message"": {
                                    ""msgNo"": ""999"",
                                    ""msgTyp"": ""S"",
                                    ""msg"": """",
                                    ""msgCat"": """"
                                }
                            }
                        ]
                    }";
                var resp = JsonConvert.DeserializeObject<SimulatePrice.ResponseType>(json);
                if (!_wsdlConfig.IsDevMode)
                    resp = OutClient.Simulate(request);
                await LogResponseWsdl(log, resp);
                return resp;

            }

            return null;
        }
        public async Task<CustomerBalanceDTO> CustomerBalance(ZSD_AMAZON_CUSTOMER_BAL request)
        {

            BalanceCustomer.ZSD_AMAZON_CUSTOMER_BAL_SRVClient OutClient = new ZSD_AMAZON_CUSTOMER_BAL_SRVClient(ZSD_AMAZON_CUSTOMER_BAL_SRVClient.EndpointConfiguration.binding);


            string WSDLUserName = _wsdlConfig.UserName;
            string WSDLPassword = _wsdlConfig.Password;
            string WSDLEndPoint = _wsdlConfig.EndPoints.Where(x => x.EndpointConfiguration == "CustomerBalance").First().Url;

            OutClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(_wsdlConfig.BaseUrl + WSDLEndPoint);
            OutClient.ClientCredentials.UserName.UserName = WSDLUserName;
            OutClient.ClientCredentials.UserName.Password = WSDLPassword;

            WSDLLog log = await LogRequestWsdl(request, OutClient.Endpoint.Address.Uri.ToString(), "SOAP");
            using (OperationContextScope scope = new OperationContextScope(OutClient.InnerChannel))
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] =
                    "Basic " +
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            OutClient.ClientCredentials.UserName.UserName + ":" +
                            OutClient.ClientCredentials.UserName.Password
                        )
                    );
                Message[] Messages = new Message[10];
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                string json = @"{
                  ""BILLING_VAL"": 0,
                  ""CREDIT_EXP_VAL"": -132027695.9,
                  ""DELIVERY_VAL"": 0,
                  ""ET_RETURN"": [
                    {
                      ""MSG"": """",
                      ""MSGNR"": ""999"",
                      ""MSGTYP"": ""S"",
                      ""CATEGORY"": """"
                    }
                  ],
                  ""OPEN_INVOICE_VAL"": 0,
                  ""OPEN_ORDER_VAL"": 126414240
                }";
                var resp = JsonConvert.DeserializeObject<BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALResponse>(json);

                if (!_wsdlConfig.IsDevMode)
                    resp = OutClient.ZSD_AMAZON_CUSTOMER_BAL(request);

                decimal customerBalance = resp.CREDIT_EXP_VAL + resp.OPEN_ORDER_VAL + resp.OPEN_INVOICE_VAL + resp.DELIVERY_VAL + resp.BILLING_VAL;
                customerBalance = -1 * customerBalance;

                CustomerBalanceDTO customerBalanceDTO = new()
                {
                    bILLING_VALField = resp.BILLING_VAL,
                    cREDIT_EXP_VALField = resp.CREDIT_EXP_VAL,
                    CustomerBalance = customerBalance,
                    dELIVERY_VALField = resp.DELIVERY_VAL,
                    oPEN_INVOICE_VALField = resp.OPEN_INVOICE_VAL,
                    oPEN_ORDER_VALField = resp.OPEN_ORDER_VAL,
                    Message = resp.ET_RETURN.FirstOrDefault()?.MSG,
                    MessageType = resp.ET_RETURN.FirstOrDefault()?.MSGTYP
                };
                await LogResponseWsdl(log, resp);
                return customerBalanceDTO;

            }

            return null;
        }
        public async Task<ZSD_AMAZON_CUSTOMER_PAYMENTResponse> CustomerPayment(ZSD_AMAZON_CUSTOMER_PAYMENT request)
        {

            PaymentCustomer.ZSD_AMA_CUSTOMER_PAYMENT_SRVClient OutClient = new ZSD_AMA_CUSTOMER_PAYMENT_SRVClient(ZSD_AMA_CUSTOMER_PAYMENT_SRVClient.EndpointConfiguration.binding);


            string WSDLUserName = _wsdlConfig.UserName;
            string WSDLPassword = _wsdlConfig.Password;
            string WSDLEndPoint = _wsdlConfig.EndPoints.Where(x => x.EndpointConfiguration == "CustomerPayment").First().Url;

            OutClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(_wsdlConfig.BaseUrl + WSDLEndPoint);
            OutClient.ClientCredentials.UserName.UserName = WSDLUserName;
            OutClient.ClientCredentials.UserName.Password = WSDLPassword;

            WSDLLog log = await LogRequestWsdl(request, OutClient.Endpoint.Address.Uri.ToString(), "SOAP");
            using (OperationContextScope scope = new OperationContextScope(OutClient.InnerChannel))
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] =
                    "Basic " +
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            OutClient.ClientCredentials.UserName.UserName + ":" +
                            OutClient.ClientCredentials.UserName.Password
                        )
                    );
                Message[] Messages = new Message[10];
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                string json = @"{
                  ""DOC_NUM"": ""1400000168"",
                  ""ECOMPANY_CODE"": ""2000"",
                  ""ET_RETURN"": [
                    {
                      ""MSG"": ""Document Posted"",
                      ""MSGNR"": ""999"",
                      ""MSGTYP"": ""S"",
                      ""CATEGORY"": """"
                    }
                  ],
                  ""FISCAL_YEAR"": ""2024"",
                  ""OBJECT_KEY"": ""1400000168/2000/2024""
                }";
                var resp = JsonConvert.DeserializeObject<PaymentCustomer.ZSD_AMAZON_CUSTOMER_PAYMENTResponse>(json);
                if (!_wsdlConfig.IsDevMode)
                    resp = OutClient.ZSD_AMAZON_CUSTOMER_PAYMENT(request);
                await LogResponseWsdl(log, resp);
                return resp;

            }

            return null;
        }
        public async Task<ZSD_AMAZON_SALEORD_CRTResponse> CreateOrder(ZSD_AMAZON_SALEORD_CRT request)
        {

            ZSD_AMAZON_SALEORD_CRT_SRVClient OutClient = new ZSD_AMAZON_SALEORD_CRT_SRVClient(ZSD_AMAZON_SALEORD_CRT_SRVClient.EndpointConfiguration.binding);


            string WSDLUserName = _wsdlConfig.UserName;
            string WSDLPassword = _wsdlConfig.Password;
            string WSDLEndPoint = _wsdlConfig.EndPoints.Where(x => x.EndpointConfiguration == "CreateOrder").First().Url;

            OutClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(_wsdlConfig.BaseUrl + WSDLEndPoint);
            OutClient.ClientCredentials.UserName.UserName = WSDLUserName;
            OutClient.ClientCredentials.UserName.Password = WSDLPassword;

            WSDLLog log = await LogRequestWsdl(request, OutClient.Endpoint.Address.Uri.ToString(), "SOAP");
            using (OperationContextScope scope = new OperationContextScope(OutClient.InnerChannel))
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] =
                    "Basic " +
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            OutClient.ClientCredentials.UserName.UserName + ":" +
                            OutClient.ClientCredentials.UserName.Password
                        )
                    );
                Message[] Messages = new Message[10];
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                string json = @"{
                  ""ET_RETURN"": [
                    {
                      ""MSG"": ""Sale Order Created"",
                      ""MSGNR"": ""999"",
                      ""MSGTYP"": ""S"",
                      ""CATEGORY"": """"
                    }
                  ],
                  ""SALE_ORD"": ""0000257806""
                }";
                var resp = JsonConvert.DeserializeObject<CreateOrder.ZSD_AMAZON_SALEORD_CRTResponse>(json);
                if (!_wsdlConfig.IsDevMode)
                    resp = OutClient.ZSD_AMAZON_SALEORD_CRT(request);
                await LogResponseWsdl(log, resp);
                return resp;

            }

            return null;
        }
        public async Task<ZSD_AMAZON_CUSTOMER_PAY_CHGResponse> ChangeCustomerPaymentRequest(ZSD_AMAZON_CUSTOMER_PAY_CHG request)
        {

            ChangeCustomerPayment.ZSD_AMZ_CUSTOMER_PAY_CHG_SRVClient OutClient = new ZSD_AMZ_CUSTOMER_PAY_CHG_SRVClient(ZSD_AMZ_CUSTOMER_PAY_CHG_SRVClient.EndpointConfiguration.binding);


            string WSDLUserName = _wsdlConfig.UserName;
            string WSDLPassword = _wsdlConfig.Password;
            string WSDLEndPoint = _wsdlConfig.EndPoints.Where(x => x.EndpointConfiguration == "ChangeCustomerPaymentRequest").First().Url;

            OutClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(_wsdlConfig.BaseUrl + WSDLEndPoint);
            OutClient.ClientCredentials.UserName.UserName = WSDLUserName;
            OutClient.ClientCredentials.UserName.Password = WSDLPassword;

            WSDLLog log = await LogRequestWsdl(request, OutClient.Endpoint.Address.Uri.ToString(), "SOAP");
            using (OperationContextScope scope = new OperationContextScope(OutClient.InnerChannel))
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] =
                    "Basic " +
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            OutClient.ClientCredentials.UserName.UserName + ":" +
                            OutClient.ClientCredentials.UserName.Password
                        )
                    );
                Message[] Messages = new Message[10];
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                var resp = new ZSD_AMAZON_CUSTOMER_PAY_CHGResponse()
                {
                    ET_RETURN = new ChangeCustomerPayment.ZSTR_AMAZON_RESPONSE[]
                {
                    new ChangeCustomerPayment.ZSTR_AMAZON_RESPONSE()
                    {
                        MSG = "Sample message 1",
                        MSGNR = "001",
                        MSGTYP = "S",
                        CATEGORY = "Category1"
                    },
                    new ChangeCustomerPayment.ZSTR_AMAZON_RESPONSE()
                    {
                        MSG = "Sample message 2",
                        MSGNR = "002",
                        MSGTYP = "S",
                        CATEGORY = "Category2"
                    }
                }
                };
                if (!_wsdlConfig.IsDevMode)
                    resp = OutClient.ZSD_AMAZON_CUSTOMER_PAY_CHG(request);
                await LogResponseWsdl(log, resp);
                return resp;

            }
            return null;
        }
        public async Task<ZSD_AMAZON_SALEORD_CHGResponse> ChangeSaleOrderRequest(ZSD_AMAZON_SALEORD_CHG request)
        {

            zsd_amazon_saleord_chg_srvClient OutClient = new zsd_amazon_saleord_chg_srvClient(zsd_amazon_saleord_chg_srvClient.EndpointConfiguration.binding);


            string WSDLUserName = _wsdlConfig.UserName;
            string WSDLPassword = _wsdlConfig.Password;
            string WSDLEndPoint = _wsdlConfig.EndPoints.Where(x => x.EndpointConfiguration == "ChangeSaleOrderRequest").First().Url;

            OutClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(_wsdlConfig.BaseUrl + WSDLEndPoint);
            OutClient.ClientCredentials.UserName.UserName = WSDLUserName;
            OutClient.ClientCredentials.UserName.Password = WSDLPassword;

            WSDLLog log = await LogRequestWsdl(request, OutClient.Endpoint.Address.Uri.ToString(), "SOAP");
            using (OperationContextScope scope = new OperationContextScope(OutClient.InnerChannel))
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] =
                    "Basic " +
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            OutClient.ClientCredentials.UserName.UserName + ":" +
                            OutClient.ClientCredentials.UserName.Password
                        )
                    );
                Message[] Messages = new Message[10];
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                string json = @"{
                  ""ET_RETURN"": [
                    {
                      ""MSG"": ""Sale Order Changed"",
                      ""MSGNR"": ""999"",
                      ""MSGTYP"": ""S"",
                      ""CATEGORY"": """"
                    }
                  ]
                }";
                var resp = JsonConvert.DeserializeObject<ChangeSaleOrder.ZSD_AMAZON_SALEORD_CHGResponse>(json);
                if (!_wsdlConfig.IsDevMode)
                    resp = OutClient.ZSD_AMAZON_SALEORD_CHG(request);
                await LogResponseWsdl(log, resp);
                return resp;

            }
            return null;
        }
        public async Task<ZSD_AMAZ_ORDER_INV_DETAILSResponse> InvoiceDetailsRequest(ZSD_AMAZ_ORDER_INV_DETAILS request)
        {

            ZSD_AMAZ_ORDER_INV_DETAILS_SRVClient OutClient = new ZSD_AMAZ_ORDER_INV_DETAILS_SRVClient(ZSD_AMAZ_ORDER_INV_DETAILS_SRVClient.EndpointConfiguration.binding);


            string WSDLUserName = _wsdlConfig.UserName;
            string WSDLPassword = _wsdlConfig.Password;
            string WSDLEndPoint = _wsdlConfig.EndPoints.Where(x => x.EndpointConfiguration == "InvoiceDetailsRequest").First().Url;

            OutClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(_wsdlConfig.BaseUrl + WSDLEndPoint);
            OutClient.ClientCredentials.UserName.UserName = WSDLUserName;
            OutClient.ClientCredentials.UserName.Password = WSDLPassword;

            WSDLLog log = await LogRequestWsdl(request, OutClient.Endpoint.Address.Uri.ToString(), "SOAP");
            using (OperationContextScope scope = new OperationContextScope(OutClient.InnerChannel))
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] =
                    "Basic " +
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            OutClient.ClientCredentials.UserName.UserName + ":" +
                            OutClient.ClientCredentials.UserName.Password
                        )
                    );
                Message[] Messages = new Message[10];
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                string json = @"{
                  ""E_DETAILS"": [
                    {
                      ""VBELN"": ""0900335383"",
                      ""POSNR"": ""000010"",
                      ""KUNAG"": ""0045000077"",
                      ""NAME1"": ""Muhammad Rizwan"",
                      ""VKBUR"": ""ZHYD"",
                      ""ARKTX"": ""ENGRO UREA"",
                      ""VKGRP"": ""BDN"",
                      ""BEZEI"": ""Badin Territory"",
                      ""BZIRK"": ""Z00239"",
                      ""BZTXT"": ""Badin_SP"",
                      ""FKDAT"": ""2024-06-20"",
                      ""AUBEL"": ""0000257750"",
                      ""AUPOS"": ""000010"",
                      ""VGBEL"": ""0800488009"",
                      ""VGPOS"": ""000010"",
                      ""MATNR"": ""000000000010000001"",
                      ""MAKTX"": ""ENGRO UREA"",
                      ""FKIMG"": 1,
                      ""VRKME"": ""BAG"",
                      ""MATKL"": ""P-PRODUCT"",
                      ""LGORT"": ""GA03"",
                      ""LGOBE"": ""Golarchi WH3"",
                      ""WERKS"": ""F078"",
                      ""WERKS_D"": ""EFERT Golarchi"",
                      ""CMPRE"": 11788.66,
                      ""NETWR"": 10676.87,
                      ""MWSBP"": 1111.79,
                      ""KZWI6"": 1095.79,
                      ""KZWI5"": 10376.87,
                      ""XWORKG"": 200,
                      ""XWORKF"": 116,
                      ""XWORKE"": 0,
                      ""KMEIN"": ""BAG"",
                      ""WAERS"": ""PKR""
                    },
                    {
                      ""VBELN"": ""0900335384"",
                      ""POSNR"": ""000020"",
                      ""KUNAG"": ""0045000077"",
                      ""NAME1"": ""Muhammad Rizwan"",
                      ""VKBUR"": ""ZHYD"",
                      ""ARKTX"": ""Insights"",
                      ""VKGRP"": ""BDN"",
                      ""BEZEI"": ""Badin Territory"",
                      ""BZIRK"": ""Z00239"",
                      ""BZTXT"": ""Badin_SP"",
                      ""FKDAT"": ""2024-06-20"",
                      ""AUBEL"": ""0000257750"",
                      ""AUPOS"": ""000020"",
                      ""VGBEL"": ""0000257750"",
                      ""VGPOS"": ""000020"",
                      ""MATNR"": ""000000000011062432"",
                      ""MAKTX"": ""Insights"",
                      ""FKIMG"": 1,
                      ""VRKME"": ""LE"",
                      ""MATKL"": ""SRV006"",
                      ""LGORT"": """",
                      ""LGOBE"": """",
                      ""WERKS"": ""F078"",
                      ""WERKS_D"": ""EFERT Golarchi"",
                      ""CMPRE"": 261,
                      ""NETWR"": 225,
                      ""MWSBP"": 36,
                      ""KZWI6"": 0,
                      ""KZWI5"": 0,
                      ""XWORKG"": 0,
                      ""XWORKF"": 36,
                      ""XWORKE"": 225,
                      ""KMEIN"": ""LE"",
                      ""WAERS"": ""PKR""
                    }
                  ]
                }";
                var resp = JsonConvert.DeserializeObject<DetailsInvoice.ZSD_AMAZ_ORDER_INV_DETAILSResponse>(json);
                if (!_wsdlConfig.IsDevMode)
                    resp = OutClient.ZSD_AMAZ_ORDER_INV_DETAILS(request);
                await LogResponseWsdl(log, resp);
                return resp;
            }
            return null;
        }


        private async Task<WSDLLog> LogRequestWsdl(dynamic request, string requestURL, string requestMethod)
        {
            var logEntry = new WSDLLog
            {
                HttpMethod = "SOAP",
                Url = requestURL,
                RequestBody = JsonConvert.SerializeObject(request),
                RequestTimestamp = DateTime.UtcNow
            };

            // Save request log to the database
            logEntry = await _repoWrapper.LoggingRepository.AddLogEntry(logEntry);
            await _repoWrapper.SaveLogEntries();

            return logEntry;
        }

        private async Task LogResponseWsdl(WSDLLog logEntry, dynamic response)
        {

            //using (var transaction = _repoWrapper..Database.BeginTransaction())
            //{

            //    dbContext.ResponseLogs.Add(new ResponseLog
            //    {
            //        StatusCode = context.Response.StatusCode,
            //        Body = responseLog,
            //        Timestamp = DateTime.UtcNow,
            //        RequestId = reqLog.RequestId
            //    });
            //    //dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT RequestLogs ON");
            //    //dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ResponseLogs ON");
            //    await dbContext.SaveChangesAsync();
            //    transaction.Commit();
            //}
            // Update the log entry with response details
            logEntry.Status = "Success";
            logEntry.ResponseBody = JsonConvert.SerializeObject(response);
            logEntry.ResponseTimestamp = DateTime.UtcNow;

            // Save response details to the database
            await _repoWrapper.LoggingRepository.UpdateLogEntry(logEntry);
            await _repoWrapper.SaveLogEntries();
        }

    }
}

