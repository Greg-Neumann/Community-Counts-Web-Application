namespace CommunityCounts.Models.Master
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.1funders")]
    public partial class C1funders
    {
        public C1funders()
        {
            C1servicetypes = new HashSet<C1servicetypes>();
        }

        [Key]
        [StringLength(15)]
        [Display(Name = "Funder Short Name")]
        public string FunderCode { get; set; }

        [StringLength(45)]
        [Display(Name = "Funders Full Name")]
        public string FunderName { get; set; }

        public virtual ICollection<C1servicetypes> C1servicetypes { get; set; }
    }
}
