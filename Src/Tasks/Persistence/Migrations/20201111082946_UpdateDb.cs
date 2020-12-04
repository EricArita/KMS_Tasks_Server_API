using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class UpdateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Sections",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Project",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Project",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<byte>(
                name: "role",
                table: "UserProjects",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Project",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_ParentId",
                table: "Project",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Project_ParentId",
                table: "Project",
                column: "ParentId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Project_ParentId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_ParentId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "role",
                table: "UserProjects");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Project");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Project",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Project",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Project",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SectionId",
                table: "Tasks",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_ProjectId",
                table: "Sections",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Sections",
                table: "Tasks",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
