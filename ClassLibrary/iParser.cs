using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
  public  interface IParser
    {
        IEnumerable<Vacancy> ParseByCategory(string keyCategory);
        IEnumerable<Vacancy> ParseByDate(string keyCategory, DateTime date);
        string SiteName { get;}
    }
}
