using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ClassLibrary;
using System.Threading;
using System.Threading.Tasks;

namespace ParserService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class Service1 : IService1
    {
        List<ClassLibrary.IParser> parseSites = new List<IParser>() {new ParseJobsUa(),new ParseRabotaUa(),new ParserWorkUa()};
        public Service1()
        {

        }
        DBmodel model = new DBmodel();
        private void renewalDateSite(IParser site)
        {
            ClassLibrary.Vacancies tempvac = new ClassLibrary.Vacancies();
            try
            {
                foreach (var vac in tempvac.s)
                {
                    try
                    {
                        List<ClassLibrary.Vacancies> tempvacancies = site.StartParseforDate(vac.Key, DateTime.Now);
                        foreach (var items in tempvacancies)
                        {
                            try
                            {
                                model.Vacancies.Add(items);
                                model.SaveChanges();
                            }
                            catch
                            {
                            }
                        }


                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
        }
        private void renewalDate()
        {
            
            while (true)
            {
                Thread.Sleep(300000);
                try
                {

                   foreach(var item in parseSites)
                    {
                        Task.Run(() => renewalDateSite(item));
                    }
                }
                catch
                {

                }
            }
        }
        public List<string> GetCategory()
        {
            List<string> categories = new List<string>();
            ClassLibrary.Vacancies tempvac = new ClassLibrary.Vacancies();
            foreach (var item in tempvac.s)
            {
                categories.Add(item.Key);
            }
            return categories;
        }

        public List<string> GetSites()
        {
            List<string> names = new List<string>();
            foreach (var item in model.Sites)
            {
                names.Add(item.name);
            }
            return names;
        }

        public List<Vacancies> GetVacancies(string Category)
        {

            return model.Vacancies.Where(x => x.Сategory == Category).ToList();
               
        }

        public List<Vacancies> GetVacancies(string Category, string sity)
        {
            return model.Vacancies.Where(x => x.Сategory == Category && x.Sity == sity).ToList();
        }

        public List<Vacancies> GetVacancies(string Category, string sity, string site)
        {
            var sitetemp = model.Sites.Where(x => x.name == site).First();
            return model.Vacancies.Where(x => x.Сategory == Category && x.Sity == sity&&x.Id_pars_site==sitetemp.id).ToList();
        }

        public List<Vacancies> GetVacanciesSearch(string NameVacancy)
        {
            return model.Vacancies.Where(x => x.Title.Contains(NameVacancy)).ToList();
        }

        public List<Vacancies> GetVacanciesSearch(string NameVacancy, string Category)
        {
            return model.Vacancies.Where(x => x.Title.Contains(NameVacancy)&&x.Сategory==Category).ToList();
        }

        public List<Vacancies> GetVacanciesSearch(string NameVacancy, string Category, string sity)
        {
            return model.Vacancies.Where(x => x.Title.Contains(NameVacancy)&&x.Сategory==Category&&x.Sity==sity).ToList();
        }

        public List<Vacancies> GetVacanciesSearch(string NameVacancy, string Category, string sity, string site)
        {
            var sitetemp = model.Sites.Where(x => x.name == site).First();
            return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Сategory == Category && x.Sity == sity&& x.Id_pars_site == sitetemp.id).ToList();
        }
    }
}
