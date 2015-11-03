namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1client")]
    public partial class C1client
    {
        public C1client()
        {
            C1attendance = new HashSet<C1attendance>();
            C1biometrics = new HashSet<C1biometrics>();
            C1surressca = new HashSet<C1surressca>();
            C1surrestxt = new HashSet<C1surrestxt>();
            C1empltck = new HashSet<C1empltck>();
            C1roles = new HashSet<C1roles>();
            C1service = new HashSet<C1service>();
        }

        [Key]
        public int idClient { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int idPostcode { get; set; }

        [Display(Name = "Registration Year")]
        public int idRegYear { get; set; }

        [StringLength(256)]
        [Display(Name = "House No.")]
        public string HouseNumber { get; set; }

        
        [StringLength(256)]
        [Display(Name = "Street")]
        public string AddressLine1 { get; set; }

        [StringLength(256)]
        [Display(Name = "2nd Line")]
        public string AddressLine2 { get; set; }

        [Display(Name = "Town / City")]
        public int idCity { get; set; }

        [Display(Name = "County")]
        public int? idCounty { get; set; }

        [StringLength(256)]
        [Display(Name = "Telephone")]
        public string Phone { get; set; }

        [StringLength(256)]
        [Display(Name = "Email Address")] // is stored encrypted so cannot use email type annotation
        public string Email { get; set; }

        [Display(Name = "Gender")]
        public int idGender { get; set; }

        [Display(Name = "Age")]
        public int idAgeRange { get; set; }

        [Display(Name = "Ethnicity")]
        public int idEthnicity { get; set; }

        [Column("Ethnicity-Other")]
        [Display(Name = "Ethnicity (Other)")]
        [StringLength(45)]
        public string Ethnicity_Other { get; set; }

        [Display(Name = "Employment")]
        public int idOccupation { get; set; }

        [Column("Occupation-Other")]
        [Display(Name = "Employment (Other)")]
        [StringLength(60)]
        public string Occupation_Other { get; set; }

        [Display(Name = "Disability")]
        public int idDisability { get; set; }

          [Display(Name = "Benefits")]
        public int idBenefits { get; set; }

         [Display(Name = "Travel Method")]
        public int idTravelMethod { get; set; }

          [Display(Name = "Hear of Services")]
        public int idHearOfServices { get; set; }

        [StringLength(45)]
        [Display(Name = "Hear (Other)")]
        public string HearOther { get; set; }

        [Display(Name = "Attainment Needs Tracking?")]
        public bool AttainmentTracked { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Memory Stick Date")]
        public DateTime? MemoryStickIssued { get; set; }

        public DateTime ChangedDateTime { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public bool ConfirmSigned { get; set; }

        [Display(Name = "Preferred Language")]
        public int idFirstLanguage { get; set; }

        [Display(Name = "Housing Status")]
        public int idHousingStatus { get; set; }

        [Display(Name = "Landlord")]
        public int? idTenantStatus { get; set; }

        public int idCust { get; set; }

        [Display(Name = "Currently Armed Services")]
        public bool ArmedServCur { get; set; }

          [Display(Name = "Previously Armed Services")]
        public bool ArmedSerPre { get; set; }

        [StringLength(45)]
        [Display(Name = "Preferred Language (Other)")]
        public string FirstLanguageOther { get; set; }

        public bool scramble { get; set; }

        public virtual ICollection<C1attendance> C1attendance { get; set; }

        public virtual ICollection<C1biometrics> C1biometrics { get; set; }

        public virtual ICollection<C1surressca> C1surressca { get; set; }

        public virtual ICollection<C1surrestxt> C1surrestxt { get; set; }

        public virtual citylist citylist { get; set; }

        public virtual countylist countylist { get; set; }

        public virtual customer customer { get; set; }

        public virtual postcode postcode { get; set; }

        public virtual refdata refdata { get; set; }

        public virtual refdata refdata1 { get; set; }

        public virtual refdata refdata2 { get; set; }

        public virtual refdata refdata3 { get; set; }

        public virtual refdata refdata4 { get; set; }

        public virtual refdata refdata5 { get; set; }

        public virtual refdata refdata6 { get; set; }

        public virtual refdata refdata7 { get; set; }

        public virtual refdata refdata8 { get; set; }

        public virtual refdata refdata9 { get; set; }

        public virtual refdata refdata10 { get; set; }

        public virtual regyear regyear { get; set; }

        public virtual ICollection<C1empltck> C1empltck { get; set; }

        public virtual ICollection<C1roles> C1roles { get; set; }

        public virtual ICollection<C1service> C1service { get; set; }
    }
}
