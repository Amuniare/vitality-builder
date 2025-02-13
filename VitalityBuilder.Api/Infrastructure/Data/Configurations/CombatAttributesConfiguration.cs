using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VitalityBuilder.Domain.Attributes;
using VitalityBuilder.Domain.Constants;

namespace VitalityBuilder.Infrastructure.Data.Configurations;

public class CombatAttributesConfiguration : IEntityTypeConfiguration<CombatAttributes>
{
    public void Configure(EntityTypeBuilder<CombatAttributes> builder)
    {
        // Primary Key
        builder.HasKey(ca => ca.Id);

        // Required Character Relationship
        builder.HasOne(ca => ca.Character)
            .WithOne(c => c.CombatAttributes)
            .HasForeignKey<CombatAttributes>(ca => ca.CharacterId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Core Attributes
        builder.Property(ca => ca.Focus)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(ca => ca.Power)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(ca => ca.Mobility)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(ca => ca.Endurance)
            .IsRequired()
            .HasDefaultValue(0);

        // Computed Total Points
        builder.Property<int>("TotalPoints")
            .HasComputedColumnSql(
                "[Focus] + [Power] + [Mobility] + [Endurance]", 
                stored: true);

        // Ensure attributes don't exceed tier
        builder.HasCheckConstraint("CK_CombatAttributes_Focus",
            "Focus >= 0 AND Focus <= (SELECT Tier FROM Characters WHERE Id = CharacterId)");

        builder.HasCheckConstraint("CK_CombatAttributes_Power",
            "Power >= 0 AND Power <= (SELECT Tier FROM Characters WHERE Id = CharacterId)");

        builder.HasCheckConstraint("CK_CombatAttributes_Mobility",
            "Mobility >= 0 AND Mobility <= (SELECT Tier FROM Characters WHERE Id = CharacterId)");

        builder.HasCheckConstraint("CK_CombatAttributes_Endurance",
            "Endurance >= 0 AND Endurance <= (SELECT Tier FROM Characters WHERE Id = CharacterId)");

        // Ensure total points don't exceed tier × 2
        builder.HasCheckConstraint("CK_CombatAttributes_TotalPoints",
            "([Focus] + [Power] + [Mobility] + [Endurance]) <= (SELECT Tier * 2 FROM Characters WHERE Id = CharacterId)");

        // Computed Columns for Derived Stats
        builder.Property<int>("Avoidance")
            .HasComputedColumnSql(
                "10 + (SELECT Tier FROM Characters WHERE Id = CharacterId) + [Mobility]",
                stored: true);

        builder.Property<int>("Durability")
            .HasComputedColumnSql(
                "(SELECT Tier FROM Characters WHERE Id = CharacterId) + CEILING(CAST([Endurance] as float) * 1.5)",
                stored: true);

        builder.Property<int>("ResolveResistance")
            .HasComputedColumnSql(
                "10 + (SELECT Tier FROM Characters WHERE Id = CharacterId) + [Focus]",
                stored: true);

        builder.Property<int>("StabilityResistance")
            .HasComputedColumnSql(
                "10 + (SELECT Tier FROM Characters WHERE Id = CharacterId) + [Power]",
                stored: true);

        builder.Property<int>("VitalityResistance")
            .HasComputedColumnSql(
                "10 + (SELECT Tier FROM Characters WHERE Id = CharacterId) + [Endurance]",
                stored: true);

        // Indexes for Common Queries
        builder.HasIndex(ca => new { ca.CharacterId, ca.Focus });
        builder.HasIndex(ca => new { ca.CharacterId, ca.Power });
        builder.HasIndex(ca => new { ca.CharacterId, ca.Mobility });
        builder.HasIndex(ca => new { ca.CharacterId, ca.Endurance });

        // Querying Helpers
        builder.HasQueryFilter(ca => 
            ca.Focus >= 0 && ca.Power >= 0 && 
            ca.Mobility >= 0 && ca.Endurance >= 0);

        // Audit Fields
        builder.Property<DateTime>("CreatedAt")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property<DateTime>("LastModifiedAt")
            .HasDefaultValueSql("GETUTCDATE()");

        // Update Trigger
        builder.ToTable(t => t.HasTrigger("TR_CombatAttributes_UpdateModifiedDate"));
    }
}