namespace CommunityCounts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.employmenttypes")]
    public partial class employmenttype
    {
        [Key]
        public int EmploymentTypesID { get; set; }

        [StringLength(60)]
        public string EmploymentTypes { get; set; }
    }
}
