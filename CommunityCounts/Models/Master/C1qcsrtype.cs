namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1qcsrtype")]
    public partial class C1qcsrtype
    {
        public C1qcsrtype()
        {
            C1qcsr = new HashSet<C1qcsr>();
        }

        [Key]
        public int idQCSRType { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "S/R Category")]
        public string QCSRType { get; set; }

        [Display(Name = "Signpost? (else Referral)")]
        public bool Signpost { get; set; }

        public virtual ICollection<C1qcsr> C1qcsr { get; set; }
    }
}
