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
       public ParserWorkUa(int id)
        {

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
    }
}
