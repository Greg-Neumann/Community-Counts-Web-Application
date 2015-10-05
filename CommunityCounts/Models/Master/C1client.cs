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
        public string FirstName { get; set; }

        [Required]
        [StringLength(256)]
        public string LastName { get; set; }

        public int idPostcode { get; set; }

        public int idRegYear { get; set; }

        [StringLength(256)]
        public string HouseNumber { get; set; }

        [Required]
        [StringLength(256)]
        public string AddressLine1 { get; set; }

        [StringLength(256)]
        public string AddressLine2 { get; set; }

        public int idCity { get; set; }

        public int? idCounty { get; set; }

        [StringLength(256)]
        public string Phone { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        public int idGender { get; set; }

        public int idAgeRange { get; set; }

        public int idEthnicity { get; set; }

        [Column("Ethnicity-Other")]
        [StringLength(45)]
        public string Ethnicity_Other { get; set; }

        public int idOccupation { get; set; }

        [Column("Occupation-Other")]
        [StringLength(60)]
        public string Occupation_Other { get; set; }

        public int idDisability { get; set; }

        public int idBenefits { get; set; }

        public int idTravelMethod { get; set; }

        public int idHearOfServices { get; set; }

        [StringLength(45)]
        public string HearOther { get; set; }

        public bool AttainmentTracked { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MemoryStickIssued { get; set; }

        public DateTime ChangedDateTime { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public bool ConfirmSigned { get; set; }

        public int idFirstLanguage { get; set; }

        public int idHousingStatus { get; set; }

        public int? idTenantStatus { get; set; }

        public int idCust { get; set; }

        public bool ArmedServCur { get; set; }

        public bool ArmedSerPre { get; set; }

        [StringLength(45)]
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
