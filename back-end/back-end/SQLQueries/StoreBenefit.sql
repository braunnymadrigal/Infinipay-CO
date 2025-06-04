CREATE OR ALTER PROCEDURE StoreBenefit
	@User VARCHAR(100),
	@FormulaType VARCHAR(10),
	@UrlApi VARCHAR(256) = NULL,
	@Param1 VARCHAR(50),
	@Param2 VARCHAR(50) = NULL,
	@Param3 VARCHAR(50) = NULL,
	@Name VARCHAR(100),
	@MinTime decimal(4,2) = 0,
	@Description VARCHAR(256),
	@ElegibleEmployees VARCHAR(14) = 'todos'
AS
BEGIN
	-- Create Audit
	DECLARE @IdAudit UNIQUEIDENTIFIER = NEWID();
	INSERT INTO Auditoria (id, usuarioCreador) VALUES
	(@IdAudit , @User);

	-- Create Formula
	DECLARE @IdFormula UNIQUEIDENTIFIER = NEWID();
	INSERT INTO Formula(id, tipoFormula, urlAPI, paramUno, paramDos, paramTres) VALUES
	(@IdFormula, @FormulaType, @UrlApi, @Param1, @Param2, @Param3);

	-- Find CompanyId
	DECLARE @CompanyId UNIQUEIDENTIFIER;
	SELECT TOP 1 @CompanyId = pj.id FROM Usuario u JOIN Empleador e ON
	u.idPersonaFisica = e.idPersonaFisica JOIN PersonaJuridica pj ON e.idPersonaJuridica = pj.id
	WHERE u.nickname = @User;

	-- Create Benefit
	DECLARE @BenefitId UNIQUEIDENTIFIER = NEWID(); 
	INSERT INTO Beneficio(id, nombre, tiempoMinimo, descripcion, empleadoElegible, idPersonaJuridica, idAuditoria) VALUES
	(@BenefitId, @Name, @MinTime, @Description, @ElegibleEmployees, @CompanyId, @IdAudit);

	--Create Deduccion
	INSERT INTO Deduccion (nombre, descripcion, idPersonaJuridica, idBeneficio, idFormula, idAuditoria) VALUES
	(@Name, @Description, @CompanyId, @BenefitId, @IdFormula, @IdAudit);
END;

