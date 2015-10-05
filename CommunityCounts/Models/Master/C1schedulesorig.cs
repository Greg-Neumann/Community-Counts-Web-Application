namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1schedulesorig")]
    public partial class C1schedulesorig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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

        public virtual C1schedules C1schedules { get; set; }
    }
}
