namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1resourcetypes")]
    public partial class C1resourcetypes
    {
        public C1resourcetypes()
        {
            C1resources = new HashSet<C1resources>();
        }

        [Key]
        [StringLength(30)]
        [Display(Name="Resource Type")]
        public string ResourceType { get; set; }

        [Required]
        [StringLength(60)]
        [Display(Name="Description of resource type")]
        public string ResourceTypeDesc { get; set; }

        public virtual ICollection<C1resources> C1resources { get; set; }
    }
}
