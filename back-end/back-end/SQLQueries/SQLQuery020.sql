USE [InfinipayDB];
GO

-- obtiene la fecha de horas aprobadas mas reciente de un empleado

CREATE FUNCTION dbo.function_getEmployeeCurrentHours(
	@employee uniqueidentifier,
	@startDate date,
	@endDate date
)
RETURNS TABLE
AS
RETURN
	SELECT MAX(fecha) AS fechaHoras 
	FROM Horas
	WHERE [idEmpleado] = @employee 
	and [aprobadas] = 1 
	and ([fecha] BETWEEN @startDate and @endDate);
GO
