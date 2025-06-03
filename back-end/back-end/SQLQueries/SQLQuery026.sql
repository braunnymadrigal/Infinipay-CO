USE [InfinipayDB];
GO

ALTER TABLE [dbo].[Beneficio]
DROP CONSTRAINT [CHK_Beneficio_empleadoElegible];
GO

-- Por si tienen datos en la base
UPDATE dbo.Beneficio
SET empleadoElegible = 'servicios'
WHERE empleadoElegible NOT IN ('servicios', 'tiempoCompleto', 'medioTiempo', 'horas');
GO

ALTER TABLE [dbo].[Beneficio]
ADD CONSTRAINT [CHK_Beneficio_empleadoElegible]
CHECK ([empleadoElegible] IN ('servicios', 'tiempoCompleto', 'medioTiempo', 'horas', 'todos'));
GO
