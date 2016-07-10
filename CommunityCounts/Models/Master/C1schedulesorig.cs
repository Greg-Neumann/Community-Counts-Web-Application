namespace CommunityCounts.Models.Master
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
        [DisplayFormat(DataFormatString = "{0:ddd dd MMM yy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:ddd dd MMM yy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; }

        public int idScheduleType { get; set; }

        [Column(TypeName = "char")]
        [StringLength(2)]
        [Display(Name = "Rep?")]
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
