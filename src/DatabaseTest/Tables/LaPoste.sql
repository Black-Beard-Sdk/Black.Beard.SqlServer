

CREATE TABLE [LaPoste]
(
    [_$id] UNIQUEIDENTIFIER NOT NULL, 
    [datasetid] VARCHAR, 
    [recordid] VARCHAR, 
    [record_timestamp] DATETIME, 
    [fields_longitude] FLOAT, 
    [fields_latitude] FLOAT, 
    [fields_libelle_du_site] VARCHAR(MAX), 
    [fields_site_acores_de_rattachement] VARCHAR, 
    [fields_identifiant_a] VARCHAR, 
    [fields_localite] VARCHAR, 
    [fields_adresse] VARCHAR, 
    [fields_code_insee] VARCHAR, 
    [fields_precision_du_geocodage] VARCHAR, 
    [fields_numero_de_telephone] VARCHAR, 
    [fields_pays] VARCHAR, 
    [fields_complement_d_adresse] VARCHAR, 
    [fields_code_postal] VARCHAR, 
    [fields_caracteristique_du_site] VARCHAR, 
    [fields_lieu_dit] VARCHAR, 
    [geometry_type] VARCHAR, 
    PRIMARY KEY NONCLUSTERED
    (
        [_$id] ASC
    ) ON [PRIMARY2]

);
