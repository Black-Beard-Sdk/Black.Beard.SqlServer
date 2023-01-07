using Bb.SqlServer.Structures.Dacpacs;
using Bb.SqlServer.Structures.Ddl;
using Bb.SqlServerStructures;
using System.Text;

namespace Bb.SqlServer.Structures
{

    public class ScriptUpdateDatabase
    {
        private readonly ScriptContext _ctx;
        private readonly StringBuilder _sb;
        private readonly DatabaseStructure _structure;

        public ScriptUpdateDatabase(ScriptContext ctx, DatabaseStructure structure)
        {
            _ctx = ctx;
            _sb = new StringBuilder();
            _structure = structure;
        }

        public string PathDatabaseFolder { get; internal set; }

        public void GenerateScript()
        {

            _sb.Clear();
            Writer writer = new Writer(_sb);

            if (_ctx.CreateDatabase)
            {
                var c = new CreateDatabase(writer, _ctx, this.PathDatabaseFolder);
                c.Parse(this._structure);
            }

            var i = new CreateFilegroup(writer, _ctx, this.PathDatabaseFolder);
            i.Parse(this._structure);

            var j = new CreateSchemas(writer, _ctx); 
            j.Parse(this._structure);

            var d = new CreateTables(writer, _ctx);
            d.Parse(this._structure);

            var e = new CreatePrimaryKeys(writer, _ctx);
            e.Parse(this._structure);



            var f = new DropIndexes(writer, _ctx);
            f.Parse(this._structure);

            var g = new CreateIndexes(writer, _ctx);
            g.Parse(this._structure);

            var h = new CreateForeignKeys(writer, _ctx);
            h.Parse(this._structure);

        }

        public void WriteOnDisk(string filename, bool DeleteIfExists = false)
        {

            if (_sb.Length == 0)
                GenerateScript();

            var file = new FileInfo(filename);

            if (DeleteIfExists)
            {
                if (file.Exists)
                    file.Delete();
            }
            else
                throw new IOException($" File {filename} allready exists. please delete before.");

            File.AppendAllText(file.FullName, _sb.ToString());

        }

        public void Execute(ConnectionStringSetting settings)
        {

            if (_sb.Length == 0)
                GenerateScript();

            var processor = settings.CreateProcessor();

            processor.ExecuteNonQuery(_sb.ToString());

        }


    }

}