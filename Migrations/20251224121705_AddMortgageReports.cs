using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddMortgageReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MortgageReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PropertyPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    DownPayment = table.Column<decimal>(type: "TEXT", nullable: false),
                    InterestRate = table.Column<double>(type: "REAL", nullable: false),
                    Years = table.Column<int>(type: "INTEGER", nullable: false),
                    PaymentType = table.Column<string>(type: "TEXT", nullable: false),
                    MonthlyPayment = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalPayment = table.Column<decimal>(type: "TEXT", nullable: false),
                    Overpayment = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MortgageReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MortgageReports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MortgageReports_UserId",
                table: "MortgageReports",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MortgageReports");
        }
    }
}
