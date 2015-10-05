namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1journeycat")]
    public partial class C1journeycat
    {
        public C1journeycat()
        {
            C1service = new HashSet<C1service>();
        }

        [Key]
        public int idJourneyCat { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Journey Category")]
        public string CatName { get; set; }

        [Required]
        [StringLength(45)]
        [Display(Name = "Category Description")]
        public string CatDesc { get; set; }

        public virtual ICollection<C1service> C1service { get; set; }
    }
}
