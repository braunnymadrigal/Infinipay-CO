USE [InfinipayDB];
GO

CREATE TABLE Empleador (
  idPersonaFisica UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  idPersonaJuridica UNIQUEIDENTIFIER UNIQUE NOT NULL,

  CONSTRAINT FK_Empleador_PersonaFisica FOREIGN KEY (idPersonaFisica) REFERENCES PersonaFisica(id),
  CONSTRAINT FK_Empleador_PersonaJuridica FOREIGN KEY (idPersonaJuridica) REFERENCES PersonaJuridica(id)
);

CREATE TABLE Empleado (
  idPersonaFisica UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  rol VARCHAR(15),
  fechaContratacion DATE NOT NULL,
  idEmpleadorContratador UNIQUEIDENTIFIER NOT NULL,

  CONSTRAINT FK_Empleado_PersonaFisica FOREIGN KEY (idPersonaFisica) REFERENCES PersonaFisica(id),
  CONSTRAINT FK_Empleado_Empleador FOREIGN KEY (idEmpleadorContratador) REFERENCES Empleador(idPersonaFisica),
  CONSTRAINT CHK_Empleado_rol CHECK (rol IN ('supervisor', 'administrador'))
);

CREATE TABLE Contrato (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
  reportaHoras BIT NOT NULL,
  fechaCreacion DATE NOT NULL,
  salarioBruto DECIMAL(11,2) NOT NULL,
  tipoContrato varchar(9) NOT NULL,
  idEmpleado UNIQUEIDENTIFIER UNIQUE NOT NULL,

  CONSTRAINT FK_Contrato_Empleado FOREIGN KEY (idEmpleado) REFERENCES Empleado(idPersonaFisica),
  CONSTRAINT CHK_Contrato_tipoContrato CHECK (tipoContrato IN ('semanal', 'quincenal', 'mensual', 'servicios'))
);