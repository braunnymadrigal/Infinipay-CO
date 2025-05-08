CREATE TABLE Horas (
	id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
	fecha DATE NOT NULL,
	horasTrabajadas SMALLINT NOT NULL,
	aprobadas BIT,
	idEmpleado UNIQUEIDENTIFIER NOT NULL,
	idSupervisor UNIQUEIDENTIFIER,

	CONSTRAINT UQ_Horas_idEmpleado_fecha UNIQUE (idEmpleado, fecha),
	CONSTRAINT FK_Horas_Empleado FOREIGN KEY (idEmpleado) REFERENCES Empleado(idPersonaFisica),
	CONSTRAINT FK_Horas_Supervisor FOREIGN KEY (idSupervisor) REFERENCES Empleado(idPersonaFisica),
	CONSTRAINT CHK_Horas_horasTrabajadas CHECK (horasTrabajadas >= 0)
);

CREATE TABLE EmpleadoSupervision (
  idSupervisor UNIQUEIDENTIFIER NOT NULL,
  idEmpleado UNIQUEIDENTIFIER NOT NULL,

  CONSTRAINT PK_EmpleadoSupervision PRIMARY KEY (idSupervisor, idEmpleado),
  CONSTRAINT FK_EmpleadoSupervision_Supervisor FOREIGN KEY (idSupervisor) REFERENCES Empleado(idPersonaFisica),
  CONSTRAINT FK_EmpleadoSupervision_Empleado FOREIGN KEY (idEmpleado) REFERENCES Empleado(idPersonaFisica),
  CONSTRAINT CHK_Supervision_NoAutoSupervision CHECK (idSupervisor <> idEmpleado)
);
