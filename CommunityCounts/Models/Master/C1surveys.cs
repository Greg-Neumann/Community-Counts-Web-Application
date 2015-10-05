namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1surveys")]
    public partial class C1surveys
    {
        public C1surveys()
        {
            C1surressca = new HashSet<C1surressca>();
            C1surrestxt = new HashSet<C1surrestxt>();
            C1surveysq = new HashSet<C1surveysq>();
        }

        [Key]
        public int idSurvey { get; set; }

        public int idServiceype { get; set; }

        [Required]
        [StringLength(30)]
        public string SurveyName { get; set; }

        [StringLength(60)]
        public string SurveyDesc { get; set; }

        public bool forAllClients { get; set; }

        public int numTxtQ { get; set; }

        public int numScaQ { get; set; }

        [Required]
        [StringLength(45)]
        public string createdUser { get; set; }

        [StringLength(45)]
        public string updatedUser { get; set; }

        public DateTime createdDateTime { get; set; }

        public DateTime? updatedDateTime { get; set; }

        public bool active { get; set; }

        public bool forAnonymousClients { get; set; }

        public virtual C1servicetypes C1servicetypes { get; set; }

        public virtual ICollection<C1surressca> C1surressca { get; set; }

        public virtual ICollection<C1surrestxt> C1surrestxt { get; set; }

        public virtual ICollection<C1surveysq> C1surveysq { get; set; }
    }
}
