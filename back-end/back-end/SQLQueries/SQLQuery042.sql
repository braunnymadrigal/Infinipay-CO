CREATE OR ALTER PROCEDURE DeleteBenefit
	@id UNIQUEIDENTIFIER,
	@User VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRANSACTION
		-- Si el beneficio ya fue marcado como borrado, no hace nada
		IF EXISTS (
			SELECT 1 FROM Beneficio WHERE id = @id AND borrado = 1
		)
		BEGIN
			RETURN;
		END
		-- Revisa si es borrado logico o fisico HAY QUE MODIFICAR LA TABLAS
		IF NOT EXISTS (
			SELECT 1 FROM BeneficioPorEmpleado WHERE idBeneficio = @id
		)
		AND NOT EXISTS (
			SELECT 1 FROM DeduccionAPago dap JOIN Deduccion d ON dap.idDeduccion = d.id
			WHERE d.idBeneficio = @id
		)
		BEGIN
			-- Borrado fisico
			UPDATE a SET a.ultimoUsuarioModificador = @User FROM Auditoria a JOIN Beneficio b ON b.idAuditoria = a.id
			WHERE b.id = @id;
			DELETE FROM Deduccion WHERE idBeneficio = @id;
		END
		ELSE
		BEGIN
			
			-- Borrado logico
			UPDATE Deduccion SET borrado = 1 WHERE idBeneficio = @id;
			UPDATE Beneficio SET borrado = 1 WHERE id = @id;
			UPDATE f SET borrado = 1 FROM Formula f 
			JOIN Deduccion d ON d.idFormula = f.id WHERE d.idBeneficio = @id;
			UPDATE a SET a.ultimoUsuarioModificador = @User FROM Auditoria a
			JOIN Beneficio b ON b.idAuditoria = a.id WHERE b.id = @id;

			-- Desasigna a todos los empleados del beneficio
			UPDATE bpe SET borrado = 1 FROM BeneficioPorEmpleado bpe WHERE bpe.idBeneficio = @id;

			-- Si el beneficio no se ha pagado en ninguna planilla, no se debe pagar en la siguiente
			IF NOT EXISTS (
				SELECT 1 FROM DeduccionAPago dap JOIN Deduccion d ON dap.idDeduccion = d.id
				WHERE d.idBeneficio = @id
			)
			BEGIN
				UPDATE bpe SET ElegibleParaPlanilla = 0 FROM BeneficioPorEmpleado bpe WHERE bpe.idBeneficio = @id;
			END
		END
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END