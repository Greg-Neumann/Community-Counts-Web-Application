namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1clientneedsheader")]
    public partial class C1clientneedsheader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public C1clientneedsheader()
        {
            C1clientneedsdetail = new HashSet<C1clientneedsdetail>();
            C1clientneedsdocs = new HashSet<C1clientneedsdocs>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idClientNeeds { get; set; }

        public int idClient { get; set; }

        [Display (Name ="Date that these Needs apply")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ClientNeedsDate { get; set; }

        //[Column(TypeName = "text")]
        [Display(Name = "Notes on the Client Needs")]
        [StringLength(65536)]
        public string ClientNeedsNotes { get; set; }

        public virtual C1client C1client { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C1clientneedsdetail> C1clientneedsdetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C1clientneedsdocs> C1clientneedsdocs { get; set; }
    }
}
