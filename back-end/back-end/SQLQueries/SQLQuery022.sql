-----------------------------------------------------------------------

USE [InfinipayDB];
GO

ALTER TABLE [PersonaJuridica]
ADD nombreAsociacion varchar(256) NOT NULL;
GO

ALTER TABLE [BeneficioPorEmpleado]
ADD cantidadDependientes TINYINT NOT NULL DEFAULT 0;
GO

-----------------------------------------------------------------------