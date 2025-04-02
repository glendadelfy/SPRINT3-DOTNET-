using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OdontoAPIMinimal.Migrations
{
    /// <inheritdoc />
    public partial class AddIdempotencyKeyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdempotencyKeys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    StatusCode = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ResponseBody = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdempotencyKeys", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdempotencyKeys");
        }
    }
}
