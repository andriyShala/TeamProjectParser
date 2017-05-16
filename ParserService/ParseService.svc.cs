using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.ServiceModel;

namespace ParserService
{
    public static class temp
    {
        public static object lockthread = new object();
        public static int id = 3151;
    }
    [ServiceBehavior(
    ConcurrencyMode = ConcurrencyMode.Single
  )]
    public class ParseService : IParseService
    {
        private const int Timeout = 10800000;
        List<Parser> parseSites = new List<Parser>() { new ParseJobsUa(523),new RabotaUAParser(325),new ParserOlxUa(231)};
        DBmodel model = new DBmodel();
        private static object lockthread = new object();

        public ParseService()
        {
            if (model.Sites.Count() == 0)
            {
                foreach (var site in parseSites)
                {
                    model.Sites.Add(new Site() { id = temp.id++, name = site.SiteName });
                }
                model.SaveChanges();
            }
            Task.Run(() => UpdateDate());
        }
        private void UpdateSite(Parser site)
        {

            foreach (var vac in Category.categoryCollection)
            {

                try
                {
                    foreach (var items in site.ParseByCategory(vac))
                    {
                        try
                        {
                            lock (lockthread)
                            {
                                model.Vacancies.Add(items);
                                model.SaveChanges();
                            }
                        }
                        catch 
                        {
                            lock (lockthread)
                            {
                                model.Vacancies.Remove(items);
                                model.SaveChanges();
                            }

                        }
                    }
                }
                catch
                {

                }
            }
        }
        private void UpdateDateSite(Parser site)
        {
          
            foreach (var vac in Category.categoryCollection)
            {

                try
                {
                    foreach (var items in site.ParseByDate(vac, DateTime.Today))
                    {
                        try
                        {
                            lock (lockthread)
                            {
                                model.Vacancies.Add(items);
                                model.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            lock (lockthread)
                            {
                                model.Vacancies.Remove(items);
                                model.SaveChanges();
                            }

                        }
                    }
                }
                catch
                {

                }
            }
        }
        private void UpdateDate()
        {
            while (true)
            {
                foreach (var item in parseSites)
                {
                    Task.Run(()=>UpdateDateSite(item));
                }
                Thread.Sleep(Timeout);
            }
        }
        public List<string> GetCategory()
        {
            List<string> categories = new List<string>();
          
            foreach (var item in Category.categoryCollection)
            {
                categories.Add(item);
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
        private DateTime ConvertIntToDate(int Day)
        {
            DateTime rangedata = new DateTime();
            switch (Day)
            {
                case 0:
                    rangedata = new DateTime();
                    break;
                case 1:
                    rangedata = DateTime.Today;
                    break;
                case 7:
                    rangedata = (DateTime.Today).Subtract(new TimeSpan(7, 0, 0, 0));
                    break;
                case 14:
                    rangedata = (DateTime.Today).Subtract(new TimeSpan(14, 0, 0, 0));
                    break;
                case 30:
                    rangedata = (DateTime.Today).Subtract(new TimeSpan(30, 0, 0, 0));
                    break;
            }
            return rangedata;
        }
        public List<Vacancy> GetVacancies(string Category, string City, string Site, int Day)
        {
            DateTime rangedata = ConvertIntToDate(Day);

            if (Category != null && City == null && Site == null)
            {
                try
                {
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Сategory == Category).ToList();
                    else
                        return model.Vacancies.Where(x => x.Сategory == Category && x.PublicationDate >= rangedata).ToList();
                }
                catch
                {
                    return new List<Vacancy>();
                }
            }
            else if (Category == null && City != null && Site == null)
            {
                try
                {
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Location == City).ToList();
                    else
                        return model.Vacancies.Where(x => x.Location == City && x.PublicationDate >= rangedata).ToList();

                }
                catch
                {
                    return new List<Vacancy>();
                }
            }
            else if (Category == null && City == null && Site != null)
            {
                try
                {
                    var site = model.Sites.Where(x => x.name == Site).FirstOrDefault();
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.ParseSiteId == site.id).ToList();
                    else
                        return model.Vacancies.Where(x => x.ParseSiteId == site.id && x.PublicationDate >= rangedata).ToList();

                }
                catch
                {
                    return new List<Vacancy>();
                }
            }
            else if (Category != null && City != null && Site == null)
            {
                try
                {
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Сategory == Category && x.Location == City).ToList();
                    else
                        return model.Vacancies.Where(x => x.Сategory == Category && x.Location == City && x.PublicationDate >= rangedata).ToList();
                }
                catch
                {
                    return new List<Vacancy>();
                }
            }
            else if (Category != null && City == null && Site != null)
            {
                try
                {
                    var site = model.Sites.Where(x => x.name == Site).FirstOrDefault();
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Сategory == Category && x.ParseSiteId == site.id).ToList();
                    else
                        return model.Vacancies.Where(x => x.Сategory == Category && x.ParseSiteId == site.id && x.PublicationDate >= rangedata).ToList();


                }
                catch
                {
                    return new List<Vacancy>();
                }
            }
            else if (Category == null && City != null && Site != null)
            {
                try
                {
                    var site = model.Sites.Where(x => x.name == Site).FirstOrDefault();
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Location == City && x.ParseSiteId == site.id).ToList();
                    else
                        return model.Vacancies.Where(x => x.Location == City && x.ParseSiteId == site.id && x.PublicationDate >= rangedata).ToList();

                }
                catch
                {
                    return new List<Vacancy>();
                }

            }
            else if (Category != null && City != null && Site != null)
            {
                try
                {
                    var site = model.Sites.Where(x => x.name == Site).FirstOrDefault();
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Сategory == Category && x.ParseSiteId == site.id && x.Location == City).ToList();
                    else
                        return model.Vacancies.Where(x => x.Сategory == Category && x.ParseSiteId == site.id && x.Location == City && x.PublicationDate >= rangedata).ToList();

                }
                catch
                {
                    return new List<Vacancy>();
                }
            }
            else
            {
                return new List<Vacancy>();
            }

        }

        public List<Vacancy> GetVacanciesBySearch(string NameVacancy, string Category, string City, string Site, int Day)
        {
            DateTime rangedata = ConvertIntToDate(Day);

            if (Category != null && City == null && Site == null)
            {
                try
                {
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Сategory == Category).ToList();
                    else
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Сategory == Category && x.PublicationDate >= rangedata).ToList();

                }
                catch
                {
                    return new List<Vacancy>();
                }
            }
            else if (Category == null && City != null && Site == null)
            {
                try
                {
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Location == City).ToList();
                    else
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Location == City && x.PublicationDate >= rangedata).ToList();

                }
                catch
                {
                    return new List<Vacancy>();
                }
            }
            else if (Category == null && City == null && Site != null)
            {
                try
                {
                    var site = model.Sites.Where(x => x.name == Site).FirstOrDefault();
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.ParseSiteId == site.id).ToList();
                    else
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.ParseSiteId == site.id && x.PublicationDate >= rangedata).ToList();

                }
                catch
                {
                    return new List<Vacancy>();
                }
            }
            else if (Category != null && City != null && Site == null)
            {
                try
                {
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Сategory == Category && x.Location == City).ToList();
                    else
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Сategory == Category && x.Location == City && x.PublicationDate >= rangedata).ToList();

                }
                catch
                {
                    return new List<Vacancy>();
                }
            }
            else if (Category != null && City == null && Site != null)
            {
                try
                {
                    var site = model.Sites.Where(x => x.name == Site).FirstOrDefault();
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Сategory == Category && x.ParseSiteId == site.id).ToList();
                    else
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Сategory == Category && x.ParseSiteId == site.id && x.PublicationDate >= rangedata).ToList();

                }
                catch
                {
                    return new List<Vacancy>();
                }
            }
            else if (Category == null && City != null && Site != null)
            {
                try
                {
                    var site = model.Sites.Where(x => x.name == Site).FirstOrDefault();
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Location == City && x.ParseSiteId == site.id).ToList();
                    else
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Location == City && x.ParseSiteId == site.id && x.PublicationDate >= rangedata).ToList();

                }
                catch
                {
                    return new List<Vacancy>();
                }

            }
            else if (Category != null && City != null && Site != null)
            {
                try
                {
                    var site = model.Sites.Where(x => x.name == Site).FirstOrDefault();
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Сategory == Category && x.ParseSiteId == site.id && x.Location == City).ToList();
                    else
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Сategory == Category && x.ParseSiteId == site.id && x.Location == City && x.PublicationDate >= rangedata).ToList();

                }
                catch
                {
                    return new List<Vacancy>();
                }
            }
            else if (NameVacancy != null)
            {
                try
                {

                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy)).ToList();
                    else
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.PublicationDate >= rangedata).ToList();

                }
                catch
                {
                    return new List<Vacancy>();
                }
            }
            else
                return new List<Vacancy>();
        }

        public List<string> GetCity()
        {
            List<string> City = new List<string>();
            foreach (var item in model.Vacancies.GroupBy(x => x.Location))
            {
                City.Add(item.Key);
            }
            return City;
        }
        private void UpdateAll()
        {
            foreach (var item in parseSites)
            {
                Task.Run(() => UpdateSite(item));
            }
        }
        public void Start()
        {
            Task.Run(() => UpdateAll());
        }

      

       
    }
}
