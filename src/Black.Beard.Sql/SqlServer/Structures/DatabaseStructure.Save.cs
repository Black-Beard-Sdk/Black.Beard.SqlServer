using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using Bb.Extended;
using Bb.SqlServer.Bulks;
using Bb.SqlServer.Structures.Dacpacs;
using Bb.SqlServer.Structures.Ddl;
using Bb.SqlServerStructures;

namespace Bb.SqlServer.Structures
{


    public partial class DatabaseStructure
    {



        public string Application { get; set; }

        public string Environment { get; set; }



        public void Save(ConnectionStringSetting setting)
        {

            var dbo = "dbo";
            var sql = setting.CreateProcessor();

            using (var cnx = sql.GetConnexion())
            using (var transaction = cnx.BeginTransaction())
            {

                var bulk = setting.GetBulkLoader();
                if (bulk != null)
                {


                    var DiskTables = new MemoryDbDataReader<TableDescriptor>(
                          this.Tables
                        , ("Id",  c => c.Id)
                        , ("ApplicationEnvironmentId", c => 0)
                        , ("TableSchema", c => c.Schema)
                        , ("Name", c => c.Name)
                        , ("PartitionSchemeName", c => c.PartitionSchemeName)
                        );
                    bulk.Write(DiskTables, dbo, "DiskTables", sql, transaction);


                    var DiskTablesIndex = new MemoryDbDataReader<(TableDescriptor, IndexDescriptor)>(
                         this.Indexes
                       , ("Id", c => c.Item2.Id)
                       , ("TableId", c => c.Item1.Name)
                       , ("PrimaryKey", c => c.Item2.IsPrimaryKey)
                       , ("Name", c => c.Item2.Name)
                       , ("Clustered", c => c.Item2.Clustered)
                       , ("Unique", c => c.Item2.Unique)
                       , ("PartitionSchemeName", c => c.Item2.PartitionSchemeName)
                       , ("PadIndex", c => c.Item2.Properties.PadIndex)
                       , ("StatisticsNoRecompute", c => c.Item2.Properties.StatisticsNorecompute)
                       , ("AllowRowLocks", c => c.Item2.Properties.AllowRowLocks)
                       , ("AllowPageLocks", c => c.Item2.Properties.AllowPageLocks)
                       , ("OptimizeForSequentialKey", c => c.Item2.Properties.OptimizeForSequentialKey)
                       );
                    bulk.Write(DiskTablesIndex, dbo, "DiskTablesIndex", sql, transaction);


                    var DiskTablesColumns = new MemoryDbDataReader<(TableDescriptor, ColumnDescriptor)>(
                         this.Columns
                       , ("Id", c => c.Item2.Id)
                       , ("TableId", c => c.Item1.Name)
                       , ("Name", c => c.Item2.Name)
                       , ("Type", c => c.Item2.SqlType.SqlDataType.SqlLabel)
                       , ("Precision", c => c.Item2.SqlType.Argument1)
                       , ("Scale", c => c.Item2.SqlType.Argument2)
                       , ("AllowNull", c => c.Item2.AllowNull)
                       , ("Caption", c => c.Item2.Caption)
                       , ("DefaultValue", c => c.Item2.DefaultValue)
                       );
                    bulk.Write(DiskTablesColumns, dbo, "DiskTablesColumns", sql, transaction);

                    
                    //var Diagrams = new MemoryDbDataReader<DiagramModel>(
                    //     this.Diagrams
                    //   , ("Id", c => 0)
                    //   , ("Name", c => 0)
                    //   );
                    //bulk.Write(Diagrams, dbo, "Diagrams", sql, transaction);


                    //var DiagramDiskTables = new MemoryDbDataReader<DiagramDiskTableReferenceModel>(
                    //     this.DiagramDiskTables
                    //   , ("Id", c => 0)
                    //   , ("DiagramsId", c => 0)
                    //   , ("DiskTableId", c => 0)
                    //   , ("X", c => 0)
                    //   , ("Y", c => 0)
                    //   );
                    //bulk.Write(DiagramDiskTables, dbo, "DiagramDiskTables", sql, transaction);


                }


                transaction.Commit();
            }

        }


    }


}