using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AmazonFarmer.WSDL.Helpers
{

    [XmlRoot(ElementName = "Messages", Namespace = "")]
    public class Messages
    {

        [XmlElement(ElementName = "Msg", Namespace = "")]
        public string Msg;

        [XmlElement(ElementName = "MsgNr", Namespace = "")]
        public int MsgNr;

        [XmlElement(ElementName = "MsgTyp", Namespace = "")]
        public string MsgTyp;

        [XmlElement(ElementName = "Category", Namespace = "")]
        public object Category;
    }

    [XmlRoot(ElementName = "Response", Namespace = "")]
    public class Response
    {

        [XmlElement(ElementName = "CUST_NUM", Namespace = "")]
        public int CUSTNUM;

        [XmlElement(ElementName = "Messages", Namespace = "")]
        public Messages Messages;
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Body
    {

        [XmlElement(ElementName = "Response", Namespace = "")]
        public Response Response;
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {

        [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public object Header;

        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body;

        [XmlAttribute(AttributeName = "soap", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Soap;

        [XmlText]
        public string Text;
    }


}
