using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.AppApi.Migrations
{
    /// <inheritdoc />
    public partial class columnAddedToPatientLabTestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppointmentId",
                table: "PatientLabTest_Tbl",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "PatientLabTest_Tbl",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportFilePath",
                table: "PatientLabTest_Tbl",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "PatientLabTest_Tbl",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientLabTest_Tbl_AppointmentId",
                table: "PatientLabTest_Tbl",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabTest_Tbl_Appointment_Tbl_AppointmentId",
                table: "PatientLabTest_Tbl",
                column: "AppointmentId",
                principalTable: "Appointment_Tbl",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabTest_Tbl_Appointment_Tbl_AppointmentId",
                table: "PatientLabTest_Tbl");

            migrationBuilder.DropIndex(
                name: "IX_PatientLabTest_Tbl_AppointmentId",
                table: "PatientLabTest_Tbl");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "PatientLabTest_Tbl");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "PatientLabTest_Tbl");

            migrationBuilder.DropColumn(
                name: "ReportFilePath",
                table: "PatientLabTest_Tbl");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "PatientLabTest_Tbl");
        }
    }
}
