
using Bb.Dacpacs;
using System.Data;

namespace Bb.SqlServer.Structures.Dacpacs
{

    public class DacpacToDataSetVisitor
    {

        public DacpacToDataSetVisitor(DataSetToCodeDomOption options = null)
        {

            if (options == null)
                options = new DataSetToCodeDomOption();

            this._options = options;

            _pack = new DacPackage();

        }

        public DacPackage Package { get => _pack; }

        public void Visit(DataSet node)
        {

            foreach (DataTable item in node.Tables)
                VisitKeys(item);

            foreach (DataTable item in node.Tables)
                Visit(item);

            foreach (DataRelation item in node.Relations)
                Visit(item);

        }

        private void Visit(DataTable table)
        {
            if (table == null)
                return;

            _pack.Schema.Table(table.Namespace, table.TableName, "PRIMARY", t =>
             {

                 foreach (DataColumn item in table.Columns)
                     Visit(item, t);

             });

            if (table.ExtendedProperties.ContainsKey("Caption"))
            {
                var c = table.ExtendedProperties["Caption"]?.ToString() ?? String.Empty;
                this._pack.Schema.Model.SqlDescription(table.Namespace, table.TableName, c);
            }

        }

        public void Visit(DataColumn node, DacRelationship t)
        {

            var table = node.Table;
            string name = table.TableName;
            string @namespace = table.Namespace;
            var allowNull = node.AllowDBNull;
            Action<DacSqlSimpleColumn> action = c =>
            {

                if (!string.IsNullOrEmpty(node.Caption))
                {
                    this._pack.Schema.Model.SqlDescription(@namespace, name, node.ColumnName, node.Caption); ;
                }

                if (node.Unique && !node.Table.PrimaryKey.Any(c => c.ColumnName == node.ColumnName))
                {
                    //field.IsUnique = true;
                    Stop();
                }

                if (node.DefaultValue != null && node.DefaultValue != DBNull.Value)
                {
                    Stop();
                }

                if (!string.IsNullOrEmpty(node.Expression))
                {
                    Stop();
                }

                if (!string.IsNullOrEmpty(node.Prefix))
                {
                    Stop();
                }

                if (node.AutoIncrement)
                {
                    Stop();
                }

            };

            var typename = node.DataType.Name.ToLower();

            switch (typename)
            {

                case "timespan":
                    Stop();
                    break;

                case "byte[]":
                    Stop();
                    t.ColumnBinary(@namespace, name, node.ColumnName, allowNull, node.MaxLength, action);
                    break;

                case "datetime":
                    t.ColumnDatetime(@namespace, name, node.ColumnName, allowNull, action);
                    break;

                //    switch (node.DateTimeMode)
                //    {
                //case DataSetDateTime.Local:
                //            Stop();
                //            t.ColumnDatetime(@namespace, name, node.ColumnName, allowNull, action);
                //            break;
                //        case DataSetDateTime.Unspecified:
                //            Stop();
                //            break;
                //        case DataSetDateTime.UnspecifiedLocal:
                //            Stop();
                //            break;
                //        case DataSetDateTime.Utc:
                //            Stop();
                //            break;
                //        default:
                //            Stop();
                //            break;
                //    }

                case "datetimeoffset":
                    Stop();
                    t.ColumnDatetimeoffset(@namespace, name, node.ColumnName, allowNull, action);
                    break;

                case "string":
                    t.ColumnVarchar(@namespace, name, node.ColumnName, allowNull, node.MaxLength, action);
                    break;

                case "guid":
                    t.ColumnUniqueIdentifier(@namespace, name, node.ColumnName, allowNull, action);
                    break;

                case "uint16":
                case "int16":
                    Stop();
                    t.ColumnTinyInt(@namespace, name, node.ColumnName, allowNull, action);
                    break;

                case "int32":
                case "uint32":
                    Stop();
                    t.ColumnInt(@namespace, name, node.ColumnName, allowNull, action);
                    break;

                case "uint64":
                case "int64":
                    Stop();
                    t.ColumnBigInt(@namespace, name, node.ColumnName, allowNull, action);
                    break;

                case "boolean":
                    Stop();
                    t.ColumnBit(@namespace, name, node.ColumnName, allowNull, action);
                    break;

                case "char":
                    Stop();
                    t.ColumnNChar(@namespace, name, node.ColumnName, allowNull, 1, action);
                    break;

                case "byte":
                    t.ColumnBinary(@namespace, name, node.ColumnName, allowNull, 1, action);
                    Stop();
                    break;

                case "single":
                    Stop();
                    t.ColumnReal(@namespace, name, node.ColumnName, allowNull, action);
                    break;

                case "double":
                    t.ColumnFloat(@namespace, name, node.ColumnName, allowNull, action);
                    break;

                case "decimal":
                    Stop();
                    t.ColumnNumeric(@namespace, name, node.ColumnName, allowNull, 13, 2, action);
                    break;

                default:
                    Stop();
                    break;
            }

        }

        private void VisitKeys(DataTable table)
        {
            _pack.Schema.PrimaryKey(table.Namespace, table.TableName, "PRIMARY", true, table.PrimaryKey.Select(c => (c.ColumnName, SortIndex.Ascending)).ToArray(), null);
        }

        private void Visit(DataRelation node)
        {

            _pack.Schema.ForeignKey(node.ParentTable.Namespace, node.RelationName
                    , node.ParentTable.TableName, node.ParentColumns.Select(c => c.ColumnName).ToArray()
                    , node.ChildTable.TableName, node.ChildColumns.Select(c => c.ColumnName).ToArray()
                );

        }

        private readonly DataSetToCodeDomOption _options;
        private readonly DacPackage _pack;

        [System.Diagnostics.DebuggerHidden]
        [System.Diagnostics.DebuggerNonUserCode]
        [System.Diagnostics.DebuggerStepThrough]
        protected static void Stop()
        {
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
        }

    }

}
