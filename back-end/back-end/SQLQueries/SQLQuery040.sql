CREATE OR ALTER TRIGGER TRGR_DeleteCascade_Deduccion
ON Deduccion
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Eliminar registros de Formula
    DELETE f
    FROM Formula f
    JOIN deleted d ON f.id = d.idFormula;

    -- Eliminar registros de Beneficio
    DELETE b
    FROM Beneficio b
    JOIN deleted d ON b.id = d.idBeneficio;
END;
