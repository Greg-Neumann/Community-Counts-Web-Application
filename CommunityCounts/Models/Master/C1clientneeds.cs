namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1clientneeds")]
    public partial class C1clientneeds
    {
        [Key]
        public int idClientNeeds { get; set; }

        public int idClient { get; set; }
    }
}
