namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1bookings")]
    public partial class C1bookings
    {
        [Key]
        public int idBookings { get; set; }

        public int idSchedules { get; set; }

        public int idResource { get; set; }

        public int idServiceType { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        public TimeSpan StartTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public TimeSpan EndTime { get; set; }

        public DateTime UpdateDateTime { get; set; }

        public virtual C1schedules C1schedules { get; set; }

        public virtual C1resources C1resources { get; set; }

        public virtual C1servicetypes C1servicetypes { get; set; }
    }
}
