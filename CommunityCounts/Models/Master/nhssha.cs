namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.nhssha")]
    public partial class nhssha
    {
        public nhssha()
        {
            postcodes = new HashSet<postcode>();
        }

        [Key]
        public int idNHSSHACode { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(9)]
        public string NHSSHACode { get; set; }

        [Required]
        [StringLength(45)]
        public string NHSSHAName { get; set; }

        public int idCPDate { get; set; }

        public virtual ICollection<postcode> postcodes { get; set; }
    }
}
