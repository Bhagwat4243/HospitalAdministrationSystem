using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.AppApi.Migrations
{
    /// <inheritdoc />
    public partial class columnAddedToAppointmenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Appointment_Tbl",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Appointment_Tbl");
        }
    }
}
