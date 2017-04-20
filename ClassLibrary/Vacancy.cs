using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
   public class Content
    {
        public string Tietle { get; set; }
        public string Teat { get; set; }
        public DateTime Date { get; set;}
        public Content()
        {
            this.Tietle = String.Empty;
            this.Teat = String.Empty;
            this.Date = new DateTime(1999, 01, 01);
        }
        public Content(string Tietle,string Teat,DateTime Date)
        {
            this.Tietle = Tietle;
            this.Teat = Teat;
            this.Date = Date;
        }
    }

   public class Vacancy
    {
       public int ID { get; set; }
        public string Сategory { get; set; }
        public string Sity { get; set; }
        public Content Contents { get; set; }
        public Vacancy()
        {
            this.ID = 0;
            this.Сategory = String.Empty;
            this.Contents = null;
            this.Sity = String.Empty;
        }
        public Vacancy(int ID,string Category,string Sity,Content content)
        {
            this.ID = ID;
            this.Сategory = Category;
            this.Contents = content;
            this.Sity = Sity;
        }

    }
}
