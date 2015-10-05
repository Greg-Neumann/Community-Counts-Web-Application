namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1empldest")]
    public partial class C1empldest
    {
        public C1empldest()
        {
            C1empltck = new HashSet<C1empltck>();
        }

        [Key]
        public int idEmploymentDestinations { get; set; }

        [Required]
        [StringLength(45)]
        public string EmploymentDestDesc { get; set; }

        public virtual ICollection<C1empltck> C1empltck { get; set; }
    }
}
