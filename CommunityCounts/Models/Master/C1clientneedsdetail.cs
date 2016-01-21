namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1clientneedsdetail")]
    public partial class C1clientneedsdetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idClientNeedsDetail { get; set; }

        public int idClientNeedsCat { get; set; }

        public int idClientNeeds { get; set; }

        public bool hasThisNeed { get; set; }

        public virtual C1clientneedscat C1clientneedscat { get; set; }

        public virtual C1clientneedsheader C1clientneedsheader { get; set; }
    }
}
