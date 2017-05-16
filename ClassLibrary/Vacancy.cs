namespace ClassLibrary
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Vacancies")]
    public class Vacancy
    {
        public string Сategory { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
        public string Salary { get; set; }
        public string Company { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string PhoneNumber { get; set; }
        public string TypeOfEmployment { get; set; }
        public string ContactPerson { get; set; }
        public string CompanyWebSite { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Description { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string VacancyHref { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ParseSiteId { get; set; }
    }
}
