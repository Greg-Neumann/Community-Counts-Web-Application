namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1schedules")]
    public partial class C1schedules
    {
        public C1schedules()
        {
            C1attendance = new HashSet<C1attendance>();
            C1bookings = new HashSet<C1bookings>();
        }

        [Key]
        public int idSchedules { get; set; }

        public int idRegYear { get; set; }

        public int idResource { get; set; }

        public int idServiceType { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        public TimeSpan StartTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public TimeSpan EndTime { get; set; }

        public int idScheduleType { get; set; }

        [Column(TypeName = "char")]
        [StringLength(2)]
        public string Repetition { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        [StringLength(45)]
        public string UpdatedUser { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [Required]
        [StringLength(45)]
        public string CreatedUser { get; set; }

        public bool Generated { get; set; }

        public virtual ICollection<C1attendance> C1attendance { get; set; }

        public virtual ICollection<C1bookings> C1bookings { get; set; }

        public virtual C1resources C1resources { get; set; }

        public virtual regyear regyear { get; set; }

        public virtual C1schedulesorig C1schedulesorig { get; set; }

        public virtual refdata refdata { get; set; }

        public virtual C1servicetypes C1servicetypes { get; set; }
    }
}
