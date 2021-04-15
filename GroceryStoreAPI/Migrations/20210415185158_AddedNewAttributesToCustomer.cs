using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GroceryStoreAPI.Migrations
{
    public partial class AddedNewAttributesToCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Age = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Age", "CreatedDate", "Email", "Name", "Phone", "UpdatedDate" },
                values: new object[] { new Guid("82c98f0c-2093-4ef7-8bd4-a13e00c4d02e"), "449 Simcoe St., London ON N6L 1Z7", 28, new DateTimeOffset(new DateTime(2021, 4, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "darryl.gollan@gmail.com", "Darryl", "226-268-4611", null });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Age", "CreatedDate", "Email", "Name", "Phone", "UpdatedDate" },
                values: new object[] { new Guid("ef0c4d03-12b4-4677-8eed-801ce18fc883"), "433 Dufferin Ave., London ON N6L 1Z7", 38, new DateTimeOffset(new DateTime(2021, 4, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "michelle@gmail.com", "Michelle", "226-268-6611", null });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Age", "CreatedDate", "Email", "Name", "Phone", "UpdatedDate" },
                values: new object[] { new Guid("38d93d92-b202-41c1-a441-66e283d50f25"), "523 Brringer St., Belleville ON N6L 1Z7", 64, new DateTimeOffset(new DateTime(2021, 4, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "sandy@gmail.com", "Sandy", "613-233-9811", null });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Age", "CreatedDate", "Email", "Name", "Phone", "UpdatedDate" },
                values: new object[] { new Guid("641f530d-dd74-4d57-91a8-66ff7443669b"), "444 Oxford St West, London ON N6L 1Z7", 44, new DateTimeOffset(new DateTime(2021, 4, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "sandra@gmail.com", "Sandra", "519-232-9877", null });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Age", "CreatedDate", "Email", "Name", "Phone", "UpdatedDate" },
                values: new object[] { new Guid("286abd03-f3d8-49d4-aef3-35cf3a9d66d4"), "923 Oxford St West, London ON N6L 1Z7", 54, new DateTimeOffset(new DateTime(2021, 4, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "raichel@gmail.com", "Raichel", "519-232-8823", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
