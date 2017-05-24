﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsFormsApplication3.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IParseService")]
    public interface IParseService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IParseService/GetVacancies", ReplyAction="http://tempuri.org/IParseService/GetVacanciesResponse")]
        ClassLibrary.Vacancy[] GetVacancies(string Category, string City, string Site, int Day);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IParseService/GetVacancies", ReplyAction="http://tempuri.org/IParseService/GetVacanciesResponse")]
        System.Threading.Tasks.Task<ClassLibrary.Vacancy[]> GetVacanciesAsync(string Category, string City, string Site, int Day);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IParseService/GetVacanciesBySearch", ReplyAction="http://tempuri.org/IParseService/GetVacanciesBySearchResponse")]
        ClassLibrary.Vacancy[] GetVacanciesBySearch(string NameVacancy, string Category, string City, string Site, int Day);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IParseService/GetVacanciesBySearch", ReplyAction="http://tempuri.org/IParseService/GetVacanciesBySearchResponse")]
        System.Threading.Tasks.Task<ClassLibrary.Vacancy[]> GetVacanciesBySearchAsync(string NameVacancy, string Category, string City, string Site, int Day);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IParseService/GetSites", ReplyAction="http://tempuri.org/IParseService/GetSitesResponse")]
        string[] GetSites();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IParseService/GetSites", ReplyAction="http://tempuri.org/IParseService/GetSitesResponse")]
        System.Threading.Tasks.Task<string[]> GetSitesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IParseService/GetCategory", ReplyAction="http://tempuri.org/IParseService/GetCategoryResponse")]
        string[] GetCategory();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IParseService/GetCategory", ReplyAction="http://tempuri.org/IParseService/GetCategoryResponse")]
        System.Threading.Tasks.Task<string[]> GetCategoryAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IParseService/GetCity", ReplyAction="http://tempuri.org/IParseService/GetCityResponse")]
        string[] GetCity();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IParseService/GetCity", ReplyAction="http://tempuri.org/IParseService/GetCityResponse")]
        System.Threading.Tasks.Task<string[]> GetCityAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IParseService/Start", ReplyAction="http://tempuri.org/IParseService/StartResponse")]
        void Start();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IParseService/Start", ReplyAction="http://tempuri.org/IParseService/StartResponse")]
        System.Threading.Tasks.Task StartAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IParseServiceChannel : WindowsFormsApplication3.ServiceReference1.IParseService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ParseServiceClient : System.ServiceModel.ClientBase<WindowsFormsApplication3.ServiceReference1.IParseService>, WindowsFormsApplication3.ServiceReference1.IParseService {
        
        public ParseServiceClient() {
        }
        
        public ParseServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ParseServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ParseServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ParseServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public ClassLibrary.Vacancy[] GetVacancies(string Category, string City, string Site, int Day) {
            return base.Channel.GetVacancies(Category, City, Site, Day);
        }
        
        public System.Threading.Tasks.Task<ClassLibrary.Vacancy[]> GetVacanciesAsync(string Category, string City, string Site, int Day) {
            return base.Channel.GetVacanciesAsync(Category, City, Site, Day);
        }
        
        public ClassLibrary.Vacancy[] GetVacanciesBySearch(string NameVacancy, string Category, string City, string Site, int Day) {
            return base.Channel.GetVacanciesBySearch(NameVacancy, Category, City, Site, Day);
        }
        
        public System.Threading.Tasks.Task<ClassLibrary.Vacancy[]> GetVacanciesBySearchAsync(string NameVacancy, string Category, string City, string Site, int Day) {
            return base.Channel.GetVacanciesBySearchAsync(NameVacancy, Category, City, Site, Day);
        }
        
        public string[] GetSites() {
            return base.Channel.GetSites();
        }
        
        public System.Threading.Tasks.Task<string[]> GetSitesAsync() {
            return base.Channel.GetSitesAsync();
        }
        
        public string[] GetCategory() {
            return base.Channel.GetCategory();
        }
        
        public System.Threading.Tasks.Task<string[]> GetCategoryAsync() {
            return base.Channel.GetCategoryAsync();
        }
        
        public string[] GetCity() {
            return base.Channel.GetCity();
        }
        
        public System.Threading.Tasks.Task<string[]> GetCityAsync() {
            return base.Channel.GetCityAsync();
        }
        
        public void Start() {
            base.Channel.Start();
        }
        
        public System.Threading.Tasks.Task StartAsync() {
            return base.Channel.StartAsync();
        }
    }
}