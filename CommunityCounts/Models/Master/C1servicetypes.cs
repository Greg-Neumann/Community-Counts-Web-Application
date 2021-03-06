namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.1servicetypes")]
    public partial class C1servicetypes
    {
        public C1servicetypes()
        {
            C1attendance = new HashSet<C1attendance>();
            C1bookings = new HashSet<C1bookings>();
            C1schedules = new HashSet<C1schedules>();
            C1service = new HashSet<C1service>();
            C1surveys = new HashSet<C1surveys>();
            C1clientcaseservice = new HashSet<C1clientcaseservice>();
        }

        [Key]
        public int idServiceType { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Activity Name")]
        public string ServiceType { get; set; }

        [Display(Name = "Attendance recording type")]
        public int AttendanceType { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Funder")]
        public string FunderCode { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ended Date")]
        public DateTime? EndedDate { get; set; }

        public virtual ICollection<C1attendance> C1attendance { get; set; }

        public virtual ICollection<C1bookings> C1bookings { get; set; }

        public virtual C1funders C1funders { get; set; }

        public virtual ICollection<C1schedules> C1schedules { get; set; }

        public virtual ICollection<C1service> C1service { get; set; }

        public virtual ICollection<C1surveys> C1surveys { get; set; }

        public virtual refdata refdata { get; set; }
        public virtual ICollection<C1clientcaseservice> C1clientcaseservice { get; set; }
    }
}
