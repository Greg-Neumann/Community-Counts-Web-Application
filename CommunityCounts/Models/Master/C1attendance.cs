namespace CommunityCounts.Models.Master
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.1attendance")]
    public partial class C1attendance
    {
        [Key]
        public int idAttendance { get; set; }

        public int idResource { get; set; }

        public int idServiceType { get; set; }

        public int idSchedules { get; set; }

        public int idClient { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddd}", ApplyFormatInEditMode = true)]
        public DateTime SessionDate { get; set; }

        public TimeSpan SessionTime { get; set; }

        public int AttendedCount { get; set; }

        public TimeSpan AttendedTime { get; set; }

        public TimeSpan SignInTime { get; set; }

        public TimeSpan SignOutTime { get; set; }

        public virtual C1client C1client { get; set; }

        public virtual C1resources C1resources { get; set; }

        public virtual C1schedules C1schedules { get; set; }

        public virtual C1servicetypes C1servicetypes { get; set; }
    }
}
