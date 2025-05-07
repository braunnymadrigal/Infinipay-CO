CREATE TABLE Persona (
	id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
	correoElectronico VARCHAR(100) UNIQUE NOT NULL,
	tipoIdentificacion VARCHAR(8) NOT NULL,
	identificacion VARCHAR(12) NOT NULL,
	numeroTelefono VARCHAR(15) UNIQUE NOT NULL,
	idAuditoria UNIQUEIDENTIFIER UNIQUE NOT NULL,

	CONSTRAINT CHK_Persona_tipoIdentificacion CHECK (tipoIdentificacion IN ('juridica', 'fisica')),
	CONSTRAINT UQ_Persona_tipoIdentificacion_identificacion UNIQUE (tipoIdentificacion, identificacion),
	CONSTRAINT FK_Persona_Auditoria FOREIGN KEY (idAuditoria) REFERENCES Auditoria(id)
);

CREATE TABLE Direccion (
	idPersona UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	provincia VARCHAR(50) NOT NULL,
	canton VARCHAR(50) NOT NULL,
	distrito VARCHAR(50) NOT NULL,
	otrasSenas VARCHAR(256) NOT NULL,

	CONSTRAINT FK_Direccion_Persona FOREIGN KEY (idPersona) REFERENCES Persona(id)
);