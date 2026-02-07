using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherDashboard.Api.Migrations
{
    public partial class RemoveUserFavouriteIsDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "UserFavourites");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "UserFavourites",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
