using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
   public class Vacancy
    {
        public string Сategory { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string Salary { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Description { get; set; }
        public Vacancy()
        {
            this.Сategory = String.Empty;
            this.Description = String.Empty;
            this.Location = String.Empty;
        }
        public Vacancy(string Category,string Sity, string content)
        {
            this.Сategory = Category;
            this.Description = content;
            this.Location = Sity;
        }

    }
}
