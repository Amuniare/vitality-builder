using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VitalityBuilder.Domain.Attributes;
using VitalityBuilder.Domain.Constants;

namespace VitalityBuilder.Infrastructure.Data.Configurations;

public class UtilityAttributesConfiguration : IEntityTypeConfiguration<UtilityAttributes>
{
    public void Configure(EntityTypeBuilder<UtilityAttributes> builder)
    {
        // Primary Key
        builder.HasKey(ua => ua.Id);

        // Required Character Relationship
        builder.HasOne(ua => ua.Character)
            .WithOne(c => c.UtilityAttributes)
            .HasForeignKey<UtilityAttributes>(ua => ua.CharacterId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Core Attributes
        builder.Property(ua => ua.Awareness)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(ua => ua.Communication)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(ua => ua.Intelligence)
            .IsRequired()
            .HasDefaultValue(0);

        // Computed Total Points
        builder.Property<int>("TotalPoints")
            .HasComputedColumnSql(
                "[Awareness] + [Communication] + [Intelligence]", 
                stored: true);

        // Ensure attributes don't exceed tier
        builder.HasCheckConstraint("CK_UtilityAttributes_Awareness",
            "Awareness >= 0 AND Awareness <= (SELECT Tier FROM Characters WHERE Id = CharacterId)");

        builder.HasCheckConstraint("CK_UtilityAttributes_Communication",
            "Communication >= 0 AND Communication <= (SELECT Tier FROM Characters WHERE Id = CharacterId)");

        builder.HasCheckConstraint("CK_UtilityAttributes_Intelligence",
            "Intelligence >= 0 AND Intelligence <= (SELECT Tier FROM Characters WHERE Id = CharacterId)");

        // Ensure total points don't exceed tier
        builder.HasCheckConstraint("CK_UtilityAttributes_TotalPoints",
            "([Awareness] + [Communication] + [Intelligence]) <= (SELECT Tier FROM Characters WHERE Id = CharacterId)");

        // Computed Columns for Derived Stats
        builder.Property<int>("InitiativeBonus")
            .HasComputedColumnSql("[Awareness]", stored: true);

        builder.Property<int>("DynamicEntryAllies")
            .HasComputedColumnSql(
                "CASE WHEN [Intelligence] < 1 THEN 1 ELSE [Intelligence] END",
                stored: true);

        builder.Property<int>("DynamicEntrySpaces")
            .HasComputedColumnSql(
                "CASE WHEN [Communication] < 1 THEN 1 ELSE [Communication] END",
                stored: true);

        // Indexes for Common Queries
        builder.HasIndex(ua => new { ua.CharacterId, ua.Awareness });
        builder.HasIndex(ua => new { ua.CharacterId, ua.Communication });
        builder.HasIndex(ua => new { ua.CharacterId, ua.Intelligence });

        // Query Filters
        builder.HasQueryFilter(ua => 
            ua.Awareness >= 0 && 
            ua.Communication >= 0 && 
            ua.Intelligence >= 0);

        // Value Conversions
        builder.Property<string>("SkillBonuses")
            .HasConversion(
                v => string.Join(";", v),
                v => v.Split(";", StringSplitOptions.RemoveEmptyEntries));

        // Audit Fields
        builder.Property<DateTime>("CreatedAt")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property<DateTime>("LastModifiedAt")
            .HasDefaultValueSql("GETUTCDATE()");

        // Update Trigger
        builder.ToTable(t => t.HasTrigger("TR_UtilityAttributes_UpdateModifiedDate"));

        // Additional Metadata
        builder.HasMetadata("AttributeType", "Utility");
        builder.HasMetadata("MaxTotalPoints", "Tier");
        builder.HasMetadata("AllowsExpertise", "true");
    }
}