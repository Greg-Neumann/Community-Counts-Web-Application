namespace CommunityCounts.Models.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.1clientcaseservicedetail")]
    public partial class C1clientcaseservicedetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idClientCaseServiceDetail { get; set; }

        public int idClientCaseDetail { get; set; }
        [Display(Name ="Time")]
        public TimeSpan CaseServiceTime { get; set; }
        [Display(Name ="Casework Started")]
        public DateTime CaseServiceDate { get; set; }

        public int? CaseServiceStaffid { get; set; }

       // [Column(TypeName = "text")]
       [Display(Name ="CaseWork Notes")]
        [StringLength(65536)]
        public string CaseServiceNotes { get; set; }
        [Display(Name ="Last Edit Date/Time")]
        public DateTime CaseServiceEditDate { get; set; }

        public virtual C1clientcaseservice C1clientcaseservice { get; set; }

        public virtual user user { get; set; }
    }
}
