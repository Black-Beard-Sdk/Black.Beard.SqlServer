using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

// https://dataedo.com/kb/query/sql-server
namespace Bb.SqlServer.Queries
{

    public class TextQueries
    {

        public static string SelectDatabases() => $"SELECT name FROM master.sys.databases";
        
        public static string TestDatabaseNoExists(string databaseName) => $"IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'{databaseName}')";
        
        public static string TestConstraintExists(string constraintName) => $"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME='{constraintName}')";

        public static string TestTableNotExists(string schema, string table) => $"IF NOT EXISTS (SELECT schema_name(t.schema_id) AS schemaName, t.name AS table_name FROM sys.tables t WHERE t.name = '{table}' AND schema_name(t.schema_id) = '{schema}')";
        public static string TestTableExists(string schema, string table) => $"IF EXISTS (SELECT schema_name(t.schema_id) AS schemaName, t.name AS table_name FROM sys.tables t WHERE t.name = '{table}' AND schema_name(t.schema_id) = '{schema}')";

        public static string TestIndexExists(string name) => $"IF EXISTS (SELECT * FROM sys.indexes pk where name = '{name}')";


        public static string Schemas = @"SELECT SCHEMA_NAME, SCHEMA_OWNER from information_schema.SCHEMATA";

        public static string Structures (string databaseName, string Filter = null) => @"

SELECT
	OBJECT_SCHEMA_NAME(tbs.[object_id],	DB_ID('$dbId'))	AS [Schema],
    tbs.[name]													AS [table_name],
    AC.[name]													AS [column_name],
    UPPER(TY.[name])											AS [system_data_type],
    AC.[max_length],
    AC.[precision],
	AC.[scale],
	AC.[is_filestream],
	AC.[is_computed],
	AC.[is_identity],
	AC.[is_rowguidcol],
	AC.[is_ansi_padded],
	AC.[collation_name],
	AC.[is_nullable],
    COALESCE(sep.value , '')									AS [Description],
	COALESCE(sip.seed_value, 0)									AS [Seed],
	COALESCE(sip.increment_value, 0)							AS [Increment]


FROM sys.[tables] AS tbs
	INNER JOIN sys.[all_columns] AC ON tbs.[object_id] = AC.[object_id]

	INNER JOIN sys.[types] TY ON AC.[system_type_id] = TY.[system_type_id]
								AND AC.[user_type_id] = TY.[user_type_id]

	LEFT JOIN sys.extended_properties sep ON tbs.object_id = sep.major_id
											AND ac.column_id = sep.minor_id
											AND sep.name = 'MS_Description'

	LEFT JOIN sys.identity_columns sip ON tbs.object_id = sip.object_id											
											

WHERE tbs.[is_ms_shipped] = 0 $Filter

ORDER BY tbs.[name], AC.[column_id]

"
 .Replace("$Filter", string.IsNullOrEmpty(Filter) ? string.Empty : "AND " + Filter)
 .Replace("$dbId", databaseName)
 ;


        public static string Keys = @"
select 
	schema_name(tab.schema_id)						            as [schema_name], 
    tab.[name]										            as table_name,
    substring(column_names, 1, len(column_names)-1)             as [columns],
	substring(column_descending, 1, len(column_descending)-1)   as [column_descendings],
    pk.[name]										            as pk_name,
    case when pk.[type] = 0 then 'HEAP'
         when pk.[type] = 1 then 'Clustered index'
         when pk.[type] = 2 then 'Nonclustered index'
         when pk.[type] = 3 then 'XML index'
         when pk.[type] = 4 then 'Spatial index'
         when pk.[type] = 5 then 'Clustered columnstore index'
         when pk.[type] = 6 then 'Nonclustered columnstore index'
         when pk.[type] = 7 then 'Nonclustered hash index'
     end                                                        as index_type,
	pk.is_unique                                                as is_unique,
	pk.is_padded                                                as is_padded,
	pk.allow_page_locks                                         as allow_page_locks,
	pk.allow_row_locks                                          as allow_row_locks,
	pk.optimize_for_sequential_key                              as optimize_sequential_key,
	s.no_recompute                                              as no_recompute

from sys.tables tab
    inner join sys.indexes pk
        on tab.object_id = pk.object_id 
        and pk.is_primary_key = 1

	inner join sys.stats s
		on tab.object_id = s.object_id

	cross apply (select col.[name] + ', '
                    from sys.index_columns ic
                        inner join sys.columns col
                            on ic.object_id = col.object_id
                            and ic.column_id = col.column_id
                    where ic.object_id = tab.object_id
                        and ic.index_id = pk.index_id
                            order by col.column_id
                            for xml path ('') ) D (column_names)

	cross apply (select CAST(ic.is_descending_key AS varchar(1)) + ', '
                    from sys.index_columns ic
                        inner join sys.columns col
                            on ic.object_id = col.object_id
                            and ic.column_id = col.column_id
                    where ic.object_id = tab.object_id
                        and ic.index_id = pk.index_id
                            order by col.column_id
                            for xml path ('') ) E (column_descending)

order by 
	schema_name(tab.schema_id),
	pk.[name]
";

