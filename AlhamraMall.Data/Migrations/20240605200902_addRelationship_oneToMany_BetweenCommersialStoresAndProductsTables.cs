using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlhamraMall.Data.Migrations
{
    /// <inheritdoc />
    public partial class addRelationship_oneToMany_BetweenCommersialStoresAndProductsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CommercialStoreId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Products_CommercialStoreId",
                table: "Products",
                column: "CommercialStoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CommercialStores_CommercialStoreId",
                table: "Products",
                column: "CommercialStoreId",
                principalTable: "CommercialStores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CommercialStores_CommercialStoreId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CommercialStoreId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CommercialStoreId",
                table: "Products");
        }
    }
}
