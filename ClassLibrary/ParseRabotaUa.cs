using System;
using System.Linq;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class RabotaUAParser : IParser
    {
        const string website = "https://rabota.ua/";
        Dictionary<string, string> rubric;
        string dayAgo = null;
        int webSiteId;
        bool checkDate = false;

        public RabotaUAParser(int id)
        {
            rubric = new Dictionary<string, string>() {
                { "HR, управление персоналом", "https://rabota.ua/%D0%B2%D0%B0%D0%BA%D0%B0%D0%BD%D1%81%D0%B8%D0%B8/hr-%D1%81%D0%BF%D0%B5%D1%86%D0%B8%D0%B0%D0%BB%D0%B8%D1%81%D1%82%D1%8B/%D1%83%D0%BA%D1%80%D0%B0%D0%B8%D0%BD%D0%B0" },
                { "IT, WEB специалисты", "https://rabota.ua/%D0%B2%D0%B0%D0%BA%D0%B0%D0%BD%D1%81%D0%B8%D0%B8/%D0%B2_%D0%B8%D0%BD%D1%82%D0%B5%D1%80%D0%BD%D0%B5%D1%82%D0%B5/%D1%83%D0%BA%D1%80%D0%B0%D0%B8%D0%BD%D0%B0" },
                { "Банковское дело, ломбарды", "https://rabota.ua/%D0%B2%D0%B0%D0%BA%D0%B0%D0%BD%D1%81%D0%B8%D0%B8/%D0%B2_%D0%B1%D0%B0%D0%BD%D0%BA%D0%B5/%D1%83%D0%BA%D1%80%D0%B0%D0%B8%D0%BD%D0%B0" },
                { "Бухгалтерия, финансы, учет/аудит", "https://rabota.ua/%D0%B2%D0%B0%D0%BA%D0%B0%D0%BD%D1%81%D0%B8%D0%B8/%D0%B2_%D1%84%D0%B8%D0%BD%D0%B0%D0%BD%D1%81%D0%B0%D1%85/%D1%83%D0%BA%D1%80%D0%B0%D0%B8%D0%BD%D0%B0" },
                { "Гостиничный бизнес", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b3%d0%be%d1%81%d1%82%d0%b8%d0%bd%d0%b8%d1%86%d1%8b-%d1%80%d0%b5%d1%81%d1%82%d0%be%d1%80%d0%b0%d0%bd%d1%8b/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Дизайн, творчество", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b4%d0%b8%d0%b7%d0%b0%d0%b9%d0%bd-%d0%b3%d1%80%d0%b0%d1%84%d0%b8%d0%ba%d0%b0-%d1%84%d0%be%d1%82%d0%be/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Домашний сервис", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d1%80%d0%b0%d0%b1%d0%be%d1%87%d0%b8%d0%b5_%d1%81%d0%bf%d0%b5%d1%86%d0%b8%d0%b0%d0%bb%d1%8c%d0%bd%d0%be%d1%81%d1%82%d0%b8/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Издательство, полиграфия", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b2_%d1%81%d0%bc%d0%b8/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Консалтинг", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b2_%d0%ba%d0%be%d0%bd%d1%81%d0%b0%d0%bb%d1%82%d0%b8%d0%bd%d0%b3%d0%b5/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Красота и SPA-услуги", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b2_%d0%bc%d0%b0%d1%80%d0%ba%d0%b5%d1%82%d0%b8%d0%bd%d0%b3%d0%b5/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Легкая промышленность", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b2_%d0%bf%d1%80%d0%be%d0%b8%d0%b7%d0%b2%d0%be%d0%b4%d1%81%d1%82%d0%b2%d0%b5/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Логистика, доставка, склад", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%bb%d0%be%d0%b3%d0%b8%d1%81%d1%82%d0%b8%d0%ba%d0%b0-%d1%82%d0%b0%d0%bc%d0%be%d0%b6%d0%bd%d1%8f-%d1%81%d0%ba%d0%bb%d0%b0%d0%b4/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Медицина, фармацевтика", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b2_%d0%bc%d0%b5%d0%b4%d0%b8%d1%86%d0%b8%d0%bd%d0%b5/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Наука, образование, переводы", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%bd%d0%b0%d1%83%d0%ba%d0%b0-%d0%be%d0%b1%d1%80%d0%b0%d0%b7%d0%be%d0%b2%d0%b0%d0%bd%d0%b8%d0%b5/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Недвижимость и страхование", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%bd%d0%b5%d0%b4%d0%b2%d0%b8%d0%b6%d0%b8%d0%bc%d0%be%d1%81%d1%82%d1%8c/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Офисный персонал", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b2_%d0%bc%d0%b0%d1%80%d0%ba%d0%b5%d1%82%d0%b8%d0%bd%d0%b3%d0%b5/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Охрана, безопасность", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b1%d0%b5%d0%b7%d0%be%d0%bf%d0%b0%d1%81%d0%bd%d0%be%d1%81%d1%82%d1%8c/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Производство", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b2_%d0%bf%d1%80%d0%be%d0%b8%d0%b7%d0%b2%d0%be%d0%b4%d1%81%d1%82%d0%b2%d0%b5/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Реклама, маркетинг, PR", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b2_%d0%bc%d0%b0%d1%80%d0%ba%d0%b5%d1%82%d0%b8%d0%bd%d0%b3%d0%b5/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Ремонт техники и предметов быта", "" },
                { "Ресторанный бизнес, кулинария", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b3%d0%be%d1%81%d1%82%d0%b8%d0%bd%d0%b8%d1%86%d1%8b-%d1%80%d0%b5%d1%81%d1%82%d0%be%d1%80%d0%b0%d0%bd%d1%8b/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Руководство", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d1%82%d0%be%d0%bf-%d0%bc%d0%b5%d0%bd%d0%b5%d0%b4%d0%b6%d0%bc%d0%b5%d0%bd%d1%82/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Сельское хозяйство, агробизнес", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d1%81%d0%b5%d0%bb%d1%8c%d1%81%d0%ba%d0%be%d0%b5_%d1%85%d0%be%d0%b7%d1%8f%d0%b9%d1%81%d1%82%d0%b2%d0%be/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "СМИ, TV, Радио", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b2_%d1%81%d0%bc%d0%b8/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Строительство, архитектура", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d1%81%d1%82%d1%80%d0%be%d0%b8%d1%82%d0%b5%d0%bb%d1%8c%d1%81%d1%82%d0%b2%d0%be-%d0%b0%d1%80%d1%85%d0%b8%d1%82%d0%b5%d0%ba%d1%82%d1%83%d1%80%d0%b0/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Сфера развлечений", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b2_%d1%88%d0%be%d1%83_%d0%b1%d0%b8%d0%b7%d0%bd%d0%b5%d1%81%d0%b5/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Телекоммуникации и связь", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d1%82%d0%b5%d0%bb%d0%b5%d0%ba%d0%be%d0%bc%d0%bc%d1%83%d0%bd%d0%b8%d0%ba%d0%b0%d1%86%d0%b8%d0%b8-%d1%81%d0%b2%d1%8f%d0%b7%d1%8c/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Торговля, продажи, закупки", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b2_%d0%bf%d1%80%d0%be%d0%b4%d0%b0%d0%b6%d0%b0%d1%85/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Транспорт, автосервис", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b2_%d0%b0%d0%b2%d1%82%d0%be%d0%b1%d0%b8%d0%b7%d0%bd%d0%b5%d1%81%d0%b5/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Туризм и спорт", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d1%82%d1%83%d1%80%d0%b8%d0%b7%d0%bc-%d0%bf%d1%83%d1%82%d0%b5%d1%88%d0%b5%d1%81%d1%82%d0%b2%d0%b8%d1%8f/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Юриспруденция, право", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d1%8e%d1%80%d0%b8%d1%81%d1%82%d1%8b-%d0%b0%d0%b4%d0%b2%d0%be%d0%ba%d0%b0%d1%82%d1%8b/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Работа без квалификации", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b4%d0%bb%d1%8f_%d1%81%d1%82%d1%83%d0%b4%d0%b5%d0%bd%d1%82%d0%be%d0%b2/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Работа для студентов", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b4%d0%bb%d1%8f_%d1%81%d1%82%d1%83%d0%b4%d0%b5%d0%bd%d1%82%d0%be%d0%b2/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Работа за рубежом", "" },
                { "Людям с ограниченными возможностями", "" },
                { "Другие предложения", "" },
                { "Морские специальности", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b2_%d0%bc%d0%be%d1%80%d0%b5/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0" },
                { "Государственные учреждения - Местное самоуправление", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%b3%d0%be%d1%81%d1%83%d0%b4%d0%b0%d1%80%d1%81%d1%82%d0%b2%d0%b5%d0%bd%d0%bd%d1%8b%d0%b5_%d1%83%d1%87%d1%80%d0%b5%d0%b6%d0%b4%d0%b5%d0%bd%d0%b8%d1%8f/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0"},
                { "Некоммерческие - Общественные организации", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d0%bd%d0%b5%d0%ba%d0%be%d0%bc%d0%bc%d0%b5%d1%80%d1%87%d0%b5%d1%81%d0%ba%d0%b8%d0%b5_%d0%be%d1%80%d0%b3%d0%b0%d0%bd%d0%b8%d0%b7%d0%b0%d1%86%d0%b8%d0%b8/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0"},
                { "Страхование", "https://rabota.ua/%d0%b2%d0%b0%d0%ba%d0%b0%d0%bd%d1%81%d0%b8%d0%b8/%d1%81%d1%82%d1%80%d0%b0%d1%85%d0%be%d0%b2%d0%b0%d0%bd%d0%b8%d0%b5/%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0"} };
            webSiteId = id;
        }

        public int GetNumberOfPages(string category)
        {
            var itemNode = new HtmlWeb().Load(category).DocumentNode.SelectSingleNode("//dl[@class='f-text-royal-blue fd-merchant f-pagination']").ChildNodes;
            foreach (var childNode in itemNode)
            {
                if (childNode == itemNode[itemNode.Count - 2])
                    return Convert.ToInt32(childNode.LastChild.InnerText);
            }
            return 0;
        }

        public void ParseVacancyPage(KeyValuePair<string,string> caregory, int pageNumber, ref List<Vacancy> list, DateTime dateTime)
        {
            var vacancyCollection = new HtmlWeb().Load(caregory.Value + "/pg" + pageNumber).DocumentNode.Descendants("table").Where(x => x.Attributes["class"].Value == "f-vacancylist-tablewrap").FirstOrDefault().ChildNodes;
            foreach (var itemNode in vacancyCollection)
            {
                if (itemNode != vacancyCollection[vacancyCollection.Count - 1])
                {
                    ParseVacancyHeader(new Vacancy { VacancyId = Convert.ToInt32(itemNode.Attributes["id"].Value), ParseSiteId = webSiteId,Сategory=caregory.Key }, itemNode, ref list, dateTime);
                }
            }
        }
        public void ParseVacancyHeader(Vacancy vacancy, HtmlNode node, ref List<Vacancy> list, DateTime dateTime)
        {
            var items = node.Descendants("div").Where(x => x.Attributes["class"].Value == "fd-f1").FirstOrDefault().ChildNodes;
            if (node.Descendants("p").Where(x => x.Attributes["class"].Value == "f-vacancylist-agotime f-text-light-gray fd-craftsmen").FirstOrDefault() != null)
            {
                dayAgo = node.Descendants("p").Where(x => x.Attributes["class"].Value == "f-vacancylist-agotime f-text-light-gray fd-craftsmen").FirstOrDefault().FirstChild.InnerText;
            }
            foreach (var itemNode in items)
            {
                if (itemNode.NodeType == HtmlNodeType.Element)
                {
                    if (itemNode.Attributes["class"].Value == "fd-beefy-gunso f-vacancylist-vacancytitle")
                    {
                        vacancy.Title = itemNode.InnerText;
                        vacancy.VacancyHref = (itemNode.LastChild.NodeType == HtmlNodeType.Element) ? website + itemNode.LastChild.Attributes["href"].Value
                            : website + itemNode.FirstChild.Attributes["href"].Value;
                    }
                    else if (itemNode.Attributes["class"].Value == "f-vacancylist-companyname fd-merchant f-text-dark-bluegray")
                    {
                        vacancy.Company = itemNode.InnerText;
                    }
                    else if (itemNode.Attributes["class"].Value == "f-vacancylist-characs-block fd-f-left-middle")
                    {
                        foreach (var nextChildNode in itemNode.ChildNodes)
                        {
                            if (nextChildNode.NodeType == HtmlNodeType.Element)
                            {
                                if (nextChildNode.Attributes["class"].Value == "fd-merchant")
                                {
                                    vacancy.Location = nextChildNode.InnerText;
                                }
                                else if (nextChildNode.Attributes["class"].Value == "fd-beefy-soldier -price")
                                {
                                    vacancy.Salary = nextChildNode.InnerText;
                                }
                            }
                        }
                    }
                }
            }
            ParseVacancy(vacancy, ref list, dateTime);
        }

        public void ParseVacancy(Vacancy vacancy, ref List<Vacancy> list, DateTime dateTime)
        {
            HtmlDocument page = new HtmlWeb().Load(vacancy.VacancyHref);
            vacancy.PublicationDate = Convert.ToDateTime(page.DocumentNode.SelectSingleNode("//meta[@property='article:published_time']").Attributes["content"].Value.Substring(0, 10));

            if (dateTime != new DateTime())
            {
                if (vacancy.PublicationDate < dateTime)
                {
                    checkDate = true;
                    return;
                }
            }

            if (page.DocumentNode.SelectNodes("//div[@class='f-vacancy-inner-wrapper']") != null)
            {
                try
                {
                    ParceFirstTemplateVacancyParams(vacancy, page.DocumentNode.SelectSingleNode("//div[@class='f-vacancy-inner-wrapper']"));
                    ParseVacancyDescription(vacancy, page.DocumentNode.SelectSingleNode("//div[@class='f-vacancy-description']").ChildNodes["div"].ChildNodes["div"]);
                }
                catch { }
            }
            else if (page.DocumentNode.SelectNodes("//div[@id='content_vcVwPopup_VacancyViewInner1_pnlBody']//span//table//tbody//tr[3]//td//div[1]") != null)
            {
                ParseThirdTemplateVacancyParams(vacancy, page.DocumentNode.SelectSingleNode("//div[@id='content_vcVwPopup_VacancyViewInner1_pnlBody']//span//table//tbody//tr[3]//td//div[1]"));
                ParseVacancyDescription(vacancy, page.DocumentNode.SelectSingleNode("//div[@class='descr']"));
            }
            else if (page.DocumentNode.SelectNodes("//div[@class='descr']") != null)
            {
                ParseVacancyDescription(vacancy, page.DocumentNode.SelectSingleNode("//div[@class='descr']"));
            }
            else if (page.DocumentNode.SelectSingleNode("//div[@id='content_vcVwPopup_VacancyViewInner1_pnlBody']//span//div//table//tr//td[2]//div") != null)
            {
                ParseVacancyDescription(vacancy, page.DocumentNode.SelectSingleNode("//div[@id='content_vcVwPopup_VacancyViewInner1_pnlBody']//span//div//table//tr//td[2]//div"));
            }
            else if (page.DocumentNode.SelectSingleNode("//*[@id='content_vcVwPopup_VacancyViewInner1_pnlBody']//span//table//tbody//tr[2]//td//table//tbody//tr//td[2]//div[2]") != null)
            {
                ParseVacancyDescription(vacancy, page.DocumentNode.SelectSingleNode("//*[@id='content_vcVwPopup_VacancyViewInner1_pnlBody']//span//table//tbody//tr[2]//td//table//tbody//tr//td[2]//div[2]"));
            }
            else if (page.DocumentNode.SelectNodes("//div[@class='d_des']") != null)
            {
                if (page.DocumentNode.SelectNodes("//div[@class='d-items']") != null && page.DocumentNode.SelectNodes("//div[@class='d_des']").FirstOrDefault().LastChild.Name == "div")
                {
                    ParseThirdTemplateVacancyParams(vacancy, page.DocumentNode.SelectSingleNode("//div[@class='d_des']"));
                    ParseVacancyDescription(vacancy, page.DocumentNode.SelectSingleNode("//div[@class='d_des']").LastChild);
                }
                else if (page.DocumentNode.SelectNodes("//div[@class='d-items']") != null)
                {
                    ParseThirdTemplateVacancyParams(vacancy, page.DocumentNode.SelectSingleNode("//div[@class='d_des']"));
                    ParseVacancyDescription(vacancy, page.DocumentNode.SelectSingleNode("//div[@class='d_des']"));
                }

                else if (page.DocumentNode.SelectNodes("//div[@class='d_des_in']") != null)
                {
                    ParseVacancyDescription(vacancy, page.DocumentNode.SelectSingleNode("//div[@class='d_des_in']"));
                }
                else
                {
                    ParseSecondTemplateVacancyParams(vacancy, page.DocumentNode.SelectSingleNode("//div[@class='d_des']"));
                    ParseVacancyDescription(vacancy, page.DocumentNode.SelectSingleNode("//div[@class='d_des']"));
                }
            }
            list.Add(vacancy);
        }
        public void ParceFirstTemplateVacancyParams(Vacancy vacancy, HtmlNode node)
        {
            foreach (var itemNode in node.SelectSingleNode("//ul[@class='fd-thin-farmer']").ChildNodes)
            {
                switch (itemNode.FirstChild.InnerText)
                {
                    case "Контакт:": vacancy.ContactPerson = itemNode.LastChild.InnerText; break;
                    case "Телефон:": vacancy.PhoneNumber = itemNode.LastChild.LastChild.InnerText; break;
                    case "Сайт:": vacancy.CompanyWebSite = itemNode.LastChild.InnerText; break;
                }
            }

            foreach (var itemNode in node.SelectSingleNode("//div[@class='f-additional-params']").ChildNodes)
            {
                if (itemNode.Attributes["title"].Value == "Вид занятости")
                {
                    vacancy.TypeOfEmployment = itemNode.InnerText;
                }
            }
        }
        public void ParseSecondTemplateVacancyParams(Vacancy vacancy, HtmlNode node)
        {
            try
            {
                foreach (var itemNode in node.SelectSingleNode("//div[@class='d_des']").ChildNodes["table"].ChildNodes)
                {
                    if (itemNode.NodeType == HtmlNodeType.Element)
                    {
                        foreach (var childNode in itemNode.ChildNodes)
                        {
                            if (childNode.NodeType == HtmlNodeType.Element)
                            {
                                if (childNode.InnerText.Contains("Сайт"))
                                {
                                    vacancy.CompanyWebSite = childNode.InnerText;
                                }
                                else if (childNode.InnerText.Contains("Вид занятости"))
                                {
                                    vacancy.TypeOfEmployment = childNode.InnerText;
                                }
                                else if (childNode.InnerText.Contains("Контактное лицо"))
                                {
                                    vacancy.ContactPerson = childNode.InnerText;
                                }
                                else if (childNode.InnerText.Contains("Опыт работы"))
                                {
                                    vacancy.Experience = childNode.InnerText;
                                }
                                else if (childNode.InnerText.Contains("Телефон"))
                                {
                                    vacancy.PhoneNumber = childNode.LastChild.InnerText;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }
        public void ParseThirdTemplateVacancyParams(Vacancy vacancy, HtmlNode node)
        {
            try
            {
                foreach (var itemNode in node.SelectSingleNode("//div[@class='d-items']").ChildNodes)
                {
                    if (itemNode.NodeType == HtmlNodeType.Element)
                    {
                        foreach (var childNode in itemNode.ChildNodes)
                        {
                            if (childNode.NodeType == HtmlNodeType.Element)
                            {
                                if (childNode.FirstChild.InnerText.Contains("Контактное лицо") || childNode.FirstChild.InnerText.Contains("Контактна особа") || childNode.FirstChild.InnerText.Contains("Contact person"))
                                {
                                    vacancy.ContactPerson = childNode.LastChild.InnerText;
                                }
                                else if (childNode.FirstChild.InnerText.Contains("Контактный телефон") || childNode.FirstChild.InnerText.Contains("Контактний телефон") || childNode.FirstChild.InnerText.Contains("Phone"))
                                {
                                    vacancy.PhoneNumber = childNode.LastChild.LastChild.InnerText;
                                }
                                else if (childNode.FirstChild.InnerText.Contains("Вид занятости") || childNode.FirstChild.InnerText.Contains("Вид занятості") || childNode.FirstChild.InnerText.Contains("Job Type"))
                                {
                                    vacancy.TypeOfEmployment = childNode.LastChild.InnerText;
                                }
                                else if (childNode.FirstChild.InnerText.Contains("Сайт") || childNode.FirstChild.InnerText.Contains("Website"))
                                {
                                    vacancy.CompanyWebSite = childNode.LastChild.Attributes["href"].Value;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }
        public void ParseVacancyDescription(Vacancy vacancy, HtmlNode node)
        {
            foreach (var itemNode in node.ChildNodes)
            {
                if (itemNode.NodeType == HtmlNodeType.Element && (itemNode.Name == "p" || itemNode.Name == "ul"))
                {
                    if (itemNode.InnerText == "&nbsp;")
                    {
                        continue;
                    }
                    if (itemNode.Name == "ul")
                    {
                        foreach (var childNode in itemNode.ChildNodes)
                        {
                            vacancy.Description += childNode.InnerText + Environment.NewLine;
                        }
                    }
                    else
                    {
                        vacancy.Description += itemNode.InnerText + Environment.NewLine;
                    }
                }
            }
        }

        public List<Vacancy> ParseByCategory(string keyCategory)
        {
            var item = rubric.Where(x => x.Key == keyCategory && x.Value != string.Empty).FirstOrDefault();

            if (item.Value != null)
            {
                List<Vacancy> temp = new List<Vacancy>();
                int numberOfPages = GetNumberOfPages(item.Value);

                for (int i = 1; i <= numberOfPages; i++)
                {
                    ParseVacancyPage(item, i, ref temp, new DateTime());
                }
                return temp;
            }

            return null;
        }

        public List<Vacancy> ParseByDate(string keyCategory, DateTime date)
        {
            var item = rubric.Where(x => x.Key == keyCategory && x.Value != string.Empty).FirstOrDefault();
            if (item.Value != null)
            {
                List<Vacancy> temp = new List<Vacancy>();
                int numberOfPages = GetNumberOfPages(item.Value);
                checkDate = false;

                for (int i = 1; i <= numberOfPages; i++)
                {
                    if (checkDate && dayAgo != "1&nbsp;день назад")
                    {
                        checkDate = false;
                    }
                    if (!checkDate)
                    {
                        ParseVacancyPage(item, i, ref temp, date);
                    }
                    else
                    {
                        break;
                    }
                }
                return temp;
            }
            return null;
        }
    }
}
