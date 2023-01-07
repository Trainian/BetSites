using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    AuxiliaryLocator = table.Column<string>(type: "TEXT", nullable: false),
                    BetTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Score = table.Column<string>(type: "TEXT", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coefficients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RatioFirst = table.Column<double>(type: "REAL", nullable: false),
                    RatioSecond = table.Column<double>(type: "REAL", nullable: false),
                    RatioThird = table.Column<double>(type: "REAL", nullable: false),
                    BetTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Score = table.Column<string>(type: "TEXT", nullable: false),
                    IsMadeBet = table.Column<bool>(type: "INTEGER", nullable: false),
                    BetId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coefficients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coefficients_Bets_BetId",
                        column: x => x.BetId,
                        principalTable: "Bets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coefficients_BetId",
                table: "Coefficients",
                column: "BetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coefficients");

            migrationBuilder.DropTable(
                name: "Bets");
        }
    }
}
