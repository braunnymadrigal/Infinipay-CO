CREATE TABLE DatosAuditoria (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    fechaCreacion DATETIME2 NOT NULL,
    usuarioCreador VARCHAR(100) NOT NULL,
    UltimaFechaModificacion DATETIME2,
    UltimoUsuarioModificador VARCHAR(100)
);