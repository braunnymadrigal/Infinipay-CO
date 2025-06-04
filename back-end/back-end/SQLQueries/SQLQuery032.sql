CREATE OR ALTER PROCEDURE UpdateBenefit
	@id UNIQUEIDENTIFIER,
	@User VARCHAR(100),
	@FormulaType VARCHAR(10),
	@UrlApi VARCHAR(256) = NULL,
	@Param1 VARCHAR(50),
	@Param2 VARCHAR(50) = NULL,
	@Param3 VARCHAR(50) = NULL,
	@Name VARCHAR(100),
	@CompanyId UNIQUEIDENTIFIER,
	@MinTime decimal(4,2) = 0,
	@Description VARCHAR(256),
	@ElegibleEmployees VARCHAR(14) = 'todos'
AS
BEGIN

	--Update Deduccion
	UPDATE Deduccion SET nombre = @Name, descripcion = @Description WHERE idBeneficio = @id;

	-- Update Benefit
	UPDATE Beneficio SET nombre = @Name, tiempoMinimo = @MinTime, descripcion = @Description, empleadoElegible = @ElegibleEmployees
	WHERE id = @id;

	-- Update Formula
	UPDATE f SET f.tipoFormula = @FormulaType, f.urlAPI = @UrlApi, f.paramUno = @Param1,
	f.paramDos = @Param2, f.paramTres = @Param3 FROM Formula f JOIN Deduccion d on d.idFormula = f.id
	JOIN Beneficio b ON b.id = d.idBeneficio WHERE b.id = @id;

	-- Update Audit
	UPDATE a SET a.ultimoUsuarioModificador = @User FROM Auditoria a JOIN Beneficio b ON b.idAuditoria = a.id
	WHERE b.id = @id; 

END;
