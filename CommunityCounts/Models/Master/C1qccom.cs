namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1qccom")]
    public partial class C1qccom
    {
        [Key]
        public int idQuickContactsComment { get; set; }

        [Required]
        [StringLength(255)]
        public string Comment { get; set; }

        public int idCategory { get; set; }

        public DateTime CreateDateTime { get; set; }

        public int idRegYear { get; set; }

        public virtual C1qccomtype C1qccomtype { get; set; }

        public virtual regyear regyear { get; set; }
    }
}
