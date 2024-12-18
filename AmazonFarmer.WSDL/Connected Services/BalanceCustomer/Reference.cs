﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BalanceCustomer
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", ConfigurationName="BalanceCustomer.ZSD_AMAZON_CUSTOMER_BAL_SRV")]
    public interface ZSD_AMAZON_CUSTOMER_BAL_SRV
    {
        
        // CODEGEN: Generating message contract since the operation ZSD_AMAZON_CUSTOMER_BAL is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="urn:sap-com:document:sap:rfc:functions:ZSD_AMAZON_CUSTOMER_BAL_SRV:ZSD_AMAZON_CUS" +
            "TOMER_BALRequest", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALResponse1 ZSD_AMAZON_CUSTOMER_BAL(BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:sap-com:document:sap:rfc:functions:ZSD_AMAZON_CUSTOMER_BAL_SRV:ZSD_AMAZON_CUS" +
            "TOMER_BALRequest", ReplyAction="*")]
        System.Threading.Tasks.Task<BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALResponse1> ZSD_AMAZON_CUSTOMER_BALAsync(BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZSD_AMAZON_CUSTOMER_BAL
    {
        
        private string cREDIT_SEGMENTField;
        
        private string cUST_NUMField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string CREDIT_SEGMENT
        {
            get
            {
                return this.cREDIT_SEGMENTField;
            }
            set
            {
                this.cREDIT_SEGMENTField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string CUST_NUM
        {
            get
            {
                return this.cUST_NUMField;
            }
            set
            {
                this.cUST_NUMField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZSTR_AMAZON_RESPONSE
    {
        
        private string mSGField;
        
        private string mSGNRField;
        
        private string mSGTYPField;
        
        private string cATEGORYField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string MSG
        {
            get
            {
                return this.mSGField;
            }
            set
            {
                this.mSGField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string MSGNR
        {
            get
            {
                return this.mSGNRField;
            }
            set
            {
                this.mSGNRField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string MSGTYP
        {
            get
            {
                return this.mSGTYPField;
            }
            set
            {
                this.mSGTYPField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string CATEGORY
        {
            get
            {
                return this.cATEGORYField;
            }
            set
            {
                this.cATEGORYField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZSD_AMAZON_CUSTOMER_BALResponse
    {
        
        private decimal bILLING_VALField;
        
        private decimal cREDIT_EXP_VALField;
        
        private decimal dELIVERY_VALField;
        
        private ZSTR_AMAZON_RESPONSE[] eT_RETURNField;
        
        private decimal oPEN_INVOICE_VALField;
        
        private decimal oPEN_ORDER_VALField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public decimal BILLING_VAL
        {
            get
            {
                return this.bILLING_VALField;
            }
            set
            {
                this.bILLING_VALField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public decimal CREDIT_EXP_VAL
        {
            get
            {
                return this.cREDIT_EXP_VALField;
            }
            set
            {
                this.cREDIT_EXP_VALField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public decimal DELIVERY_VAL
        {
            get
            {
                return this.dELIVERY_VALField;
            }
            set
            {
                this.dELIVERY_VALField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZSTR_AMAZON_RESPONSE[] ET_RETURN
        {
            get
            {
                return this.eT_RETURNField;
            }
            set
            {
                this.eT_RETURNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public decimal OPEN_INVOICE_VAL
        {
            get
            {
                return this.oPEN_INVOICE_VALField;
            }
            set
            {
                this.oPEN_INVOICE_VALField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public decimal OPEN_ORDER_VAL
        {
            get
            {
                return this.oPEN_ORDER_VALField;
            }
            set
            {
                this.oPEN_ORDER_VALField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ZSD_AMAZON_CUSTOMER_BALRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        public BalanceCustomer.ZSD_AMAZON_CUSTOMER_BAL ZSD_AMAZON_CUSTOMER_BAL;
        
        public ZSD_AMAZON_CUSTOMER_BALRequest()
        {
        }
        
        public ZSD_AMAZON_CUSTOMER_BALRequest(BalanceCustomer.ZSD_AMAZON_CUSTOMER_BAL ZSD_AMAZON_CUSTOMER_BAL)
        {
            this.ZSD_AMAZON_CUSTOMER_BAL = ZSD_AMAZON_CUSTOMER_BAL;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ZSD_AMAZON_CUSTOMER_BALResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ZSD_AMAZON_CUSTOMER_BAL.Response", Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("ZSD_AMAZON_CUSTOMER_BAL.Response")]
        public BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALResponse ZSD_AMAZON_CUSTOMER_BALResponse;
        
        public ZSD_AMAZON_CUSTOMER_BALResponse1()
        {
        }
        
        public ZSD_AMAZON_CUSTOMER_BALResponse1(BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALResponse ZSD_AMAZON_CUSTOMER_BALResponse)
        {
            this.ZSD_AMAZON_CUSTOMER_BALResponse = ZSD_AMAZON_CUSTOMER_BALResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public interface ZSD_AMAZON_CUSTOMER_BAL_SRVChannel : BalanceCustomer.ZSD_AMAZON_CUSTOMER_BAL_SRV, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public partial class ZSD_AMAZON_CUSTOMER_BAL_SRVClient : System.ServiceModel.ClientBase<BalanceCustomer.ZSD_AMAZON_CUSTOMER_BAL_SRV>, BalanceCustomer.ZSD_AMAZON_CUSTOMER_BAL_SRV
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public ZSD_AMAZON_CUSTOMER_BAL_SRVClient() : 
                base(ZSD_AMAZON_CUSTOMER_BAL_SRVClient.GetDefaultBinding(), ZSD_AMAZON_CUSTOMER_BAL_SRVClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.binding.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ZSD_AMAZON_CUSTOMER_BAL_SRVClient(EndpointConfiguration endpointConfiguration) : 
                base(ZSD_AMAZON_CUSTOMER_BAL_SRVClient.GetBindingForEndpoint(endpointConfiguration), ZSD_AMAZON_CUSTOMER_BAL_SRVClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ZSD_AMAZON_CUSTOMER_BAL_SRVClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(ZSD_AMAZON_CUSTOMER_BAL_SRVClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ZSD_AMAZON_CUSTOMER_BAL_SRVClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(ZSD_AMAZON_CUSTOMER_BAL_SRVClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ZSD_AMAZON_CUSTOMER_BAL_SRVClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALResponse1 BalanceCustomer.ZSD_AMAZON_CUSTOMER_BAL_SRV.ZSD_AMAZON_CUSTOMER_BAL(BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALRequest request)
        {
            return base.Channel.ZSD_AMAZON_CUSTOMER_BAL(request);
        }
        
        public BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALResponse ZSD_AMAZON_CUSTOMER_BAL(BalanceCustomer.ZSD_AMAZON_CUSTOMER_BAL ZSD_AMAZON_CUSTOMER_BAL1)
        {
            BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALRequest inValue = new BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALRequest();
            inValue.ZSD_AMAZON_CUSTOMER_BAL = ZSD_AMAZON_CUSTOMER_BAL1;
            BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALResponse1 retVal = ((BalanceCustomer.ZSD_AMAZON_CUSTOMER_BAL_SRV)(this)).ZSD_AMAZON_CUSTOMER_BAL(inValue);
            return retVal.ZSD_AMAZON_CUSTOMER_BALResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALResponse1> BalanceCustomer.ZSD_AMAZON_CUSTOMER_BAL_SRV.ZSD_AMAZON_CUSTOMER_BALAsync(BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALRequest request)
        {
            return base.Channel.ZSD_AMAZON_CUSTOMER_BALAsync(request);
        }
        
        public System.Threading.Tasks.Task<BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALResponse1> ZSD_AMAZON_CUSTOMER_BALAsync(BalanceCustomer.ZSD_AMAZON_CUSTOMER_BAL ZSD_AMAZON_CUSTOMER_BAL)
        {
            BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALRequest inValue = new BalanceCustomer.ZSD_AMAZON_CUSTOMER_BALRequest();
            inValue.ZSD_AMAZON_CUSTOMER_BAL = ZSD_AMAZON_CUSTOMER_BAL;
            return ((BalanceCustomer.ZSD_AMAZON_CUSTOMER_BAL_SRV)(this)).ZSD_AMAZON_CUSTOMER_BALAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.binding))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.binding))
            {
                return new System.ServiceModel.EndpointAddress("https://onesapbtpqa.it-cpi016-rt.cfapps.ap20.hana.ondemand.com/cxf/Amazon/Customer" +
                        "Balance");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return ZSD_AMAZON_CUSTOMER_BAL_SRVClient.GetBindingForEndpoint(EndpointConfiguration.binding);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return ZSD_AMAZON_CUSTOMER_BAL_SRVClient.GetEndpointAddress(EndpointConfiguration.binding);
        }
        
        public enum EndpointConfiguration
        {
            
            binding,
        }
    }
}
