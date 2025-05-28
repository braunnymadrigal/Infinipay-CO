USE [InfinipayDB];
GO

ALTER TABLE Planilla
ADD fechaInicio DATE NOT NULL;
GO

ALTER TABLE Planilla
ADD fechaFin DATE NOT NULL;
GO

ALTER TABLE Planilla
ADD estado VARCHAR(11) NOT NULL;
GO

ALTER TABLE Planilla
ADD CONSTRAINT CHK_Planilla_estado 
CHECK (estado IN ('en progreso', 'completado', 'error'));
GO