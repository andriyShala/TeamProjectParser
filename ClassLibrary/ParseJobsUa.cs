using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    public class ParseJobsUa : iParser
    {
        private static string pattern = @"([0-9][0-9]*)(\s\S*)";
        Regex regex = new Regex(pattern);
        private int GetNumberMouth(string Mounth)
        {
            Mounth = Mounth.Replace(" ", "");
            switch(Mounth)
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
        HtmlWeb web = new HtmlWeb();
    
        public static string[] category = { "hr_human_resources"/*Hr,управления персоналом*/, "book_keeping_bank_finance_audit"/*Бухгалтерия,финанси,учет/аудит*/,"home_service"/*Домашний сервис*/,"beauty_spa"/*Красота и СПА*/,
                                            "medicine_farmatsiya"/*Медицина,фармацевтика*/,"office_personnel"/*офісний персонал*/,"advertising_marketing_pr"/*Реклама,маркетинг,PR*/,"supervisor"/*Руководство*/,
                                            "building_architecture"/*Строитильство,архитектура*/,"trade_sales_purchase"/*Торговля,продажи,закупки*/,"jurisprudence_law"/*Юриспруденция,право*/,"work_abroad"/*Работа за рубежом*/,
                                            "it_web_specialists"/*It,Web специалисти*/,"hotel_business"/*гостиний бизнес*/,"publishing_polygraphy"/*Издательство,полиграфия*/,"textile_industry"/*легкая промишлиность*/,
                                            "education_science_translate"/*наука,образование,переводи*/,"security_guard"/*Охрана,безопасность*/,"repair_of_equipment"/*Ремонт техники*/,
                                            "agriculture_agribusiness"/*сельское хозяйство,агробизнес*/,"entertainment"/*Сфера развичений*/,"transport_autoservice"/*Транспорт,автосервис*/,
                                            "without_qualification_job"/*Робота без квалификации*/,"for_people_with_disabilities"/*инвалидам*/,"banking"/*Банковское дело,ломбарди*/,"design_creative"/*Дизайн,творчество*/,
                                            "consulting"/*Консалтинг*/,"logistic_storage"/*логистика,доставка,склад*/,"real_estate_insurance"/*недвижимость и страхование*/,"production"/*производство*/,
                                            "restaurant_cookery"/*Рестораний бизнес,кулинария*/,"media_tv_radio"/*СИМ,ТВ,Радио*/,"telecommunications_connection"/*Телекоммуникация  извязь*/,"tourism_sport"/*Туризм и спорт*/,
                                            "work_for_students"/*Робота для студентов*/,"other"/*Другие предложения*/};
       private Vacancy GetContentFromHttp(string href,Vacancy vac)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument document = web.Load(href);
                HtmlNode[] links = document.DocumentNode.SelectNodes("//div[@class='b-vacancy-full js-item_full']").ToArray();
                foreach (var item in links[0].ChildNodes)
                {
                    if (item.Attributes[0].Value == "b-vacancy-full__block b-text")
                    {

                        vac.Contents.Teat = item.InnerText;
                    }
                    else if (item.ChildNodes[0].Attributes[0].Value == "js-contacts-block")
                    {
                        vac.PhoneNumber = item.ChildNodes[0].InnerText;
                    }
                }
            }
            catch
            {
                vac.Contents.Teat = "";
                vac.PhoneNumber = "";
            }

            return vac;
        }
        private int GetnumbersOfPage(string href)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
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
        private Vacancy getVacancyfromNode(HtmlNodeCollection child)
        {
            Vacancy tempvacancy = new Vacancy();
            foreach (var item2 in child)
            {
               
                if (item2.Name != "#text")
                    switch (item2.Attributes[0].Value)
                    {
                        case "b-vacancy__top":
                            foreach (var it in item2.ChildNodes)
                            {
                                if (it.Name == "a")
                                {
                                    tempvacancy=GetContentFromHttp(it.Attributes["href"].Value,tempvacancy);
                                    tempvacancy.Name = it.InnerText;
                                }
                                else if (it.Name == "div")
                                    tempvacancy.Salary = it.InnerText;
                                else if (it.Name == "span")
                                {
                                    string input = it.InnerText;
                                    input.Replace("&nbsp;", "");
                                    MatchCollection match = regex.Matches(input);
                                        tempvacancy.Date = new DateTime(DateTime.Now.Year,GetNumberMouth(match[0].Groups[2].Value),Convert.ToInt32(match[0].Groups[1].Value));
                                }
                            }
                            break;
                        case "b-vacancy__tech":
                            foreach (var it in item2.ChildNodes)
                            {
                                if (it.Name != "#text")
                                    if (it.Attributes[0].Value == "b-vacancy__tech__item")
                                        tempvacancy.Company = it.InnerText;
                                    else
                                        tempvacancy.Sity = it.ChildNodes[2].InnerText;
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
                                    tempvacancy.WorkSchedule = item2.ChildNodes[3].InnerText;
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
        public List<Vacancy> ParseVacancy(string Vacancy)
        {
            List<Vacancy> temp = new List<Vacancy>();

            string href = "https://jobs.ua/vacancy" + Vacancy;
            string additionalPeriod = "";
            int countpages = GetnumbersOfPage(href);
            for (int i = 1; i < countpages; i++)
            {
                if (i > 1)
                    additionalPeriod = "/page-" + i;
                HtmlDocument document=null;
                HtmlNode[] links=null;
                try
                {
                    document = web.Load(href + additionalPeriod);
                    links = document.DocumentNode.SelectNodes("//ul[@class='b-vacancy__list js-items_block']").ToArray();
                }
                catch
                {
                    throw new Exception("Недоступний сайт");
                }
                foreach (var item in links[0].ChildNodes)
                {
                    temp.Add(getVacancyfromNode(item.ChildNodes));
                }
            }
                return temp;
        }
       
    }
}
