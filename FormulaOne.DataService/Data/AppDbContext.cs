using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;

namespace FormulaOne.DataService.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Represents driver entity
    /// </summary>
    public virtual DbSet<Driver> Drivers { get; set; }
    /// <summary>
    /// Represents achievement entity
    /// </summary>
    public virtual DbSet<Achievement> Achievements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Define relationships between entity
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasOne(a => a.Driver)
                .WithMany(d => d.Achievements)
                .HasForeignKey(a => a.DriverId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Achievements_Driver");
        });
    }
}
