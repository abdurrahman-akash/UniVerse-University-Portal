using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddFacultyAndNoticeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faculties_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Audience = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notices", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Faculties",
                columns: new[] { "Id", "DepartmentId", "Designation", "Email", "FirstName", "JoiningDate", "LastName", "Phone" },
                values: new object[,]
                {
                    { 1, 1, "Professor", "kamal@universe.edu", "Kamal", new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Local), "Hossain", null },
                    { 2, 2, "Associate Professor", "nasrin@universe.edu", "Nasrin", new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Local), "Akter", null },
                    { 3, 3, "Professor", "rafiq@universe.edu", "Rafiq", new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Local), "Islam", null }
                });

            migrationBuilder.InsertData(
                table: "Notices",
                columns: new[] { "Id", "Audience", "Description", "PublishDate", "Title" },
                values: new object[,]
                {
                    { 1, "Students", "Mid-term exam will start from 15th March.", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mid-Term Exam Routine Published" },
                    { 2, "All", "Registration for Spring 2026 is now open.", new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "New Semester Registration Open" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_DepartmentId",
                table: "Faculties",
                column: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "Notices");
        }
    }
}
