USE [InfinipayDB];
GO

ALTER TABLE DetallePago
DROP CONSTRAINT UQ_DetallePago_fecha_idPlanilla_idEmpleado;
GO

ALTER TABLE DetallePago
DROP COLUMN fecha;
GO

ALTER TABLE DetallePago
ADD CONSTRAINT UQ_DetallePago_idPlanilla_idEmpleado UNIQUE(idPlanilla, idEmpleado);
GO

ALTER TABLE DetallePago
DROP COLUMN monto;
GO

ALTER TABLE DetallePago
ADD fechaInicio DATE NOT NULL;
GO

ALTER TABLE DetallePago
ADD fechaFin DATE NOT NULL;
GO

ALTER TABLE DetallePago
ADD salarioBruto DECIMAL(11,2) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE DetallePago
ADD salarioNeto DECIMAL(11,2) NOT NULL DEFAULT 0.0;
GO