using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ClassLibrary;
namespace ParserService
{
    [ServiceContract]
    public interface IService1
    {

        [OperationContractAttribute(Name ="g1")]
        List<Vacancies> GetVacancies(string Category);
        [OperationContractAttribute(Name ="g2")]
        List<Vacancies> GetVacancies(string Category, string sity);
        [OperationContractAttribute(Name = "g3")]
        List<Vacancies> GetVacancies(string Category, string sity, string site);
        [OperationContractAttribute(Name = "gs1")]
        List<Vacancies> GetVacanciesSearch(string NameVacancy);
        [OperationContractAttribute(Name = "gs2")]
        List<Vacancies> GetVacanciesSearch(string NameVacancy, string Category);
        [OperationContractAttribute(Name = "gs3")]
        List<Vacancies> GetVacanciesSearch(string NameVacancy, string Category, string sity);
        [OperationContractAttribute(Name = "gs4")]
        List<Vacancies> GetVacanciesSearch(string NameVacancy, string Category, string sity, string site);
        [OperationContract]
        List<string> GetSites();
        [OperationContract]
        List<string> GetCategory();
    }
}
