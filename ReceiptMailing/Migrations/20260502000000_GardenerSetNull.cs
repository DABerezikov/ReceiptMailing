using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptMailing.Migrations
{
    [Microsoft.EntityFrameworkCore.Infrastructure.DbContext(typeof(ReceiptMailing.Data.Context.ParcelDb))]
    [Migration("20260502000000_GardenerSetNull")]
    public partial class GardenerSetNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys=OFF");

            migrationBuilder.Sql(@"
                CREATE TABLE ""Parcels_new"" (
                    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_Parcels"" PRIMARY KEY AUTOINCREMENT,
                    ""GardenerId"" INTEGER NULL
                        CONSTRAINT ""FK_Parcels_Gardeners_GardenerId"" REFERENCES ""Gardeners"" (""Id"") ON DELETE SET NULL,
                    ""Street"" TEXT NULL,
                    ""PlotArea"" REAL NOT NULL,
                    ""CadastralNumber"" TEXT NULL,
                    ""Details"" TEXT NULL,
                    ""Electrification"" INTEGER NOT NULL,
                    ""HavingHouse"" INTEGER NOT NULL,
                    ""HouseNumber"" TEXT NULL,
                    ""Category"" TEXT NULL,
                    ""Status"" TEXT NULL,
                    ""Description"" TEXT NULL,
                    ""Number"" TEXT NOT NULL
                )");

            migrationBuilder.Sql(@"INSERT INTO ""Parcels_new"" SELECT * FROM ""Parcels""");
            migrationBuilder.Sql(@"DROP TABLE ""Parcels""");
            migrationBuilder.Sql(@"ALTER TABLE ""Parcels_new"" RENAME TO ""Parcels""");
            migrationBuilder.Sql(@"CREATE INDEX ""IX_Parcels_GardenerId"" ON ""Parcels"" (""GardenerId"")");

            migrationBuilder.Sql("PRAGMA foreign_keys=ON");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys=OFF");

            migrationBuilder.Sql(@"
                CREATE TABLE ""Parcels_old"" (
                    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_Parcels"" PRIMARY KEY AUTOINCREMENT,
                    ""GardenerId"" INTEGER NOT NULL
                        CONSTRAINT ""FK_Parcels_Gardeners_GardenerId"" REFERENCES ""Gardeners"" (""Id"") ON DELETE CASCADE,
                    ""Street"" TEXT NULL,
                    ""PlotArea"" REAL NOT NULL,
                    ""CadastralNumber"" TEXT NULL,
                    ""Details"" TEXT NULL,
                    ""Electrification"" INTEGER NOT NULL,
                    ""HavingHouse"" INTEGER NOT NULL,
                    ""HouseNumber"" TEXT NULL,
                    ""Category"" TEXT NULL,
                    ""Status"" TEXT NULL,
                    ""Description"" TEXT NULL,
                    ""Number"" TEXT NOT NULL
                )");

            migrationBuilder.Sql(@"INSERT INTO ""Parcels_old"" SELECT * FROM ""Parcels"" WHERE ""GardenerId"" IS NOT NULL");
            migrationBuilder.Sql(@"DROP TABLE ""Parcels""");
            migrationBuilder.Sql(@"ALTER TABLE ""Parcels_old"" RENAME TO ""Parcels""");
            migrationBuilder.Sql(@"CREATE INDEX ""IX_Parcels_GardenerId"" ON ""Parcels"" (""GardenerId"")");

            migrationBuilder.Sql("PRAGMA foreign_keys=ON");
        }
    }
}
