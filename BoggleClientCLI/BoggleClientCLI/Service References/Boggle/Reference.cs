﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BoggleClientCLI.Boggle {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Boggle.IBoggleService", SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IBoggleService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBoggleService/napraviIgru", ReplyAction="http://tempuri.org/IBoggleService/napraviIgruResponse")]
        bool napraviIgru();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBoggleService/napraviIgru", ReplyAction="http://tempuri.org/IBoggleService/napraviIgruResponse")]
        System.Threading.Tasks.Task<bool> napraviIgruAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBoggleService/dohvatiSlova", ReplyAction="http://tempuri.org/IBoggleService/dohvatiSlovaResponse")]
        string[] dohvatiSlova();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBoggleService/dohvatiSlova", ReplyAction="http://tempuri.org/IBoggleService/dohvatiSlovaResponse")]
        System.Threading.Tasks.Task<string[]> dohvatiSlovaAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBoggleService/provjeriRijec", ReplyAction="http://tempuri.org/IBoggleService/provjeriRijecResponse")]
        string provjeriRijec(string rijec);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBoggleService/provjeriRijec", ReplyAction="http://tempuri.org/IBoggleService/provjeriRijecResponse")]
        System.Threading.Tasks.Task<string> provjeriRijecAsync(string rijec);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBoggleService/preostaloVremena", ReplyAction="http://tempuri.org/IBoggleService/preostaloVremenaResponse")]
        int preostaloVremena();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBoggleService/preostaloVremena", ReplyAction="http://tempuri.org/IBoggleService/preostaloVremenaResponse")]
        System.Threading.Tasks.Task<int> preostaloVremenaAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBoggleService/popisRezultata", ReplyAction="http://tempuri.org/IBoggleService/popisRezultataResponse")]
        System.Collections.Generic.KeyValuePair<string, string>[] popisRezultata();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBoggleService/popisRezultata", ReplyAction="http://tempuri.org/IBoggleService/popisRezultataResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.KeyValuePair<string, string>[]> popisRezultataAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBoggleServiceChannel : BoggleClientCLI.Boggle.IBoggleService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BoggleServiceClient : System.ServiceModel.ClientBase<BoggleClientCLI.Boggle.IBoggleService>, BoggleClientCLI.Boggle.IBoggleService {
        
        public BoggleServiceClient() {
        }
        
        public BoggleServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BoggleServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BoggleServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BoggleServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool napraviIgru() {
            return base.Channel.napraviIgru();
        }
        
        public System.Threading.Tasks.Task<bool> napraviIgruAsync() {
            return base.Channel.napraviIgruAsync();
        }
        
        public string[] dohvatiSlova() {
            return base.Channel.dohvatiSlova();
        }
        
        public System.Threading.Tasks.Task<string[]> dohvatiSlovaAsync() {
            return base.Channel.dohvatiSlovaAsync();
        }
        
        public string provjeriRijec(string rijec) {
            return base.Channel.provjeriRijec(rijec);
        }
        
        public System.Threading.Tasks.Task<string> provjeriRijecAsync(string rijec) {
            return base.Channel.provjeriRijecAsync(rijec);
        }
        
        public int preostaloVremena() {
            return base.Channel.preostaloVremena();
        }
        
        public System.Threading.Tasks.Task<int> preostaloVremenaAsync() {
            return base.Channel.preostaloVremenaAsync();
        }
        
        public System.Collections.Generic.KeyValuePair<string, string>[] popisRezultata() {
            return base.Channel.popisRezultata();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.KeyValuePair<string, string>[]> popisRezultataAsync() {
            return base.Channel.popisRezultataAsync();
        }
    }
}
