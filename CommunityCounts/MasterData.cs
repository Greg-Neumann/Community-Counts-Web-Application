namespace CommunityCounts
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MasterData : DbContext
    {
        public MasterData()
            : base("name=MasterData")
        {
        }

        public virtual DbSet<calendardata> calendardatas { get; set; }
        public virtual DbSet<country> countries { get; set; }
        public virtual DbSet<customer> customers { get; set; }
        public virtual DbSet<district> districts { get; set; }
        public virtual DbSet<employmenttype> employmenttypes { get; set; }
        public virtual DbSet<postcode> postcodes { get; set; }
        public virtual DbSet<surveyresponsegrade> surveyresponsegrades { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<calendardata>()
                .Property(e => e.Qtr)
                .IsUnicode(false);

            modelBuilder.Entity<calendardata>()
                .Property(e => e.Half)
                .IsUnicode(false);

            modelBuilder.Entity<calendardata>()
                .Property(e => e.Year)
                .IsUnicode(false);

            modelBuilder.Entity<calendardata>()
                .Property(e => e.YearMonth)
                .IsUnicode(false);

            modelBuilder.Entity<country>()
                .Property(e => e.CountryCode)
                .IsUnicode(false);

            modelBuilder.Entity<country>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<country>()
                .HasOptional(e => e.district)
                .WithRequired(e => e.country)
                .WillCascadeOnDelete();

            modelBuilder.Entity<customer>()
                .Property(e => e.CustName)
                .IsUnicode(false);

            modelBuilder.Entity<district>()
                .Property(e => e.CountryCode)
                .IsUnicode(false);

            modelBuilder.Entity<district>()
                .Property(e => e.Districtcode)
                .IsUnicode(false);

            modelBuilder.Entity<district>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<employmenttype>()
                .Property(e => e.EmploymentTypes)
                .IsUnicode(false);

            modelBuilder.Entity<postcode>()
                .Property(e => e.PostCode1)
                .IsUnicode(false);

            modelBuilder.Entity<surveyresponsegrade>()
                .Property(e => e.ResponseGrade)
                .IsUnicode(false);

            modelBuilder.Entity<surveyresponsegrade>()
                .Property(e => e.ResponseStrength)
                .IsUnicode(false);
        }
    }
}
