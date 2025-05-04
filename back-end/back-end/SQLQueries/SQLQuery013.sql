ALTER TABLE Empleado
DROP CONSTRAINT CHK_Empleado_rol;

ALTER TABLE Empleado
ADD CONSTRAINT CHK_Empleado_rol CHECK (rol IN ('supervisor', 'administrador', 'empleado'));

ALTER TABLE Empleado
ALTER COLUMN rol VARCHAR(15) NOT NULL;