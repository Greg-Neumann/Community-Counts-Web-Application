namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
        }

        [Key]
        public int idServiceType { get; set; }

        [Required]
        [StringLength(50)]
        public string ServiceType { get; set; }

        public int AttendanceType { get; set; }

        [Required]
        [StringLength(15)]
        public string FunderCode { get; set; }

        public bool EmploymentTracked { get; set; }

        public bool BiometricTracked { get; set; }

        public virtual ICollection<C1attendance> C1attendance { get; set; }

        public virtual ICollection<C1bookings> C1bookings { get; set; }

        public virtual C1funders C1funders { get; set; }

        public virtual ICollection<C1schedules> C1schedules { get; set; }

        public virtual ICollection<C1service> C1service { get; set; }

        public virtual ICollection<C1surveys> C1surveys { get; set; }

        public virtual refdata refdata { get; set; }
    }
}
