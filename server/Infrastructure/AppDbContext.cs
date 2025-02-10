using Microsoft.EntityFrameworkCore;
using VitalityBuilder.Api.Infrastructure;
using VitalityBuilder.Api.Models.Entities;
using VitalityBuilder.Api.Models.Archetypes;

namespace VitalityBuilder.Api.Infrastructure;

public class VitalityBuilderContext(DbContextOptions<VitalityBuilderContext> options) : DbContext(options)
{
    public DbSet<Models.Archetypes.CharacterArchetypes> CharacterArchetypes => Set<Models.Archetypes.CharacterArchetypes>();
    public DbSet<Character> Characters => Set<Character>();
    public DbSet<CombatAttributes> CombatAttributes => Set<CombatAttributes>();
    public DbSet<UtilityAttributes> UtilityAttributes => Set<UtilityAttributes>();
    public DbSet<Expertise> Expertise => Set<Expertise>();
    public DbSet<SpecialAttack> SpecialAttacks => Set<SpecialAttack>();
    public DbSet<UniquePower> UniquePowers => Set<UniquePower>();

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Character>()
            .Property(c => c.MainPointPool)
            .HasComputedColumnSql("CASE WHEN [Tier] >= 2 THEN ([Tier] - 2) * 15 ELSE 0 END");   


            

        // Configure MovementArchetype
        modelBuilder.Entity<Models.Archetypes.MovementArchetype>(entity =>
        {
            entity.Property(e => e.SpeedBonusByTier)
                .HasConversion(new JsonValueConverter<Dictionary<int, int>>())
                .Metadata.SetValueComparer(new JsonValueComparer<Dictionary<int, int>>());
        });

        // Configure SpecialAttack
        modelBuilder.Entity<SpecialAttack>(entity =>
        {
            entity.Property(e => e.Limits)
                .HasConversion(new JsonValueConverter<List<string>>())
                .Metadata.SetValueComparer(new JsonValueComparer<List<string>>());

            entity.Property(e => e.Upgrades)
                .HasConversion(new JsonValueConverter<List<string>>())
                .Metadata.SetValueComparer(new JsonValueComparer<List<string>>());
        });

        // Configure UniqueAbilityArchetype
        modelBuilder.Entity<Models.Archetypes.UniqueAbilityArchetype>(entity =>
        {
            entity.Property(e => e.StatBonuses)
                .HasConversion(new JsonValueConverter<Dictionary<string, int>>())
                .Metadata.SetValueComparer(new JsonValueComparer<Dictionary<string, int>>());
        });

        // Configure UtilityArchetype
        modelBuilder.Entity<Models.Archetypes.UtilityArchetype>(entity =>
        {
            entity.Property(e => e.Restrictions)
                .HasConversion(new JsonValueConverter<List<string>>())
                .Metadata.SetValueComparer(new JsonValueComparer<List<string>>());
        });
    }
}