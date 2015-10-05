namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1qcsr")]
    public partial class C1qcsr
    {
        public C1qcsr()
        { }
        [Key]
        public int idQuickContactsSR { get; set; }

        public int idCategory { get; set; }
       
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Created")]
        public DateTime CreateDateTime { get; set; }

        public int idRegYear { get; set; }
        [Display(Name="Comment")]
        public string comment { get; set; }

        public virtual C1qcsrtype C1qcsrtype { get; set; }

        public virtual regyear regyear { get; set; }
    }
}
