using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.AppApi.Migrations
{
    /// <inheritdoc />
    public partial class createpatientlabtest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabTest_AspNetUsers_DoctorId",
                table: "PatientLabTest");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabTest_AspNetUsers_PatientId",
                table: "PatientLabTest");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabTest_LabTest_Tbl_LabTestId",
                table: "PatientLabTest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientLabTest",
                table: "PatientLabTest");

            migrationBuilder.RenameTable(
                name: "PatientLabTest",
                newName: "PatientLabTest_Tbl");

            migrationBuilder.RenameIndex(
                name: "IX_PatientLabTest_PatientId",
                table: "PatientLabTest_Tbl",
                newName: "IX_PatientLabTest_Tbl_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientLabTest_LabTestId",
                table: "PatientLabTest_Tbl",
                newName: "IX_PatientLabTest_Tbl_LabTestId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientLabTest_DoctorId",
                table: "PatientLabTest_Tbl",
                newName: "IX_PatientLabTest_Tbl_DoctorId");

            migrationBuilder.AddColumn<string>(
                name: "TestResult",
                table: "PatientLabTest_Tbl",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientLabTest_Tbl",
                table: "PatientLabTest_Tbl",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabTest_Tbl_AspNetUsers_DoctorId",
                table: "PatientLabTest_Tbl",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabTest_Tbl_AspNetUsers_PatientId",
                table: "PatientLabTest_Tbl",
                column: "PatientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabTest_Tbl_LabTest_Tbl_LabTestId",
                table: "PatientLabTest_Tbl",
                column: "LabTestId",
                principalTable: "LabTest_Tbl",
                principalColumn: "LabTestId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabTest_Tbl_AspNetUsers_DoctorId",
                table: "PatientLabTest_Tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabTest_Tbl_AspNetUsers_PatientId",
                table: "PatientLabTest_Tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabTest_Tbl_LabTest_Tbl_LabTestId",
                table: "PatientLabTest_Tbl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientLabTest_Tbl",
                table: "PatientLabTest_Tbl");

            migrationBuilder.DropColumn(
                name: "TestResult",
                table: "PatientLabTest_Tbl");

            migrationBuilder.RenameTable(
                name: "PatientLabTest_Tbl",
                newName: "PatientLabTest");

            migrationBuilder.RenameIndex(
                name: "IX_PatientLabTest_Tbl_PatientId",
                table: "PatientLabTest",
                newName: "IX_PatientLabTest_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientLabTest_Tbl_LabTestId",
                table: "PatientLabTest",
                newName: "IX_PatientLabTest_LabTestId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientLabTest_Tbl_DoctorId",
                table: "PatientLabTest",
                newName: "IX_PatientLabTest_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientLabTest",
                table: "PatientLabTest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabTest_AspNetUsers_DoctorId",
                table: "PatientLabTest",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabTest_AspNetUsers_PatientId",
                table: "PatientLabTest",
                column: "PatientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabTest_LabTest_Tbl_LabTestId",
                table: "PatientLabTest",
                column: "LabTestId",
                principalTable: "LabTest_Tbl",
                principalColumn: "LabTestId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
