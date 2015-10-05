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
        public C1bookings()
        {}
        [Key]
        public int idBookings { get; set; }

        public int idSchedules { get; set; }

        public int idResource { get; set; }

        public int idServiceType { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:ddd dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name="Start")]
        [DisplayFormat(DataFormatString = "{0:hh':'mm}", ApplyFormatInEditMode = true)]
        public TimeSpan StartTime { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:ddd dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name="End")]
        [DisplayFormat(DataFormatString = "{0:hh':'mm}", ApplyFormatInEditMode = true)]
        public TimeSpan EndTime { get; set; }

        public DateTime UpdateDateTime { get; set; }

        public virtual C1resources C1resources { get; set; }

        public virtual C1schedules C1schedules { get; set; }

        public virtual C1servicetypes C1servicetypes { get; set; }
    }
}
