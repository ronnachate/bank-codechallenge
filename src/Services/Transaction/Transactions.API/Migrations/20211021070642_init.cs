using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CodeChallenge.Services.Transactions.Api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransactionTypeId = table.Column<int>(type: "integer", nullable: false),
                    TransactionNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    AccountNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    AccountName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Fee = table.Column<decimal>(type: "numeric", nullable: false),
                    RecieverAccountNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    RecieverAccountName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionTypes_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalSchema: "public",
                        principalTable: "TransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionTypeId",
                schema: "public",
                table: "Transactions",
                column: "TransactionTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TransactionTypes",
                schema: "public");
        }
    }
}
