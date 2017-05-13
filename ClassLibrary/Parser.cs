using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public abstract class Parser
    {
        public abstract IEnumerable<Vacancy> ParseByCategory(string keyCategory);
        public abstract IEnumerable<Vacancy> ParseByDate(string keyCategory, DateTime date);
        public abstract string SiteName { get; }
    }
}
