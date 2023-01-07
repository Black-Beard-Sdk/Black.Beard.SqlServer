using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics.Metrics;
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

            if (string.IsNullOrEmpty(schema))
                schema = "dbo";

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

                    new TableDescriptor(schema, "Hosts")
                        .AddColumn("Id", SqlTypeDescriptor.IdentityInt(1, 1), false)
                        .AddColumn("Name", SqlTypeDescriptor.Varchar(70), false)
                        .AddColumn("Instance", SqlTypeDescriptor.Varchar(170), true)
                        .AddColumn("Kind", SqlTypeDescriptor.SmallInt(), false)
                        .AddPrimaryKeys("Id"),

                    new TableDescriptor(schema, "Connections")
                        .AddColumn("Id", SqlTypeDescriptor.IdentityBigInt(1, 1), false)
                        .AddColumn("Name", SqlTypeDescriptor.Varchar(70), false)
                        .AddColumn("HostId", SqlTypeDescriptor.Int(), false)
                        .AddColumn("Userid", SqlTypeDescriptor.Varchar(170), true)
                        .AddColumn("Password", SqlTypeDescriptor.Varchar(170), true)
                        .AddPrimaryKeys("Id")
                        .AddForeignKey(null, schema, "Hosts", c =>
                        {
                            c.AddLocalColumns("HostId")
                             .AddRemoteColumns("Id")
                            ;
                        }),

                    new TableDescriptor(schema, "Applications")
                        .AddColumn("Id", SqlTypeDescriptor.IdentityBigInt(1, 1), false)
                        .AddColumn("Name", SqlTypeDescriptor.Varchar(70), false)
                        .AddPrimaryKeys("Id"),

                    new TableDescriptor(schema, "ApplicationEnvironments")
                        .AddColumn("Id", SqlTypeDescriptor.IdentityBigInt(1, 1), false)
                        .AddColumn("Name", SqlTypeDescriptor.Varchar(70), false)
                        .AddColumn("ApplicationId", SqlTypeDescriptor.BigInt(), false)
                        .AddColumn("ConnectionId", SqlTypeDescriptor.BigInt(), false)
                        .AddPrimaryKeys("Id")
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
                        .AddColumn("Id", SqlTypeDescriptor.IdentityBigInt(1, 1), false)
                        .AddColumn("ApplicationEnvironmentId", SqlTypeDescriptor.BigInt(), false)
                        .AddColumn("TableSchema", SqlTypeDescriptor.Varchar(70), false)
                        .AddColumn("Name", SqlTypeDescriptor.Varchar(70), false)
                        .AddColumn("PartitionSchemeName", SqlTypeDescriptor.Varchar(160), true)
                        .AddPrimaryKeys("Id")
                        .AddForeignKey(null, schema, "ApplicationEnvironments", c =>
                        {
                            c.AddLocalColumns("ApplicationEnvironmentId")
                             .AddRemoteColumns("Id")
                            //.DeleteCascade(true)
                            //.UpdateCascade(true)
                            ;

                        }),

                    new TableDescriptor(schema, "DiskTablesIndex")
                        .AddColumn("Id", SqlTypeDescriptor.IdentityBigInt(1, 1), false)
                        .AddColumn("TableId", SqlTypeDescriptor.BigInt(), false)
                        .AddColumn("PrimaryKey", SqlTypeDescriptor.Bit(), false)
                        .AddColumn("Name", SqlTypeDescriptor.Varchar(70), false)
                        .AddColumn("Clustered", SqlTypeDescriptor.Bit(), false)
                        .AddColumn("Unique", SqlTypeDescriptor.Bit(), false)
                        .AddColumn("PartitionSchemeName", SqlTypeDescriptor.Varchar(60), true)
                        .AddColumn("PadIndex", SqlTypeDescriptor.Bit(), true)
                        .AddColumn("StatisticsNoRecompute", SqlTypeDescriptor.Bit(), true)
                        .AddColumn("AllowRowLocks", SqlTypeDescriptor.Bit(), true)
                        .AddColumn("AllowPageLocks", SqlTypeDescriptor.Bit(), true)
                        .AddColumn("OptimizeForSequentialKey", SqlTypeDescriptor.Bit(), true)
                        .AddPrimaryKeys("Id")
                        .AddForeignKey(null, schema, "DiskTables", c =>
                        {
                            c.AddLocalColumns("TableId");
                            c.AddRemoteColumns("Id");
                        }),

                    new TableDescriptor(schema, "DiskTablesColumns")
                        .AddColumn("Id", SqlTypeDescriptor.IdentityBigInt(1, 1), false)
                        .AddColumn("Table", SqlTypeDescriptor.BigInt(), false)
                        .AddColumn("Name", SqlTypeDescriptor.Varchar(70), false)
                        .AddColumn("Type", SqlTypeDescriptor.Varchar(70), false)
                        .AddColumn("Precision", SqlTypeDescriptor.Int(), false)
                        .AddColumn("Scale", SqlTypeDescriptor.Int(), false)
                        .AddColumn("AllowNull", SqlTypeDescriptor.Bit(), true)
                        .AddColumn("Caption", SqlTypeDescriptor.Varchar(160), true)
                        .AddColumn("DefaultValue", SqlTypeDescriptor.Varchar(160), true)
                        .AddPrimaryKeys("Id")
                        .AddForeignKey("FK_DiskTablesColumns_Table_2_DiskTables_Id", schema, "DiskTables", c =>
                        {
                            c.AddLocalColumns("Table");
                            c.AddRemoteColumns("Id");
                        }),

                    new TableDescriptor(schema, "Diagrams")
                        .AddColumn("Id", SqlTypeDescriptor.IdentityBigInt(1, 1), false)
                        .AddColumn("Name", SqlTypeDescriptor.Varchar(70), false)

                        .AddPrimaryKeys("Id"),

                    new TableDescriptor(schema, "DiagramDiskTables")
                        .AddColumn("Id", SqlTypeDescriptor.IdentityBigInt(1, 1), false)
                        .AddColumn("DiagramsId", SqlTypeDescriptor.BigInt(), false)
                        .AddColumn("DiskTableId", SqlTypeDescriptor.BigInt(), false)
                        .AddColumn("X", SqlTypeDescriptor.Decimal(12, 5), false)
                        .AddColumn("Y", SqlTypeDescriptor.Decimal(12, 5), false)
                        .AddPrimaryKeys("Id")
                        .AddForeignKey(null, schema, "DiskTables", c =>
                        {
                            c.AddLocalColumns("DiskTableId")
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

