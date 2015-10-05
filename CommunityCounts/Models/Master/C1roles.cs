namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1roles")]
    public partial class C1roles
    {
        public C1roles()
        {
            C1skills = new HashSet<C1skills>();
        }

        [Key]
        public int idRole { get; set; }

        public int idClient { get; set; }

        public int RoleTypeID { get; set; }

        public int RoleSeqNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AchievedDate { get; set; }

        [StringLength(20)]
        public string Mentor { get; set; }

        public virtual C1client C1client { get; set; }

        public virtual C1roletypes C1roletypes { get; set; }

        public virtual ICollection<C1skills> C1skills { get; set; }
    }
}
