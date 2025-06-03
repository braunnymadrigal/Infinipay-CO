USE [InfinipayDB];
GO

ALTER TABLE Contrato
DROP CONSTRAINT CHK_Contrato_tipoContrato;
GO
  
ALTER TABLE Contrato
ALTER COLUMN tipoContrato VARCHAR(20) NOT NULL;
GO

-- Por si tienen datos en la base
UPDATE Contrato SET tipoContrato = 'tiempoCompleto' WHERE tipoContrato = 'mensual';
UPDATE Contrato SET tipoContrato = 'medioTiempo' WHERE tipoContrato = 'quincenal';
UPDATE Contrato SET tipoContrato = 'servicios'     WHERE tipoContrato = 'semanal';
UPDATE Contrato SET tipoContrato = 'horas'     WHERE tipoContrato = 'semanal';
GO
  
ALTER TABLE Contrato
ADD CONSTRAINT CHK_Contrato_tipoContrato
CHECK (tipoContrato IN ('tiempoCompleto', 'medioTiempo', 'horas','servicios'));
GO
-- Si la empresa es de pago mensual, el pago de todos los empleados debe ser mensual.
-- Si la empresa es de pago quincenal, el pago de todos los empleados debe ser quincenal.
-- Si la empresa es de pago semanal, el pago de todos los empleados debe ser semanal.
