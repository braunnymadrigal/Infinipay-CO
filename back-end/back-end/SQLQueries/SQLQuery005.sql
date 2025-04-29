CREATE TABLE PersonaFisica(
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	primerNombre VARCHAR(50) NOT NULL,
	segundoNombre VARCHAR(50) NOT NULL,
	primerApellido VARCHAR(50) NOT NULL,
	segundoApellido VARCHAR(50) NOT NULL,
	CONSTRAINT FK_PersonaFisica_Persona FOREIGN KEY (Id) REFERENCES Persona(Id)
);

CREATE TABLE PersonaJuridica(
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	descripcion VARCHAR(250) NOT NULL,
	tipoPago VARCHAR(30) NOT NULL,
	beneficiosPorEmpleado TINYINT NOT NULL,
	razonSocial VARCHAR(254) NOT NULL,
	CONSTRAINT FK_PersonaJuridica_Persona FOREIGN KEY (Id) REFERENCES Persona(Id)
);

EXEC sp_rename 'PersonaFisica.Id', 'id', 'COLUMN';
EXEC sp_rename 'PersonaJuridica.Id', 'id', 'COLUMN';