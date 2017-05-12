using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
namespace ClassLibrary
{
    public class ParserWorkUa : Parser
    {
        public static Vacancy GetVacancy { get; set; }

        public override string SiteName
        {
            get
            {
                return "Work.ua";
            }
        }

        public ParserWorkUa(int id)
        {

        }

        public override IEnumerable<Vacancy> ParseByCategory(string keyCategory)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Vacancy> ParseByDate(string keyCategory, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
