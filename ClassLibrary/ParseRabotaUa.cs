using System;
using System.Linq;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class RabotaUAParser : Parser
    {
        const string webSite = "https://rabota.ua/";
        Dictionary<string, string> category;
        bool checkDate;
        string dayAgo;
        int webSiteId;

        public override string SiteName
        {
            get
            {
                return "Rabota.ua";
            }
        }

        public RabotaUAParser(int siteId)
        {
            category = new Dictionary<string, string>() {
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
            webSiteId = siteId;
        }
        public int GetNumberOfPages(string category)
        {
            HtmlNodeCollection itemNode = new HtmlWeb().Load(category).DocumentNode.SelectSingleNode("//dl[@class='f-text-royal-blue fd-merchant f-pagination']").ChildNodes;
            foreach (var childNode in itemNode)
            {
                if (childNode == itemNode[itemNode.Count - 2])
                {
                    return Convert.ToInt32(childNode.LastChild.InnerText);
                }
            }
            return 0;
        }

        public Vacancy ParseVacancy(HtmlNode node, ref Vacancy vacancy, DateTime date)
        {
            HtmlDocument page = new HtmlWeb().Load(vacancy.VacancyHref);
            vacancy.PublicationDate = Convert.ToDateTime(page.DocumentNode.SelectSingleNode("//meta[@property='article:published_time']").Attributes["content"].Value.Substring(0, 10));

            if (date != new DateTime())
            {
                if (vacancy.PublicationDate < date)
                {
                    checkDate = true;
                    return null;
                }
            }

            if (page.DocumentNode.SelectNodes("//div[@class='f-vacancy-inner-wrapper']") != null)
            {
                try
                {
                    ParceFirstTemplateVacancyParams(page.DocumentNode.SelectSingleNode("//div[@class='f-vacancy-inner-wrapper']"), ref vacancy);
                    ParseVacancyDescription(page.DocumentNode.SelectSingleNode("//div[@class='f-vacancy-description']").ChildNodes["div"].ChildNodes["div"], ref vacancy);
                }
                catch { }
            }
            else if (page.DocumentNode.SelectNodes("//div[@id='content_vcVwPopup_VacancyViewInner1_pnlBody']//span//table//tbody//tr[3]//td//div[1]") != null)
            {
                ParseThirdTemplateVacancyParams(page.DocumentNode.SelectSingleNode("//div[@id='content_vcVwPopup_VacancyViewInner1_pnlBody']//span//table//tbody//tr[3]//td//div[1]"), ref vacancy);
                ParseVacancyDescription(page.DocumentNode.SelectSingleNode("//div[@class='descr']"), ref vacancy);
            }
            else if (page.DocumentNode.SelectNodes("//div[@class='descr']") != null)
            {
                ParseVacancyDescription(page.DocumentNode.SelectSingleNode("//div[@class='descr']"), ref vacancy);
            }
            else if (page.DocumentNode.SelectSingleNode("//div[@id='content_vcVwPopup_VacancyViewInner1_pnlBody']//span//div//table//tr//td[2]//div") != null)
            {
                ParseVacancyDescription(page.DocumentNode.SelectSingleNode("//div[@id='content_vcVwPopup_VacancyViewInner1_pnlBody']//span//div//table//tr//td[2]//div"), ref vacancy);
            }
            else if (page.DocumentNode.SelectSingleNode("//*[@id='content_vcVwPopup_VacancyViewInner1_pnlBody']//span//table//tbody//tr[2]//td//table//tbody//tr//td[2]//div[2]") != null)
            {
                ParseVacancyDescription(page.DocumentNode.SelectSingleNode("//*[@id='content_vcVwPopup_VacancyViewInner1_pnlBody']//span//table//tbody//tr[2]//td//table//tbody//tr//td[2]//div[2]"), ref vacancy);
            }
            else if (page.DocumentNode.SelectNodes("//div[@class='d_des']") != null)
            {
                if (page.DocumentNode.SelectNodes("//div[@class='d-items']") != null && page.DocumentNode.SelectNodes("//div[@class='d_des']").FirstOrDefault().LastChild.Name == "div")
                {
                    ParseThirdTemplateVacancyParams(page.DocumentNode.SelectSingleNode("//div[@class='d_des']"), ref vacancy);
                    ParseVacancyDescription(page.DocumentNode.SelectSingleNode("//div[@class='d_des']").LastChild, ref vacancy);
                }
                else if (page.DocumentNode.SelectNodes("//div[@class='d-items']") != null)
                {
                    ParseThirdTemplateVacancyParams(page.DocumentNode.SelectSingleNode("//div[@class='d_des']"), ref vacancy);
                    ParseVacancyDescription(page.DocumentNode.SelectSingleNode("//div[@class='d_des']"), ref vacancy);
                }

                else if (page.DocumentNode.SelectNodes("//div[@class='d_des_in']") != null)
                {
                    ParseVacancyDescription(page.DocumentNode.SelectSingleNode("//div[@class='d_des_in']"), ref vacancy);
                }
                else
                {
                    ParseSecondTemplateVacancyParams(page.DocumentNode.SelectSingleNode("//div[@class='d_des']"), ref vacancy);
                    ParseVacancyDescription(page.DocumentNode.SelectSingleNode("//div[@class='d_des']"), ref vacancy);
                }
            }
            return vacancy;
        }
        public void ParseVacancyHeader(HtmlNode node, ref Vacancy vacancy, DateTime date)
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
                        vacancy.VacancyHref = (itemNode.LastChild.NodeType == HtmlNodeType.Element) ? webSite + itemNode.LastChild.Attributes["href"].Value
                            : webSite + itemNode.FirstChild.Attributes["href"].Value;
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
            ParseVacancy(node, ref vacancy, date);
        }

        public void ParceFirstTemplateVacancyParams(HtmlNode node, ref Vacancy vacancy)
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
        public void ParseSecondTemplateVacancyParams(HtmlNode node, ref Vacancy vacancy)
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
        public void ParseThirdTemplateVacancyParams(HtmlNode node, ref Vacancy vacancy)
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

        public void ParseVacancyDescription(HtmlNode node, ref Vacancy vacancy)
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
           vacancy.Description=vacancy.Description.Replace("&nbsp;", " "); 
        }

        public override IEnumerable<Vacancy> ParseByCategory(string category)
        {
            var item = this.category.Where(x => x.Key == category && x.Value != string.Empty).FirstOrDefault();

            if (item.Value != null)
            {
                int numberOfPages = GetNumberOfPages(item.Value);

                for (int i = 1; i <= numberOfPages; i++)
                {
                    HtmlNodeCollection vacancyCollection = new HtmlWeb().Load(item.Value + "/pg" + i).DocumentNode.Descendants("table").Where(x => x.Attributes["class"].Value == "f-vacancylist-tablewrap").FirstOrDefault().ChildNodes;
                    foreach (var itemNode in vacancyCollection)
                    {
                        if (itemNode != vacancyCollection[vacancyCollection.Count - 1])
                        {
                            Vacancy vacancy = new Vacancy { VacancyId = Convert.ToInt32(itemNode.Attributes["id"].Value), ParseSiteId = webSiteId ,Сategory=item.Key};
                            ParseVacancyHeader(itemNode, ref vacancy, new DateTime());
                            yield return vacancy;
                        }
                    }
                }
            }
        }
        public override IEnumerable<Vacancy> ParseByDate(string category, DateTime date)
        {
            var item = this.category.Where(x => x.Key == category && x.Value != string.Empty).FirstOrDefault();

            if (item.Value != null)
            {
                int numberOfPages = GetNumberOfPages(item.Value);
                checkDate = false;

                for (int i = 1; i <= numberOfPages; i++)
                {
                    HtmlNodeCollection vacancyCollection = new HtmlWeb().Load(item.Value + "/pg" + i).DocumentNode.Descendants("table").Where(x => x.Attributes["class"].Value == "f-vacancylist-tablewrap").FirstOrDefault().ChildNodes;
                    foreach (var itemNode in vacancyCollection)
                    {
                        if (itemNode != vacancyCollection[vacancyCollection.Count - 1])
                        {
                            if (checkDate && dayAgo != "1&nbsp;день назад")
                            {
                                checkDate = false;
                            }
                            if (!checkDate)
                            {
                                Vacancy vacancy = new Vacancy { VacancyId = Convert.ToInt32(itemNode.Attributes["id"].Value), ParseSiteId = webSiteId ,Сategory=item.Key};
                                ParseVacancyHeader(itemNode, ref vacancy, date);
                                yield return vacancy;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}