using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
  public  interface IParser
    {
        List<Vacancies> StartParseAll(string keyCategory);
        List<Vacancies> StartParseforDate(string keyCategory, DateTime date);
    }
}
