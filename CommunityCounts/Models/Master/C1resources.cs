namespace CommunityCounts.Models.Master
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.1resources")]
    public partial class C1resources
    {
        public C1resources()
        {
            C1attendance = new HashSet<C1attendance>();
            C1bookings = new HashSet<C1bookings>();
            C1schedules = new HashSet<C1schedules>();
        }

        [Key]
        public int idResource { get; set; }

        [Display(Name = "Location")]
        public int idLocation { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Resource Name")]
        public string ResourceName { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Resource Type")]
        public string ResourceType { get; set; }

        public virtual ICollection<C1attendance> C1attendance { get; set; }

        public virtual ICollection<C1bookings> C1bookings { get; set; }

        public virtual C1locations C1locations { get; set; }

        public virtual C1resourcetypes C1resourcetypes { get; set; }

        public virtual ICollection<C1schedules> C1schedules { get; set; }
    }
}
