using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitalityBuilder.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixMainPointPoolComputed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CharacterArchetypes_CharacterId",
                table: "CharacterArchetypes");

            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "SpecialAttacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Total",
                table: "CombatAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MainPointPool",
                table: "Characters",
                type: "int",
                nullable: false,
                computedColumnSql: "CASE WHEN [Tier] >= 2 THEN ([Tier] - 2) * 15 ELSE 0 END",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterArchetypes_CharacterId",
                table: "CharacterArchetypes",
                column: "CharacterId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CharacterArchetypes_CharacterId",
                table: "CharacterArchetypes");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "SpecialAttacks");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "CombatAttributes");

            migrationBuilder.AlterColumn<int>(
                name: "MainPointPool",
                table: "Characters",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComputedColumnSql: "CASE WHEN [Tier] >= 2 THEN ([Tier] - 2) * 15 ELSE 0 END");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterArchetypes_CharacterId",
                table: "CharacterArchetypes",
                column: "CharacterId");
        }
    }
}
