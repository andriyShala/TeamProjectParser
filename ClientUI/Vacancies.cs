using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI
{

    public class Vacancy
    {
        public string Сategory { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
        public string Salary { get; set; }
        public string Company { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string PhoneNumber { get; set; }
        public string TypeOfEmployment { get; set; }
        public string ContactPerson { get; set; }
        public string CompanyWebSite { get; set; }
        public string VacancyHref { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Description { get; set; }


        public int VacancyId { get; set; }

        public int ParseSiteId { get; set; }
        public override string ToString()
        {
            return Title;
        }
    }


}
