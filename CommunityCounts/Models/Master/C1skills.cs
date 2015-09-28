namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1skills")]
    public partial class C1skills
    {
        public C1skills()
        { }
        [Key]
        public int idSkill { get; set; }

        public int idRole { get; set; }

        public int idSkillType { get; set; }

        public int SkillSeqNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AchievedDate { get; set; }

        public virtual C1roles C1roles { get; set; }

        public virtual C1skilltypes C1skilltypes { get; set; }
    }
}
