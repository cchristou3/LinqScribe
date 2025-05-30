using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinqScribeTests.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfRelatives = table.Column<byte>(type: "tinyint", nullable: false),
                    NumberOfSiblings = table.Column<short>(type: "smallint", nullable: false),
                    Salary = table.Column<short>(type: "smallint", nullable: false),
                    ExpectedSalary = table.Column<int>(type: "int", nullable: false),
                    NumberOfFriends = table.Column<long>(type: "bigint", nullable: false),
                    NumberOfDays = table.Column<long>(type: "bigint", nullable: false),
                    NumberOfSeconds = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    TaxRate = table.Column<float>(type: "real", nullable: false),
                    SocialInsuranceRate = table.Column<double>(type: "float", nullable: false),
                    IndexFundsRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entities");
        }
    }
}
