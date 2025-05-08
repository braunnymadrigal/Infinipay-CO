USE [InfinipayDB];
GO

CREATE TABLE Usuario (
	id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	nickname VARCHAR(100) NOT NULL,
	contrasena BINARY(64) NOT NULL,
	idPersonaFisica UNIQUEIDENTIFIER NOT NULL,

	CONSTRAINT PK_Usuario PRIMARY KEY (id),
	CONSTRAINT Unique_Nickname UNIQUE (nickname),
	CONSTRAINT Unique_idPersonaFisica UNIQUE (idPersonaFisica),
	CONSTRAINT FK_Usuario_PersonaFisica FOREIGN KEY (idPersonaFisica) REFERENCES PersonaFisica(id),
);