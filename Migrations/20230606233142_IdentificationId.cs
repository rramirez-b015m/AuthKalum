﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KalumAuthManagement.Migrations
{
    public partial class IdentificationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "identificationId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "identificationId",
                table: "AspNetUsers");
        }
    }
}
