using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Content
    {
        public string Tietle { get; set; }
        public string Teat { get; set; }
        public Content()
        {
            this.Tietle = String.Empty;
            this.Teat = String.Empty;
        }
        public Content(string Tietle, string Teat)
        {
            this.Tietle = Tietle;
            this.Teat = Teat;
        }
    }

    public class Vacancy
    {
        public int ID { get; set; }
        public string Сategory { get; set; }
        public string Sity { get; set; }
        public string Name { get; set; }
        public string Salary { get; set;}
        public string Company { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string WorkSchedule { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set;}
        public Content Contents { get; set; }
        public Vacancy()
        {
            this.ID = 0;
            this.Сategory = String.Empty;
            this.Contents = new Content();
            this.Sity = String.Empty;
        }
        public Vacancy(int ID, string Category, string Sity, Content content)
        {
            this.ID = ID;
            this.Сategory = Category;
            this.Contents = content;
            this.Sity = Sity;
        }

    }
}
