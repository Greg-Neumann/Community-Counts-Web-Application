namespace CommunityCounts.Models.Master
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.1qcsr")]
    public partial class C1qcsr
    {
        [Key]
        public int idQuickContactsSR { get; set; }

        public int idCategory { get; set; }

        [Column(TypeName = "datetime")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Created")]
        public DateTime CreateDateTime { get; set; }

        public int idRegYear { get; set; }

        [StringLength(256)]
        [Display(Name = "Comment")]
        public string comment { get; set; }

        public virtual C1qcsrtype C1qcsrtype { get; set; }

        public virtual regyear regyear { get; set; }
    }
}
