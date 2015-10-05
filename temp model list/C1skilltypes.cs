namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1skilltypes")]
    public partial class C1skilltypes
    {
        public C1skilltypes()
        {
            C1skills = new HashSet<C1skills>();
        }

        [Key]
        public int idSkillType { get; set; }

        public int SkillTypeSeqNum { get; set; }

        [Required]
        [StringLength(60)]
        public string Skill { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public bool Active { get; set; }

        public int SkillLevel { get; set; }

        public virtual ICollection<C1skills> C1skills { get; set; }
    }
}
