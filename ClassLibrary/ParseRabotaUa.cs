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

        public override int Id
        {
            get
            {
                return this.webSiteId;
            }
        }

        public RabotaUAParser(int siteId)
        {
            category = new Dictionary<string, string>() {
                { "HR, управление персоналом", "https://rabota.ua/вакансии/hr-специалисты/украина" },
                { "IT, WEB специалисты", "https://rabota.ua/вакансии/в_интернете/украина" },
                { "Банковское дело, ломбарды", "https://rabota.ua/вакансии/в_банке/украина" },
                { "Бухгалтерия, финансы, учет/аудит", "https://rabota.ua/вакансии/в_финансах/украина" },
                { "Гостиничный бизнес", "https://rabota.ua/вакансии/гостиницы-рестораны/украина" },
                { "Дизайн, творчество", "https://rabota.ua/вакансии/дизайн-графика-фото/украина" },
                { "Домашний сервис", "https://rabota.ua/вакансии/рабочие_специальности/украина" },
                { "Издательство, полиграфия", "https://rabota.ua/вакансии/в_сми/украина" },
                { "Консалтинг", "https://rabota.ua/вакансии/в_консалтинге/украина" },
                { "Красота и SPA-услуги", "https://rabota.ua/вакансии/в_спорте/украина" },
                { "Логистика, доставка, склад", "https://rabota.ua/вакансии/логистика-таможня-склад/украина" },
                { "Медицина, фармацевтика", "https://rabota.ua/вакансии/в_медицине/украина" },
                { "Наука, образование, переводы", "https://rabota.ua/вакансии/наука-образование/украина" },
                { "Недвижимость и страхование", "https://rabota.ua/вакансии/недвижимость/украина" },
                { "Офисный персонал", "https://rabota.ua/вакансии/в_офисе/украина" },
                { "Закупки - Снабжение", "https://rabota.ua/вакансии/в_снабжении/украина"},
                { "Охрана, безопасность", "https://rabota.ua/вакансии/безопасность/украина" },
                { "Производство", "https://rabota.ua/вакансии/в_производстве/украина" },
                { "Реклама, маркетинг, PR", "https://rabota.ua/вакансии/в_маркетинге/украина" },
                { "Руководство", "https://rabota.ua/вакансии/топ-менеджмент/украина" },
                { "Сельское хозяйство, агробизнес", "https://rabota.ua/вакансии/сельское_хозяйство/украина" },
                { "Строительство, архитектура", "https://rabota.ua/вакансии/строительство-архитектура/украина" },
                { "Сфера развлечений", "https://rabota.ua/вакансии/в_шоу_бизнесе/украина" },
                { "Телекоммуникации и связь", "https://rabota.ua/вакансии/телекоммуникации-связь/украина" },
                { "Торговля, продажи, закупки", "https://rabota.ua/вакансии/в_продажах/украина" },
                { "Транспорт, автосервис", "https://rabota.ua/вакансии/в_автобизнесе/украина" },
                { "Торговля","https://rabota.ua/вакансии/торговля/украина"},
                { "Туризм и спорт", "https://rabota.ua/вакансии/туризм-путешествия/украина" },
                { "Юриспруденция, право", "https://rabota.ua/вакансии/юристы-адвокаты/украина" },
                { "Работа для студентов", "https://rabota.ua/вакансии/для_студентов/украина" },
                { "Морские специальности", "https://rabota.ua/вакансии/в_море/украина" },
                { "Государственные учреждения - Местное самоуправление", "https://rabota.ua/вакансии/государственные_учреждения/украина"},
                { "Некоммерческие - Общественные организации", "https://rabota.ua/вакансии/некоммерческие_организации/украина"},
                { "Страхование", "https://rabota.ua/вакансии/страхование/украина"} };
            webSiteId = siteId;
        }
        private int GetNumberOfPages(string category)
        {
            try
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
            catch { return 0; }
        }

        private void ParseVacancyHeader(HtmlNode node, ref Vacancy vacancy, DateTime date)
        {
            try
            {
                var items = node.Descendants("div").Where(x => x.Attributes["class"].Value == "fd-f1").FirstOrDefault().ChildNodes;
                if (node.Descendants("p").Where(x => x.Attributes["class"].Value == "f-vacancylist-agotime f-text-light-gray fd-craftsmen").FirstOrDefault() != null)
                {
                    dayAgo = node.Descendants("p").Where(x => x.Attributes["class"].Value == "f-vacancylist-agotime f-text-light-gray fd-craftsmen").FirstOrDefault().FirstChild.InnerText;
                }
                else
                {
                    dayAgo = null;
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
                            vacancy.Company = itemNode.InnerText.Trim();
                        }
                        else if (itemNode.Attributes["class"].Value == "f-vacancylist-characs-block fd-f-left-middle")
                        {
                            foreach (var nextChildNode in itemNode.ChildNodes)
                            {
                                if (nextChildNode.NodeType == HtmlNodeType.Element)
                                {
                                    if (nextChildNode.Attributes["class"].Value == "fd-merchant")
                                    {
                                        vacancy.Location = nextChildNode.InnerText.Split(',')[0].Trim();
                                    }
                                    else if (nextChildNode.Attributes["class"].Value == "fd-beefy-soldier -price")
                                    {
                                        vacancy.Salary = nextChildNode.InnerText.Trim();
                                    }
                                }
                            }
                        }
                    }
                }
                ParseVacancy(node, ref vacancy, date);
            }
            catch { ParseVacancy(node, ref vacancy, date); }
        }
        private Vacancy ParseVacancy(HtmlNode node, ref Vacancy vacancy, DateTime date)
        {
            try
            {
                HtmlDocument page = new HtmlWeb().Load(vacancy.VacancyHref);
                vacancy.PublicationDate = Convert.ToDateTime(page.DocumentNode.SelectSingleNode("//meta[@property='article:published_time']").Attributes["content"].Value.Substring(0, 10));

                if (date != new DateTime())
                {
                    if (vacancy.PublicationDate < date)
                    {
                        checkDate = true;
                        return vacancy = null;
                    }
                }
                if (page.DocumentNode.SelectNodes("//div[@class='f-vacancy-inner-wrapper']") != null)
                {
                    ParceFirstTemplateVacancyParams(page.DocumentNode.SelectSingleNode("//div[@class='f-vacancy-inner-wrapper']"), ref vacancy);
                    ParseVacancyDescription(page.DocumentNode.SelectSingleNode("//div[@class='f-vacancy-description']").ChildNodes["div"].ChildNodes["div"], ref vacancy);
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
            catch { return vacancy; }
        }

        private void ParceFirstTemplateVacancyParams(HtmlNode node, ref Vacancy vacancy)
        {
            try
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
            catch { }
        }
        private void ParseSecondTemplateVacancyParams(HtmlNode node, ref Vacancy vacancy)
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
                                    vacancy.CompanyWebSite = childNode.InnerText.Trim();
                                }
                                else if (childNode.InnerText.Contains("Вид занятости"))
                                {
                                    vacancy.TypeOfEmployment = childNode.InnerText.Trim();
                                }
                                else if (childNode.InnerText.Contains("Контактное лицо"))
                                {
                                    vacancy.ContactPerson = childNode.InnerText.Trim();
                                }
                                else if (childNode.InnerText.Contains("Опыт работы"))
                                {
                                    vacancy.Experience = childNode.InnerText.Trim();
                                }
                                else if (childNode.InnerText.Contains("Телефон"))
                                {
                                    vacancy.PhoneNumber = childNode.LastChild.InnerText.Trim();
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }
        private void ParseThirdTemplateVacancyParams(HtmlNode node, ref Vacancy vacancy)
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
                                    vacancy.ContactPerson = childNode.LastChild.InnerText.Trim();
                                }
                                else if (childNode.FirstChild.InnerText.Contains("Контактный телефон") || childNode.FirstChild.InnerText.Contains("Контактний телефон") || childNode.FirstChild.InnerText.Contains("Phone"))
                                {
                                    vacancy.PhoneNumber = childNode.LastChild.LastChild.InnerText.Trim();
                                }
                                else if (childNode.FirstChild.InnerText.Contains("Вид занятости") || childNode.FirstChild.InnerText.Contains("Вид занятості") || childNode.FirstChild.InnerText.Contains("Job Type"))
                                {
                                    vacancy.TypeOfEmployment = childNode.LastChild.InnerText.Trim();
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

        private void ParseVacancyDescription(HtmlNode node, ref Vacancy vacancy)
        {
            try
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
                                vacancy.Description += childNode.InnerText.Trim() + Environment.NewLine;
                            }
                        }
                        else
                        {
                            vacancy.Description += itemNode.InnerText.Trim() + Environment.NewLine;
                        }
                    }
                }
            }
            catch { }
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
                            Vacancy vacancy = new Vacancy { ParseSiteId = webSiteId, Сategory = item.Key };
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
                                Vacancy vacancy = new Vacancy { ParseSiteId = webSiteId, Сategory = item.Key };
                                ParseVacancyHeader(itemNode, ref vacancy, date);
                                if (vacancy != null)
                                {
                                    yield return vacancy;
                                }
                                else
                                {
                                    continue;
                                }
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
