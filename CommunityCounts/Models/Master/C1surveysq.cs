namespace CommunityCounts.Models.Master
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.1surveysq")]
    public partial class C1surveysq
    {
        [Key]
        public int idSurveysQ { get; set; }

        public int idSurvey { get; set; }

        public int surveyQSeqNo { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(1)]
        public string surveyQType { get; set; }

        [Required]
        [StringLength(45)]
        public string surveyQText { get; set; }

        public virtual C1surveys C1surveys { get; set; }
    }
}
