

CREATE TABLE [fields_latlong]
(
    [_$id] UNIQUEIDENTIFIER NOT NULL, 
    [LaPoste_$id] UNIQUEIDENTIFIER NOT NULL, 
    [value] BINARY(50), 
    [valuew] [sys].[geography] NULL, 
    [valuex] [sys].[geometry] NULL, 
    [valuea] [sys].[hierarchyid] NULL, 
    [valueb] [sys].[sysname] NULL, 
    PRIMARY KEY CLUSTERED
    (
        [_$id] ASC
    )
);

GO

exec sp_AddExtendedProperty 
      'MS_Description'
    , 'The phone number (e.g., "(555) 253-5970 x1150"'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'fields_latlong'
    , 'COLUMN', '_$id'

GO

exec sp_AddExtendedProperty 
      'MS_Description'
    , 'datas''test'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'fields_latlong'
    , 'COLUMN', 'value'

