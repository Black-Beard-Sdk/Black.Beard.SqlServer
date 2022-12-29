using System.Text;
using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{

    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
    /// </summary>
    public class DacRelationship : DacListOfModel<DacEntry>
    {

        public DacRelationship(string key = null)
            : base(key ?? "Relationship")
        {

        }

        public DacRelationship ColumnGeography(string @namespace, string table, string columnName, bool isNullable, int length, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "sys.geography", isNullable, 0, length, 0, action);
        }

        public DacRelationship ColumnGeometry(string @namespace, string table, string columnName, bool isNullable, int length, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "sys.geometry", isNullable, 0, length, 0, action);
        }

        public DacRelationship ColumnHierarchyid(string @namespace, string table, string columnName, bool isNullable, int length, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "sys.hierarchyid", isNullable, 0, length, 0, action);
        }

        public DacRelationship ColumnSysname(string @namespace, string table, string columnName, bool isNullable, int length, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "sys.sysname", isNullable, 0, length, 0, action);
        }

        public DacRelationship ColumnBinary(string @namespace, string table, string columnName, bool isNullable, int length, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "binary", isNullable, 0, length, 0, action);
        }

        public DacRelationship ColumnVarBinary(string @namespace, string table, string columnName, bool isNullable, int length, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "varbinary", isNullable, 0, length, 0, action);
        }

        public DacRelationship ColumnImage(string @namespace, string table, string columnName, bool isNullable, int length, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "image", isNullable, 0, length, 0, action);
        }

        public DacRelationship ColumnBit(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "bit", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnDatetime(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "datetime", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnTime7(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "datetime", isNullable, 0, 0, 7, action);
        }

        public DacRelationship ColumnTimestamp(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "timestamp", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnTime(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "time", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnSmallDatetime(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "smalldatetime", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnDatetime2(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "datetime2", isNullable, 0, 0, 0, action);
        }

        public DacRelationship Columndecimal(string @namespace, string table, string columnName, bool isNullable, int precision = 18, int scale = 5, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "decimal", isNullable, precision, scale, 0, action);
        }

        public DacRelationship ColumnNumeric(string @namespace, string table, string columnName, bool isNullable, int precision = 18, int scale = 5, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "numeric", isNullable, precision, 0, scale, action);
        }

        public DacRelationship ColumnDatetimeoffset(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "datetimeoffset", isNullable, 0, 0, 7, action);
        }

        public DacRelationship ColumnDate(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "date", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnInt(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "int", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnTinyInt(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "tinyint", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnSmallInt(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "smallint", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnBigInt(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "bigint", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnFloat(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "float", isNullable, 53, 0, 0, action);
        }

        public DacRelationship ColumnReal(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "real", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnRowVersion(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "rowversion", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnChar(string @namespace, string table, string columnName, bool isNullable, int length, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "char", isNullable, 0, length, 0, action);
        }

        public DacRelationship ColumnMoney(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "money", isNullable, 0, 10, 0, action);
        }

        public DacRelationship ColumnSmallMoney(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "smallmoney", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnSqlVariant(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "sql_variant", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnNChar(string @namespace, string table, string columnName, bool isNullable, int length, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "nchar", isNullable, 0, length, 0, action);
        }

        public DacRelationship ColumnText(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "text", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnNText(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "ntext", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnXml(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "xml", isNullable, 0, 0, 0, action);
        }

        public DacRelationship ColumnVarchar(string @namespace, string table, string columnName, bool isNullable, int length, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "varchar", isNullable, 0, length, 0, action);
        }

        public DacRelationship ColumnNVarchar(string @namespace, string table, string columnName, bool isNullable, int length, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "nvarchar", isNullable, 0, length, 0, action);
        }

        public DacRelationship ColumnUniqueIdentifier(string @namespace, string table, string columnName, bool isNullable, Action<DacSqlSimpleColumn> action = null)
        {
            return Column(@namespace, table, columnName, "uniqueidentifier", isNullable, 0, 0, 0, action);
        }


        public DacRelationship Column(string @namespace, string table, string columnName, string type, bool isNullable, int precision = 0, int length = 0, int scale = 0, Action<DacSqlSimpleColumn> action = null)
        {

            if (string.IsNullOrEmpty(@namespace))
                @namespace = "dbo";

            Entry(e =>
            {

                e.Column($"[{@namespace}].[{table}].[{columnName}]", c =>
                {

                    if (!isNullable)
                        c.Property("IsNullable", "False");

                    c.Relationship(RelationshipNamePropertyValue.TypeSpecifier, r =>
                    {

                        r.Entry(e2 =>
                        {
                            e2.Element(ElementTypePropertyValue.SqlTypeSpecifier, e3 =>
                            {

                                if (precision > 0)
                                    e3.Property("Precision", precision.ToString());

                                if (scale > 0)
                                    e3.Property("scale", precision.ToString());

                                var t = type.ToLower();
                                if (t == "varchar" || t == "varbinay" || t == "nvarchar")
                                {
                                    if (length > 0)
                                        e3.Property("Length", length.ToString());
                                    else
                                        e3.Property("IsMax", "True");
                                }

                                e3.Relationship(RelationshipNamePropertyValue.Type, r2 =>
                                {
                                    r2.Entry(e4 =>
                                    {
                                        e4.References(Enquote(type), "BuiltIns");
                                    });
                                });

                            });
                        });

                    })
                    ;

                    if (action != null)
                        action(c);

                });

            });

            return this;

        }

        public DacRelationship Entry(Action<DacEntry> action)
        {
            var e = new DacEntry();
            Add(e);
            action(e);
            return this;
        }

        public RelationshipNamePropertyValue Name { get; set; }

        public override XElement Serialize()
        {
            var xml = new XElement(XName.Get(Key));
            xml.Add(new XAttribute(XName.Get("Name"), Name.Value));

            foreach (var item in this)
                xml.Add(item.Serialize());

            return xml;
        }

    }



}
