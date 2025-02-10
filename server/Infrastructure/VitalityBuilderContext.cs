using Microsoft.EntityFrameworkCore;
using VitalityBuilder.Api.Models.Entities;
using VitalityBuilder.Api.Models.Archetypes;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace VitalityBuilder.Api.Infrastructure;

public class VitalityBuilderContext(DbContextOptions<VitalityBuilderContext> options) : DbContext(options)
{
    // Use fully qualified names to resolve ambiguity
    public DbSet<Models.Archetypes.CharacterArchetypes> CharacterArchetypes => Set<Models.Archetypes.CharacterArchetypes>();
    public DbSet<CharacterEntity> Characters => Set<CharacterEntity>();
    public DbSet<CombatAttributes> CombatAttributes => Set<CombatAttributes>();
    public DbSet<UtilityAttributes> UtilityAttributes => Set<UtilityAttributes>();
    public DbSet<Expertise> Expertise => Set<Expertise>();
    public DbSet<SpecialAttack> SpecialAttacks => Set<SpecialAttack>();
    public DbSet<UniquePower> UniquePowers => Set<UniquePower>();

    
    /// <summary>
    /// Database set for character archetypes and their associated components
    /// </summary>
    public DbSet<CharacterArchetypes> CharacterArchetypesEntity => Set<CharacterArchetypes>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureCharacterArchetypes(modelBuilder);
        ConfigureMovementArchetype(modelBuilder);
        ConfigureAttackTypeArchetype(modelBuilder);
        ConfigureEffectTypeArchetype(modelBuilder);
        ConfigureUniqueAbilityArchetype(modelBuilder);
        ConfigureSpecialAttackArchetype(modelBuilder);
        ConfigureUtilityArchetype(modelBuilder);
    }

    private void ConfigureCharacterArchetypes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CharacterArchetypes>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            // Configure one-to-one relationship with Character
            entity.HasOne(e => e.Character)
                .WithOne()
                .HasForeignKey<CharacterArchetypes>(e => e.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure required navigation properties
            entity.Navigation(e => e.MovementArchetype).IsRequired();
            entity.Navigation(e => e.AttackTypeArchetype).IsRequired();
            entity.Navigation(e => e.EffectTypeArchetype).IsRequired();
            entity.Navigation(e => e.UniqueAbilityArchetype).IsRequired();
            entity.Navigation(e => e.SpecialAttackArchetype).IsRequired();
            entity.Navigation(e => e.UtilityArchetype).IsRequired();

            // Configure table name
            entity.ToTable("CharacterArchetypes");
        });
    }

    private void ConfigureMovementArchetype(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MovementArchetype>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.SpeedBonusByTier)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<Dictionary<int, int>>(v, JsonSerializerOptions.Default)
                         ?? new Dictionary<int, int>(),
                    new ValueComparer<Dictionary<int, int>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => new Dictionary<int, int>(c)
                    )
                );

            entity.Property(e => e.Type)
                .HasConversion<string>();

            entity.ToTable("MovementArchetypes");
        });
    }

    private void ConfigureAttackTypeArchetype(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttackTypeArchetype>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Category)
                .HasConversion<string>();

            entity.ToTable("AttackTypeArchetypes");
        });
    }

    private void ConfigureEffectTypeArchetype(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EffectTypeArchetype>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Category)
                .HasConversion<string>();

            entity.ToTable("EffectTypeArchetypes");
        });
    }

    private void ConfigureUniqueAbilityArchetype(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UniqueAbilityArchetype>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.StatBonuses)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<Dictionary<string, int>>(v, JsonSerializerOptions.Default)
                         ?? new Dictionary<string, int>(),
                    new ValueComparer<Dictionary<string, int>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => new Dictionary<string, int>(c)
                    )
                );

            entity.Property(e => e.Category)
                .HasConversion<string>();

            entity.ToTable("UniqueAbilityArchetypes");
        });
    }

    private void ConfigureSpecialAttackArchetype(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SpecialAttackArchetype>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.RequiredLimits)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default)
                         ?? new List<string>(),
                    new ValueComparer<List<string>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => new List<string>(c)
                    )
                );

            entity.Property(e => e.Category)
                .HasConversion<string>();

            entity.ToTable("SpecialAttackArchetypes");
        });
    }

    private void ConfigureUtilityArchetype(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UtilityArchetype>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Restrictions)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default)
                         ?? new List<string>(),
                    new ValueComparer<List<string>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => new List<string>(c)
                    )
                );

            entity.Property(e => e.Category)
                .HasConversion<string>();

            entity.ToTable("UtilityArchetypes");
        });
    }
}
}