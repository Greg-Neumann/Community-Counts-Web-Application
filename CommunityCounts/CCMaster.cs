namespace CommunityCounts
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CCMaster : DbContext
    {
        public CCMaster()
            : base("name=CCMaster")
        {
        }

        public virtual DbSet<customer> customers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<customer>()
                .Property(e => e.Name)
                .IsUnicode(false);
        }
    }
}
