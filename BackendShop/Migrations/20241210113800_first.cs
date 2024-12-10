using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendShop.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ImageCategoryPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCategories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "tblSubCategories",
                columns: table => new
                {
                    SubCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ImageSubCategoryPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSubCategories", x => x.SubCategoryId);
                    table.ForeignKey(
                        name: "FK_tblSubCategories_tblCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "tblCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblProducts",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Form = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityInPack = table.Column<int>(type: "int", nullable: true),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProducts", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_tblProducts_tblSubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "tblSubCategories",
                        principalColumn: "SubCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDescImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDescImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDescImages_tblProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tblProducts",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "ProductImageEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImageEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImageEntity_tblProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tblProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDescImages_ProductId",
                table: "ProductDescImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImageEntity_ProductId",
                table: "ProductImageEntity",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_tblProducts_SubCategoryId",
                table: "tblProducts",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_tblSubCategories_CategoryId",
                table: "tblSubCategories",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDescImages");

            migrationBuilder.DropTable(
                name: "ProductImageEntity");

            migrationBuilder.DropTable(
                name: "tblProducts");

            migrationBuilder.DropTable(
                name: "tblSubCategories");

            migrationBuilder.DropTable(
                name: "tblCategories");
        }
    }
}
