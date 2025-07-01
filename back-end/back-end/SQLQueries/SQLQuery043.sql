CREATE NONCLUSTERED INDEX IX_DeduccionAPago_idDetallePago
ON dbo.DeduccionAPago (idDetallePago);

CREATE NONCLUSTERED INDEX IX_DetallePago_Join
ON dbo.DetallePago (idPlanilla, idEmpleado);

CREATE NONCLUSTERED INDEX IX_EmpleadoDePlanilla_Join
ON dbo.EmpleadoDePlanilla (idPlanilla, idEmpleado);

CREATE NONCLUSTERED INDEX IX_Planilla_idPersonaJuridica
ON dbo.Planilla (idPersonaJuridica);
