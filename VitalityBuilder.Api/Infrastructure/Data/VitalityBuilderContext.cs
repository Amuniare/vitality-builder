using Microsoft.EntityFrameworkCore;
using VitalityBuilder.Api.Domain.Attributes;
using VitalityBuilder.Api.Domain.Character;
using VitalityBuilder.Api.Infrastructure.Data.Configurations;

namespace VitalityBuilder.Api.Infrastructure.Data;

public class VitalityBuilderContext : DbContext
{
    public VitalityBuilderContext(DbContextOptions<VitalityBuilderContext> options)
        : base(options)
    {
    }

    // Core Entities
    public DbSet<Character> Characters { get; set; } = null!;
    public DbSet<CombatAttributes> CombatAttributes { get; set; } = null!;
    public DbSet<UtilityAttributes> UtilityAttributes { get; set; } = null!;
    public DbSet<CharacterArchetypes> CharacterArchetypes { get; set; } = null!;

    // Collections
    public DbSet<SpecialAttack> SpecialAttacks { get; set; } = null!;
    public DbSet<CharacterFeature> Features { get; set; } = null!;
    public DbSet<CharacterExpertise> Expertise { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply entity configurations
        modelBuilder.ApplyConfiguration(new CharacterConfiguration());
        modelBuilder.ApplyConfiguration(new CombatAttributesConfiguration());
        modelBuilder.ApplyConfiguration(new UtilityAttributesConfiguration());
        modelBuilder.ApplyConfiguration(new CharacterArchetypesConfiguration());

        // Global query filters
        modelBuilder.Entity<Character>()
            .HasQueryFilter(c => !c.IsDeleted);

        // Common properties for all entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Add audit fields if they exist
            if (entityType.FindProperty("CreatedAt") != null)
            {
                modelBuilder.Entity(entityType.Name)
                    .Property("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");
            }

            if (entityType.FindProperty("LastModifiedAt") != null)
            {
                modelBuilder.Entity(entityType.Name)
                    .Property("LastModifiedAt")
                    .HasDefaultValueSql("GETUTCDATE()");
            }

            // Add soft delete if supported
            if (entityType.FindProperty("IsDeleted") != null)
            {
                modelBuilder.Entity(entityType.Name)
                    .Property("IsDeleted")
                    .HasDefaultValue(false);
            }
        }

        // Configure database triggers
        modelBuilder.HasDbFunction(typeof(VitalityBuilderContext)
            .GetMethod(nameof(CalculateCharacterPoints))!)
            .HasName("fn_CalculateCharacterPoints");
    }

    // Database functions
    public int CalculateCharacterPoints(int characterId, string poolType)
        => throw new NotSupportedException();

    // Override SaveChanges to handle audit fields
    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        var currentTime = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Property("CreatedAt")?.CurrentValue == null)
                {
                    entry.Property("CreatedAt").CurrentValue = currentTime;
                }
            }

            if (entry.Property("LastModifiedAt")?.CurrentValue != null)
            {
                entry.Property("LastModifiedAt").CurrentValue = currentTime;
            }
        }
    }

    // Helper methods for common operations
    public async Task<bool> CharacterExistsAsync(int id)
    {
        return await Characters.AnyAsync(c => c.Id == id && !c.IsDeleted);
    }

    public IQueryable<Character> GetCharactersWithRelated()
    {
        return Characters
            .Include(c => c.CombatAttributes)
            .Include(c => c.UtilityAttributes)
            .Include(c => c.Archetypes)
            .Include(c => c.SpecialAttacks)
            .Include(c => c.Features)
            .Include(c => c.Expertise)
            .AsSplitQuery(); // Split into multiple SQL queries for better performance
    }
}