namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1caldat")]
    public partial class C1caldat
    {
        public C1caldat()
        {
        }

        [Key]
        [Column(TypeName = "date")]
        public DateTime ccCalDate { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(7)]
        public string ccCalQtr { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(7)]
        public string ccCalHalf { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(4)]
        public string ccCalYear { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(10)]
        public string ccCalYearMonth { get; set; }

    }
}
