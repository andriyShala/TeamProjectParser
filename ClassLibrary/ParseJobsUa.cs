using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Threading;
namespace ClassLibrary
{
    public class ParseJobsUa : IParser
    {
        private class primaryVacancy
        {
            public List<Vacancy> vac;
            public HtmlNode node;
            public string vacancy;
            public primaryVacancy(ref List<Vacancy> vac, HtmlNode node, string vacancy)
            {
                this.vac = vac;
                this.node = node;
                this.vacancy = vacancy;
            }
        }
        private string pattern;
        private Task asingparse;
        private Regex regex;
        private HtmlWeb web;
        private const int countMaxPool = 5;
        private static object threadlock;
        static int runningWorkers;
        private Dictionary<string, string> mycategory;
        private int idsite;



        public ParseJobsUa(int idParseSite)
        {
            pattern = @"([0-9][0-9]*)(\s\S*)";
            regex = new Regex(pattern);
            HtmlWeb web = new HtmlWeb();
            threadlock = new object();
            runningWorkers = countMaxPool;
            idsite = idParseSite;
            mycategory = new Dictionary<string, string>() { { "HR, управление персоналом", "hr_human_resources" }, { "IT, WEB специалисты", "it_web_specialists" }, { "Банковское дело, ломбарды", "banking" },
                { "Бухгалтерия, финансы, учет/аудит", "book_keeping_bank_finance_audit" }, { "Гостиничный бизнес", "hotel_business" }, { "Дизайн, творчество", "design_creative" }, { "Домашний сервис", "home_service" },
                { "Издательство, полиграфия", "publishing_polygraphy" }, { "Консалтинг", "consulting" }, { "Красота и SPA-услуги", "beauty_spa" }, { "Легкая промышленность", "textile_industry" },
                { "Логистика, доставка, склад", "logistic_storage" }, { "Медицина, фармацевтика", "medicine_farmatsiya" }, { "Наука, образование, переводы", "education_science_translate" },
                { "Недвижимость и страхование", "real_estate_insurance" }, { "Офисный персонал", "office_personnel" }, { "Охрана, безопасность", "security_guard" }, { "Производство", "production" },
                { "Реклама, маркетинг, PR", "advertising_marketing_pr" }, { "Ремонт техники и предметов быта", "repair_of_equipment" }, { "Ресторанный бизнес, кулинария", "restaurant_cookery" },
                { "Руководство", "supervisor" }, { "Сельское хозяйство, агробизнес", "agriculture_agribusiness" }, { "СМИ, TV, Радио", "media_tv_radio" }, { "Строительство, архитектура", "building_architecture" },
                { "Сфера развлечений", "entertainment" }, { "Телекоммуникации и связь", "telecommunications_connection" }, { "Торговля, продажи, закупки", "trade_sales_purchase" },
                { "Транспорт, автосервис", "transport_autoservice" }, { "Туризм и спорт", "tourism_sport" }, { "Юриспруденция, право", "jurisprudence_law" }, { "Работа без квалификации", "without_qualification_job" },
                { "Работа для студентов", "work_for_students" }, { "Работа за рубежом", "work_abroad" }, { "Людям с ограниченными возможностями", "for_people_with_disabilities" }, { "Другие предложения", "other" } };

        }
        private int GetNumberMouth(string Mounth)
        {
            Mounth = Mounth.Replace(" ", "");
            switch (Mounth)
            {
                case "января":
                    return 1;
                case "февраля":
                    return 2;
                case "марта":
                    return 3;
                case "апреля":
                    return 4;
                case "мая":
                    return 5;
                case "июня":
                    return 6;
                case "июля":
                    return 7;
                case "августа":
                    return 8;
                case "сентября":
                    return 9;
                case "октября":
                    return 10;
                case "ноября":
                    return 11;
                case "декабря":
                    return 12;
                default:
                    return 0;
            }
        }
        private Vacancy GetContentFromHttp(string href, Vacancy vac)
        {
            HtmlAgilityPack.HtmlDocument document = web.Load(href);
            HtmlNode[] links = document.DocumentNode.SelectNodes("//div[@class='b-vacancy-full js-item_full']").ToArray();
            foreach (var item in links[0].ChildNodes)
            {
                try
                {
                    if (item.Attributes["class"].Value == "b-vacancy-full__block b-text")
                        vac.Description = item.InnerText;
                    else if (item.ChildNodes[0].Attributes["class"].Value == "js-contacts-block")
                    {
                        vac.PhoneNumber = item.ChildNodes[0].InnerText;
                        return vac;
                    }
                }
                catch
                {
                    vac.Description = "()";
                }
            }
            return vac;
        }
        private int GetnumbersOfPage(string href)
        {
            try
            {

                HtmlAgilityPack.HtmlDocument document = web.Load(href);
                HtmlNode[] links = document.DocumentNode.SelectNodes("//div[@class='b-pager__inner']").ToArray();
                string s = links[0].ChildNodes[links[0].ChildNodes.Count - 1].InnerText;
                return Convert.ToInt32(s);
            }
            catch (Exception ex)
            {
               
                return 0;
            }
        }
        private Vacancy getVacancyfromNode(HtmlNode child, string category)
        {
            Vacancy tempvacancy = new Vacancy();
            tempvacancy.Сategory = category;
            tempvacancy.ParseSiteId = idsite;
            tempvacancy.VacancyId = Convert.ToInt32(child.Attributes["id"].Value);
            foreach (var item2 in child.ChildNodes)
            {

                if (item2.Name != "#text")
                    switch (item2.Attributes[0].Value)
                    {
                        case "b-vacancy__top":
                            foreach (var it in item2.ChildNodes)
                            {
                                if (it.Name == "br" || it.Name == "#text")
                                    continue;
                                if (it.Name == "a")
                                {
                                    tempvacancy.VacancyHref = it.Attributes["href"].Value;
                                    tempvacancy = GetContentFromHttp(tempvacancy.VacancyHref, tempvacancy);
                                    tempvacancy.Title = it.InnerText;
                                }
                                else if (it.Name == "div")
                                    tempvacancy.Salary = it.InnerText.Replace("&nbsp;","");
                                else if (it.Name == "span")
                                {
                                    string input = it.InnerText;
                                    input.Replace("&nbsp;", "");
                                    MatchCollection match = regex.Matches(input);
                                    tempvacancy.PublicationDate = new DateTime(DateTime.Now.Year, GetNumberMouth(match[0].Groups[2].Value), Convert.ToInt32(match[0].Groups[1].Value));
                                    break;
                                }
                            }
                            break;
                        case "b-vacancy__tech":
                            foreach (var it in item2.ChildNodes)
                            {
                                if (it.Name != "#text")
                                    if (it.Attributes["class"].Value == "b-vacancy__tech__item")
                                        tempvacancy.Company = it.InnerText.Replace(" ","");
                                    else
                                    {
                                        if (it.ChildNodes.Count > 2)
                                            tempvacancy.Location = it.ChildNodes[2].InnerText;

                                        break;
                                    }
                            }
                            break;
                        case "b-vacancy__tech__item":
                            switch (item2.ChildNodes[1].InnerText)
                            {
                                case "Образование":
                                    tempvacancy.Education = item2.ChildNodes[3].InnerText;
                                    break;
                                case "Опыт работы":
                                    tempvacancy.Experience = item2.ChildNodes[3].InnerText;
                                    break;
                                case "График работы":
                                    tempvacancy.TypeOfEmployment = item2.ChildNodes[3].InnerText;
                                    break;
                                default:
                                    break;

                            }
                            break;
                        default:
                            break;
                    }
            }

            return tempvacancy;
        }

