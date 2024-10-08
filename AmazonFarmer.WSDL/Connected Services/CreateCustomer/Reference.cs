﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CreateCustomer
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.example.org/CustomerCreate/", ConfigurationName="CreateCustomer.CustomerCreate")]
    public interface CustomerCreate
    {
        
        // CODEGEN: Parameter 'Response' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'Microsoft.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://www.example.org/CustomerCreate/Create", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="Response")]
        CreateCustomer.CreateResponse Create(CreateCustomer.CreateRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.example.org/CustomerCreate/Create", ReplyAction="*")]
        System.Threading.Tasks.Task<CreateCustomer.CreateResponse> CreateAsync(CreateCustomer.CreateRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.org/CustomerCreate/")]
    public partial class RequestType : object, System.ComponentModel.INotifyPropertyChanged
    {
        
        private string nameField;
        
        private string searchTerm1Field;
        
        private string searchTerm2Field;
        
        private string street2Field;
        
        private string streetField;
        
        private string street4Field;
        
        private string districtField;
        
        private string postalCodeField;
        
        private string cityField;
        
        private string phoneNumField;
        
        private string mobileNumField;
        
        private string emailField;
        
        private string faxField;
        
        private string cnicField;
        
        private string ntnField;
        
        private string strnField;
        
        private string condGrp1Field;
        
        private string condGrp2Field;
        
        private string condGrp3Field;
        
        private string condGrp4Field;
        
        private string salePointField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
                this.RaisePropertyChanged("name");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string searchTerm1
        {
            get
            {
                return this.searchTerm1Field;
            }
            set
            {
                this.searchTerm1Field = value;
                this.RaisePropertyChanged("searchTerm1");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string searchTerm2
        {
            get
            {
                return this.searchTerm2Field;
            }
            set
            {
                this.searchTerm2Field = value;
                this.RaisePropertyChanged("searchTerm2");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string street2
        {
            get
            {
                return this.street2Field;
            }
            set
            {
                this.street2Field = value;
                this.RaisePropertyChanged("street2");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string street
        {
            get
            {
                return this.streetField;
            }
            set
            {
                this.streetField = value;
                this.RaisePropertyChanged("street");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string street4
        {
            get
            {
                return this.street4Field;
            }
            set
            {
                this.street4Field = value;
                this.RaisePropertyChanged("street4");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string district
        {
            get
            {
                return this.districtField;
            }
            set
            {
                this.districtField = value;
                this.RaisePropertyChanged("district");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string postalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
                this.RaisePropertyChanged("postalCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
                this.RaisePropertyChanged("city");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string phoneNum
        {
            get
            {
                return this.phoneNumField;
            }
            set
            {
                this.phoneNumField = value;
                this.RaisePropertyChanged("phoneNum");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=10)]
        public string mobileNum
        {
            get
            {
                return this.mobileNumField;
            }
            set
            {
                this.mobileNumField = value;
                this.RaisePropertyChanged("mobileNum");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=11)]
        public string email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
                this.RaisePropertyChanged("email");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=12)]
        public string fax
        {
            get
            {
                return this.faxField;
            }
            set
            {
                this.faxField = value;
                this.RaisePropertyChanged("fax");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=13)]
        public string cnic
        {
            get
            {
                return this.cnicField;
            }
            set
            {
                this.cnicField = value;
                this.RaisePropertyChanged("cnic");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=14)]
        public string ntn
        {
            get
            {
                return this.ntnField;
            }
            set
            {
                this.ntnField = value;
                this.RaisePropertyChanged("ntn");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=15)]
        public string strn
        {
            get
            {
                return this.strnField;
            }
            set
            {
                this.strnField = value;
                this.RaisePropertyChanged("strn");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=16)]
        public string condGrp1
        {
            get
            {
                return this.condGrp1Field;
            }
            set
            {
                this.condGrp1Field = value;
                this.RaisePropertyChanged("condGrp1");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=17)]
        public string condGrp2
        {
            get
            {
                return this.condGrp2Field;
            }
            set
            {
                this.condGrp2Field = value;
                this.RaisePropertyChanged("condGrp2");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=18)]
        public string condGrp3
        {
            get
            {
                return this.condGrp3Field;
            }
            set
            {
                this.condGrp3Field = value;
                this.RaisePropertyChanged("condGrp3");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=19)]
        public string condGrp4
        {
            get
            {
                return this.condGrp4Field;
            }
            set
            {
                this.condGrp4Field = value;
                this.RaisePropertyChanged("condGrp4");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=20)]
        public string salePoint
        {
            get
            {
                return this.salePointField;
            }
            set
            {
                this.salePointField = value;
                this.RaisePropertyChanged("salePoint");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.org/CustomerCreate/")]
    public partial class MessageType : object, System.ComponentModel.INotifyPropertyChanged
    {
        
        private string msgNoField;
        
        private string msgTypField;
        
        private string msgField;
        
        private string msgCatField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string msgNo
        {
            get
            {
                return this.msgNoField;
            }
            set
            {
                this.msgNoField = value;
                this.RaisePropertyChanged("msgNo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string msgTyp
        {
            get
            {
                return this.msgTypField;
            }
            set
            {
                this.msgTypField = value;
                this.RaisePropertyChanged("msgTyp");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string msg
        {
            get
            {
                return this.msgField;
            }
            set
            {
                this.msgField = value;
                this.RaisePropertyChanged("msg");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string msgCat
        {
            get
            {
                return this.msgCatField;
            }
            set
            {
                this.msgCatField = value;
                this.RaisePropertyChanged("msgCat");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.org/CustomerCreate/")]
    public partial class MessagesType : object, System.ComponentModel.INotifyPropertyChanged
    {
        
        private MessageType messageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public MessageType Message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
                this.RaisePropertyChanged("Message");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.org/CustomerCreate/")]
    public partial class ResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {
        
        private string custNumField;
        
        private MessagesType[] messagesField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string custNum
        {
            get
            {
                return this.custNumField;
            }
            set
            {
                this.custNumField = value;
                this.RaisePropertyChanged("custNum");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Messages", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public MessagesType[] Messages
        {
            get
            {
                return this.messagesField;
            }
            set
            {
                this.messagesField = value;
                this.RaisePropertyChanged("Messages");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Create", WrapperNamespace="http://www.example.org/CustomerCreate/", IsWrapped=true)]
    public partial class CreateRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.example.org/CustomerCreate/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CreateCustomer.RequestType Request;
        
        public CreateRequest()
        {
        }
        
        public CreateRequest(CreateCustomer.RequestType Request)
        {
            this.Request = Request;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CreateResponse", WrapperNamespace="http://www.example.org/CustomerCreate/", IsWrapped=true)]
    public partial class CreateResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.example.org/CustomerCreate/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CreateCustomer.ResponseType Response;
        
        public CreateResponse()
        {
        }
        
        public CreateResponse(CreateCustomer.ResponseType Response)
        {
            this.Response = Response;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public interface CustomerCreateChannel : CreateCustomer.CustomerCreate, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public partial class CustomerCreateClient : System.ServiceModel.ClientBase<CreateCustomer.CustomerCreate>, CreateCustomer.CustomerCreate
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public CustomerCreateClient() : 
                base(CustomerCreateClient.GetDefaultBinding(), CustomerCreateClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.CustomerCreateSOAP.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CustomerCreateClient(EndpointConfiguration endpointConfiguration) : 
                base(CustomerCreateClient.GetBindingForEndpoint(endpointConfiguration), CustomerCreateClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CustomerCreateClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(CustomerCreateClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CustomerCreateClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(CustomerCreateClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CustomerCreateClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CreateCustomer.CreateResponse CreateCustomer.CustomerCreate.Create(CreateCustomer.CreateRequest request)
        {
            return base.Channel.Create(request);
        }
        
        public CreateCustomer.ResponseType Create(CreateCustomer.RequestType Request)
        {
            CreateCustomer.CreateRequest inValue = new CreateCustomer.CreateRequest();
            inValue.Request = Request;
            CreateCustomer.CreateResponse retVal = ((CreateCustomer.CustomerCreate)(this)).Create(inValue);
            return retVal.Response;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CreateCustomer.CreateResponse> CreateCustomer.CustomerCreate.CreateAsync(CreateCustomer.CreateRequest request)
        {
            return base.Channel.CreateAsync(request);
        }
        
        public System.Threading.Tasks.Task<CreateCustomer.CreateResponse> CreateAsync(CreateCustomer.RequestType Request)
        {
            CreateCustomer.CreateRequest inValue = new CreateCustomer.CreateRequest();
            inValue.Request = Request;
            return ((CreateCustomer.CustomerCreate)(this)).CreateAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.CustomerCreateSOAP))
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
            if ((endpointConfiguration == EndpointConfiguration.CustomerCreateSOAP))
            {
                return new System.ServiceModel.EndpointAddress("https://onesapbtpqa.it-cpi016-rt.cfapps.ap20.hana.ondemand.com/cxf/Amazon/CreateCu" +
                        "stomer");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return CustomerCreateClient.GetBindingForEndpoint(EndpointConfiguration.CustomerCreateSOAP);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return CustomerCreateClient.GetEndpointAddress(EndpointConfiguration.CustomerCreateSOAP);
        }
        
        public enum EndpointConfiguration
        {
            
            CustomerCreateSOAP,
        }
    }
}
