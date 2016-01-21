namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.users")]
    public partial class user
    {
        public user()
        {
            C1clientcaseservicedetail = new HashSet<C1clientcaseservicedetail>();
        }
        [Key]
        public int idUsers { get; set; }

        [Required]
        [StringLength(128)]
        public string Email { get; set; }


        [StringLength(20)]
        public string UserShortName { get; set; }

        public bool readNews { get; set; }
        public int idRegYear { get; set; }

        public virtual ICollection<C1clientcaseservicedetail> C1clientcaseservicedetail { get; set; }
    }
}
