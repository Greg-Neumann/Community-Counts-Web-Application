namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1roletypes")]
    public partial class C1roletypes
    {
        public C1roletypes()
        {
            C1roles = new HashSet<C1roles>();
        }

        [Key]
        public int idRoleType { get; set; }

        public int idRoleApplication { get; set; }

        public int RoleTypeSeqNum { get; set; }

        [Required]
        [StringLength(60)]
        public string Role { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<C1roles> C1roles { get; set; }

        public virtual refdata refdata { get; set; }
    }
}
