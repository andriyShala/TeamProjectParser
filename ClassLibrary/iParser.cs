using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    interface IParser
    {
        List<Vacancy> StartParseAll(string keyCategory);
        List<Vacancy> StartParseforDate(string keyCategory, DateTime date);
    }
}
