namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.refdata")]
    public partial class refdata
    {
        public refdata()
        {
            C1biometrics = new HashSet<C1biometrics>();
            C1client = new HashSet<C1client>();
            C1client1 = new HashSet<C1client>();
            C1client2 = new HashSet<C1client>();
            C1client3 = new HashSet<C1client>();
            C1client4 = new HashSet<C1client>();
            C1client5 = new HashSet<C1client>();
            C1client6 = new HashSet<C1client>();
            C1client7 = new HashSet<C1client>();
            C1client8 = new HashSet<C1client>();
            C1client9 = new HashSet<C1client>();
            C1client10 = new HashSet<C1client>();
            C1schedules = new HashSet<C1schedules>();
            C1servicetypes = new HashSet<C1servicetypes>();
            C1surressca = new HashSet<C1surressca>();
        }

        [Key]
        public int idRefData { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(4)]
        public string RefCode { get; set; }

        [Required]
        [StringLength(30)]
        public string RefCodeValue { get; set; }

        [Required]
        [StringLength(60)]
        public string RefCodeDesc { get; set; }

        public virtual ICollection<C1biometrics> C1biometrics { get; set; }

        public virtual ICollection<C1client> C1client { get; set; }

        public virtual ICollection<C1client> C1client1 { get; set; }

        public virtual ICollection<C1client> C1client2 { get; set; }

        public virtual ICollection<C1client> C1client3 { get; set; }

        public virtual ICollection<C1client> C1client4 { get; set; }

        public virtual ICollection<C1client> C1client5 { get; set; }

        public virtual ICollection<C1client> C1client6 { get; set; }

        public virtual ICollection<C1client> C1client7 { get; set; }

        public virtual ICollection<C1client> C1client8 { get; set; }

        public virtual ICollection<C1client> C1client9 { get; set; }

        public virtual ICollection<C1client> C1client10 { get; set; }

        public virtual ICollection<C1schedules> C1schedules { get; set; }

        public virtual ICollection<C1servicetypes> C1servicetypes { get; set; }

        public virtual ICollection<C1surressca> C1surressca { get; set; }

        public virtual refdatatype refdatatype { get; set; }
    }
}
