using Microsoft.EntityFrameworkCore;

namespace VitalityBuilder.Api;

public class VitalityBuilderContext(DbContextOptions<VitalityBuilderContext> options) : DbContext(options)
{
    public DbSet<Character> Characters => Set<Character>();
    public DbSet<CombatAttributes> CombatAttributes => Set<CombatAttributes>();
    public DbSet<UtilityAttributes> UtilityAttributes => Set<UtilityAttributes>();
    public DbSet<Expertise> Expertise => Set<Expertise>();
    public DbSet<SpecialAttack> SpecialAttacks => Set<SpecialAttack>();
    public DbSet<UniquePower> UniquePowers => Set<UniquePower>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.CombatAttributes)
                .WithOne(e => e.Character)
                .HasForeignKey<CombatAttributes>(e => e.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.UtilityAttributes)
                .WithOne(e => e.Character)
                .HasForeignKey<UtilityAttributes>(e => e.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Expertise)
                .WithOne(e => e.Character)
                .HasForeignKey(e => e.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.SpecialAttacks)
                .WithOne(e => e.Character)
                .HasForeignKey(e => e.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.UniquePowers)
                .WithOne(e => e.Character)
                .HasForeignKey(e => e.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SpecialAttack>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Limits).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
            );
            entity.Property(e => e.Upgrades).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
            );
        });

        base.OnModelCreating(modelBuilder);
    }
}