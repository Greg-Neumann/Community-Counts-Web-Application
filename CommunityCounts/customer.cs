namespace CommunityCounts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.customer")]
    public partial class customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustID { get; set; }

        [StringLength(45)]
        public string CustName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }
    }
}
