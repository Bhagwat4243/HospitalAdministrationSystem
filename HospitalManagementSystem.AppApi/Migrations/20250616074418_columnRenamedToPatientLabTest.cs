using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.AppApi.Migrations
{
    /// <inheritdoc />
    public partial class columnRenamedToPatientLabTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabTest_Tbl_AspNetUsers_DoctorId",
                table: "PatientLabTest_Tbl");




            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "PatientLabTest_Tbl",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabTest_Tbl_AspNetUsers_DoctorId",
                table: "PatientLabTest_Tbl",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabTest_Tbl_AspNetUsers_DoctorId",
                table: "PatientLabTest_Tbl");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "PatientLabTest_Tbl",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "OrderedById",
                table: "PatientLabTest_Tbl",
                type: "nvarchar(max)",
                nullable: true);



            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabTest_Tbl_AspNetUsers_DoctorId",
                table: "PatientLabTest_Tbl",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
