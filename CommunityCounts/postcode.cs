namespace CommunityCounts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.postcode")]
    public partial class postcode
    {
        [Key]
        [Column("PostCode", TypeName = "char")]
        [StringLength(8)]
        public string PostCode1 { get; set; }
    }
}
