using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VitalityBuilder.Api.Domain.Character;
using VitalityBuilder.Api.Domain.Constants;

namespace VitalityBuilder.Api.Infrastructure.Data.Configurations;

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        // Primary Key
        builder.HasKey(c => c.Id);

        // Basic Properties
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Tier)
            .IsRequired()
            .HasDefaultValue(GameRuleConstants.MinimumTier);

        // Default Values
        builder.Property(c => c.HealthPool)
            .IsRequired()
            .HasDefaultValue(GameRuleConstants.BaseHealthPool);

        // Combat Attributes Relationship
        builder.HasOne(c => c.CombatAttributes)
            .WithOne(ca => ca.Character)
            .HasForeignKey<CombatAttributes>(ca => ca.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        // Utility Attributes Relationship
        builder.HasOne(c => c.UtilityAttributes)
            .WithOne(ua => ua.Character)
            .HasForeignKey<UtilityAttributes>(ua => ua.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        // Archetypes Relationship
        builder.HasOne(c => c.Archetypes)
            .WithOne(a => a.Character)
            .HasForeignKey<CharacterArchetypes>(a => a.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        // Special Attacks Collection
        builder.HasMany(c => c.SpecialAttacks)
            .WithOne()
            .HasForeignKey("CharacterId")
            .OnDelete(DeleteBehavior.Cascade);

        // Features Collection
        builder.HasMany(c => c.Features)
            .WithOne()
            .HasForeignKey("CharacterId")
            .OnDelete(DeleteBehavior.Cascade);

        // Expertise Collection
        builder.HasMany(c => c.Expertise)
            .WithOne()
            .HasForeignKey("CharacterId")
            .OnDelete(DeleteBehavior.Cascade);

        // Computed Properties
        builder.Property(c => c.MainPool)
            .HasComputedColumnSql("(([Tier] - 2) * 15)");

        builder.Property(c => c.UtilityPoints)
            .HasComputedColumnSql("(5 * ([Tier] - 1))");

        builder.Property(c => c.CombatAttributePoints)
            .HasComputedColumnSql("([Tier] * 2)");

        builder.Property(c => c.UtilityAttributePoints)
            .HasComputedColumnSql("[Tier]");

        // Indexes
        builder.HasIndex(c => c.Name);

        // Query Filters
        builder.HasQueryFilter(c => c.Tier >= GameRuleConstants.MinimumTier);

        // Validation
        builder.HasCheckConstraint("CK_Character_Tier", 
            $"Tier >= {GameRuleConstants.MinimumTier} AND Tier <= {GameRuleConstants.MaximumTier}");

        builder.HasCheckConstraint("CK_Character_HealthPool",
            $"HealthPool >= 0");

        // Value Conversions
        builder.Property<int>("SpentMainPoints")
            .HasDefaultValue(0);

        builder.Property<int>("SpentUtilityPoints")
            .HasDefaultValue(0);

        // Shadow Properties
        builder.Property<DateTime>("CreatedAt")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property<DateTime>("LastModifiedAt")
            .HasDefaultValueSql("GETUTCDATE()");

        // Triggers
        builder.ToTable(t => t.HasTrigger("TR_Character_UpdateModifiedDate"));
    }
}