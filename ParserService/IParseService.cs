﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using  ClassLibrary;
namespace ParserService
{
    [ServiceContract]
    public interface IParseService
    {

        [OperationContract]
        List<Vacancy> GetVacancies(string Category, string City, string Site,int Day);
        [OperationContract]
        List<Vacancy> GetVacanciesBySearch(string NameVacancy, string Category, string City, string Site,int Day);
        [OperationContract]
        List<string> GetSites();
        [OperationContract]
        List<string> GetCategory();
        [OperationContract]
        List<string> GetCity();
        [OperationContract]
        void Start();
        [OperationContract]
         void StartUpdateDataDate();
        [OperationContract]
        void StartParser(int idSite, string Category);

    }
}
