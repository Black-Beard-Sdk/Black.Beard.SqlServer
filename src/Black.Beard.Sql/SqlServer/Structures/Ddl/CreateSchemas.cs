using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Ddl
{
    public class CreateSchemas : DdlBase
    {

        public CreateSchemas(Writer writer, ScriptContext ctx) : base(writer, ctx)
        {

        }


        public void Parse(DatabaseStructure structure)
        {

            CommentLine("Create schemas");

            Parse(structure.Schemas.Where(c => c.Name != "dbo").ToList());

        }


        public void Parse(List<SchemaDescriptor> schemas)
        {
            foreach (var schema in schemas)
            {

                AppendEndLine("CREATE SCHEMA ", AsLabel(this._ctx.ReplaceVariables(schema.Name)));
                
                if (!string.IsNullOrEmpty(schema.Parent))
                    using (Indent())
                    {
                        AppendEndLine("AUTHORIZATION ", AsLabel(this._ctx.ReplaceVariables(schema.Parent)));
                    }

                Go();

            }



        }


    }


}