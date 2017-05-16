using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassLibrary
{
   public class RiaParser : Parser
    {

        private int siteId;

        private Dictionary<string, string> categories;

        public override string SiteName
        {
            get
            {
                return "Ria.com";
            }
        }

        public override int Id
        {
            get
            {
                return this.siteId;
            }
        }

        public RiaParser(int idParseSite)
        {
            siteId = idParseSite;

            categories = new Dictionary<string, string>() {
                { "HR, управление персоналом",              "human-resource-management" },
                { "IT, WEB специалисты",                    "it" },
                { "Банковское дело, ломбарды",              "banks" },
                { "Бухгалтерия, финансы, учет/аудит",       "accounting" },
                { "Гостиничный бизнес",                     "hotels" },
                { "Дизайн, творчество",                     "design" },
                { "Домашний сервис",                        "economic-activity" },
                { "Издательство, полиграфия",               "" },
                { "Консалтинг",                             "" },
                { "Красота и SPA-услуги",                   "beauty" },
                { "Легкая промышленность",                  "" },
                { "Логистика, доставка, склад",             "logistics-fea" },
                { "Медицина, фармацевтика",                 "medicine" },
                { "Наука, образование, переводы",           "education" },
                { "Недвижимость и страхование",             "real-estate" },
                { "Офисный персонал",                       "office-services" },
                { "Охрана, безопасность",                   "safety-and-security" },
                { "Производство",                           "production" },
                { "Реклама, маркетинг, PR",                 "advertising" },
                { "Ремонт техники и предметов быта",        "" },
                { "Ресторанный бизнес, кулинария",          "restaurants" },
                { "Руководство",                            "leading" },
                { "Сельское хозяйство, агробизнес",         "agriculture" },
                { "СМИ, TV, Радио",                         "media" },
                { "Строительство, архитектура",             "construction" },
                { "Сфера развлечений",                      "" },
                { "Телекоммуникации и связь",               "" },
                { "Торговля, продажи, закупки",             "sales" },
                { "Транспорт, автосервис",                  "motor-transport" },
                { "Туризм и спорт",                         "sport" },
                { "Юриспруденция, право",                   "jurisprudence" },
                { "Работа без квалификации",                "" },
                { "Работа для студентов",                   "rabota-dlya-studentov" },
                { "Работа за рубежом",                      "rabota-za-granytsey" },
                { "Людям с ограниченными возможностями",    "" },
                { "Другие предложения",                     "rest" } };
        }

        /// <summary>
        /// Get all job posts and return their links to the job information page
        /// </summary>
        /// <param name="html">Html page of Ria.ua website</param>
        /// <returns>List of references to parse vacancy information</returns>
        private List<VacancyPageInfo> GetJobLinksOnPage(HtmlDocument html)
        {
            if (null != html)
            {
                HtmlNode htmlNode = html.DocumentNode;

                var children = htmlNode
                    .SelectSingleNode("//div[@class='standart-view ']")
                    .ChildNodes;

                List<VacancyPageInfo> jobLinks = new List<VacancyPageInfo>();

                int jobIndex = 0;
                foreach (var child in children)
                {

                    if (child.Name == "div"
                        && child.Id.Contains("adv_"))
                    {
                        VacancyPageInfo vacancyInfo = new VacancyPageInfo();

                        vacancyInfo.JobId = child.Id.Substring("adv_".Length);
                        vacancyInfo.JobURL = GetJobLink(child, jobIndex);

                        jobLinks.Add(vacancyInfo);

                        jobIndex++;
                    }

                }

                return jobLinks;
            }
            else throw new Exception("Could not get any vacancies on specific page!");
        }


        /// <summary>
        /// Get current job post links to actual page
        /// </summary>
        /// <param name="node">Html child node of the job</param>
        /// <returns>Reference to job information page</returns>
        private string GetJobLink(HtmlNode node, int jobIndex)
        {
            return node
                .SelectNodes("//a[@class='ticket-title']")[jobIndex]
                .Attributes["href"]
                .Value;
        }

        /// <summary>
        /// Get vacancy info
        /// </summary>
        /// <param name="vacancyPage">Vacancy struct with job url and id</param>
        /// <returns>Vacancy struct if specific job post exists</returns>
        private Vacancy GetVacancy(VacancyPageInfo vacancyPage)
        {
            var htmlGet = new HtmlWeb();
            var htmlPage = htmlGet.Load(vacancyPage.JobURL);

            HtmlNode htmlNode = htmlPage.DocumentNode;

            if (null != htmlNode)
            {
                Vacancy vacancy = new Vacancy();
                JobParseHelper parser = new JobParseHelper(htmlNode);

                vacancy.Title = parser.GetTitle();
                vacancy.Salary = parser.GetSalary();
                vacancy.Location = parser.GetLocation();
                vacancy.Experience = parser.GetExperience();
                vacancy.Description = parser.GetDescription();
                vacancy.Education = parser.GetEducation();
                vacancy.PublicationDate = parser.GetCreationDate();
                vacancy.ParseSiteId = siteId;
                vacancy.VacancyHref = vacancyPage.JobURL;
                //vacancy.VacancyId = Convert.ToInt32(vacancyPage.JobId);
                vacancy.PhoneNumber = parser.GetPhoneNumber(vacancyPage.JobId);
                return vacancy;
            }
            else return null;
        }

        private int GetVacanciesPagesCount(HtmlNode htmlNode)
        {
            //get bottom horizontal page list
            var paginationDiv = htmlNode.SelectSingleNode("//div[@class='pagination-wrap']");

            if (null != paginationDiv)
            {
                var paginationElements = paginationDiv.ChildNodes.Where(x => x.Name == "a");
                int paginationCount = paginationElements.Count();
                int pageCount = Convert.ToInt32(paginationElements.ElementAt(paginationCount - 2).InnerText);

                return pageCount;
            }
            //if there's no pagination-wrap div element it means there's just a single page
            else
                return 1;
        }

        public override IEnumerable<Vacancy> ParseByCategory(string keyCategory)
        {
            List<Vacancy> tempList = new List<Vacancy>();
            const string RiaJobsAddress = "https://www.ria.com/uk/work/vacancies/";
            string category = null;
            try
            {
                category = categories[keyCategory];
            }
            catch
            {
                return null;
            }

            string JobsRequestAddress = RiaJobsAddress + category;

            HtmlWeb htmlGet = new HtmlWeb();
            HtmlDocument htmlPage = htmlGet.Load(JobsRequestAddress);

            int pageCount = GetVacanciesPagesCount(htmlPage.DocumentNode);

            for (int i = 1; i <= pageCount; i++)
            {
                htmlPage = htmlGet.Load(JobsRequestAddress + "/page/" + i);

                List<VacancyPageInfo> jobLinks = GetJobLinksOnPage(htmlPage);

                if (null != jobLinks)
                {
                    for (int v = 0; v < jobLinks.Count; v++)
                    {
                        var vacancy = GetVacancy(jobLinks[v]);

                        if (null != vacancy)
                            tempList.Add(vacancy);
                    }
                }
            }
            return tempList;

        }

        public override IEnumerable<Vacancy> ParseByDate(string keyCategory, DateTime date)
        {
            List<Vacancy> tempList = new List<Vacancy>();
            const string RiaJobsAddress = "https://www.ria.com/uk/work/vacancies/";
            string category = null;
            try
            {
                category = categories[keyCategory];
            }
            catch
            {
                return null;
            }
            string JobsRequestAddress = RiaJobsAddress + category;

            HtmlWeb htmlGet = new HtmlWeb();
            HtmlDocument htmlPage = htmlGet.Load(JobsRequestAddress);

            int pageCount = GetVacanciesPagesCount(htmlPage.DocumentNode);

            for (int i = 1; i <= pageCount; i++)
            {
                htmlPage = htmlGet.Load(JobsRequestAddress + "/page/" + i);

                List<VacancyPageInfo> jobLinks = GetJobLinksOnPage(htmlPage);

                if (null != jobLinks)
                {
                    for (int v = 0; v < jobLinks.Count; v++)
                    {
                        var vacancy = GetVacancy(jobLinks[v]);

                        if (null != vacancy && vacancy.PublicationDate == date)
                            tempList.Add(vacancy);
                    }
                }
            }
            return tempList;
        }
    }

    struct VacancyPageInfo
    {
        public string JobURL;
        public string JobId;
    }

    class JobParseHelper
    {
        HtmlNode node;

        #region Protected methods

        protected string GetTechnicalCharacteristic(string characteristicType)
        {
            var characteristicDiv = node
                .SelectSingleNode("//div[@class='technical-characteristics']/dl[@class='unstyle']")
                .ChildNodes;

            foreach (var child in characteristicDiv)
            {
                if (child.Name == "dd")
                {
                    var labelNode = child.SelectSingleNode("span[@class='label']");

                    if (null != labelNode && labelNode.InnerText == characteristicType)
                        return child.SelectSingleNode("span[@class='indent']").InnerText;
                }
            }

            return string.Empty;
        }

        #endregion

        #region Public methods

        public JobParseHelper(HtmlNode node)
        {
            this.node = node;
        }

        public string GetTitle()
        {
            return node
                .SelectSingleNode("//div[@class='heading ']")
                .InnerText
                .Trim();
        }

        public string GetSalary()
        {
            var salaryBlock = node.SelectSingleNode("//div[@class='price-seller']");
            var salary = salaryBlock.SelectSingleNode("//span[@class='i-block']");

            if (null != salary)
            {
                string salaryStr = salary.InnerText;
                return salaryStr.Trim();
            }
            else
                return "Ціна договірна";
        }

        public string GetExperience()
        {
            return GetTechnicalCharacteristic("Досвід роботи");
        }

        public string GetEducation()
        {
            return GetTechnicalCharacteristic("Освіта");
        }

        public DateTime GetCreationDate()
        {
            const int DateXPathDivIndex = 1;

            string dateString = node
                .SelectSingleNode("//div[@class='created']/div[" + DateXPathDivIndex + "]")
                .InnerText;

            dateString = dateString.Remove(0, "Додано:".Length).Trim();

            return DateTime.ParseExact(dateString, "dd.MM.yyyy", null);
        }

        public string GetDescription()
        {
            return node
                .SelectSingleNode("//dd[@class='additional-data']")
                .InnerText;
        }

        public string GetLocation()
        {
            return node
                .SelectSingleNode("//span[@class='black']")
                .InnerText;
        }

        public string GetPhoneNumber(string jobId)
        {

            //retrieve printable page with a phone number for specific job id
            string noJsPhoneNumberPage = string.Format("https://www.ria.com/uk/advertisement/print-version/{0}/", jobId);

            //download print version of advertisement page to get phone number without 
            //need to run js interpreter
            var htmlGet = new HtmlWeb();
            var htmlNode = htmlGet.Load(noJsPhoneNumberPage).DocumentNode;
            var phoneOwnerForm = htmlNode.SelectSingleNode("//dd[@class='owner_phone_js']");

            if (null != htmlNode && null != phoneOwnerForm)
            {
                string phoneNumber = phoneOwnerForm.InnerText;

                return Regex.Replace(phoneNumber, @"\t|\n|\r", "");
            }
            else
                return string.Empty;
        }

        #endregion
    }
}
