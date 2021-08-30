using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleHound.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerCount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Password = table.Column<string>(nullable: true),
                    Customer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuCustomerOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true),
                    EmployeeName = table.Column<string>(nullable: true),
                    Customer = table.Column<string>(nullable: true),
                    TableNum = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    FoodItem = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCustomerOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuEmployees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true),
                    Tables = table.Column<int>(nullable: false),
                    Categories = table.Column<string>(nullable: true),
                    Items = table.Column<string>(nullable: true),
                    Prices = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuKitchen",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true),
                    EmployeeName = table.Column<string>(nullable: true),
                    Customer = table.Column<string>(nullable: true),
                    TableNum = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    FoodItem = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuKitchen", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerCount");

            migrationBuilder.DropTable(
                name: "MenuCustomerOrder");

            migrationBuilder.DropTable(
                name: "MenuEmployees");

            migrationBuilder.DropTable(
                name: "MenuEntry");

            migrationBuilder.DropTable(
                name: "MenuKitchen");
        }
    }
}
