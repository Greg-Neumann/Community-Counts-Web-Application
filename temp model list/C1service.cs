namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1service")]
    public partial class C1service
    {
        public C1service()
        {
            C1journeys = new HashSet<C1journeys>();
        }
        [Key]
        public int idService { get; set; }

        public int idClient { get; set; }

        public int idServiceType { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created Date")]
        public DateTime CreateDate { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Unenrolled date")]
        public DateTime? EndedDate { get; set; }
        
        [Required]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrolled date")]
        public DateTime StartedDate { get; set; }

        public int JourneyedidCategory { get; set; }
        public Boolean JourneyedServices { get; set; }

        public virtual C1client C1client { get; set; }

        public virtual C1journeycat C1journeycat { get; set; }

        public virtual C1servicetypes C1servicetypes { get; set; }

        public virtual ICollection<C1journeys> C1journeys { get; set; }
       

    }
}
