using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.AppApi.Migrations
{
    /// <inheritdoc />
    public partial class tablePrescriptionCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prescription_Tbl",
                columns: table => new
                {
                    PrescriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrescriptionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescription_Tbl", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK_Prescription_Tbl_Appointment_Tbl_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment_Tbl",
                        principalColumn: "AppointmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionDetail_Tbl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrescriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionDetail_Tbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrescriptionDetail_Tbl_Prescription_Tbl_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescription_Tbl",
                        principalColumn: "PrescriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_Tbl_AppointmentId",
                table: "Prescription_Tbl",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionDetail_Tbl_PrescriptionId",
                table: "PrescriptionDetail_Tbl",
                column: "PrescriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrescriptionDetail_Tbl");

            migrationBuilder.DropTable(
                name: "Prescription_Tbl");
        }
    }
}
