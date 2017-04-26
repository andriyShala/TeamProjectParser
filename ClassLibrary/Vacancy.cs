namespace WindowsFormsApplication1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vacancies
    {

        public string Сategory { get; set; }

        public string Sity { get; set; }

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
