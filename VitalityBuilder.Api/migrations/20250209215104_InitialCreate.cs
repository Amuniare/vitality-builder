using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitalityBuilder.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tier = table.Column<int>(type: "int", nullable: false),
                    MainPointPool = table.Column<int>(type: "int", nullable: false),
                    SpecialAttacksPointPool = table.Column<int>(type: "int", nullable: false),
                    UtilityPointPool = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterArchetypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterArchetypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterArchetypes_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CombatAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Focus = table.Column<int>(type: "int", nullable: false),
                    Power = table.Column<int>(type: "int", nullable: false),
                    Mobility = table.Column<int>(type: "int", nullable: false),
                    Endurance = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CombatAttributes_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expertise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expertise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expertise_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialAttacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttackType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EffectType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Limits = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Upgrades = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialAttacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialAttacks_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UniquePowers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniquePowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UniquePowers_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UtilityAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Awareness = table.Column<int>(type: "int", nullable: false),
                    Communication = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilityAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UtilityAttributes_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttackTypeArchetype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    AccuracyPenalty = table.Column<int>(type: "int", nullable: false),
                    EffectPenalty = table.Column<int>(type: "int", nullable: false),
                    BypassesAccuracyChecks = table.Column<bool>(type: "bit", nullable: false),
                    HasFreeAOE = table.Column<bool>(type: "bit", nullable: false),
                    CharacterArchetypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttackTypeArchetype", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttackTypeArchetype_CharacterArchetypes_CharacterArchetypesId",
                        column: x => x.CharacterArchetypesId,
                        principalTable: "CharacterArchetypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EffectTypeArchetype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    HasAccessToAdvancedConditions = table.Column<bool>(type: "bit", nullable: false),
                    DamagePenalty = table.Column<int>(type: "int", nullable: false),
                    ConditionPenalty = table.Column<int>(type: "int", nullable: false),
                    RequiresHybridEffects = table.Column<bool>(type: "bit", nullable: false),
                    CharacterArchetypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EffectTypeArchetype", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EffectTypeArchetype_CharacterArchetypes_CharacterArchetypesId",
                        column: x => x.CharacterArchetypesId,
                        principalTable: "CharacterArchetypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovementArchetype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    SpeedBonusByTier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IgnoresOpportunityAttacks = table.Column<bool>(type: "bit", nullable: false),
                    IgnoresDifficultTerrain = table.Column<bool>(type: "bit", nullable: false),
                    IsImmuneToProne = table.Column<bool>(type: "bit", nullable: false),
                    MovementMultiplier = table.Column<float>(type: "real", nullable: false),
                    CharacterArchetypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovementArchetype", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovementArchetype_CharacterArchetypes_CharacterArchetypesId",
                        column: x => x.CharacterArchetypesId,
                        principalTable: "CharacterArchetypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialAttackArchetype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    BasePoints = table.Column<int>(type: "int", nullable: false),
                    MaxSpecialAttacks = table.Column<int>(type: "int", nullable: false),
                    LimitPointMultiplier = table.Column<float>(type: "real", nullable: false),
                    CanTakeLimits = table.Column<bool>(type: "bit", nullable: false),
                    RequiredLimits = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CharacterArchetypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialAttackArchetype", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialAttackArchetype_CharacterArchetypes_CharacterArchetypesId",
                        column: x => x.CharacterArchetypesId,
                        principalTable: "CharacterArchetypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UniqueAbilityArchetype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    ExtraQuickActions = table.Column<int>(type: "int", nullable: false),
                    ExtraPointPool = table.Column<int>(type: "int", nullable: false),
                    StatBonuses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CharacterArchetypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniqueAbilityArchetype", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UniqueAbilityArchetype_CharacterArchetypes_CharacterArchetypesId",
                        column: x => x.CharacterArchetypesId,
                        principalTable: "CharacterArchetypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UtilityArchetype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    BaseUtilityPool = table.Column<int>(type: "int", nullable: false),
                    CanPurchaseExpertise = table.Column<bool>(type: "bit", nullable: false),
                    TierBonusMultiplier = table.Column<float>(type: "real", nullable: false),
                    Restrictions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CharacterArchetypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilityArchetype", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UtilityArchetype_CharacterArchetypes_CharacterArchetypesId",
                        column: x => x.CharacterArchetypesId,
                        principalTable: "CharacterArchetypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttackTypeArchetype_CharacterArchetypesId",
                table: "AttackTypeArchetype",
                column: "CharacterArchetypesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterArchetypes_CharacterId",
                table: "CharacterArchetypes",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatAttributes_CharacterId",
                table: "CombatAttributes",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EffectTypeArchetype_CharacterArchetypesId",
                table: "EffectTypeArchetype",
                column: "CharacterArchetypesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expertise_CharacterId",
                table: "Expertise",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_MovementArchetype_CharacterArchetypesId",
                table: "MovementArchetype",
                column: "CharacterArchetypesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialAttackArchetype_CharacterArchetypesId",
                table: "SpecialAttackArchetype",
                column: "CharacterArchetypesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialAttacks_CharacterId",
                table: "SpecialAttacks",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueAbilityArchetype_CharacterArchetypesId",
                table: "UniqueAbilityArchetype",
                column: "CharacterArchetypesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniquePowers_CharacterId",
                table: "UniquePowers",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_UtilityArchetype_CharacterArchetypesId",
                table: "UtilityArchetype",
                column: "CharacterArchetypesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UtilityAttributes_CharacterId",
                table: "UtilityAttributes",
                column: "CharacterId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttackTypeArchetype");

            migrationBuilder.DropTable(
                name: "CombatAttributes");

            migrationBuilder.DropTable(
                name: "EffectTypeArchetype");

            migrationBuilder.DropTable(
                name: "Expertise");

            migrationBuilder.DropTable(
                name: "MovementArchetype");

            migrationBuilder.DropTable(
                name: "SpecialAttackArchetype");

            migrationBuilder.DropTable(
                name: "SpecialAttacks");

            migrationBuilder.DropTable(
                name: "UniqueAbilityArchetype");

            migrationBuilder.DropTable(
                name: "UniquePowers");

            migrationBuilder.DropTable(
                name: "UtilityArchetype");

            migrationBuilder.DropTable(
                name: "UtilityAttributes");

            migrationBuilder.DropTable(
                name: "CharacterArchetypes");

            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
