using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using Bb.SqlServer.Structures.Dacpacs;
using Bb.SqlServer.Structures.Ddl;

namespace Bb.SqlServer.Structures
{


    public partial class DatabaseStructure
    {

        
        public DatabaseStructure Check(CheckContext ctx)
        {

            var tables = Tables.ToList();

            CheckNames(ctx);
            CheckForeignKeys(ctx, tables);
            CheckCascadesForeignKeys(ctx, tables);

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

    }

    public enum LevelCheck
    {
        Verbose = 0,
        Warning = 1,
        Error = 2,
    }

}