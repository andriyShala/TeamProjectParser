using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace ClassLibrary
{
    public class ParseJobsUa : Parser
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

        private string stringRegexDate;

        private Regex regex;
        private HtmlWeb web;
        private Dictionary<string, string> category;
        private int siteId;

        public override string SiteName
        {
            get { return "Jobs"; }
        }

        public ParseJobsUa(int siteId)
        {
            stringRegexDate = @"([0-9][0-9]*)(\s\S*)";
            regex = new Regex(stringRegexDate);
            web = new HtmlWeb();

            this.siteId = siteId;
            category = new Dictionary<string, string>()
            {
                {"HR, управление персоналом", "hr_human_resources"},
                {"IT, WEB специалисты", "it_web_specialists"},
                {"Банковское дело, ломбарды", "banking"},
                {"Бухгалтерия, финансы, учет/аудит", "book_keeping_bank_finance_audit"},
                {"Гостиничный бизнес", "hotel_business"},
                {"Дизайн, творчество", "design_creative"},
                {"Домашний сервис", "home_service"},
                {"Издательство, полиграфия", "publishing_polygraphy"},
                {"Консалтинг", "consulting"},
                {"Красота и SPA-услуги", "beauty_spa"},
                {"Легкая промышленность", "textile_industry"},
                {"Логистика, доставка, склад", "logistic_storage"},
                {"Медицина, фармацевтика", "medicine_farmatsiya"},
                {"Наука, образование, переводы", "education_science_translate"},
                {"Недвижимость и страхование", "real_estate_insurance"},
                {"Офисный персонал", "office_personnel"},
                {"Охрана, безопасность", "security_guard"},
                {"Производство", "production"},
                {"Реклама, маркетинг, PR", "advertising_marketing_pr"},
                {"Ремонт техники и предметов быта", "repair_of_equipment"},
                {"Ресторанный бизнес, кулинария", "restaurant_cookery"},
                {"Руководство", "supervisor"},
                {"Сельское хозяйство, агробизнес", "agriculture_agribusiness"},
                {"СМИ, TV, Радио", "media_tv_radio"},
                {"Строительство, архитектура", "building_architecture"},
                {"Сфера развлечений", "entertainment"},
                {"Телекоммуникации и связь", "telecommunications_connection"},
                {"Торговля, продажи, закупки", "trade_sales_purchase"},
                {"Транспорт, автосервис", "transport_autoservice"},
                {"Туризм и спорт", "tourism_sport"},
                {"Юриспруденция, право", "jurisprudence_law"},
                {"Работа без квалификации", "without_qualification_job"},
                {"Работа для студентов", "work_for_students"},
                {"Работа за рубежом", "work_abroad"},
                {"Людям с ограниченными возможностями", "for_people_with_disabilities"},
                {"Другие предложения", "other"}
            };

        }

        private int GetNumberMounth(string mounth)
        {
            mounth = mounth.Replace(" ", "");
            switch (mounth)
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

        private Vacancy GetContentFromHttp(string href, Vacancy vacancy)
        {
            HtmlDocument document = web.Load(href);
            HtmlNode[] links =
                document.DocumentNode.SelectNodes("//div[@class='b-vacancy-full js-item_full']").ToArray();
            foreach (var item in links[0].ChildNodes)
            {
                try
                {
                    if (item.Attributes["class"].Value == "b-vacancy-full__block b-text")
                    {
                        vacancy.Description = item.InnerText;
                    }
                    else if (item.ChildNodes[0].Attributes["class"].Value == "js-contacts-block")
                    {
                        foreach (var item1 in item.ChildNodes[0].ChildNodes.Where(x => x.Name == "div"))
                        {
                            if (item1.InnerText.Contains("Телефон:"))
                            {
                                vacancy.PhoneNumber = item1.InnerText.Replace("Телефон:", "");
                            }
                            else if (item1.InnerText.Contains("Контактное лицо:"))
                            {
                                vacancy.ContactPerson = item1.InnerText.Replace("Контактное лицо:", "");
                            }
                            else if (item1.InnerText.Contains("Адрес:"))
                            {
                                if (vacancy.ContactPerson == null)
                                {
                                    vacancy.ContactPerson = String.Empty;
                                }
                                vacancy.ContactPerson += item1.InnerText;
                            }
                            else if (item1.InnerText.Contains("Сайт:"))
                            {
                                vacancy.CompanyWebSite = item1.InnerText.Replace("Сайт:", "");
                            }
                        }
                        return vacancy;
                    }
                }
                catch
                {
                    vacancy.Description = "()";
                }
            }
            return vacancy;
        }

        private int GetnumbersOfPage(string href)
        {
            try
            {
                HtmlDocument document = web.Load(href);
                HtmlNode[] links = document.DocumentNode.SelectNodes("//div[@class='b-pager__inner']").ToArray();
                string s = links[0].ChildNodes[links[0].ChildNodes.Count - 1].InnerText;
                return Convert.ToInt32(s);
            }
            catch
            {
                return 0;
            }
        }

        private Vacancy GetVacancyByNode(HtmlNode node, string category)
        {
            try
            {
                if (node == null)
                {
                    return null;
                }
                Vacancy tempVacancy = new Vacancy();

                tempVacancy.Сategory = category;
                tempVacancy.ParseSiteId = siteId;
                foreach (var itemNode in node.ChildNodes.Where(x => x.NodeType != HtmlNodeType.Text))
                {
                    switch (itemNode.Attributes[0].Value)
                    {
                        case "b-vacancy__top":
                            foreach (
                                var childNode in
                                itemNode.ChildNodes.Where(x => x.NodeType != HtmlNodeType.Text || x.Name != "br"))
                            {
                                if (childNode.Name == "a")
                                {
                                    tempVacancy.VacancyHref = childNode.Attributes["href"].Value;
                                    tempVacancy = GetContentFromHttp(tempVacancy.VacancyHref, tempVacancy);
                                    tempVacancy.Title = childNode.InnerText;
                                }
                                else if (childNode.Name == "div")
                                {
                                    tempVacancy.Salary = childNode.InnerText.Replace("&nbsp;", "");
                                }
                                else if (childNode.Name == "span")
                                {
                                    string input = childNode.InnerText;
                                    input.Replace("&nbsp;", "");
                                    MatchCollection match = regex.Matches(input);
                                    tempVacancy.PublicationDate = new DateTime(DateTime.Now.Year,
                                        GetNumberMounth(match[0].Groups[2].Value),
                                        Convert.ToInt32(match[0].Groups[1].Value));
                                    break;
                                }
                            }
                            break;
                        case "b-vacancy__tech":
                            foreach (
                                var childNode in itemNode.ChildNodes.Where(item => item.NodeType != HtmlNodeType.Text))
                            {
                                if (childNode.Attributes["class"].Value == "b-vacancy__tech__item")
                                {
                                    tempVacancy.Company = childNode.InnerText.Replace(" ", "").Replace("&nbsp;", " ");
                                }
                                else
                                {
                                    if (childNode.ChildNodes.Count > 2)
                                        tempVacancy.Location = childNode.ChildNodes[2].InnerText;

                                    break;
                                }
                            }
                            break;
                        case "b-vacancy__tech__item":
                            switch (itemNode.ChildNodes[1].InnerText)
                            {
                                case "Образование":
                                    tempVacancy.Education = itemNode.ChildNodes[3].InnerText;
                                    break;
                                case "Опыт работы":
                                    tempVacancy.Experience = itemNode.ChildNodes[3].InnerText;
                                    break;
                                case "График работы":
                                    tempVacancy.TypeOfEmployment = itemNode.ChildNodes[3].InnerText;
                                    break;
                                default:
                                    break;

                            }
                            break;
                        default:
                            break;
                    }
                }
                return tempVacancy;
            }
            catch
            {
                return null;
            }
        }

        public override IEnumerable<Vacancy> ParseByCategory(string keyCategory)
        {
            string valuecategory = null;
            try
            {
                valuecategory = category[keyCategory];
            }
            catch
            {
                return new List<Vacancy>();
            }

            List<Vacancy> temp = new List<Vacancy>();
            List<Vacancy> temp2 = new List<Vacancy>();
            string href = "https://jobs.ua/vacancy/" + valuecategory;
            string additionalPeriod = "";
            int countpages = GetnumbersOfPage(href);
            for (int i = 1; i <= countpages; i++)
            {
                additionalPeriod = "/page-" + i;
                HtmlDocument document = null;
                HtmlNode[] links = null;

                document = web.Load(href + additionalPeriod);
                links = document.DocumentNode.SelectNodes("//ul[@class='b-vacancy__list js-items_block']").ToArray();
                List<HtmlNode> sitesvacancy = new List<HtmlNode>();
                foreach (var item in links[0].ChildNodes.Where(x => x.NodeType != HtmlNodeType.Text))
                {


                }

            }
            return temp;
        }
        public override IEnumerable<Vacancy> ParseByDate(string keyCategory, DateTime date)
        {
            string valuecategory = null;
            try
            {
                valuecategory = category[keyCategory];
            }
            catch
            {
                yield break;
            }
            string href = "https://jobs.ua/vacancy/" + valuecategory;
            string additionalPeriod = "";
            int countpages = GetnumbersOfPage(href);
            for (int i = 1; i <= countpages; i++)
            {
                additionalPeriod = "/page-" + i;
                HtmlAgilityPack.HtmlDocument document = null;
                HtmlNode[] links = null;
                try
                {
                    document = web.Load(href + additionalPeriod);
                }
                catch
                {

                    yield break;
                }

                links = document.DocumentNode.SelectNodes("//ul[@class='b-vacancy__list js-items_block']").ToArray();
                foreach (var item in links[0].ChildNodes.Where(x => x.NodeType != HtmlNodeType.Text))
                {
                    Vacancy tempVacancy = null;
                    try
                    {
                        tempVacancy = GetVacancyByNode(item, keyCategory);
                    }
                    catch
                    {
                        
                        yield break;
                    }
                  
                    if (tempVacancy.PublicationDate != date)
                    {
                        if(i!=1)
                        {
                            yield break;
                        }
                    }
                    else
                        yield return tempVacancy;

                }
               
            }
            yield break;
        }
    }
}
