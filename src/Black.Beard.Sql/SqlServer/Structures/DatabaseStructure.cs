﻿using System.IO.Compression;
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


        #region Checks

        public DatabaseStructure Check(CheckContext ctx)
        {

            var t2 = Tables.ToList();

            CheckNames(ctx);
            CheckForeignKeys(ctx, t2);

            return this;

        }

        private void CheckNames(CheckContext ctx)
        {

            foreach (var table in Tables)
            {

                CheckName(table, table.Name, ctx, nameof(TableDescriptor.Name), $"The table {table.Schema}.{table.Name}");

                foreach (var column in table.Columns)
                    CheckName(column, column.Name, ctx, nameof(ColumnDescriptor.Name), $"the column {column.Name} of {table.Schema}.{table.Name}");

                foreach (var index in table.Indexes)
                    CheckName(index, index.Name, ctx, nameof(IndexDescriptor.Name), $"{index.Name} of the table {table.Schema}.{table.Name}");
                
                foreach (var foreignKey in table.ForeignKeys)
                    CheckName(foreignKey, foreignKey.Name, ctx, nameof(ForeignKeyDescriptor.Name), $"{foreignKey.Name} table {table.Schema}.{table.Name}");

            }
        }

        private void CheckName(object obj, string value, CheckContext ctx, string nameOfProperty, string errorDescriptionv2)
        {

            if (value.Length > 128)
                ctx.Add(obj, nameOfProperty, errorDescriptionv2.Trim() + " must be less than 128 characters.", LevelCheck.Error);

            if (!char.IsLetter(value[0]) && value[0] != '_' && value[0] != '@')
                ctx.Add(obj, nameOfProperty, errorDescriptionv2.Trim() + " the first character me be '_', '@' or a letter as defined by the Unicode Standard 3.2. The Unicode definition of letters includes Latin characters from a through z, from A through Z, and also letter characters from other languages.", LevelCheck.Error);


        }

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
                                        if (c1.Type.Type.SqlLabel != c2.Type.Type.SqlLabel)
                                            ctx.Add(foreignKey
                                                , "Type"
                                                , $"Column {c1.Name} of type {c1.Type.Type.SqlLabel} and {c2.Name} of type {c2.Type.Type.SqlLabel} donst match."
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

        #endregion Checks


    }

    public enum LevelCheck
    {
        Verbose = 0,
        Warning = 1,
        Error = 2,
    }

}