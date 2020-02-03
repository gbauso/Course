using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Infrastructure.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Lecturer",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("6bd8dff5-4553-4c9b-8a07-47e379a9407a"), "Martin Taylor" });


            migrationBuilder.InsertData(table: "Course",
                                        columns: new[] { "Id", "Capacity", "LecturerId", "Title", "Updated" },
                                        values: new object[]
                                        { new Guid("fc81b413-73b8-4b11-a4fc-18d7dc647c7c"),
                                            5,
                                            new Guid("6bd8dff5-4553-4c9b-8a07-47e379a9407a"),
                                            "Data Science",
                                            DateTime.Now
                                        });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"TRUNCATE TABLE Course");
            migrationBuilder.Sql(@"TRUNCATE TABLE Lecturer");
        }
    }
}
