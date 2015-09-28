namespace CommunityCounts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.country")]
    public partial class country
    {
        [Key]
        [Column(TypeName = "char")]
        [StringLength(2)]
        public string CountryCode { get; set; }

        [StringLength(60)]
        public string Description { get; set; }

        public virtual district district { get; set; }
    }
}
