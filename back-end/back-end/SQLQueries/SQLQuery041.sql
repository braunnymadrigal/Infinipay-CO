CREATE OR ALTER TRIGGER TRGR_InsertDeduccionAPago_UpdateBeneficioPorEmpleado_When_BeneficioDeleted
ON DeduccionAPago
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    -- Actualiza el beneficio para que cuando el beneficio esta borrado
	-- pero elegible para la planilla actual, lo inhabilite para la siguiente

    UPDATE bpe SET bpe.ElegibleParaPlanilla = 0 FROM BeneficioPorEmpleado bpe
	JOIN Deduccion d ON d.idBeneficio = bpe.idBeneficio JOIN inserted i ON i.idDeduccion = d.id
	JOIN Beneficio b ON b.id = bpe.idBeneficio
	WHERE b.borrado = 1 AND bpe.ElegibleParaPlanilla = 1;
    
END;
