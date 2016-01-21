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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idClientCaseServiceDetail { get; set; }

        public int idClientCaseDetail { get; set; }

        public TimeSpan CaseServiceTime { get; set; }

        public DateTime CaseServiceDate { get; set; }

        public int? CaseServiceStaffid { get; set; }

        [Column(TypeName = "tinytext")]
        [StringLength(255)]
        public string CaseServiceNotes { get; set; }

        public virtual C1clientcaseservice C1clientcaseservice { get; set; }

        public virtual user user { get; set; }
    }
}
