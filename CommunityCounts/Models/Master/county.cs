namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.county")]
    public partial class county
    {
        public county()
        {
            postcodes = new HashSet<postcode>();
        }

        [Key]
        public int idCountyListCode { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(9)]
        public string CountyCode { get; set; }

        [Required]
        [StringLength(45)]
        public string CountyName { get; set; }

        public int idCPDate { get; set; }

        public virtual ICollection<postcode> postcodes { get; set; }
    }
}
