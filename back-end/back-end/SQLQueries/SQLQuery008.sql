CREATE TABLE Planilla (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
  idAuditoria UNIQUEIDENTIFIER NOT NULL,
  idPersonaJuridica UNIQUEIDENTIFIER NOT NULL,

  CONSTRAINT UQ_Planilla_idAuditoria_idPersonaJuridica UNIQUE (idAuditoria, idPersonaJuridica),
  CONSTRAINT FK_Planilla_Auditoria FOREIGN KEY (idAuditoria) REFERENCES Auditoria(id),
  CONSTRAINT FK_Planilla_PersonaJuridica FOREIGN KEY (idPersonaJuridica) REFERENCES PersonaJuridica(id)
);

CREATE TABLE EmpleadoDePlanilla (
  idPlanilla UNIQUEIDENTIFIER NOT NULL,
  idEmpleado UNIQUEIDENTIFIER NOT NULL,

  CONSTRAINT PK_EmpleadoDePlanilla PRIMARY KEY (idPlanilla, idEmpleado),
  CONSTRAINT FK_EmpleadoDePlanilla_Planilla FOREIGN KEY (idPlanilla) REFERENCES Planilla(id),
  CONSTRAINT FK_EmpleadoDePlanilla_Empleado FOREIGN KEY (idEmpleado) REFERENCES Empleado(idPersonaFisica)
);

CREATE TABLE Formula (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
  tipoFormula VARCHAR(10) NOT NULL,
  urlAPI VARCHAR(256),
  paramUno VARCHAR(50) NOT NULL,
  paramDos VARCHAR(50),
  paramTres VARCHAR(50),

  CONSTRAINT CHK_Formula_tipoFormula CHECK (tipoFormula IN ('montoFijo', 'porcentaje', 'api'))
);

CREATE TABLE Deduccion ( 
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
  nombre VARCHAR(50) NOT NULL,
  descripcion VARCHAR(256) NOT NULL,
  idPersonaJuridica UNIQUEIDENTIFIER NOT NULL,
  idBeneficio UNIQUEIDENTIFIER UNIQUE,
  idFormula UNIQUEIDENTIFIER UNIQUE NOT NULL,
  idAuditoria UNIQUEIDENTIFIER UNIQUE NOT NULL,

  CONSTRAINT UQ_Deduccion_nombre_idPersonaJuridica UNIQUE (nombre, idPersonaJuridica),
  CONSTRAINT FK_Deduccion_PersonaJuridica FOREIGN KEY (idPersonaJuridica) REFERENCES PersonaJuridica(id),
  CONSTRAINT FK_Deduccion_Beneficio FOREIGN KEY (idBeneficio) REFERENCES Beneficio(id),
  CONSTRAINT FK_Deduccion_Formula FOREIGN KEY (idFormula) REFERENCES Formula(id),
  CONSTRAINT FK_Deduccion_Auditoria FOREIGN KEY (idAuditoria) REFERENCES Auditoria(id)
);

