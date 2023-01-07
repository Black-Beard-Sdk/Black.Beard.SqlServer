using Bb.Dacpacs;
using Bb.SqlServer.Structures;
using System.Data;
using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{

    public class ConvertStructureToDacPac
    {

        public ConvertStructureToDacPac(string dacpacName, DatabaseStructure db, DacpacContext ctx)
        {

            if (string.IsNullOrEmpty(dacpacName))
                throw new ArgumentNullException("dacpacName");

            _db = db;
            _pack = new DacPackage()
            {
                Name = dacpacName,
            };
            _model = _pack.Schema;
            _ctx = ctx;
        }

        public DacPackage GenerateDacpac()
        {

            foreach (var table in _db.Tables)
                VisitKeys(table);

            foreach (var table in _db.Tables)
            {

                _model.Table(table.Schema, _ctx.ReplaceVariables(table.Name), table.PartitionSchemeName, t =>
                {

                    foreach (var column in table.Columns)
                    {
                        VisitColumn(table, column, t);
                    }

                });

            }


            foreach (var table in _db.Tables)
                VisitIndexes(table);


            foreach (var table in _db.Tables)
            {

                foreach (var foreignKeys in table.ForeignKeys)
                {
                    var remote = foreignKeys.RemoteColumns;
                    Visit(
                        table.Schema, foreignKeys.Name,
                        table.Name, foreignKeys.LocalColumns.Select(c => c.Name).ToArray(),
                        remote.TableName, remote.Select(c => c.Name).ToArray()
                    );
                }

            }

            if (this._ctx.CreateFileGroupPolicy != PolicyEnum.None)
                VisitFilegroups();

            if (this._ctx.CreateSchemaPolicy != PolicyEnum.None)
                VisitSchemas();

            return _pack;

        }

        private void ComputeFilegroups()
        {

            HashSet<string> keys = new HashSet<string>();
            foreach (var table in _db.Tables)
            {

                keys.Add(table.PartitionSchemeName);

                foreach (var key in table.Keys)
                    keys.Add(key.PartitionSchemeName);

                foreach (var index in table.Indexes)
                    keys.Add(index.PartitionSchemeName);

            }

        }

        private void ResolveFilegroups()
        {

            HashSet<string> keys = new HashSet<string>();
            foreach (var table in _db.Tables)
            {

                keys.Add(table.PartitionSchemeName);

                foreach (var key in table.Keys)
                    keys.Add(key.PartitionSchemeName);

                foreach (var index in table.Indexes)
                    keys.Add(index.PartitionSchemeName);

            }

            foreach (var name in keys)
                _db.FileGroups.AddIfNotExists(name);

        }

        private void VisitFilegroups()
        {

            ResolveFilegroups();

            foreach (var item in this._db.FileGroups)
                if (EvaluateFileGroup(item.Name))
                    _pack.Schema.FileGroup(item.Name);

        }

        private bool EvaluateFileGroup(string partitionSchemeName)
        {

            if (partitionSchemeName.ToUpper() == "PRIMARY")
                return false;

            PolicyEnum policy = this._ctx.CreateFileGroupPolicy;

            if (policy == PolicyEnum.Create)
                return true;
                    
            if (policy == PolicyEnum.CreateIfNotExists && _ctx.TargetState != null)
                return this._ctx.TargetState.FileGroups.Exists(c => c.Name == partitionSchemeName);

            return false;


        }

        private void VisitSchemas()
        {

            ResolveSchemas();

            foreach (var item in _db.Schemas)
                if (EvaluateSchemas(item.Name))
                    _pack.Schema.Schema(item.Name, item.Parent);

        }

        private bool EvaluateSchemas(string schema)
        {

            if (schema.ToUpper() == "DBO")
                return false;

            PolicyEnum policy = this._ctx.CreateSchemaPolicy;

            if (policy == PolicyEnum.Create)
                return true;

            if (policy == PolicyEnum.CreateIfNotExists && _ctx.TargetState != null)
                return this._ctx.TargetState.Schemas.Exists(c => c.Name == schema);

            return false;


        }

        private void ResolveSchemas()
        {
            HashSet<string> schemas = new HashSet<string>();

            foreach (var table in _db.Tables)
                schemas.Add(table.Schema);

            foreach (var schema in schemas)
                _db.Schemas.AddIfNotExists(schema, string.Empty);
        }

        private void VisitKeys(TableDescriptor table)
        {

            foreach (var key in table.Keys)
            {

                var columns = key.Select(c => (c.Name, c.Sort)).ToArray();

                if (columns.Length > 0)
                    _pack.Schema.PrimaryKey(table.Schema, table.Name, table.PartitionSchemeName, key.Clustered, columns, key.Properties);

            }

        }

        private void VisitIndexes(TableDescriptor table)
        {

            string onpartitionSchemeName = table.PartitionSchemeName;

            foreach (var index in table.Indexes)
            {

                var columns = index.Select(c => (c.Name, c.Sort)).ToArray();

                if (columns.Length > 0)
                    _pack.Schema.Index(index.Name, table.Schema, table.Name, onpartitionSchemeName, index.Clustered, index.Unique, columns, index.Properties);
            }

        }

        private void VisitColumn(TableDescriptor currentTable, ColumnDescriptor column, DacRelationship tableSource)
        {

            string tableName = currentTable.Name;
            string @namespace = currentTable.Schema;
            var allowNull = column.AllowNull;
            string columnName = _ctx.ReplaceVariables(column.Name);

            Action<DacSqlSimpleColumn> action = c =>
            {

                if (!string.IsNullOrEmpty(column.Caption))
                {
                    _pack.Schema.Model.SqlDescription(@namespace, tableName, columnName, column.Caption); ;
                }

                //if (node.Unique && !currentTable.Keys.Any(c => c.Name == node.Name))
                //{
                //    //field.IsUnique = true;
                //    Stop();
                //}

                if (column.DefaultValue != null && column.DefaultValue != DBNull.Value)
                {
                    Stop();
                }

                //if (!string.IsNullOrEmpty(node.Expression))
                //{
                //    Stop();
                //}

                //if (!string.IsNullOrEmpty(node.Prefix))
                //{
                //    Stop();
                //}

                //if (node.AutoIncrement)
                //{
                //    Stop();
                //}

            };

            var t = column.SqlType;
            var typename = t.SqlDataType.SqlLabel.ToUpper();

            switch (typename)
            {


                case SqlTypeDescriptor._BINARY:
                    tableSource.ColumnBinary(@namespace, tableName, columnName, allowNull, t.Argument1.Value, action);
                    break;

                case SqlTypeDescriptor._DATETIME:
                    tableSource.ColumnDatetime(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._DATETIMEOFFSET:
                    tableSource.ColumnDatetimeoffset(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._VARCHAR:
                    tableSource.ColumnVarchar(@namespace, tableName, columnName, allowNull, t.Argument1.Value, action);
                    break;

                case SqlTypeDescriptor._UNIQUEIDENTIFIER:
                    tableSource.ColumnUniqueIdentifier(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._SMALLINT:
                    tableSource.ColumnSmallInt(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._INT:
                    tableSource.ColumnInt(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._TINYINT:
                    tableSource.ColumnTinyInt(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._BIGINT:
                    tableSource.ColumnBigInt(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._BIT:
                    tableSource.ColumnBit(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._NVARCHAR:
                    tableSource.ColumnNVarchar(@namespace, tableName, columnName, allowNull, t.Argument1.Value, action);
                    break;

                case SqlTypeDescriptor._NCHAR:
                    tableSource.ColumnNChar(@namespace, tableName, columnName, allowNull, t.Argument1.Value, action);
                    break;

                case SqlTypeDescriptor._CHAR:
                    tableSource.ColumnChar(@namespace, tableName, columnName, allowNull, t.Argument1.Value, action);
                    break;

                case SqlTypeDescriptor._REAL:
                    tableSource.ColumnReal(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._FLOAT:
                    tableSource.ColumnFloat(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._DECIMAL:
                    tableSource.ColumnNumeric(@namespace, tableName, columnName, allowNull, 13, 2, action);
                    break;

                case SqlTypeDescriptor._NUMERIC:
                    tableSource.ColumnNumeric(@namespace, tableName, columnName, allowNull, 13, 2, action);
                    break;

                case SqlTypeDescriptor._SMALLMONEY:
                    tableSource.ColumnSmallMoney(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._MONEY:
                    tableSource.ColumnMoney(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._DATE:
                    tableSource.ColumnDate(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._DATETIME2:
                    tableSource.ColumnDatetime2(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._SMALLDATETIME:
                    tableSource.ColumnSmallDatetime(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._TIME:
                    tableSource.ColumnTime(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._TIMESTAMP:
                    tableSource.ColumnTimestamp(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._TEXT:
                    tableSource.ColumnText(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._NTEXT:
                    tableSource.ColumnNText(@namespace, tableName, columnName, allowNull, action);
                    break;

                case SqlTypeDescriptor._VARBINARY:
                    tableSource.ColumnVarBinary(@namespace, tableName, columnName, allowNull, t.Argument1.Value, action);
                    break;

                case SqlTypeDescriptor._IMAGE:
                    tableSource.ColumnImage(@namespace, tableName, columnName, allowNull, t.Argument1.Value, action);
                    break;

                //case SqlServer._ROWVERSION:
                //    tableSource.ColumnNumeric(@namespace, tableName, columnName, allowNull, 13, 2, action);
                //    break;

                //case SqlServer._SQL_VARIANT:
                //    tableSource.ColumnNumeric(@namespace, tableName, columnName, allowNull, 13, 2, action);
                //    break;

                //case SqlServer._XML:
                //    tableSource.ColumnNumeric(@namespace, tableName, columnName, allowNull, 13, 2, action);
                //    break;

                //case SqlServer._GEOMETRY:
                //    tableSource.ColumnNumeric(@namespace, tableName, columnName, allowNull, 13, 2, action);
                //    break;

                //case SqlServer._GEOGRAPHY:
                //    tableSource.ColumnNumeric(@namespace, tableName, columnName, allowNull, 13, 2, action);
                //    break;

                //    switch (node.DateTimeMode)
                //    {
                //case DataSetDateTime.Local:
                //            Stop();
                //            t.ColumnDatetime(@namespace, name, node.Name, allowNull, action);
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

                //case SqlServer._FILESTREAM:
                //    tableSource.ColumnStream(@namespace, tableName, columnName, allowNull, action);
                //    break;

                default:
                    Stop();
                    break;
            }

        }

        private void Visit(string schema, string relationName, string parentTableName, string[] parentColumns, string childTableName, string[] childColumns)
        {

            _pack.Schema.ForeignKey(
                  schema, relationName
                , parentTableName, parentColumns
                , childTableName, childColumns
            );

        }


        [System.Diagnostics.DebuggerHidden]
        [System.Diagnostics.DebuggerNonUserCode]
        [System.Diagnostics.DebuggerStepThrough]
        protected static void Stop()
        {
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
        }

        private readonly DatabaseStructure _db;
        private readonly DacDataSchemaModel _model;
        private readonly DacPackage _pack;
        private readonly DacpacContext _ctx;

    }



}