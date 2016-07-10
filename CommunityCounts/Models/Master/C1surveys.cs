namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
        [Display(Name = "Unique id")]
        public int idSurvey { get; set; }
         [Display(Name = "Activity")]
        public int idServiceype { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Survey Name")]
        public string SurveyName { get; set; }

        [StringLength(60)]
        [Display(Name = "Description")]
        public string SurveyDesc { get; set; }

        [Display(Name = "All Clients?")]
        public bool forAllClients { get; set; }

        [Display(Name = "Number of Text Questions")]
        public int numTxtQ { get; set; }

        [Display(Name = "Number of Numeric Questions")]
        public int numScaQ { get; set; }

        [StringLength(45)]
        [Display(Name = "Created by")]
        public string createdUser { get; set; }

        [StringLength(45)]
        [Display(Name = "Updated by")]
        public string updatedUser { get; set; }

          [Display(Name = "Created on")]
        public DateTime createdDateTime { get; set; }

           [Display(Name = "Updated at")]
        public DateTime? updatedDateTime { get; set; }

            [Display(Name = "Active?")]
        public bool active { get; set; }

             [Display(Name = "Allow Anonomous Responses?")]
        public bool forAnonymousClients { get; set; }

        public virtual C1servicetypes C1servicetypes { get; set; }

        public virtual ICollection<C1surressca> C1surressca { get; set; }

        public virtual ICollection<C1surrestxt> C1surrestxt { get; set; }

        public virtual ICollection<C1surveysq> C1surveysq { get; set; }
    }
}
