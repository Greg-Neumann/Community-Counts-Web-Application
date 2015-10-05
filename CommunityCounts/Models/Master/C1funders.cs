namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1funders")]
    public partial class C1funders
    {
        public C1funders()
        {
            C1servicetypes = new HashSet<C1servicetypes>();
        }

        [Key]
        [StringLength(15)]
        public string FunderCode { get; set; }

        [StringLength(45)]
        public string FunderName { get; set; }

        public virtual ICollection<C1servicetypes> C1servicetypes { get; set; }
    }
}
