USE [InfinipayDB];
GO

ALTER TABLE Contrato
DROP CONSTRAINT CHK_Contrato_tipoContrato;

ALTER TABLE Contrato
ALTER COLUMN tipoContrato VARCHAR(20) NOT NULL;

UPDATE Contrato SET tipoContrato = 'tiempoCompleto' WHERE tipoContrato = 'mensual';
UPDATE Contrato SET tipoContrato = 'medioTiempo' WHERE tipoContrato = 'quincenal';
UPDATE Contrato SET tipoContrato = 'servicios'     WHERE tipoContrato = 'semanal';

ALTER TABLE Contrato
ADD CONSTRAINT CHK_Contrato_tipoContrato
CHECK (tipoContrato IN ('tiempoCompleto', 'medioTiempo', 'servicios'));

-- Si la empresa es de pago mensual, el pago de todos los empleados debe ser mensual.
-- Si la empresa es de pago quincenal, el pago de todos los empleados debe ser quincenal.
-- Si la empresa es de pago semanal, el pago de todos los empleados debe ser semanal.