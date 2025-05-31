---------------------------------------------

USE [InfinipayDB];
GO

ALTER TABLE [DeduccionAPago]
DROP CONSTRAINT CHK_DeduccionAPago_Tipo;
GO

ALTER TABLE DeduccionAPago
DROP COLUMN [tipo];
GO

ALTER TABLE DeduccionAPago
ADD tipo VARCHAR(13) NOT NULL;
GO

ALTER TABLE DeduccionAPago
ADD CONSTRAINT CHK_DeduccionAPago_tipo
CHECK (tipo IN ('ccssEmpleado', 'ccssEmpleador', 'renta', 'beneficio'));
GO

---------------------------------------------