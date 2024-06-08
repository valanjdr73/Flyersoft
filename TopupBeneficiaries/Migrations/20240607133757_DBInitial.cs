using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TopupBeneficiaries.Migrations
{
    /// <inheritdoc />
    public partial class DBInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "topUpOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TopUpAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdateDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topUpOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    BalanceAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsVerifiedUser = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdatedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TopUpBeneficiaries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    NickName = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdatedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopUpBeneficiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopUpBeneficiaries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopUpTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    TopUpBeneficiaryId = table.Column<long>(type: "INTEGER", nullable: false),
                    TopUpOptionId = table.Column<int>(type: "INTEGER", nullable: false),
                    TopupDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TopUpChargeAmount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopUpTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopUpTransactions_TopUpBeneficiaries_TopUpBeneficiaryId",
                        column: x => x.TopUpBeneficiaryId,
                        principalTable: "TopUpBeneficiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopUpTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopUpTransactions_topUpOptions_TopUpOptionId",
                        column: x => x.TopUpOptionId,
                        principalTable: "topUpOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BalanceAmount", "CreatedDateTime", "IsDeleted", "IsVerifiedUser", "LastUpdatedDateTime", "PhoneNumber", "UserName" },
                values: new object[,]
                {
                    { 1L, 20000m, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4480), false, false, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4480), "7863256215", "Test User 1" },
                    { 2L, 4000m, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4480), false, true, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4480), "9863256215", "Test User 2" }
                });

            migrationBuilder.InsertData(
                table: "topUpOptions",
                columns: new[] { "Id", "CreatedDateTime", "IsDeleted", "LastUpdateDateTime", "Name", "TopUpAmount" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4410), false, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4410), "5", 5m },
                    { 2, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4420), false, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4420), "10", 10m },
                    { 3, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4420), false, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4420), "20", 20m },
                    { 4, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4420), false, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4420), "30", 30m },
                    { 5, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4420), false, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4420), "50", 50m },
                    { 6, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4420), false, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4420), "75", 75m },
                    { 7, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4420), false, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4420), "100", 100m }
                });

            migrationBuilder.InsertData(
                table: "TopUpBeneficiaries",
                columns: new[] { "Id", "CreatedDateTime", "IsActive", "IsDeleted", "LastUpdatedDateTime", "NickName", "PhoneNumber", "UserId" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4500), true, false, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4500), "Bene 1", "1234567890", 1L },
                    { 2L, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4500), true, false, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4500), "Bene 2", "1234567891", 1L },
                    { 3L, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4500), true, false, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4500), "Bene 3", "1234567892", 1L },
                    { 4L, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4500), true, false, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4500), "Bene 1", "9876543210", 2L },
                    { 5L, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4510), true, false, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4510), "Bene 2", "9876543211", 2L },
                    { 6L, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4510), true, false, new DateTime(2024, 6, 7, 13, 37, 57, 548, DateTimeKind.Utc).AddTicks(4510), "Bene 3", "9876543212", 2L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopUpBeneficiaries_UserId",
                table: "TopUpBeneficiaries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TopUpTransactions_TopUpBeneficiaryId",
                table: "TopUpTransactions",
                column: "TopUpBeneficiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_TopUpTransactions_TopUpOptionId",
                table: "TopUpTransactions",
                column: "TopUpOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TopUpTransactions_UserId",
                table: "TopUpTransactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopUpTransactions");

            migrationBuilder.DropTable(
                name: "TopUpBeneficiaries");

            migrationBuilder.DropTable(
                name: "topUpOptions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
