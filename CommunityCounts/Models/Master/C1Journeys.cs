namespace CommunityCounts.Models.Master
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.1journeys")]
    public partial class C1journeys
    {
        [Key]
        public int idJourneys { get; set; }

        public int OrigidService { get; set; }

        public int JourneyedidService { get; set; }

        public int JourneyDepth { get; set; }

        public virtual C1service C1service { get; set; }
    }
}
