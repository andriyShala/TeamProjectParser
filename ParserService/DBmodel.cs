namespace ParserService
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using ClassLibrary;

    public partial class DBmodel : DbContext
    {
        public DBmodel()
            : base("name=DBmodel")
        {
        }

        public virtual DbSet<Vacancy> Vacancies { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
