using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using System.Threading;
using System.Threading.Tasks;

namespace ParserService
{

    public class ParseService : IParseService
    {
        List<ClassLibrary.IParser> parseSites = new List<IParser>() { new ParseJobsUa(523), new ParseRabotaUa(325), new ParserWorkUa(253) };
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
            Task.Run(() => renewalDate());
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
                Thread.Sleep(300000);
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

        public List<Vacancy> GetVacancies(string Category, string City, string Site,int Day)
        {
          
          
            if(Category!=null&&City==null&&Site==null)
            {
                try
                {

                    return model.Vacancies.Where(x => x.Сategory == Category).ToList();
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
                    return model.Vacancies.Where(x => x.Location == City).ToList();
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
                    return model.Vacancies.Where(x => x.ParseSiteId == site.id).ToList();
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
                    return model.Vacancies.Where(x => x.Сategory == Category && x.Location == City).ToList();
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
                    return model.Vacancies.Where(x => x.Сategory == Category && x.ParseSiteId == site.id).ToList();
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
                    return model.Vacancies.Where(x => x.Location == City && x.ParseSiteId == site.id).ToList();
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
                    return model.Vacancies.Where(x => x.Сategory == Category && x.ParseSiteId == site.id&&x.Location==City).ToList();
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
        
            if (Category != null && City == null && Site == null)
            {
                try
                {
                    return model.Vacancies.Where(x =>x.Title.Contains(NameVacancy)&&x.Сategory == Category).ToList();
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
                    return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Location == City).ToList();
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
                    return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.ParseSiteId == site.id).ToList();
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
                    return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Сategory == Category && x.Location == City).ToList();
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
                    return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Сategory == Category && x.ParseSiteId == site.id).ToList();
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
                    return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Location == City && x.ParseSiteId == site.id).ToList();
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
                    return model.Vacancies.Where(x => x.Title.Contains(NameVacancy) && x.Сategory == Category && x.ParseSiteId == site.id && x.Location == City).ToList();
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
    }
}
