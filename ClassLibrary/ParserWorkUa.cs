using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp;


namespace ClassLibrary.Work.ua
{

    class ParserWorkUa : IParser
    {
        private string site;
        private string prefUrl;
        private string prefPage;
        private int counter;
        private bool isParsing;
        IConfiguration config;


        public ParserWorkUa() 
        {
            config = Configuration.Default.WithDefaultLoader();
            site = "https://www.work.ua";
            prefPage = "? page=";
        }

        public List<Vacancies> StartParseAll(string keyCategory)
        {
            isParsing = true;

            return new List<Vacancies>();
        }


        public List<Vacancies> StartParseforDate(string keyCategory, DateTime date)
        {
            isParsing = true;

            return new List<Vacancies>();
        }

        public void Stop()
        {
            this.isParsing = false;
        }



        protected async Task<ICollection<Vacancies>> ParserOnDate(string category, DateTime date)
        {
            List<Vacancies> list = new List<Vacancies>();
            var Url = this.site + this.prefUrl + this.prefPage;

            for (int i = 1; i < 5; i++)
            {
                if (!isParsing)
                    break;
                
                var document = await BrowsingContext.New(config).OpenAsync(Url + i);
                var cells = document.QuerySelectorAll("#center > div > div.row > div.col-md-8.col-left > div.card.card-hover.card-visited.job-link");
                if (cells.Length == 0)
                    break;

                foreach (var item in cells)
                {
                    var title = item.QuerySelector("h2 > a");
                    var id = title.GetAttribute("href").Split('/')[2];
                    var company = item.QuerySelector("div > span");
                    var salary = item.QuerySelectorAll("h2 > span.text-muted > span");
                    var city = item.QuerySelectorAll("div > span > span.text-muted");

                    //list.Add("ID: " + id);
                    //list.Add("Company: " + company.TextContent);
                    //list.Add("Title: " + title.TextContent);
                    //list.Add("City: " + city[0].PreviousSibling.TextContent);

                    //if (salary.Length > 0)
                    //    list.Add("Salary: " + salary[1].TextContent);
                    //list.Add(new string('-', 10));

                }

                counter = counter + cells.Length;

            }
            return list;
        }
    }
}
