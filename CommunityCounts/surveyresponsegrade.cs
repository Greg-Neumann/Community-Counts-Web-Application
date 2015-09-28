namespace CommunityCounts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ccmaster.surveyresponsegrades")]
    public partial class surveyresponsegrade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idSurveyResponseGrades { get; set; }

        [Column(TypeName = "char")]
        [StringLength(1)]
        public string ResponseGrade { get; set; }

        [StringLength(45)]
        public string ResponseStrength { get; set; }
    }
}
