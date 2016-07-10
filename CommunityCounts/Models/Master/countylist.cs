namespace CommunityCounts.Models.Master
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.countylist")]
    public partial class countylist
    {
        public countylist()
        {
            C1client = new HashSet<C1client>();
            customers = new HashSet<customer>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idCountyList { get; set; }

        [Required]
        [StringLength(45)]
        public string County { get; set; }

        public virtual ICollection<C1client> C1client { get; set; }

        public virtual ICollection<customer> customers { get; set; }
    }
}
