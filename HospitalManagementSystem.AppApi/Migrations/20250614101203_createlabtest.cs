using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.AppApi.Migrations
{
    /// <inheritdoc />
    public partial class createlabtest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LabTest_Tbl",
                columns: table => new
                {
                    LabTestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SampleRequired = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preparation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTest_Tbl", x => x.LabTestId);
                });

            migrationBuilder.CreateTable(
                name: "PatientLabTest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LabTestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientLabTest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientLabTest_AspNetUsers_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientLabTest_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientLabTest_LabTest_Tbl_LabTestId",
                        column: x => x.LabTestId,
                        principalTable: "LabTest_Tbl",
                        principalColumn: "LabTestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientLabTest_DoctorId",
                table: "PatientLabTest",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLabTest_LabTestId",
                table: "PatientLabTest",
                column: "LabTestId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLabTest_PatientId",
                table: "PatientLabTest",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientLabTest");

            migrationBuilder.DropTable(
                name: "LabTest_Tbl");
        }
    }
}
