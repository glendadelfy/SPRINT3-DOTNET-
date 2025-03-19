using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OdontoAPIMinimal.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "table_usuario_usuario_odonto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    name_user = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    email_user = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    password_user = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    status_active_user = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    is_complete = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    role_user = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    avatar_user = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_table_usuario_usuario_odonto", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "table_usuario_usuario_odonto");
        }
    }
}
