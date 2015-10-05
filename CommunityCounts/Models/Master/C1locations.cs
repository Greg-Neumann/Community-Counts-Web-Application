namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1locations")]
    public partial class C1locations
    {
        public C1locations()
        {
            C1empltck = new HashSet<C1empltck>();
            C1resources = new HashSet<C1resources>();
        }

        [Key]
        public int idLocations { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Location Short Name")]
        public string LocationCode { get; set; }

        [Required]
        [StringLength(45)]
        [Display(Name = "Location Full Name")]
        public string LocationName { get; set; }

        public virtual ICollection<C1empltck> C1empltck { get; set; }

        public virtual ICollection<C1resources> C1resources { get; set; }
    }
}
