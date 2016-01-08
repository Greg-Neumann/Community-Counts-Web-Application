namespace CommunityCounts.Models.Master
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.times")]
    public partial class time
    {
        [Key]
        public TimeSpan Times { get; set; }
    }
}
