using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AmazonFarmer.WSDL.Helpers
{

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class CreateCustTypeRequestEnvelope
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public CreateCustTypeRequestBody Body { get; set; }
    }

    public class CreateCustTypeRequestBody
    {
        [XmlElement(ElementName = "Data", Namespace = "http://www.example.org/Amazon/")]
        public CreateCustTypeRequestData Data { get; set; }
    }
    [XmlRoot(ElementName = "Data", Namespace = "http://www.example.org/Amazon/")]
    public class CreateCustTypeRequestData
    {
        [XmlElement(ElementName = "NAME")]
        public string NAME { get; set; }

        [XmlElement(ElementName = "SEARCH_TERM1")]
        public string SEARCH_TERM1 { get; set; }

        [XmlElement(ElementName = "SEARCH_TERM2")]
        public string SEARCH_TERM2 { get; set; }

        [XmlElement(ElementName = "STREET_2")]
        public string STREET_2 { get; set; }

        [XmlElement(ElementName = "STREET")]
        public string STREET { get; set; }

        [XmlElement(ElementName = "STREET_4")]
        public string STREET_4 { get; set; }

        [XmlElement(ElementName = "DISTRICT")]
        public string DISTRICT { get; set; }

        [XmlElement(ElementName = "POSTAL_CODE")]
        public string POSTAL_CODE { get; set; }

        [XmlElement(ElementName = "CITY")]
        public string CITY { get; set; }

        [XmlElement(ElementName = "PHONE_NUM")]
        public string PHONE_NUM { get; set; }

        [XmlElement(ElementName = "MOBILE_NUM")]
        public string MOBILE_NUM { get; set; }

        [XmlElement(ElementName = "EMAIL")]
        public string EMAIL { get; set; }

        [XmlElement(ElementName = "FAX")]
        public string FAX { get; set; }

        [XmlElement(ElementName = "CNIC")]
        public string CNIC { get; set; }

        [XmlElement(ElementName = "STRN")]
        public string STRN { get; set; }

        [XmlElement(ElementName = "NTN")]
        public string NTN { get; set; }

        [XmlElement(ElementName = "COND_GRP1")]
        public string COND_GRP1 { get; set; }

        [XmlElement(ElementName = "COND_GRP2")]
        public string COND_GRP2 { get; set; }

        [XmlElement(ElementName = "COND_GRP3")]
        public string COND_GRP3 { get; set; }

        [XmlElement(ElementName = "COND_GRP4")]
        public string COND_GRP4 { get; set; }

        [XmlElement(ElementName = "SALEPOINT")]
        public string SALEPOINT { get; set; }
    }
}
