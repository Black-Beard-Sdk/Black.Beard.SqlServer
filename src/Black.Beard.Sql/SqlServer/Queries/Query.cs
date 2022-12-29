﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://dataedo.com/kb/query/sql-server
namespace Bb.SqlServer.Queries
{

    public enum ColumnStructures
    {
        Schema,
        table_name,
        column_name,
        system_data_type,
        max_length,
        precision,
        scale,
        is_filestream,
        is_computed,
        is_identity,
        is_rowguidcol,
        is_ansi_padded,
        collation_name,
        is_nullable,
        Description,
        Seed,
        Increment
    }

    public enum IndexColumns
    {
        name,
        schema_name,
        tableView, 
        object_type,
        is_primary,
        is_unique,
        is_padded,
        allow_page_locks,
        allow_row_locks,
        optimize_sequential_key,
        no_recompute,
        columns,
        column_descendings,
        index_type,
        Filegroup
    }

    public enum FileGroupColumns
    {
        DataSpaceId, 
        Name, 
        Type,
        isDdefault,
        isSystem,
        isRead_only,
        isAutogrowAllFiles,
        filegroupGuid,
        logFilegroupId
    }

    public enum SchemaColumns
    {
        SCHEMA_NAME, 
        SCHEMA_OWNER
    }

    public class Query
    {

        public static string Schemas = @"SELECT SCHEMA_NAME, SCHEMA_OWNER from information_schema.SCHEMATA";

        public static string Structures = @"

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
											

WHERE tbs.[is_ms_shipped] = 0

ORDER BY tbs.[name], AC.[column_id]

";

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
	schema_name(fk_tab.schema_id) + '.' + fk_tab.name AS foreign_table,    
    schema_name(pk_tab.schema_id) + '.' + pk_tab.name AS primary_table,
    substring(column_names, 1, len(column_names)-1) AS [fk_columns],
    fk.name as fk_constraint_name

FROM sys.foreign_keys fk
    inner join sys.tables fk_tab ON fk_tab.object_id = fk.parent_object_id
    inner join sys.tables pk_tab ON pk_tab.object_id = fk.referenced_object_id
    cross apply (SELECT col.[name] + ', '
                 
				 FROM sys.foreign_key_columns fk_c
					INNER JOIN sys.columns col ON fk_c.parent_object_id = col.object_id
												AND fk_c.parent_column_id = col.column_id

                 WHERE fk_c.parent_object_id = fk_tab.object_id
						AND fk_c.constraint_object_id = fk.object_id
                 
				 ORDER BY col.column_id

                 FOR XML PATH ('') ) D (column_names)

ORDER BY 
	schema_name(fk_tab.schema_id) + '.' + fk_tab.name,
    schema_name(pk_tab.schema_id) + '.' + pk_tab.name

";

        public static string Checks = @"SELECT CONSTRAINT_SCHEMA, CONSTRAINT_NAME, CHECK_CLAUSE FROM INFORMATION_SCHEMA.CHECK_CONSTRAINTS";

        public static string Filegroups = @"SELECT data_space_id, name, type, is_default, is_system, is_read_only, is_autogrow_all_files, filegroup_guid, log_filegroup_id FROM sys.filegroups";

        public static string Indexes = @"
select 
	i.name,
    schema_name(t.schema_id)                                    as [schema],
	t.[name]                                                    as tableView, 
    case when t.[type] = 'U' then 'Table'
        when t.[type] = 'V' then 'View'
        end                                                     as [object_type],
    i.index_id,
	i.is_primary_key                                            as is_primary,
	i.is_unique                                                 as is_unique,
	i.is_padded                                                 as is_padded,
	i.allow_page_locks                                          as allow_page_locks,
	i.allow_row_locks                                           as allow_row_locks,
	i.optimize_for_sequential_key                               as optimize_sequential_key,
	s.no_recompute                                              as no_recompute,
	
    substring(column_names, 1, len(column_names)-1)             as [columns],
    substring(column_descendings, 1, len(column_descendings)-1) as [column_descendings],
    case when i.[type] = 0 then 'HEAP'
         when i.[type] = 1 then 'Clustered index'
         when i.[type] = 2 then 'Nonclustered index'
         when i.[type] = 3 then 'XML index'
         when i.[type] = 4 then 'Spatial index'
         when i.[type] = 5 then 'Clustered columnstore index'
         when i.[type] = 6 then 'Nonclustered columnstore index'
         when i.[type] = 7 then 'Nonclustered hash index'
    end                                                         as index_type,
    f.name                                                      as Filegroup

from sys.objects t
    inner join sys.indexes i
        on t.object_id = i.object_id

	inner join sys.stats s
		on t.object_id = s.object_id

    INNER JOIN sys.filegroups f
        ON i.data_space_id = f.data_space_id

    cross apply (select col.[name] + ', '
                    from sys.index_columns ic
                        inner join sys.columns col
                            on ic.object_id = col.object_id
                            and ic.column_id = col.column_id
                    where ic.object_id = t.object_id
                        and ic.index_id = i.index_id
                            order by col.column_id
                            for xml path ('') ) D (column_names)

    cross apply (select CAST(ic.is_descending_key as varchar(1)) + ', '
                    from sys.index_columns ic
                        inner join sys.columns col
                            on ic.object_id = col.object_id
                            and ic.column_id = col.column_id
                    where ic.object_id = t.object_id
                        and ic.index_id = i.index_id
                            order by col.column_id
                            for xml path ('') ) E (column_descendings)

where t.is_ms_shipped <> 1
and index_id > 0
order by schema_name(t.schema_id) + '.' + t.[name], i.index_id
";



    }

}
