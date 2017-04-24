using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    interface IParser
    {
        List<Vacancy> ParseVacancy(string lincVacancy);
        List<Vacancy> ParsePageVacancy(string lincVacancy, int page);
    }
}
