namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.postcode")]
    public partial class postcode
    {
        public postcode()
        {
            C1client = new HashSet<C1client>();
            customers = new HashSet<customer>();
        }

        [Key]
        public int idPostCode { get; set; }

        [Column("PostCode", TypeName = "char")]
        [Required]
        [StringLength(8)]
        public string PostCode1 { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(3)]
        public string CountyCode { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(2)]
        public string DistrictCode { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(2)]
        public string WardCode { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(4)]
        public string NHSHACode { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(4)]
        public string NHSRegHACode { get; set; }

        public virtual ICollection<C1client> C1client { get; set; }

        public virtual ICollection<customer> customers { get; set; }

        public virtual ward ward { get; set; }
    }
}
