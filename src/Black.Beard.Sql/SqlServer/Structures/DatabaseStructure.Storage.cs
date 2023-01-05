using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml.Linq;
using Bb.SqlServer.Queries;
using Bb.SqlServer.Structures.Dacpacs;
using Bb.SqlServerStructures;
using static System.Formats.Asn1.AsnWriter;

namespace Bb.SqlServer.Structures
{


    public partial class DatabaseStructure
    {

        public static DatabaseStructure GetStructure(string databaseName, string schema)
        {

            return new DatabaseStructure()
            {
                DatabaseName = databaseName,
            }

                .AddFileGroup(
                    new FileGroupDescriptor() { Name = "PRIMARY" }
                )

                .AddSchemas(
                    new SchemaDescriptor() { Name = schema }
                )

                .AddTables(

                    new TableDescriptor(schema, "Connections")
                        .AddColumns(
                            new ColumnDescriptor() { Name = "Id", Type = SqlTypeDescriptor.IdentityBigInt(1, 1), AllowNull = false },
                            new ColumnDescriptor() { Name = "Name", Type = SqlTypeDescriptor.Varchar(70), AllowNull = false },
                            new ColumnDescriptor() { Name = "Server", Type = SqlTypeDescriptor.Varchar(170), AllowNull = false },
                            new ColumnDescriptor() { Name = "Database", Type = SqlTypeDescriptor.Varchar(170), AllowNull = false },
                            new ColumnDescriptor() { Name = "Userid", Type = SqlTypeDescriptor.Varchar(170), AllowNull = true },
                            new ColumnDescriptor() { Name = "Password", Type = SqlTypeDescriptor.Varchar(170), AllowNull = true }
                    )
                    .AddPrimaryKeys(
                        new IndexedColumnReferenceDescriptor() { Name = "Id" }
                    ),

                    new TableDescriptor(schema, "Applications")
                        .AddColumns(
                            new ColumnDescriptor() { Name = "Id", Type = SqlTypeDescriptor.IdentityBigInt(1, 1), AllowNull = false },
                            new ColumnDescriptor() { Name = "Name", Type = SqlTypeDescriptor.Varchar(70), AllowNull = false }
                        )
                        .AddPrimaryKeys(
                            new IndexedColumnReferenceDescriptor() { Name = "Id" }
                        ),

                    new TableDescriptor(schema, "ApplicationEnvironments")
                        .AddColumns(
                            new ColumnDescriptor() { Name = "Id", Type = SqlTypeDescriptor.IdentityBigInt(1, 1), AllowNull = false },
                            new ColumnDescriptor() { Name = "Name", Type = SqlTypeDescriptor.Varchar(70), AllowNull = false },
                            new ColumnDescriptor() { Name = "ApplicationId", Type = SqlTypeDescriptor.BigInt(), AllowNull = false },
                            new ColumnDescriptor() { Name = "ConnectionId", Type = SqlTypeDescriptor.BigInt(), AllowNull = false }
                        )
                        .AddPrimaryKeys(
                            new IndexedColumnReferenceDescriptor() { Name = "Id" }
                        )
                        .AddForeignKey(null, schema, "Applications", c =>
                        {
                            c.AddLocalColumns("ApplicationId")
                             .AddRemoteColumns("Id")
                             //.DeleteCascade(true)
                             //.UpdateCascade(true)
                            ;

                        })
                        .AddForeignKey(null, schema, "Connections", c =>
                        {
                            c.AddLocalColumns("ConnectionId")
                             .AddRemoteColumns("Id")
                             //.DeleteCascade(true)
                             //.UpdateCascade(true)
                            ;

                        }),

                    new TableDescriptor(schema, "DiskTables")
                        .AddColumns(
                            new ColumnDescriptor() { Name = "Id", Type = SqlTypeDescriptor.IdentityBigInt(1, 1), AllowNull = false },
                            new ColumnDescriptor() { Name = "ApplicationEnvironmentId", Type = SqlTypeDescriptor.BigInt(), AllowNull = false },
                            new ColumnDescriptor() { Name = "TableSchema", Type = SqlTypeDescriptor.Varchar(70), AllowNull = false },
                            new ColumnDescriptor() { Name = "Name", Type = SqlTypeDescriptor.Varchar(70), AllowNull = false },
                            new ColumnDescriptor() { Name = "PartitionSchemeName", Type = SqlTypeDescriptor.Varchar(160), AllowNull = true }
                        )
                        .AddPrimaryKeys(
                            new IndexedColumnReferenceDescriptor() { Name = "Id" }
                        )
                        .AddForeignKey(null, schema, "ApplicationEnvironments", c =>
                        {
                            c.AddLocalColumns("ApplicationEnvironmentId")
                             .AddRemoteColumns("Id")
                             //.DeleteCascade(true)
                             //.UpdateCascade(true)
                            ;

                        }),

                    new TableDescriptor(schema, "DiskTablesIndex")
                        .AddColumns(
                           new ColumnDescriptor() { Name = "Id", Type = SqlTypeDescriptor.IdentityBigInt(1, 1), AllowNull = false },
                           new ColumnDescriptor() { Name = "Table", Type = SqlTypeDescriptor.BigInt(), AllowNull = false },
                           new ColumnDescriptor() { Name = "PrimaryKey", Type = SqlTypeDescriptor.Bit(), AllowNull = false },
                           new ColumnDescriptor() { Name = "Name", Type = SqlTypeDescriptor.Varchar(70), AllowNull = false },
                           new ColumnDescriptor() { Name = "Clustered", Type = SqlTypeDescriptor.Bit(), AllowNull = false },
                           new ColumnDescriptor() { Name = "Unique", Type = SqlTypeDescriptor.Bit(), AllowNull = false },
                           new ColumnDescriptor() { Name = "PartitionSchemeName", Type = SqlTypeDescriptor.Varchar(60), AllowNull = true },
                           new ColumnDescriptor() { Name = "PadIndex", Type = SqlTypeDescriptor.Bit(), AllowNull = true },
                           new ColumnDescriptor() { Name = "StatisticsNoRecompute", Type = SqlTypeDescriptor.Bit(), AllowNull = true },
                           new ColumnDescriptor() { Name = "AllowRowLocks", Type = SqlTypeDescriptor.Bit(), AllowNull = true },
                           new ColumnDescriptor() { Name = "AllowPageLocks", Type = SqlTypeDescriptor.Bit(), AllowNull = true },
                           new ColumnDescriptor() { Name = "OptimizeForSequentialKey", Type = SqlTypeDescriptor.Bit(), AllowNull = true }
                        )
                        .AddPrimaryKeys(
                            new IndexedColumnReferenceDescriptor() { Name = "Id" }
                        )
                        .AddForeignKey(null, schema, "DiskTables", c =>
                        {
                            c.AddLocalColumns("Table");
                            c.AddRemoteColumns("Id");
                        }),

                    new TableDescriptor(schema, "DiskTablesColumns")
                        .AddColumns(
                            new ColumnDescriptor() { Name = "Id", Type = SqlTypeDescriptor.IdentityBigInt(1, 1), AllowNull = false },
                            new ColumnDescriptor() { Name = "Table", Type = SqlTypeDescriptor.BigInt(), AllowNull = false },
                            new ColumnDescriptor() { Name = "Name", Type = SqlTypeDescriptor.Varchar(70), AllowNull = false },
                            new ColumnDescriptor() { Name = "Type", Type = SqlTypeDescriptor.Varchar(70), AllowNull = false },
                            new ColumnDescriptor() { Name = "Precision", Type = SqlTypeDescriptor.Int(), AllowNull = false },
                            new ColumnDescriptor() { Name = "Scale", Type = SqlTypeDescriptor.Int(), AllowNull = false },
                            new ColumnDescriptor() { Name = "AllowNull", Type = SqlTypeDescriptor.Bit(), AllowNull = true },
                            new ColumnDescriptor() { Name = "Caption", Type = SqlTypeDescriptor.Varchar(160), AllowNull = true },
                            new ColumnDescriptor() { Name = "DefaultValue", Type = SqlTypeDescriptor.Varchar(160), AllowNull = true }
                        )
                        .AddPrimaryKeys(
                            new IndexedColumnReferenceDescriptor() { Name = "Id" }
                        )
                        .AddForeignKey("FK_DiskTablesColumns_Table_2_DiskTables_Id", schema, "DiskTables", c =>
                        {
                            c.AddLocalColumns("Table");
                            c.AddRemoteColumns("Id");
                        }),

                    new TableDescriptor(schema, "Diagrams")
                        .AddColumns(
                            new ColumnDescriptor() { Name = "Id", Type = SqlTypeDescriptor.IdentityBigInt(1, 1), AllowNull = false },
                            new ColumnDescriptor() { Name = "Name", Type = SqlTypeDescriptor.Varchar(70), AllowNull = false }
                        )
                        .AddPrimaryKeys(
                            new IndexedColumnReferenceDescriptor() { Name = "Id" }
                        ),

                    new TableDescriptor(schema, "DiagramDiskTables")
                        .AddColumns(
                            new ColumnDescriptor() { Name = "ApplicationEnvironmentId", Type = SqlTypeDescriptor.BigInt(), AllowNull = false },
                            new ColumnDescriptor() { Name = "DiagramsId", Type = SqlTypeDescriptor.BigInt(), AllowNull = false },
                            new ColumnDescriptor() { Name = "DiskTableId", Type = SqlTypeDescriptor.BigInt(), AllowNull = false },
                            new ColumnDescriptor() { Name = "X", Type = SqlTypeDescriptor.Decimal(12, 5), AllowNull = false },
                            new ColumnDescriptor() { Name = "Y", Type = SqlTypeDescriptor.Decimal(12, 5), AllowNull = false }
                        )
                        .AddPrimaryKeys(
                            new IndexedColumnReferenceDescriptor() { Name = "ApplicationEnvironmentId" },
                            new IndexedColumnReferenceDescriptor() { Name = "DiagramsId" },
                            new IndexedColumnReferenceDescriptor() { Name = "DiskTableId" }
                        )
                        .AddForeignKey(null, schema, "ApplicationEnvironments", c =>
                        {
                            c.AddLocalColumns("ApplicationEnvironmentId")
                            .AddRemoteColumns("Id")
                            //.DeleteCascade(true)
                            //.UpdateCascade(true)
                            ;
                        })
                        .AddForeignKey(null, schema, "Diagrams", c =>
                        {
                            c.AddLocalColumns("DiagramsId")
                            .AddRemoteColumns("Id")
                            //.DeleteCascade(true)
                            //.UpdateCascade(true)
                            ;
                        })
                        .AddForeignKey(null, schema, "DiskTables", c =>
                        {
                            c.AddLocalColumns("DiskTableId")
                            .AddRemoteColumns("Id")
                            //.DeleteCascade(true)
                            //.UpdateCascade(true)
                            ;
                        })

                );


        }

    }

}

