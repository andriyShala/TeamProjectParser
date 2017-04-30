using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using AngleSharp;


namespace ClassLibrary.Work.ua
{

    class ParserWorkUa : IParser
    {
        private string site;
        private string prefUrl;
        private string prefPage;

        private int counter;
        IConfiguration config;

        public ParserWorkUa() 
        {
            config = Configuration.Default.WithDefaultLoader();
        }

        public List<Vacancies> StartParseAll(string keyCategory)
        {
            throw new NotImplementedException();
        }


        public List<Vacancies> StartParseforDate(string keyCategory, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
