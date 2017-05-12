using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using System.Threading;
using System.Threading.Tasks;

namespace ParserService
{
    public static class temp
    {
        public static object lockthread = new object();
        public static int id = 0;
    }
    public class ParseService : IParseService
    {
        List<ClassLibrary.IParser> parseSites = new List<IParser>() { new ParseJobsUa(523), new RabotaUAParser(325), /*new ParserWorkUa(253) */};
        DBmodel model = new DBmodel();

        public ParseService()
        {
            if (model.Sites.Count() == 0)
            {
                model.Sites.Add(new Site() { id = 523, name = "Jobs.ua" });
                model.Sites.Add(new Site() { id = 325, name = "Rabota.ua" });
                model.Sites.Add(new Site() { id = 253, name = "Work.ua" });
                model.SaveChanges();
            }
        }
              private void renewalDateSite(IParser site)
        {
            ClassLibrary.Category tempvac = new ClassLibrary.Category();
            try
            {
                foreach (var vac in tempvac.categoryCollection)
                {
                    try
                    {
                        List<ClassLibrary.Vacancy> tempvacancies = site.StartParseforDate(vac.Key, DateTime.Now);
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
                try
                {
                    foreach (var item in parseSites)
                    {
                        Task.Run(() => renewalDateSite(item));
                    }
                }
                catch
                {

                }
                Thread.Sleep(300000);
            }
        }
        public List<string> GetCategory()
        {
            List<string> categories = new List<string>();
            ClassLibrary.Category tempvac = new ClassLibrary.Category();
            foreach (var item in tempvac.categoryCollection)
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
        public List<Vacancy> GetVacancies(string Category, string City, string Site,int Day)
        {
            DateTime rangedata = ConvertIntToDate(Day);
         
            if(Category!=null&&City==null&&Site==null)
            {
                try
                {
                    if (rangedata == new DateTime())
                        return model.Vacancies.Where(x => x.Сategory == Category).ToList();
                    else
                        return model.Vacancies.Where(x => x.Сategory == Category&&x.PublicationDate>=rangedata).ToList();
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
                        return model.Vacancies.Where(x => x.Location == City&&x.PublicationDate>=rangedata).ToList();

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
                        return model.Vacancies.Where(x => x.ParseSiteId == site.id&& x.PublicationDate >= rangedata).ToList();

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
                        return model.Vacancies.Where(x => x.Сategory == Category && x.ParseSiteId == site.id&&x.Location==City).ToList();
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

        public List<Vacancy> GetVacanciesBySearch(string NameVacancy, string Category, string City, string Site,int Day)
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
                        return model.Vacancies.Where(x => x.Title.Contains(NameVacancy)&&x.PublicationDate>=rangedata).ToList();

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
            foreach (var item in model.Vacancies.GroupBy(x=>x.Location))
            {
                City.Add(item.Key);
            }
            return City;
        }
    }
}
