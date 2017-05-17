using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;

namespace ClassLibrary
{
    public class ParserOlxUa : Parser
    {
        private int id = 0;
        public ParserOlxUa(int id)
        {
            this.id = id;
        }
        public override int Id
        {
            get
            {
                return this.id;
            }
        }
        private void getInnerInformation(string link, ref Vacancy vacancy)
        {
            var Webget = new HtmlWeb();
            var doc = Webget.Load(link);

            int startIndex = link.IndexOf("D");
            string numberId = link.Substring(startIndex + 1, 5);
            try
            {
                using (var client = new WebClient())
                {
                    var responseString = client.DownloadString(string.Format("https://www.olx.ua/ajax/misc/contact/phone/{0}", numberId));
                    Regex regexF = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *\d{4}");
                    Regex regexS = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *\d{2}-? *\d{2}");
                    var numbersOne = regexF.Matches(responseString);
                    var numbersTwo = regexS.Matches(responseString);

                    if (numbersOne.Count != 0)
                    {
                        foreach (var item in numbersOne)
                            vacancy.PhoneNumber += item + "/n";
                    }
                    else
                    {
                        foreach (var item in numbersTwo)
                        {
                            vacancy.PhoneNumber += item + "/n";
                        }
                    }
                }
            }
            catch
            { }

            if (doc.DocumentNode.SelectSingleNode("//p[@class='pding10 lheight20 large']") != null)
                vacancy.Description = doc.DocumentNode.SelectSingleNode("//p[@class='pding10 lheight20 large']").InnerText.Trim();
            if (doc.DocumentNode.SelectSingleNode("//ul[@class='offer-parameters']//li[2]//a//strong") != null)
                vacancy.TypeOfEmployment = doc.DocumentNode.SelectSingleNode("//ul[@class='offer-parameters']//li[2]//a//strong").InnerText.Trim();
            if (doc.DocumentNode.SelectSingleNode("//div[@class='offer-user__details']//h4//a") != null)
                vacancy.ContactPerson = doc.DocumentNode.SelectSingleNode("//div[@class='offer-user__details']//h4//a").InnerText.Trim();
            if (doc.DocumentNode.SelectSingleNode("//strong[@class='x-large not-arranged']") != null)
                vacancy.Salary = doc.DocumentNode.SelectSingleNode("//strong[@class='x-large not-arranged']").InnerText.Trim();

            vacancy.PublicationDate = getTime(link);


        }

        private DateTime getTime(string link)
        {
            var Webget = new HtmlWeb();
            var doc = Webget.Load(link);
            string subDateString = "";
            string fullDate = "";

            if (doc.DocumentNode.SelectSingleNode("//div[@class='offer-titlebox__details']//em") != null)
                subDateString = doc.DocumentNode.SelectSingleNode("//div[@class='offer-titlebox__details']//em").InnerText.Trim();
            Regex getDate = new Regex("([^,][^,]*)");
            try
            {
                string date = getDate.Matches(subDateString)[1].Value;


                string[] dateSplit = date.Split(' ');

                string month = dateSplit[2];
                fullDate = dateSplit[1];

                switch (month)
                {
                    case "января":
                        fullDate += ".01.";
                        break;
                    case "февраля":
                        fullDate += ".02.";
                        break;
                    case "марта":
                        fullDate += ".03.";
                        break;
                    case "апреля":
                        fullDate += ".04.";
                        break;
                    case "мая":
                        fullDate += ".05.";
                        break;
                    case "июня":
                        fullDate += ".06.";
                        break;
                    case "июля":
                        fullDate += ".07.";
                        break;
                    case "августа":
                        fullDate += ".08.";
                        break;
                    case "сентября":
                        fullDate += ".09.";
                        break;
                    case "октября":
                        fullDate += ".10.";
                        break;
                    case "ноября":
                        fullDate += ".11.";
                        break;
                    case "декабря":
                        fullDate += ".12.";
                        break;
                }
                fullDate += dateSplit[3];
            }
            catch
            {

            }
            return Convert.ToDateTime(fullDate);
        }





        public override IEnumerable<Vacancy> ParseByCategory(string keyCategory)
        {
            List<Vacancy> tempList = new List<Vacancy>();
            int page = 0;
            string url = "";
            try
            {
                url = "https://www.olx.ua/rabota/" + categoryCollection[keyCategory];
            }
            catch
            {
                return null;
            }
            var Webget = new HtmlWeb();
            var doc = Webget.Load(url);


            var pages = doc.DocumentNode.SelectSingleNode("//div[@class='pager rel clr']").ChildNodes;
            int pageCount = Convert.ToInt32(pages[pages.Count - 4].InnerText.Trim());

            if (categoryCollection[keyCategory] != "")
            {

                while (page < pageCount)
                {


                    foreach (var node in doc.DocumentNode.SelectNodes("//table//tbody//tr[@class='wrap']//td//article"))
                    {

                        Vacancy newVacancy = null;
                        try
                        {
                            newVacancy = new Vacancy() { ParseSiteId = id };
                            newVacancy.Сategory = keyCategory;
                            if (node.SelectSingleNode("//h3") != null)
                            {
                                string title = node.SelectSingleNode("div[1]//h3").InnerText.Trim();
                                newVacancy.Title = title;
                                string link = node.SelectSingleNode("div[1]//h3//a").Attributes["href"].Value;
                                newVacancy.VacancyHref = link;
                                getInnerInformation(link, ref newVacancy);

                            }

                            if (node.SelectSingleNode("//ul") != null)
                            {
                                string city = node.SelectSingleNode("div[1]//ul//li[1]").InnerText;
                                if (city.Contains(","))
                                {
                                    city = city.Remove(city.IndexOf(','));
                                    newVacancy.Location = city;
                                }
                                else
                                {
                                    newVacancy.Location = city;
                                }
                            }
                        }
                        catch
                        {

                        }
                        if (newVacancy != null)
                            tempList.Add(newVacancy);
                    }
                    page++;
                    url = null;
                    while (url == null)
                    {
                        try
                        {
                            url = "https://www.olx.ua/rabota/" + categoryCollection[keyCategory] + "/?page=" + page;
                        }
                        catch { }
                    }
                    Webget = new HtmlWeb();
                    doc = null;
                    while (doc == null)
                    {
                        try
                        {
                            doc = Webget.Load(url);
                        }
                        catch { }
                    }

                }
            }
            return tempList;
        }

