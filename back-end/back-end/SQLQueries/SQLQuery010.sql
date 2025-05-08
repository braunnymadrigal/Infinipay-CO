CREATE TABLE DetallePago (
	id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
	monto DECIMAL(11,2) NOT NULL,
	fecha DATE NOT NULL,
	idPlanilla UNIQUEIDENTIFIER NOT NULL,
	idEmpleado UNIQUEIDENTIFIER NOT NULL,

	CONSTRAINT UQ_DetallePago_fecha_idPlanilla_idEmpleado UNIQUE(fecha, idPlanilla, idEmpleado),
	CONSTRAINT FK_DetallePago_Planilla FOREIGN KEY (idPlanilla) REFERENCES Planilla(id),
	CONSTRAINT FK_DetallePago_Empleado FOREIGN KEY (idEmpleado) REFERENCES Empleado(idPersonaFisica)
)

CREATE TABLE DeduccionAPago (
	idDeduccion UNIQUEIDENTIFIER NOT NULL,
	idDetallePago UNIQUEIDENTIFIER NOT NULL,

	CONSTRAINT PK_DeduccionAPago PRIMARY KEY (idDeduccion, idDetallePago),
	CONSTRAINT FK_DeduccionAPago_Deduccion FOREIGN KEY (idDeduccion) REFERENCES Deduccion(id),
	CONSTRAINT FK_DeduccionAPago_DetallePago FOREIGN KEY (idDetallePago) REFERENCES DetallePago(id)
);