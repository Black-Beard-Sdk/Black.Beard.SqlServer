using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using Bb.SqlServer.Structures.Dacpacs;
using Bb.SqlServer.Structures.Ddl;

namespace Bb.SqlServer.Structures
{


    public partial class DatabaseStructure
    {


        public DatabaseStructure()
        {
            Tables = new TableListDescriptor();
            Schemas = new SchemaListDescriptor();
            FileGroups = new FileGroupListDescriptor();
        }

        public DatabaseStructure(params SqlServerDescriptor[] objects) : this()
        {

            foreach (var item in objects)
            {

                if (item is TableDescriptor o1)
                    Tables.Add(o1);

                else
                    throw new NotImplementedException(item.GetType().Name);

            }

        }

        public TableDescriptor? GetTable(string schema, string table)
        {
            return Tables.FirstOrDefault(c => c.Schema == schema && c.Name == table);
        }

        public IEnumerable<TableDescriptor> GetTable(string table)
        {
            return Tables.Where(c => c.Name == table).ToList();
        }

        public DatabaseStructure AddTables(params TableDescriptor[] tables)
        {
            Tables.AddRange(tables);
            return this;
        }

        public DatabaseStructure AddSchemas(params SchemaDescriptor[] schemas)
        {
            Schemas.AddRange(schemas);
            return this;
        }

        public DatabaseStructure AddFileGroup(params FileGroupDescriptor[] fileGroups)
        {
            FileGroups.AddRange(fileGroups);
            return this;
        }

        public DacPackage GenerateDacpac(string dacpacName, DacpacContext ctx)
        {

            var converter = new ConvertStructureToDacPac(dacpacName, this, ctx);
            var result = converter.GenerateDacpac();

            return result;

        }

        public ScriptUpdateDatabase GetScriptGenerator(ScriptContext ctx, string pathDatabaseFolder = null)
        {

            var result = new ScriptUpdateDatabase(ctx, this)
            {
                PathDatabaseFolder = pathDatabaseFolder
            };

            return result;

        }
              
        public TableListDescriptor Tables { get; }

        public SchemaListDescriptor Schemas { get; }

        public FileGroupListDescriptor FileGroups { get; }

    }

}