        public override IEnumerable<Vacancy> ParseByDate(string keyCategory, DateTime date)
        {
            List<Vacancy> tempList = new List<Vacancy>();
            int countFive = 0;
            int page = 1;
            string url = "";
            try
            {
                url = "https://www.olx.ua/rabota/" + categoryCollection[keyCategory];
            }
            catch
            {
                return null;
            }
            var Webget = new HtmlWeb();
            var doc = Webget.Load(url);


            var pages = doc.DocumentNode.SelectSingleNode("//div[@class='pager rel clr']").ChildNodes;
            int pageCount = Convert.ToInt32(pages[pages.Count - 4].InnerText.Trim());

            if (categoryCollection[keyCategory] != "")
            {
                while (page < pageCount)
                {
                    foreach (var node in doc.DocumentNode.SelectNodes("//table//tbody//tr[@class='wrap']//td//article"))
                    {
                        Vacancy newVacancy = new Vacancy();
                        if (node.SelectSingleNode("//h3") != null)
                        {
                            string title = node.SelectSingleNode("div[1]//h3").InnerText.Trim();
                            newVacancy.Title = title;
                            string link = node.SelectSingleNode("div[1]//h3//a").Attributes["href"].Value;
                            if ((getTime(link) < date) && countFive >= 5)
                                return tempList;
                            else if (getTime(link) < date)
                            {
                                countFive++;
                                continue;
                            }

                            newVacancy.VacancyHref = link;
                            getInnerInformation(link, ref newVacancy);
                        }
                        if (node.SelectSingleNode("//ul") != null)
                        {
                            string city = node.SelectSingleNode("div[1]//ul//li[1]").InnerText;
                            if (city.Contains(","))
                            {
                                city = city.Remove(city.IndexOf(','));
                                newVacancy.Location = city;
                            }
                            else
                            {
                                newVacancy.Location = city;
                            }
                        }

                        if (node.SelectSingleNode("//div[2]//div[1]") != null)
                        {
                            string salary = node.SelectSingleNode("//div[@class='list-item__col list-item__col--price']//div[@class='list-item__price']").InnerText.Trim();
                            newVacancy.Salary = salary;
                        }

                        tempList.Add(newVacancy);
                        countFive++;
                    }
                    page++;
                    url = "https://www.olx.ua/rabota/" + categoryCollection[keyCategory] + "/?page=" + page;
                    Webget = new HtmlWeb();
                    doc = Webget.Load(url);

                }
            }
            return tempList;
        }

        public Dictionary<string, string> categoryCollection = new Dictionary<string, string>() {
            { "HR, управление персоналом", "upravleniye-personalom-hr" },
            { "IT, WEB специалисты", "it-telekom-kompyutery" },
            { "Банковское дело, ломбарды", "banki-finansy-strakhovaniye" },
            { "Домашний сервис", "domashniy-personal" },
            { "Красота и SPA-услуги", "krasota-fitnes-sport" },
            { "Логистика, доставка, склад", "transport-logistika" },
            { "Медицина, фармацевтика", "meditsina-farmatsiya" },
            { "Наука, образование, переводы", "obrazovanie" },
            { "Недвижимость и страхование", "nedvizhimost" },
            { "Офисный персонал", "cekretariat-aho" },
            { "Охрана, безопасность", "ohrana-bezopasnost" },
            { "Производство", "proizvodstvo-energetika" },
            { "Реклама, маркетинг, PR", "marketing-reklama-dizayn" },
            { "Ресторанный бизнес, кулинария", "bary-restorany-razvlecheniya" },
            { "Сельское хозяйство, агробизнес", "selskoye-khozyaystvo-agrobiznes-lesnoye-khozyaystvo" },
            { "Строительство, архитектура", "stroitelstvo" },
            { "Торговля, продажи, закупки", "roznichnaya-torgovlya-prodazhi-zakupki" },
            { "Туризм и спорт", "turizm-otdyh-razvlecheniya" },
            { "Юриспруденция, право", "yurisprudentsiya-i-buhgalteriya" },
            { "Работа для студентов", "nachalo-karery-studenty" },
            { "Работа за рубежом", "rabota-za-rubezhom" },
            { "Другие предложения", "drugie-sfery-zanyatiy" } };

        public override string SiteName
        {
            get { return "OLX.UA"; }
        }
    }
}
