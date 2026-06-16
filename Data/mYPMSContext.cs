#nullable disable
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mYPMS.Models;

namespace mYPMS.Data
{
    public partial class mYPMSContext : IdentityDbContext
    {
        public mYPMSContext()
        {
        }

        public mYPMSContext(DbContextOptions<mYPMSContext> options)
            : base(options)
        {
        }
        public virtual DbSet<tGate> tGates { get; set; }
        public virtual DbSet<tParking> tParkings { get; set; }
        public virtual DbSet<tPaymentMethod> tPaymentMethods { get; set; }
        public virtual DbSet<tSetting> tSettings { get; set; }
        public virtual DbSet<tTariff> tTariffs { get; set; }
        public virtual DbSet<tTraffic> tTraffics { get; set; }
        public virtual DbSet<tTagGroup> tTagGroup { get; set; }
        public virtual DbSet<tTagList> tTagList { get; set; }
        public virtual DbSet<vParked> vParkeds { get; set; }

/*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tGate>(entity =>
            {
                entity.Property(e => e.xId).ValueGeneratedNever();
                entity.Property(e => e.xDirection).HasDefaultValueSql("((0))");
                entity.HasMany(b => b.xEntryGates).WithOne();
                entity.HasMany(b => b.xDepartureGates).WithOne();
                entity.HasOne(b => b.xParking).WithMany(b => b.xGates).HasForeignKey(b => b.xParkingId);
            });

            modelBuilder.Entity<tParking>(entity =>
            {
                entity.Property(e => e.xId).ValueGeneratedNever();
                entity.Property(e => e.xCapacity).HasDefaultValueSql("((0))");
                entity.HasMany(b => b.xGates).WithOne();
                entity.HasMany(b => b.xTraffics).WithOne();
                entity.HasOne(b => b.xTariff).WithMany(b => b.xParkings).HasForeignKey(b => b.xTariffxId);
            });

            modelBuilder.Entity<tPaymentMethod>(entity =>
            {
                entity.Property(e => e.xId).ValueGeneratedNever();
                entity.HasMany(b => b.xTraffics).WithOne();
            });

            modelBuilder.Entity<tSetting>(entity =>
            {
                entity.Property(e => e.xId).ValueGeneratedNever();
            });

            modelBuilder.Entity<tTariff>(entity =>
            {
                entity.Property(e => e.xId).ValueGeneratedNever();
                entity.HasMany(b => b.xParkings).WithOne();
                //entity.Navigation(b => b.xParkings).UsePropertyAccessMode(PropertyAccessMode.Property);
                entity.HasMany(b => b.xTraffics).WithOne();
                entity.HasMany(b => b.xTagGroup).WithOne();
            });

            modelBuilder.Entity<tTraffic>(entity =>
            {
                entity.Property(e => e.xId).ValueGeneratedNever();
                entity.HasOne(b => b.xParking).WithMany(b => b.xTraffics).HasForeignKey(b => b.xParkingId);
                entity.HasOne(b => b.xEntryGate).WithMany(b => b.xEntryGates).HasForeignKey(b => b.xEntryGateId);
                entity.HasOne(b => b.xDepartureGate).WithMany(b => b.xDepartureGates).HasForeignKey(b => b.xDepartureGateId);
                entity.HasOne(b => b.xPaidMethod).WithMany(b => b.xTraffics).HasForeignKey(b => b.xPaidMethodId);
                entity.HasOne(b => b.xEntryOperator).WithMany(b => b.xEntryOperators).HasForeignKey(b => b.xEntryOperatorId);
                entity.HasOne(b => b.xDepartureOperator).WithMany(b => b.xDepartureOperators).HasForeignKey(b => b.xDepartureOperatorId);
                entity.HasOne(b => b.xTariff).WithMany(b => b.xTraffics).HasForeignKey(b => b.xTariffId);
            });

            modelBuilder.Entity<tUser>(entity =>
            {
                entity.Property(e => e.xId).ValueGeneratedNever();
                entity.HasMany(b => b.xEntryOperators).WithOne();
                entity.HasMany(b => b.xDepartureOperators).WithOne();
            });

            modelBuilder.Entity<tTagGroup>(entity =>
            {
                entity.Property(e => e.xId).ValueGeneratedNever();
                entity.HasOne(b => b.xTariff).WithMany(b => b.xTagGroup).HasForeignKey(b => b.xTariffId);
                entity.HasMany(b => b.xTagList).WithOne();
            });

            modelBuilder.Entity<tTagList>(entity =>
            {
                entity.Property(e => e.xId).ValueGeneratedNever();
                entity.HasOne(b => b.xTagGroup).WithMany(b => b.xTagList).HasForeignKey(b => b.xTagGroupId);
            });

            modelBuilder.Entity<vParked>(entity =>
            {
                entity.ToView("vParked");
            });

            OnModelCreatingGeneratedProcedures(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }
*/
        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}