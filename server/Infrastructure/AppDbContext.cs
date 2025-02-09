using Microsoft.EntityFrameworkCore;
using VitalityBuilder.Api.Infrastructure;
using VitalityBuilder.Api.Models;
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
        base.OnModelCreating(modelBuilder);

        // Configure Character relationships
        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.CombatAttributes)
                .WithOne(e => e.Character)
                .HasForeignKey<CombatAttributes>(e => e.CharacterId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.UtilityAttributes)
                .WithOne(e => e.Character)
                .HasForeignKey<UtilityAttributes>(e => e.CharacterId)
                .IsRequired()
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