        public List<Vacancy> ParseVacancy(string Vacancykey)
        {
            string valuecategory = null;
            try
            {
                valuecategory = mycategory[Vacancykey];
            }
            catch
            {
                return new List<Vacancy>();
            }
            ThreadPool.SetMaxThreads(countMaxPool, countMaxPool);
            List<Vacancy> temp = new List<Vacancy>();
            List<Vacancy> temp2 = new List<Vacancy>();
            string href = "https://jobs.ua/vacancy/" + valuecategory;
            string additionalPeriod = "";
            int countpages = GetnumbersOfPage(href);
            for (int i = 1; i <= countpages; i++)
            {
                additionalPeriod = "/page-" + i;
                HtmlAgilityPack.HtmlDocument document = null;
                HtmlNode[] links = null;

                document = web.Load(href + additionalPeriod);
                links = document.DocumentNode.SelectNodes("//ul[@class='b-vacancy__list js-items_block']").ToArray();
                List<HtmlNode> sitesvacancy = new List<HtmlNode>();
                foreach (var item in links[0].ChildNodes)
                {

                    if (item.Name != "#text")
                        if (item.ChildNodes.Count > 5)
                        {
                            if (sitesvacancy.Count < countMaxPool)
                            {
                                sitesvacancy.Add(item);
                            }
                            if (asingparse == null && sitesvacancy.Count >= countMaxPool)
                            {
                                asingparse = new Task(delegate () { temp2 = Parsevac5(sitesvacancy, Vacancykey); });
                                asingparse.Start();
                                sitesvacancy.Clear();
                            }
                            if (asingparse != null && sitesvacancy.Count >= countMaxPool)
                            {
                                while (temp2.Count != countMaxPool)
                                {

                                }
                                temp.AddRange(temp2);
                                asingparse = new Task(delegate () { temp2 = Parsevac5(sitesvacancy, Vacancykey); });
                                asingparse.Start();
                                sitesvacancy.Clear();

                            }
                        }
                }
                if (asingparse != null)
                {
                    while (temp2.Count != countMaxPool)
                    {

                    }
                    temp.AddRange(temp2);

                }
                if (sitesvacancy.Count != 0)
                {
                    asingparse = new Task(delegate () { temp2 = Parsevac5(sitesvacancy, Vacancykey); });
                    asingparse.Start();
                    while (temp2.Count != sitesvacancy.Count)
                    {
                    }
                    temp.AddRange(temp2);
                    temp2.Clear();
                }

            }
            return temp;
        }
        public List<Vacancy> ParseDayVacancy(DateTime date, string Vacancykey)
        {
            string valuecategory = null;
            try
            {
                valuecategory = mycategory[Vacancykey];
            }
            catch
            {
                return new List<Vacancy>();
            }
          
            bool endread = false;
            ThreadPool.SetMaxThreads(countMaxPool, countMaxPool);
            List<Vacancy> temp = new List<Vacancy>();
            List<Vacancy> temp2 = new List<Vacancy>();
            string href = "https://jobs.ua/vacancy/" + valuecategory;
            string additionalPeriod = "";
            int countpages = GetnumbersOfPage(href);
            for (int i = 1; i <= countpages; i++)
            {
                if (endread == true)
                {
                    break;
                }
                additionalPeriod = "/page-" + i;
                HtmlAgilityPack.HtmlDocument document = null;
                HtmlNode[] links = null;

                document = web.Load(href + additionalPeriod);
                links = document.DocumentNode.SelectNodes("//ul[@class='b-vacancy__list js-items_block']").ToArray();
                List<HtmlNode> sitesvacancy = new List<HtmlNode>();
                foreach (var item in links[0].ChildNodes)
                {
                    if (endread == true)
                    { break; }
                    if (item.Name != "#text")
                        if (item.ChildNodes.Count > 5)
                        {
                            if (sitesvacancy.Count < countMaxPool)
                            {
                                sitesvacancy.Add(item);
                            }
                            if (asingparse == null && sitesvacancy.Count >= countMaxPool)
                            {
                                asingparse = new Task(delegate () { temp2 = Parsevac5(sitesvacancy, Vacancykey); });
                                asingparse.Start();
                                sitesvacancy.Clear();
                            }
                            if (asingparse != null && sitesvacancy.Count >= countMaxPool)
                            {
                                while (temp2.Count != countMaxPool)
                                {

                                }
                                foreach (var it in temp2)
                                {
                                    if (it.PublicationDate.Day != date.Day)
                                    {
                                        break;
                                        endread = true;
                                    }
                                    else
                                        temp.Add(it);
                                }
                                asingparse = new Task(delegate () { temp2 = Parsevac5(sitesvacancy, Vacancykey); });
                                asingparse.Start();
                                sitesvacancy.Clear();

                            }
                        }
                }
                if (asingparse != null)
                {
                    while (temp2.Count != countMaxPool)
                    {

                    }
                    foreach (var it in temp2)
                    {
                        if (it.PublicationDate.Day != date.Day)
                        {
                            break;
                            endread = true;
                        }
                        else
                            temp.Add(it);
                    }

                }
                if (sitesvacancy.Count != 0)
                {
                    asingparse = new Task(delegate () { temp2 = Parsevac5(sitesvacancy, Vacancykey); });
                    asingparse.Start();
                    while (temp2.Count != sitesvacancy.Count)
                    {
                    }
                    foreach (var it in temp2)
                    {
                        if (it.PublicationDate.Day != date.Day)
                        {
                            break;
                            endread = true;
                        }
                        else
                            temp.Add(it);
                    }
                    temp2.Clear();
                }

            }
            return temp;

        }


        private List<Vacancy> Parsevac5(List<HtmlNode> node, string Vacancy)
        {
            runningWorkers = countMaxPool;
            List<Vacancy> listVac = new List<Vacancy>();
            for (int i = 0; i < node.Count; i++)
            {
                ThreadPool.QueueUserWorkItem(getVacancyfromPrimaryVacancy, new primaryVacancy(ref listVac, node[i], Vacancy));
            }
                while (runningWorkers > 0)
                {
                    Monitor.Wait(threadlock);
                }
            return listVac;
        }
        private void getVacancyfromPrimaryVacancy(object ev)
        {
            primaryVacancy tempEV = ev as primaryVacancy;
            Vacancy vac = new Vacancy();
            vac = getVacancyfromNode(tempEV.node, tempEV.vacancy);
            lock (threadlock)
            {
                tempEV.vac.Add(vac);
                runningWorkers--;
                Monitor.Pulse(threadlock);
            }
        }

        public List<Vacancy> StartParseAll(string keyCategory)
        {
            return ParseVacancy(keyCategory);
        }

        public List<Vacancy> StartParseforDate(string keyCategory, DateTime date)
        {
            return ParseDayVacancy(date, keyCategory);
        }



    }
}
