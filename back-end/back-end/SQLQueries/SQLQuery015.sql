ALTER TABLE [dbo].[Empleado]
DROP CONSTRAINT [CHK_Empleado_rol];
GO
ALTER TABLE [dbo].[Empleado]
ADD CONSTRAINT [CHK_Empleado_rol]
CHECK ([rol] IN ('sinRol', 'supervisor', 'administrador'));
GO