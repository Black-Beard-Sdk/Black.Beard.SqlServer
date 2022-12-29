

ALTER TABLE [LaPoste](
    ADD CONSTRAINT[rel_LaPoste_togeometry_coordinates] FOREIGN KEY ([LaPoste_$id])
        REFERENCES Parent ([_$id])
);
