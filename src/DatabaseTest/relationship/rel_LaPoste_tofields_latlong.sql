

ALTER TABLE [LaPoste](
    ADD CONSTRAINT[rel_LaPoste_tofields_latlong] FOREIGN KEY ([LaPoste_$id])
        REFERENCES Parent ([_$id])
);
