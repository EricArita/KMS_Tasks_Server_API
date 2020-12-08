using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class UpdateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Project",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tasks",
                table: "Tasks");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_HaveParentProject",
                table: "Project",
                column: "ParentId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_HaveParentTasks",
                table: "Tasks",
                column: "ParentId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_HaveParentProject",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_HaveParentTasks",
                table: "Tasks");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Project",
                table: "Project",
                column: "ParentId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tasks",
                table: "Tasks",
                column: "ParentId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
