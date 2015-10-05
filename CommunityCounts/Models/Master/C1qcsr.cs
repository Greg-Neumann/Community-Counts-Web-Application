namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1qcsr")]
    public partial class C1qcsr
    {
        [Key]
        public int idQuickContactsSR { get; set; }

        public int idCategory { get; set; }

        public DateTime CreateDateTime { get; set; }

        public int idRegYear { get; set; }

        [StringLength(256)]
        public string comment { get; set; }

        public virtual C1qcsrtype C1qcsrtype { get; set; }

        public virtual regyear regyear { get; set; }
    }
}
