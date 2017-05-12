using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
namespace ClassLibrary
{
    public class ParserWorkUa : IParser
    {
        public static Vacancy GetVacancy { get; set; }
        public const string SiteName = "Work.ua";
        public ParserWorkUa(int id)
        {

        }

        string IParser.SiteName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<Vacancy> ParseByCategory(string keyCategory)
        {
            throw new NotImplementedException();
        }

        public List<Vacancy> ParseByDate(string keyCategory, DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Vacancy> StartParseAll(string keyCategory)
        {
            throw new NotImplementedException();
        }

        public List<Vacancy> StartParseforDate(string keyCategory, DateTime date)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Vacancy> IParser.ParseByCategory(string keyCategory)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Vacancy> IParser.ParseByDate(string keyCategory, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
