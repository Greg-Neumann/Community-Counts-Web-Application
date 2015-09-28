namespace CommunityCounts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.calendardata")]
    public partial class calendardata
    {
        [Key]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "char")]
        [StringLength(2)]
        public string Qtr { get; set; }

        [Column(TypeName = "char")]
        [StringLength(2)]
        public string Half { get; set; }

        [Column(TypeName = "char")]
        [StringLength(4)]
        public string Year { get; set; }

        [Column(TypeName = "char")]
        [StringLength(7)]
        public string YearMonth { get; set; }
    }
}
