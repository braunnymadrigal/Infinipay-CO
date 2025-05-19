USE [InfinipayDB];
GO

CREATE TABLE PersonaFisica (
	id UNIQUEIDENTIFIER NOT NULL,
	primerNombre VARCHAR(50) NOT NULL,
	segundoNombre VARCHAR(50),
	primerApellido VARCHAR(50) NOT NULL,
	segundoApellido VARCHAR(50) NOT NULL,
	genero VARCHAR(9) NOT NULL,

	CONSTRAINT PK_PersonaFisica PRIMARY KEY (id),
	CONSTRAINT FK_PersonaFisica_Persona FOREIGN KEY (id) REFERENCES Persona(id),
	CONSTRAINT CHK_PersonaFisica_genero CHECK (genero IN ('masculino', 'femenino')),
);

CREATE TABLE PersonaJuridica (
	id UNIQUEIDENTIFIER NOT NULL,
	descripcion VARCHAR(256) NOT NULL,
	tipoPago VARCHAR(30) NOT NULL,
	beneficiosPorEmpleado TINYINT NOT NULL,
	razonSocial VARCHAR(256) NOT NULL,

	CONSTRAINT PK_PersonaJuridica PRIMARY KEY (id),
	CONSTRAINT FK_PersonaJuridica_Persona FOREIGN KEY (id) REFERENCES Persona(id),
	CONSTRAINT CHK_PersonaJuridica_tipoPago CHECK (tipoPago IN ('semanal', 'quincenal', 'mensual')),
);