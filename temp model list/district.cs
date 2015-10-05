namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.district")]
    public partial class district
    {
        public district()
        {
            wards = new HashSet<ward>();
        }

        [Key]
        [Column(Order = 0, TypeName = "char")]
        [StringLength(3)]
        public string CountyCode { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "char")]
        [StringLength(2)]
        public string DistrictCode { get; set; }

        [Required]
        [StringLength(80)]
        public string Description { get; set; }

        public virtual ICollection<ward> wards { get; set; }
    }
}
