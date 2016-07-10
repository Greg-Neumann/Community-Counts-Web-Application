namespace CommunityCounts.Models.Master
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.1biometrics")]
    public partial class C1biometrics
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idBiometrics { get; set; }

        public int idClient { get; set; }

        [Column(TypeName = "date")]
        public DateTime MeasureDate { get; set; }

        public int idBiometricType { get; set; }

        public float BiometricValue { get; set; }

        public virtual C1client C1client { get; set; }

        public virtual refdata refdata { get; set; }
    }
}
