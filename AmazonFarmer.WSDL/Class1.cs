using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using AmazonFarmer.WSDL.Helpers;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using CustomerCreateWsdl;

namespace AmazonFarmer.WSDL
{
    public class Class1
    {

        public static async Task<Body> CreateCustomerWSDLAsync()
        {

            CustomerCreateWsdl.CustomerCreateClient OutClient = new CustomerCreateWsdl.CustomerCreateClient(CustomerCreateClient.EndpointConfiguration.CustomerCreateSOAP);


            string WSDLUserName = "sb-9b5c51ba-3419-46b7-b9f1-04ba70686b4f!b1716|it-rt-onesapbtpqa!b37";
            string WSDLPassword = "c562b871-35e6-4ef3-8518-afc6d0ed9d82$9l70fyfW-DsAF1PvlQpUnUYfHRAX9M3XWvPWgDFWS8o=";

            OutClient.ClientCredentials.UserName.UserName = WSDLUserName;
            OutClient.ClientCredentials.UserName.Password = WSDLPassword;


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
                var request = new CustomerCreateWsdl.RequestType
                {
                    city = "KARACHI",
                    cnic = "413000000012",
                    condGrp1 = "",
                    condGrp2 = "",
                    condGrp3 = "",
                    condGrp4 = "",
                    district = "CENTRAL",
                    email = "MURSALEEN.CHAWLA@ENGRO.COM1",
                    fax = "02191574254",
                    mobileNum = "02001254221",
                    name = "y BP",
                    ntn = "",
                    phoneNum = "02196312148",
                    postalCode = "789456",
                    salePoint = "Z00796",
                    searchTerm1 = "BP",
                    searchTerm2 = "Bp",
                    street = "BRRRRRRRRRRRRR",
                    street2 = "BRRRRRRRRRRRRR",
                    street4 = "BRRRRRRRRRRRRR",
                    strn = ""
                };
                var resp = OutClient.Create(request);
                //var resp = OutClient.CustomerCreate(Data.NAME,
                //   Data.SEARCH_TERM1,
                //   Data.SEARCH_TERM2,
                //   Data.STREET_2,
                //   Data.STREET,
                //   Data.STREET_4,
                //   Data.DISTRICT,
                //   Data.POSTAL_CODE,
                //   Data.CITY,
                //   Data.PHONE_NUM,
                //   Data.MOBILE_NUM,
                //   Data.EMAIL,
                //   Data.FAX,
                //   Data.CNIC,
                //   Data.NTN,
                //   Data.STRN,
                //   Data.COND_GRP1,
                //   Data.COND_GRP2,
                //   Data.COND_GRP3,
                //   Data.COND_GRP4,
                //   Data.SALEPOINT,
                //   out Messages);
                return null;

            }

            return null;

        }

    }
}

