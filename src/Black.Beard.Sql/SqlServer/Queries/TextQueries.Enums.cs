using System;
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
        TableName,
        ColumnName,
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
        index_id,
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

    public enum ForeignColumns
    {
        foreign_schema,
        foreign_table,
        primary_schema,
        primary_table,
        no,
        fk_column_name,
        pk_column_name,
        fk_constraint_name,
        on_update,
        on_delete
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

}
