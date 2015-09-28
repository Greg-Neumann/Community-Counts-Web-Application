namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1empltck")]
    public partial class C1empltck
    {
        public C1empltck()
        { }
        [Key]
        public int idEmplTck { get; set; }

        public int idClient { get; set; }
        [Display(Name="Employed?")]
        public Boolean EmployedStatus { get; set; }

        public DateTime UpdateDateTime { get; set; }

        [Display(Name = "Empl Activity Locn")]
        public int idEmploymentClubLoc { get; set; }

        [StringLength(45)]
        public string Comment { get; set; }
        [Display(Name="Empl Dest")]
        public int? idEmploymentDest { get; set; }

        public virtual C1client C1client { get; set; }

        public virtual C1empldest C1empldest { get; set; }

        public virtual C1locations C1locations { get; set; }
    }
}
