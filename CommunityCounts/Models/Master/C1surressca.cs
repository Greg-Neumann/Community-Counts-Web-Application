namespace CommunityCounts.Models.Master
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.1surressca")]
    public partial class C1surressca
    {
        [Key]
        public int idSurResSca { get; set; }

        public int idSurvey { get; set; }

        public int idClient { get; set; }

        public int ScaledQ { get; set; }

        public int IDResponse { get; set; }

        public int responseSeqNo { get; set; }

        public virtual C1client C1client { get; set; }

        public virtual refdata refdata { get; set; }

        public virtual C1surveys C1surveys { get; set; }
    }
}
