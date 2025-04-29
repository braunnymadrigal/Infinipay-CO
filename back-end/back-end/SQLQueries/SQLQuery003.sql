EXEC sp_rename 'Persona.Id', 'id', 'COLUMN';
EXEC sp_rename 'Persona.CorreoElectronico', 'correoElectronico', 'COLUMN';
EXEC sp_rename 'Persona.TipoIdentificacion', 'tipoIdentificacion', 'COLUMN';
EXEC sp_rename 'Persona.Identificacion', 'identificacion', 'COLUMN';
EXEC sp_rename 'Persona.NumeroTelefono', 'numeroTelefono', 'COLUMN';
EXEC sp_rename 'Persona.IdDatosAuditoria', 'idDatosAuditoria', 'COLUMN';
EXEC sp_rename 'Unique_TipoIdentificacion_Identificacion', 'UQ_Persona_tipoIdentificacion_identificacion';
EXEC sp_rename 'Unique_NumeroTelefono', 'UQ_Persona_numeroTelefono';

EXEC sp_rename 'Direccion.IdPersona', 'idPersona', 'COLUMN';
EXEC sp_rename 'Direccion.Provincia', 'provincia', 'COLUMN';
EXEC sp_rename 'Direccion.Canton', 'canton', 'COLUMN';
EXEC sp_rename 'Direccion.Distrito', 'distrito', 'COLUMN';
EXEC sp_rename 'Direccion.OtrasSeñas', 'otrasSeñas', 'COLUMN';


