

CREATE TABLE [geometry_coordinates]
(
    [_$id] UNIQUEIDENTIFIER NOT NULL, 
    [LaPoste_$id] UNIQUEIDENTIFIER NOT NULL, 
    [value] FLOAT, 
    PRIMARY KEY CLUSTERED
    (
        [_$id] ASC
    )
);

GO

exec sp_AddExtendedProperty 
      'MS_Description'
    , 'datas''test'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'geometry_coordinates'
    , 'COLUMN', 'value'

