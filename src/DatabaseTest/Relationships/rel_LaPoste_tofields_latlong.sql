

ALTER TABLE [fields_latlong]
    ADD CONSTRAINT[rel_LaPoste_tofields_latlong] FOREIGN KEY ([LaPoste_$id])
        REFERENCES [LaPoste] ([_$id])
;
