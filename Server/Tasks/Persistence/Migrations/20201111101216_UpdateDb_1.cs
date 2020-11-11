using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class UpdateDb_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "UserProjects");

            migrationBuilder.AddColumn<byte>(
                name: "RoleId",
                table: "UserProjects",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "ProjectRoles",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRoles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProjects_RoleId",
                table: "UserProjects",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProjects_ProjectRole",
                table: "UserProjects",
                column: "RoleId",
                principalTable: "ProjectRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProjects_ProjectRole",
                table: "UserProjects");

            migrationBuilder.DropTable(
                name: "ProjectRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserProjects_RoleId",
                table: "UserProjects");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "UserProjects");

            migrationBuilder.AddColumn<byte>(
                name: "role",
                table: "UserProjects",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
