namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1qccomtype")]
    public partial class C1qccomtype
    {
        public C1qccomtype()
        {
            C1qccom = new HashSet<C1qccom>();
        }

        [Key]
        [Display(Name = "Comment Category")]
        public int idQCComType { get; set; }

        [Required]
        [StringLength(30)]
        public string Category { get; set; }

        public virtual ICollection<C1qccom> C1qccom { get; set; }
    }
}
