using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WhatEver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSkill_Employees_EmployeeId",
                table: "EmployeeSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSkill_Skills_SkillId",
                table: "EmployeeSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeSkill",
                table: "EmployeeSkill");

            migrationBuilder.RenameTable(
                name: "EmployeeSkill",
                newName: "EmployeeSkills");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeSkill_SkillId",
                table: "EmployeeSkills",
                newName: "IX_EmployeeSkills_SkillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeSkills",
                table: "EmployeeSkills",
                columns: new[] { "EmployeeId", "SkillId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSkills_Employees_EmployeeId",
                table: "EmployeeSkills",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSkills_Skills_SkillId",
                table: "EmployeeSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSkills_Employees_EmployeeId",
                table: "EmployeeSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSkills_Skills_SkillId",
                table: "EmployeeSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeSkills",
                table: "EmployeeSkills");

            migrationBuilder.RenameTable(
                name: "EmployeeSkills",
                newName: "EmployeeSkill");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeSkills_SkillId",
                table: "EmployeeSkill",
                newName: "IX_EmployeeSkill_SkillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeSkill",
                table: "EmployeeSkill",
                columns: new[] { "EmployeeId", "SkillId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSkill_Employees_EmployeeId",
                table: "EmployeeSkill",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSkill_Skills_SkillId",
                table: "EmployeeSkill",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
