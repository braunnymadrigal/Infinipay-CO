CREATE TRIGGER trrg_Audit_GetDateUpdate
ON Auditoria
AFTER UPDATE
AS
BEGIN
  SET NOCOUNT ON;  
  BEGIN
	UPDATE a SET a.ultimaFechaModificacion = GETDATE()
	FROM Auditoria a INNER JOIN inserted i on i.id = a.id; 
  END
END

