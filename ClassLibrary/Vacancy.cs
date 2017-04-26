namespace WindowsFormsApplication1
{
<<<<<<< HEAD
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
=======
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vacancies
    {

        public string Сategory { get; set; }

        public string Sity { get; set; }
>>>>>>> refs/remotes/origin/shala

        public string Title { get; set; }

        public string Salary { get; set; }

        public string Company { get; set; }

        public string Education { get; set; }

        public string Experience { get; set; }

        public string WorkSchedule { get; set; }

        public string ContactingInfo { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public int? IdVacancysite { get; set; }

        public int? Id_pars_site { get; set; }
    }
}
