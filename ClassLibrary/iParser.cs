using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
  public  interface IParser
    {
        List<Vacancy> ParseByCategory(string keyCategory);
        List<Vacancy> ParseByDate(string keyCategory, DateTime date);
    }
}
