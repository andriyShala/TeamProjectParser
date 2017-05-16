using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp;

namespace ClassLibrary
{
    public class ParserWorkUa : Parser
    {
        IConfiguration config;
        Dictionary<string, string> category;
        protected string site;
        protected string url { set; get; }
        protected string prefPage;
        private int siteId;


        public override string SiteName
        {
            get
            {
                return "Work.ua";
            }
        }


        public ParserWorkUa(int id)
        {
            this.site = "https://www.work.ua";
            this.siteId = id;
            this.prefPage = "?page=";
            this.config = Configuration.Default.WithDefaultLoader();

            category = new Dictionary<string, string>()
            {
                { "HR, управление персоналом", "/jobs-hr-recruitment/" },
                { "IT, WEB специалисты", "/jobs-it/" },
                { "Банковское дело, ломбарды", "/jobs-banking-finance/" },
                { "Бухгалтерия, финансы, учет/аудит", "/jobs-accounting/" },
                { "Гостиничный бизнес", "/jobs-hotel-restaurant-tourism/" },
                { "Дизайн, творчество", "/jobs-design-art/" },
                { "Домашний сервис", "/jobs/?sel_zan=76&advs=1" },

                { "Издательство, полиграфия", "/jobs-publishing-media/" },
                { "Консалтинг", "" },

                { "Красота и SPA-услуги", "/jobs-beauty-sports/" },
                { "Легкая промышленность", "" },

                { "Логистика, доставка, склад", "/jobs-logistic-supply-chain/" },
                { "Медицина, фармацевтика", "/jobs-healthcare/" },
                { "Наука, образование, переводы", "/jobs-education-scientific/" },
                { "Недвижимость и страхование", "/jobs-real-estate/" },
                { "Офисный персонал", "" },

                { "Охрана, безопасность", "/jobs-security/" },
                { "Производство", "/jobs-production-engineering/" },
                { "Реклама, маркетинг, PR", "/jobs-marketing-advertising-pr/" },
                { "Ремонт техники и предметов быта", "" },
                { "Ресторанный бизнес, кулинария", "" },
                { "Руководство", "/jobs-management-executive/" },
                { "Сельское хозяйство, агробизнес", "/jobs-agriculture/" },
                { "СМИ, TV, Радио", "" },
                { "Строительство, архитектура", "/jobs-construction-architecture/" },
                { "Сфера развлечений", "/jobs-customer-service/" },
                { "Телекоммуникации и связь", "/jobs-telecommunications/" },
                { "Торговля, продажи, закупки", "/jobs-retail/" },
                { "Транспорт, автосервис", "/jobs-auto-transport/" },
                { "Туризм и спорт", "" },
                { "Юриспруденция, право", "/jobs-legal/" },
                { "Работа без квалификации", "" },
                { "Работа для студентов", "/jobs/students/" },
                { "Работа за рубежом", "" },
                { "Людям с ограниченными возможностями", "" },
                { "Другие предложения", "" },
                { "Морские специальности", "" },
                { "Государственные учреждения - Местное самоуправление", "/jobs-administration/"},
                { "Некоммерческие - Общественные организации", ""},
            };

        }

        /*
         Парсить за 30 днів по заданій категорії
        */
        public override IEnumerable<Vacancy> ParseByCategory(string keyCategory)
        {
            var vacancy = Task.Run(() => ParserCategory(keyCategory));
            try
            {
                return vacancy.Result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /*
         Парсить за 1 день по заданій категорії
        */
        public override IEnumerable<Vacancy> ParseByDate(string keyCategory, DateTime date)
        {
            this.prefPage = "?days=122&page=";
            var vacancy = Task.Run(() => ParserCategory(keyCategory));
            try
            {
                return vacancy.Result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        private async Task<IEnumerable<Vacancy>> ParserCategory(string keyCategory)
        {
            this.url = this.category[keyCategory];

            if (this.url != string.Empty)
            {
                List<Vacancy> list = new List<Vacancy>();
                var Url = this.site + this.url + this.prefPage;
                for (int i = 1; ; i++)
                {
                    var document = await BrowsingContext.New(config).OpenAsync(Url + i);
                    var cells = document.QuerySelectorAll("#center > div > div.row > div.col-md-8.col-left > div.card.card-hover.card-visited.job-link > h2 > a");

                    if (cells.Length == 0)    // перевірка на завершення сторінок
                        break;

                    foreach (var item in cells)
                    {
                        Vacancy vacancy = new Vacancy();
                        var link = await BrowsingContext.New(config).OpenAsync(site + item.GetAttribute("href"));

                        vacancy.VacancyHref = site + item.GetAttribute("href");
                        try
                        {
                            vacancy.Title = link.QuerySelector("div.card > h1.cut-top.wordwrap").TextContent;
                            vacancy.Company = link.QuerySelector("dl.dl-horizontal > dd > a > b").TextContent;

                            var attributes = link.QuerySelectorAll("dl.dl-horizontal > dt");

                            foreach (var el in attributes)
                            {
                                SwitchAttributeByName(ref vacancy, el);
                            }
                        }
                        catch (Exception ex) { }

                        try
                        {
                            vacancy.Salary = link.QuerySelector("div.card > h3.wordwrap").TextContent;
                        }
                        catch (Exception ex) { }


                        var desc = link.QuerySelectorAll("div.card > div.overflow > p");

                        foreach (var el in desc)
                        {
                            vacancy.Description += el.TextContent;
                        }

                        list.Add(vacancy);
                    }

                }
                return list;

            }
            else { return null; }
        }


        private void SwitchAttributeByName(ref Vacancy vac, AngleSharp.Dom.IElement element)
        {

            switch (element.TextContent)
            {
                case "Контактное лицо:":
                    { vac.ContactPerson = element.NextElementSibling.TextContent; }
                    break;
                case "Телефон:":
                    { vac.PhoneNumber = element.NextElementSibling.TextContent; }
                    break;
                case "Город:":
                    { vac.Location = element.NextElementSibling.TextContent; }
                    break;
                case "Вид занятости:":
                    { vac.TypeOfEmployment = element.NextElementSibling.TextContent; }
                    break;
                case "Требования:":
                    { vac.Experience = element.NextElementSibling.TextContent; }
                    break;
            }
        }
    }
}
