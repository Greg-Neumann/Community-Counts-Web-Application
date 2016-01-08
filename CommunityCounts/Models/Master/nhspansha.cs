namespace CommunityCounts.Models.Master
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.nhspansha")]
    public partial class nhspansha
    {
        public nhspansha()
        {
            postcodes = new HashSet<postcode>();
        }

        [Key]
        public int idNHSPanSHACode { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(9)]
        public string NHSPanSHACode { get; set; }

        [Required]
        [StringLength(70)]
        public string NHSPanSHAName { get; set; }

        public int idCPDate { get; set; }

        public virtual ICollection<postcode> postcodes { get; set; }
    }
}
