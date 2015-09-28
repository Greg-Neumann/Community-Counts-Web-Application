namespace CommunityCounts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.district")]
    public partial class district
    {
        [Key]
        [Column(TypeName = "char")]
        [StringLength(2)]
        public string CountryCode { get; set; }

        [Column(TypeName = "char")]
        [StringLength(2)]
        public string Districtcode { get; set; }

        [StringLength(60)]
        public string Description { get; set; }

        public virtual country country { get; set; }
    }
}
