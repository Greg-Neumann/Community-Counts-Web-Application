namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.regyear")]
    public partial class regyear
    {
        public regyear()
        {
            C1client = new HashSet<C1client>();
            C1qccom = new HashSet<C1qccom>();
            C1qcsr = new HashSet<C1qcsr>();
            C1schedules = new HashSet<C1schedules>();
        }

        [Key]
        public int idRegYear { get; set; }

        [Column("RegYear", TypeName = "char")]
        [Required]
        [StringLength(4)]
        public string RegYear1 { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public virtual ICollection<C1client> C1client { get; set; }

        public virtual ICollection<C1qccom> C1qccom { get; set; }

        public virtual ICollection<C1qcsr> C1qcsr { get; set; }

        public virtual ICollection<C1schedules> C1schedules { get; set; }
    }
}
