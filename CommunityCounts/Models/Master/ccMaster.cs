namespace CommunityCounts.Models.Master
{
    using System;
    using System.Web;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ccMaster : DbContext
    {
 //       public ccMaster() : base("name=ccMaster"){}

        public ccMaster() : base() { }
        public ccMaster(string dbname) : base(GetConnectionString(dbname)) { }
        public static string GetConnectionString(string dbname)
        {

            var username = HttpContext.Current.User.Identity.Name; // get user's email logon name
            char[] splitChar;
            splitChar = "@".ToCharArray();
            var logonParts = username.Split(splitChar[0]); // Email domain will be in 2nd part (index=1)
            if (logonParts.Length == 2)
            {
                var domainName = logonParts[1];
                var connString = "";
                switch (domainName)
                {
                    case "bhlc.services":
                        connString = "name=ccbhlc";
                        return connString;
                    case "sydni.org":
                        connString = "name=ccsydn";
                        return connString;
                    case "nsfwd.co.uk":
                        connString = "name=cctrai";
                        return connString;
                    case "crownroutes.co.uk":
                        connString = "name=cccrow";
                        return connString;
                    default:
                        throw new ArgumentOutOfRangeException("logon domain name not recognised");
                }
            }
            else
            { return "no connection string"; } // This allows fall-thru when user is not signed in to get Identity framework login screen

        }

        public virtual DbSet<C1attendance> C1attendance { get; set; }
        public virtual DbSet<C1biometrics> C1biometrics { get; set; }
        public virtual DbSet<C1bookings> C1bookings { get; set; }
        public virtual DbSet<C1caldat> C1caldat { get; set; }
        public virtual DbSet<C1client> C1client { get; set; }
        public virtual DbSet<C1clientneeds> C1clientneeds { get; set; }
        public virtual DbSet<C1empldest> C1empldest { get; set; }
        public virtual DbSet<C1empltck> C1empltck { get; set; }
        public virtual DbSet<C1funders> C1funders { get; set; }
        public virtual DbSet<C1journeycat> C1journeycat { get; set; }
        public virtual DbSet<C1journeys> C1journeys { get; set; }
        public virtual DbSet<C1locations> C1locations { get; set; }
        public virtual DbSet<C1qccom> C1qccom { get; set; }
        public virtual DbSet<C1qccomtype> C1qccomtype { get; set; }
        public virtual DbSet<C1qcsr> C1qcsr { get; set; }
        public virtual DbSet<C1qcsrtype> C1qcsrtype { get; set; }
        public virtual DbSet<C1resources> C1resources { get; set; }
        public virtual DbSet<C1resourcetypes> C1resourcetypes { get; set; }
        public virtual DbSet<C1roles> C1roles { get; set; }
        public virtual DbSet<C1roletypes> C1roletypes { get; set; }
        public virtual DbSet<C1schedules> C1schedules { get; set; }
        public virtual DbSet<C1schedulesorig> C1schedulesorig { get; set; }
        public virtual DbSet<C1service> C1service { get; set; }
        public virtual DbSet<C1servicetypes> C1servicetypes { get; set; }
        public virtual DbSet<C1skills> C1skills { get; set; }
        public virtual DbSet<C1skilltypes> C1skilltypes { get; set; }
        public virtual DbSet<C1surressca> C1surressca { get; set; }
        public virtual DbSet<C1surrestxt> C1surrestxt { get; set; }
        public virtual DbSet<C1surveys> C1surveys { get; set; }
        public virtual DbSet<C1surveysq> C1surveysq { get; set; }
        public virtual DbSet<citylist> citylists { get; set; }
        public virtual DbSet<county> counties { get; set; }
        public virtual DbSet<countylist> countylists { get; set; }
        public virtual DbSet<customer> customers { get; set; }
        public virtual DbSet<district> districts { get; set; }
        public virtual DbSet<nhspansha> nhspanshas { get; set; }
        public virtual DbSet<nhssha> nhsshas { get; set; }
        public virtual DbSet<postcode> postcodes { get; set; }
        public virtual DbSet<refdata> refdatas { get; set; }
        public virtual DbSet<refdatatype> refdatatypes { get; set; }
        public virtual DbSet<regyear> regyears { get; set; }
        public virtual DbSet<time> times { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<ward> wards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<C1caldat>()
                .Property(e => e.ccCalQtr)
                .IsUnicode(false);

            modelBuilder.Entity<C1caldat>()
                .Property(e => e.ccCalHalf)
                .IsUnicode(false);

            modelBuilder.Entity<C1caldat>()
                .Property(e => e.ccCalYear)
                .IsUnicode(false);

            modelBuilder.Entity<C1caldat>()
                .Property(e => e.ccCalYearMonth)
                .IsUnicode(false);

            modelBuilder.Entity<C1client>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<C1client>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<C1client>()
                .Property(e => e.HouseNumber)
                .IsUnicode(false);

            modelBuilder.Entity<C1client>()
                .Property(e => e.AddressLine1)
                .IsUnicode(false);

            modelBuilder.Entity<C1client>()
                .Property(e => e.AddressLine2)
                .IsUnicode(false);

            modelBuilder.Entity<C1client>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<C1client>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<C1client>()
                .Property(e => e.Ethnicity_Other)
                .IsUnicode(false);

            modelBuilder.Entity<C1client>()
                .Property(e => e.Occupation_Other)
                .IsUnicode(false);

            modelBuilder.Entity<C1client>()
                .Property(e => e.HearOther)
                .IsUnicode(false);

            modelBuilder.Entity<C1client>()
                .Property(e => e.FirstLanguageOther)
                .IsUnicode(false);

            modelBuilder.Entity<C1client>()
                .HasMany(e => e.C1attendance)
                .WithRequired(e => e.C1client)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1empldest>()
                .Property(e => e.EmploymentDestDesc)
                .IsUnicode(false);

            modelBuilder.Entity<C1empldest>()
                .HasMany(e => e.C1empltck)
                .WithOptional(e => e.C1empldest)
                .HasForeignKey(e => e.idEmploymentDest);

            modelBuilder.Entity<C1empltck>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<C1funders>()
                .Property(e => e.FunderCode)
                .IsUnicode(false);

            modelBuilder.Entity<C1funders>()
                .Property(e => e.FunderName)
                .IsUnicode(false);

            modelBuilder.Entity<C1funders>()
                .HasMany(e => e.C1servicetypes)
                .WithRequired(e => e.C1funders)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1journeycat>()
                .Property(e => e.CatName)
                .IsUnicode(false);

            modelBuilder.Entity<C1journeycat>()
                .Property(e => e.CatDesc)
                .IsUnicode(false);

            modelBuilder.Entity<C1journeycat>()
                .HasMany(e => e.C1service)
                .WithRequired(e => e.C1journeycat)
                .HasForeignKey(e => e.JourneyedidCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1locations>()
                .Property(e => e.LocationCode)
                .IsUnicode(false);

            modelBuilder.Entity<C1locations>()
                .Property(e => e.LocationName)
                .IsUnicode(false);

            modelBuilder.Entity<C1locations>()
                .HasMany(e => e.C1empltck)
                .WithRequired(e => e.C1locations)
                .HasForeignKey(e => e.idEmploymentClubLoc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1locations>()
                .HasMany(e => e.C1resources)
                .WithRequired(e => e.C1locations)
                .HasForeignKey(e => e.idLocation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1qccom>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<C1qccomtype>()
                .Property(e => e.Category)
                .IsUnicode(false);

            modelBuilder.Entity<C1qccomtype>()
                .HasMany(e => e.C1qccom)
                .WithRequired(e => e.C1qccomtype)
                .HasForeignKey(e => e.idCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1qcsr>()
                .Property(e => e.comment)
                .IsUnicode(false);

            modelBuilder.Entity<C1qcsrtype>()
                .Property(e => e.QCSRType)
                .IsUnicode(false);

            modelBuilder.Entity<C1qcsrtype>()
                .HasMany(e => e.C1qcsr)
                .WithRequired(e => e.C1qcsrtype)
                .HasForeignKey(e => e.idCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1resources>()
                .Property(e => e.ResourceName)
                .IsUnicode(false);

            modelBuilder.Entity<C1resources>()
                .Property(e => e.ResourceType)
                .IsUnicode(false);

            modelBuilder.Entity<C1resources>()
                .HasMany(e => e.C1attendance)
                .WithRequired(e => e.C1resources)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1resources>()
                .HasMany(e => e.C1bookings)
                .WithRequired(e => e.C1resources)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1resourcetypes>()
                .Property(e => e.ResourceType)
                .IsUnicode(false);

            modelBuilder.Entity<C1resourcetypes>()
                .Property(e => e.ResourceTypeDesc)
                .IsUnicode(false);

            modelBuilder.Entity<C1resourcetypes>()
                .HasMany(e => e.C1resources)
                .WithRequired(e => e.C1resourcetypes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1roles>()
                .Property(e => e.Mentor)
                .IsUnicode(false);

            modelBuilder.Entity<C1roletypes>()
                .Property(e => e.Role)
                .IsUnicode(false);

            modelBuilder.Entity<C1roletypes>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<C1roletypes>()
                .HasMany(e => e.C1roles)
                .WithRequired(e => e.C1roletypes)
                .HasForeignKey(e => e.RoleTypeID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1schedules>()
                .Property(e => e.Repetition)
                .IsUnicode(false);

            modelBuilder.Entity<C1schedules>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<C1schedules>()
                .Property(e => e.UpdatedUser)
                .IsUnicode(false);

            modelBuilder.Entity<C1schedules>()
                .Property(e => e.CreatedUser)
                .IsUnicode(false);

            modelBuilder.Entity<C1schedules>()
                .HasMany(e => e.C1attendance)
                .WithRequired(e => e.C1schedules)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1schedules>()
                .HasMany(e => e.C1bookings)
                .WithRequired(e => e.C1schedules)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1schedules>()
                .HasOptional(e => e.C1schedulesorig)
                .WithRequired(e => e.C1schedules)
                .WillCascadeOnDelete();

            modelBuilder.Entity<C1schedulesorig>()
                .Property(e => e.Repetition)
                .IsUnicode(false);

            modelBuilder.Entity<C1schedulesorig>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<C1schedulesorig>()
                .Property(e => e.UpdatedUser)
                .IsUnicode(false);

            modelBuilder.Entity<C1schedulesorig>()
                .Property(e => e.CreatedUser)
                .IsUnicode(false);

            modelBuilder.Entity<C1service>()
                .HasMany(e => e.C1journeys)
                .WithRequired(e => e.C1service)
                .HasForeignKey(e => e.OrigidService);

            modelBuilder.Entity<C1servicetypes>()
                .Property(e => e.ServiceType)
                .IsUnicode(false);

            modelBuilder.Entity<C1servicetypes>()
                .Property(e => e.FunderCode)
                .IsUnicode(false);

            modelBuilder.Entity<C1servicetypes>()
                .HasMany(e => e.C1attendance)
                .WithRequired(e => e.C1servicetypes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1servicetypes>()
                .HasMany(e => e.C1bookings)
                .WithRequired(e => e.C1servicetypes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1servicetypes>()
                .HasMany(e => e.C1schedules)
                .WithRequired(e => e.C1servicetypes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1servicetypes>()
                .HasMany(e => e.C1service)
                .WithRequired(e => e.C1servicetypes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1servicetypes>()
                .HasMany(e => e.C1surveys)
                .WithRequired(e => e.C1servicetypes)
                .HasForeignKey(e => e.idServiceype)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1skilltypes>()
                .Property(e => e.Skill)
                .IsUnicode(false);

            modelBuilder.Entity<C1skilltypes>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<C1skilltypes>()
                .HasMany(e => e.C1skills)
                .WithRequired(e => e.C1skilltypes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1surrestxt>()
                .Property(e => e.TextQ)
                .IsUnicode(false);

            modelBuilder.Entity<C1surrestxt>()
                .Property(e => e.Response)
                .IsUnicode(false);

            modelBuilder.Entity<C1surveys>()
                .Property(e => e.SurveyName)
                .IsUnicode(false);

            modelBuilder.Entity<C1surveys>()
                .Property(e => e.SurveyDesc)
                .IsUnicode(false);

            modelBuilder.Entity<C1surveys>()
                .Property(e => e.createdUser)
                .IsUnicode(false);

            modelBuilder.Entity<C1surveys>()
                .Property(e => e.updatedUser)
                .IsUnicode(false);

            modelBuilder.Entity<C1surveys>()
                .HasMany(e => e.C1surressca)
                .WithRequired(e => e.C1surveys)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1surveys>()
                .HasMany(e => e.C1surrestxt)
                .WithRequired(e => e.C1surveys)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C1surveysq>()
                .Property(e => e.surveyQType)
                .IsUnicode(false);

            modelBuilder.Entity<C1surveysq>()
                .Property(e => e.surveyQText)
                .IsUnicode(false);

            modelBuilder.Entity<citylist>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<citylist>()
                .HasMany(e => e.C1client)
                .WithRequired(e => e.citylist)
                .HasForeignKey(e => e.idCity)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<citylist>()
                .HasMany(e => e.customers)
                .WithRequired(e => e.citylist)
                .HasForeignKey(e => e.idCity)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<county>()
                .Property(e => e.CountyCode)
                .IsUnicode(false);

            modelBuilder.Entity<county>()
                .Property(e => e.CountyName)
                .IsUnicode(false);

            modelBuilder.Entity<county>()
                .HasMany(e => e.postcodes)
                .WithRequired(e => e.county)
                .HasForeignKey(e => e.idCountyCode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<countylist>()
                .Property(e => e.County)
                .IsUnicode(false);

            modelBuilder.Entity<countylist>()
                .HasMany(e => e.C1client)
                .WithOptional(e => e.countylist)
                .HasForeignKey(e => e.idCounty);

            modelBuilder.Entity<countylist>()
                .HasMany(e => e.customers)
                .WithOptional(e => e.countylist)
                .HasForeignKey(e => e.idCounty);

            modelBuilder.Entity<customer>()
                .Property(e => e.CustName)
                .IsUnicode(false);

            modelBuilder.Entity<customer>()
                .Property(e => e.CustShortName)
                .IsUnicode(false);

            modelBuilder.Entity<customer>()
                .Property(e => e.Number)
                .IsUnicode(false);

            modelBuilder.Entity<customer>()
                .Property(e => e.AddressLine1)
                .IsUnicode(false);

            modelBuilder.Entity<customer>()
                .Property(e => e.AddressLine2)
                .IsUnicode(false);

            modelBuilder.Entity<customer>()
                .HasMany(e => e.C1client)
                .WithRequired(e => e.customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<district>()
                .Property(e => e.DistrictCode)
                .IsUnicode(false);

            modelBuilder.Entity<district>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<district>()
                .HasMany(e => e.postcodes)
                .WithRequired(e => e.district)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<nhspansha>()
                .Property(e => e.NHSPanSHACode)
                .IsUnicode(false);

            modelBuilder.Entity<nhspansha>()
                .Property(e => e.NHSPanSHAName)
                .IsUnicode(false);

            modelBuilder.Entity<nhspansha>()
                .HasMany(e => e.postcodes)
                .WithRequired(e => e.nhspansha)
                .HasForeignKey(e => e.idNHSRegHACode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<nhssha>()
                .Property(e => e.NHSSHACode)
                .IsUnicode(false);

            modelBuilder.Entity<nhssha>()
                .Property(e => e.NHSSHAName)
                .IsUnicode(false);

            modelBuilder.Entity<nhssha>()
                .HasMany(e => e.postcodes)
                .WithRequired(e => e.nhssha)
                .HasForeignKey(e => e.idNHSHACode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<postcode>()
                .Property(e => e.PostCode1)
                .IsUnicode(false);

            modelBuilder.Entity<postcode>()
                .HasMany(e => e.C1client)
                .WithRequired(e => e.postcode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<postcode>()
                .HasMany(e => e.customers)
                .WithRequired(e => e.postcode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .Property(e => e.RefCode)
                .IsUnicode(false);

            modelBuilder.Entity<refdata>()
                .Property(e => e.RefCodeValue)
                .IsUnicode(false);

            modelBuilder.Entity<refdata>()
                .Property(e => e.RefCodeDesc)
                .IsUnicode(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1biometrics)
                .WithRequired(e => e.refdata)
                .HasForeignKey(e => e.idBiometricType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1client)
                .WithRequired(e => e.refdata)
                .HasForeignKey(e => e.idAgeRange)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1client1)
                .WithRequired(e => e.refdata1)
                .HasForeignKey(e => e.idOccupation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1client2)
                .WithRequired(e => e.refdata2)
                .HasForeignKey(e => e.idTravelMethod)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1client3)
                .WithRequired(e => e.refdata3)
                .HasForeignKey(e => e.idGender)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1client4)
                .WithRequired(e => e.refdata4)
                .HasForeignKey(e => e.idEthnicity)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1client5)
                .WithRequired(e => e.refdata5)
                .HasForeignKey(e => e.idHearOfServices)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1client6)
                .WithRequired(e => e.refdata6)
                .HasForeignKey(e => e.idDisability)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1client7)
                .WithRequired(e => e.refdata7)
                .HasForeignKey(e => e.idBenefits)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1client8)
                .WithRequired(e => e.refdata8)
                .HasForeignKey(e => e.idFirstLanguage)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1client9)
                .WithRequired(e => e.refdata9)
                .HasForeignKey(e => e.idHousingStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1client10)
                .WithOptional(e => e.refdata10)
                .HasForeignKey(e => e.idTenantStatus);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1roletypes)
                .WithRequired(e => e.refdata)
                .HasForeignKey(e => e.idRoleApplication)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1schedules)
                .WithRequired(e => e.refdata)
                .HasForeignKey(e => e.idScheduleType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1servicetypes)
                .WithRequired(e => e.refdata)
                .HasForeignKey(e => e.AttendanceType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdata>()
                .HasMany(e => e.C1surressca)
                .WithRequired(e => e.refdata)
                .HasForeignKey(e => e.IDResponse)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<refdatatype>()
                .Property(e => e.TypeCode)
                .IsUnicode(false);

            modelBuilder.Entity<refdatatype>()
                .Property(e => e.TypeName)
                .IsUnicode(false);

            modelBuilder.Entity<refdatatype>()
                .HasMany(e => e.refdatas)
                .WithRequired(e => e.refdatatype)
                .HasForeignKey(e => e.RefCode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<regyear>()
                .Property(e => e.RegYear1)
                .IsUnicode(false);

            modelBuilder.Entity<regyear>()
                .HasMany(e => e.C1client)
                .WithRequired(e => e.regyear)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<regyear>()
                .HasMany(e => e.C1qccom)
                .WithRequired(e => e.regyear)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<regyear>()
                .HasMany(e => e.C1qcsr)
                .WithRequired(e => e.regyear)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<regyear>()
                .HasMany(e => e.C1schedules)
                .WithRequired(e => e.regyear)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.UserShortName)
                .IsUnicode(false);

            modelBuilder.Entity<ward>()
                .Property(e => e.WardCode)
                .IsUnicode(false);

            modelBuilder.Entity<ward>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<ward>()
                .HasMany(e => e.postcodes)
                .WithRequired(e => e.ward)
                .WillCascadeOnDelete(false);
        }
    }
}
