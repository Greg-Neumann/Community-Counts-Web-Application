namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.ward")]
    public partial class ward
    {
        public ward()
        {
            postcodes = new HashSet<postcode>();
        }

        [Key]
        [Column(Order = 0, TypeName = "char")]
        [StringLength(3)]
        public string CountyCode { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "char")]
        [StringLength(2)]
        public string DistrictCode { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "char")]
        [StringLength(2)]
        public string WardCode { get; set; }

        [Required]
        [StringLength(80)]
        public string Description { get; set; }

        public virtual district district { get; set; }

        public virtual ICollection<postcode> postcodes { get; set; }
    }
}