        public static string ForeignKeys = @"
SELECT 
	schema_name(fk_tab.schema_id) as foreign_schema,
	fk_tab.name as foreign_table,
    schema_name(pk_tab.schema_id) as primary_schema,
	pk_tab.name as primary_table,
    fk_cols.constraint_column_id as no, 
    fk_col.name as fk_column_name,
    pk_col.name as pk_column_name,
    fk.name as fk_constraint_name,
	fk.update_referential_action,
	fk.delete_referential_action

FROM sys.foreign_keys fk

    INNER JOIN sys.tables fk_tab
        ON fk_tab.object_id = fk.parent_object_id

    INNER JOIN sys.tables pk_tab
        ON pk_tab.object_id = fk.referenced_object_id

    INNER JOIN sys.foreign_key_columns fk_cols
        ON fk_cols.constraint_object_id = fk.object_id

    INNER JOIN sys.columns fk_col
        ON fk_col.column_id = fk_cols.parent_column_id
        AND fk_col.object_id = fk_tab.object_id

    INNER JOIN sys.columns pk_col
        ON pk_col.column_id = fk_cols.referenced_column_id
        AND pk_col.object_id = pk_tab.object_id

ORDER BY 
	schema_name(fk_tab.schema_id) + '.' + fk_tab.name,
    schema_name(pk_tab.schema_id) + '.' + pk_tab.name, 
    fk_cols.constraint_column_id

";

        public static string Checks = @"SELECT CONSTRAINT_SCHEMA, CONSTRAINT_NAME, CHECK_CLAUSE FROM INFORMATION_SCHEMA.CHECK_CONSTRAINTS";

        public static string Filegroups = @"SELECT data_space_id, name, type, is_default, is_system, is_read_only, is_autogrow_all_files, filegroup_guid, log_filegroup_id FROM sys.filegroups";

        public static string Indexes (string? filter = null) => @"
SELECT 
	i.name,
    schema_name(t.schema_id)                                    AS [schema],
	t.[name]                                                    AS tableView, 
    CASE WHEN t.[type] = 'U' THEN 'Table'
        WHEN t.[type] = 'V' THEN 'View'
        END                                                     AS [object_type],
    i.index_id,
	i.is_primary_key                                            AS is_primary,
	i.is_unique                                                 AS is_unique,
	i.is_padded                                                 AS is_padded,
	i.allow_page_locks                                          AS allow_page_locks,
	i.allow_row_locks                                           AS allow_row_locks,
	i.optimize_for_sequential_key                               AS optimize_sequential_key,
	s.no_recompute                                              AS no_recompute,
	
    substring(column_names, 1, len(column_names)-1)             AS [columns],
    substring(column_descendings, 1, len(column_descendings)-1) AS [column_descendings],
    CASE WHEN i.[type] = 0 THEN 'HEAP'
         WHEN i.[type] = 1 THEN 'Clustered index'
         WHEN i.[type] = 2 THEN 'Nonclustered index'
         WHEN i.[type] = 3 THEN 'XML index'
         WHEN i.[type] = 4 THEN 'Spatial index'
         WHEN i.[type] = 5 THEN 'Clustered columnstore index'
         WHEN i.[type] = 6 THEN 'Nonclustered columnstore index'
         WHEN i.[type] = 7 THEN 'Nonclustered hash index'
    END                                                         AS index_type,
    f.name                                                      AS Filegroup

FROM sys.objects t
    INNER JOIN sys.indexes i
        ON t.object_id = i.object_id

	INNER JOIN sys.stats s
		ON t.object_id = s.object_id

    INNER JOIN sys.filegroups f
        ON i.data_space_id = f.data_space_id

    CROSS APPLY (SELECT col.[name] + ', '
                    FROM sys.index_columns ic
                        INNER JOIN sys.columns col
                            ON ic.object_id = col.object_id
                            AND ic.column_id = col.column_id
                    WHERE ic.object_id = t.object_id
                        AND ic.index_id = i.index_id
                            ORDER BY col.column_id
                            FOR XML PATH ('') ) D (column_names)

    CROSS APPLY (SELECT CAST(ic.is_descending_key AS varchar(1)) + ', '
                    FROM sys.index_columns ic
                        INNER JOIN sys.columns col
                            ON ic.object_id = col.object_id
                            AND ic.column_id = col.column_id
                    WHERE ic.object_id = t.object_id
                        AND ic.index_id = i.index_id
                            ORDER BY col.column_id
                            FOR XML PATH ('') ) E (column_descendings)

WHERE t.is_ms_shipped <> 1
    $filter

ORDER BY schema_name(t.schema_id) + '.' + t.[name], i.index_id
"
    .Replace("$filter", filter ?? string.Empty)
;



    }

}
