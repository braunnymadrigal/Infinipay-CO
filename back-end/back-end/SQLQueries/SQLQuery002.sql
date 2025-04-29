CREATE TABLE Personas (
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	CorreoElectronico VARCHAR(100) UNIQUE NOT NULL,
	TipoIdentificacion VARCHAR(50) UNIQUE NOT NULL,
	Identificacion INT NOT NULL,
	IdDatosAuditoria UNIQUEIDENTIFIER,
	CONSTRAINT Unique_TipoIdentificacion_Identificacion UNIQUE (TipoIdentificacion, Identificacion)
);

ALTER TABLE Personas
Add NumeroTelefono VARCHAR(8) NOT NULL,
CONSTRAINT Unique_NumeroTelefono UNIQUE (NumeroTelefono);

CREATE TABLE Direccion (
	IdPersona UNIQUEIDENTIFIER,
	Provincia VARCHAR(50) NOT NULL,
	Canton VARCHAR(50) NOT NULL,
	Distrito VARCHAR(50) NOT NULL,
	OtrasSeñas VARCHAR(50) NOT NULL
	CONSTRAINT FK_Direccion_Persona FOREIGN KEY (IdPersona) REFERENCES Personas(Id)
)

EXEC sp_rename 'Personas', 'Persona';