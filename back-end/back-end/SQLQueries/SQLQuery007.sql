CREATE TABLE Beneficio (
	id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
	nombre VARCHAR(100) NOT NULL,
	tiempoMinimo DECIMAL(4,2) NOT NULL,
	descripcion VARCHAR(256) NOT NULL,
	empleadoElegible VARCHAR(9) NOT NULL,
	idPersonaJuridica UNIQUEIDENTIFIER NOT NULL,
	idAuditoria UNIQUEIDENTIFIER UNIQUE NOT NULL,

	CONSTRAINT UQ_Beneficio_nombre_idPersonaJuridica UNIQUE (nombre, idPersonaJuridica),
	CONSTRAINT FK_Beneficio_PersonaJuridica FOREIGN KEY (idPersonaJuridica) REFERENCES PersonaJuridica(id),
	CONSTRAINT FK_Beneficio_Auditoria FOREIGN KEY (idAuditoria) REFERENCES Auditoria(id),
	CONSTRAINT CHK_Beneficio_empleadoElegible CHECK (empleadoElegible IN ('todos', 'semanal', 'quincenal', 'mensual'))
);

CREATE TABLE BeneficioPorEmpleado (
	idBeneficio UNIQUEIDENTIFIER NOT NULL,
	idEmpleado UNIQUEIDENTIFIER NOT NULL,
	mesesValidos SMALLINT NOT NULL,

	CONSTRAINT PK_BeneficioPorEmpleado PRIMARY KEY (idBeneficio, idEmpleado),
	CONSTRAINT FK_BeneficioPorEmpleado_Beneficio FOREIGN KEY (idBeneficio) REFERENCES Beneficio(id),
	CONSTRAINT FK_BeneficioPorEmpleado_Empleado FOREIGN KEY (idEmpleado) REFERENCES Empleado(idPersonaFisica),
	CONSTRAINT CHK_BeneficioPorEmpleado_mesesValidos CHECK (mesesValidos >= 0)
);