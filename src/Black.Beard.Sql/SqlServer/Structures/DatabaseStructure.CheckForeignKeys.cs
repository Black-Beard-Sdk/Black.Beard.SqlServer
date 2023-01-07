using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using Bb.SqlServer.Structures.Dacpacs;
using Bb.SqlServer.Structures.Ddl;

namespace Bb.SqlServer.Structures
{


    public partial class DatabaseStructure
    {


        private void CheckForeignKeys(CheckContext ctx, List<TableDescriptor> t2)
        {

            foreach (var tableParent in Tables)
            {
                foreach (var foreignKey in tableParent.ForeignKeys)
                {

                    var tables = t2.Where(c => c.Name == foreignKey.RemoteColumns.TableName).ToList();
                    if (tables.Count == 0)
                        ctx.Add(foreignKey.RemoteColumns
                            , nameof(RemoteColumnReferenceListDescriptor.TableName)
                            , $"Missing table named {foreignKey.RemoteColumns.TableName}"
                            , LevelCheck.Error);

                    else
                    {

                        var schemas = tables.Select(c => c.Schema).ToList();
                        tables = tables.Where(c => c.Schema == foreignKey.RemoteColumns.Schema).ToList();
                        if (tables.Count == 0)
                            ctx.Add(foreignKey.RemoteColumns
                                , nameof(RemoteColumnReferenceListDescriptor.Schema)
                                , $"Table {foreignKey.RemoteColumns.TableName} exists but with different schema {string.Join(", ", schemas)}"
                                , LevelCheck.Error);

                        else if (tables.Count > 1)
                            ctx.Add(foreignKey.RemoteColumns
                               , nameof(RemoteColumnReferenceListDescriptor.Schema)
                               , $"Ambigues tables {foreignKey.RemoteColumns.Schema}.{foreignKey.RemoteColumns.TableName}"
                               , LevelCheck.Error);

                        else
                        {

                            var tableChild = tables[0];

                            if (foreignKey.LocalColumns.Count == foreignKey.RemoteColumns.Count)
                            {
                                for (int i = 0; i < foreignKey.LocalColumns.Count; i++)
                                {

                                    var c1 = tableParent.Columns.Where(c => c.Name == foreignKey.LocalColumns[i].Name).FirstOrDefault();
                                    var c2 = tableChild.Columns.Where(c => c.Name == foreignKey.RemoteColumns[i].Name).FirstOrDefault();

                                    if (c1 == null)
                                        ctx.Add(foreignKey.LocalColumns
                                            , "local reference column"
                                            , $"Column {foreignKey.LocalColumns[i].Name} is missing in the table {tableParent.Name}."
                                            , LevelCheck.Error);

                                    if (c2 == null)
                                        ctx.Add(foreignKey.RemoteColumns
                                            , "remote reference column"
                                            , $"Column {foreignKey.RemoteColumns[i].Name} is missing in the table {tableChild.Name}."
                                            , LevelCheck.Error);

                                    if (c1 != null && c2 != null)
                                    {
                                        if (c1.SqlType.SqlDataType.SqlLabel != c2.SqlType.SqlDataType.SqlLabel)
                                            ctx.Add(foreignKey
                                                , "Type"
                                                , $"Column {c1.Name} of type {c1.SqlType.SqlDataType.SqlLabel} and {c2.Name} of type {c2.SqlType.SqlDataType.SqlLabel} donst match."
                                                , LevelCheck.Error);
                                    }

                                }

                            }
                            else
                                ctx.Add(foreignKey
                                    , "Columns.Count"
                                    , $"tables {foreignKey.Name} haven't same count of column"
                                    , LevelCheck.Error);

                        }

                    }

                }
            }

        }

        private void CheckCascadesForeignKeys(CheckContext ctx, List<TableDescriptor> t2)
        {

            HashSet<string> keys = new HashSet<string>();

            foreach (var tableParent in Tables)
                foreach (var foreign in tableParent.ForeignKeys)
                    if (foreign.OnDeleteCascade || foreign.OnUpdateCascade)
                        if (!keys.Add(foreign.RemoteColumns.Schema + "." + foreign.RemoteColumns.TableName))
                            ctx.Add(foreign, nameof(foreign.OnDeleteCascade), $"Msg 1785, Level 16. Introducing FOREIGN KEY constraint '{foreign.Name}' on table '{foreign.RemoteColumns.TableName}' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.", LevelCheck.Error);

        }

    }

}