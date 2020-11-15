using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleCrm.SqlDbServices.Migrations
{
    public partial class CustomerExpanded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Customers",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastContactDate",
                table: "Customers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PreferredContactMethod",
                table: "Customers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Customers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastContactDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PreferredContactMethod",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Customers");
        }
    }
}
