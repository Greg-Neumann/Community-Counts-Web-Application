namespace CommunityCounts.Models.Master
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.1surrestxt")]
    public partial class C1surrestxt
    {
        [Key]
        public int idSurResTxt { get; set; }

        public int idSurvey { get; set; }

        public int idClient { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(3)]
        public string TextQ { get; set; }

        [Required]
        [StringLength(255)]
        public string Response { get; set; }

        public int responseSeqNo { get; set; }

        public virtual C1client C1client { get; set; }

        public virtual C1surveys C1surveys { get; set; }
    }
}
