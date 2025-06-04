CREATE PROCEDURE sp_CreateNewEmployer
  @idNumber NVARCHAR(50),
  @phoneNumber NVARCHAR(20),
  @email NVARCHAR(100),
  @birthDay INT,
  @birthMonth INT,
  @birthYear INT,
  @firstName NVARCHAR(100),
  @secondName NVARCHAR(100),
  @firstLastName NVARCHAR(100),
  @secondLastName NVARCHAR(100),
  @gender NVARCHAR(10),
  @province NVARCHAR(50),
  @canton NVARCHAR(50),
  @district NVARCHAR(50),
  @otherSigns NVARCHAR(MAX),
  @username NVARCHAR(50),
  @password NVARCHAR(100)
AS
BEGIN
  SET NOCOUNT ON;
  BEGIN TRY
    BEGIN TRANSACTION;

    IF EXISTS (SELECT 1 FROM Persona WHERE identificacion = @idNumber)
      THROW 51000, 'CEDULA_DUPLICADA', 1;
    IF EXISTS (SELECT 1 FROM Persona WHERE numeroTelefono = @phoneNumber)
      THROW 51001, 'TELEFONO_DUPLICADO', 1;
    IF EXISTS (SELECT 1 FROM Persona WHERE correoElectronico = @email)
      THROW 51002, 'EMAIL_DUPLICADO', 1;
    IF EXISTS (SELECT 1 FROM Usuario WHERE nickname = @username)
      THROW 51003, 'USERNAME_DUPLICADO', 1;

    DECLARE @auditId UNIQUEIDENTIFIER = NEWID();
    INSERT INTO Auditoria (id, usuarioCreador)
    VALUES (@auditId, @username);

    DECLARE @personId UNIQUEIDENTIFIER = NEWID();
    INSERT INTO Persona (
      id, identificacion, numeroTelefono, correoElectronico,
      tipoIdentificacion, idAuditoria, fechaNacimiento
    )
    VALUES (
      @personId, @idNumber, @phoneNumber, @email,
      'fisica', @auditId, DATEFROMPARTS(@birthYear, @birthMonth, @birthDay)
    );

    INSERT INTO PersonaFisica (
      id, primerNombre, segundoNombre, primerApellido, segundoApellido, genero
    )
    VALUES (
      @personId, @firstName, @secondName, @firstLastName, @secondLastName, @gender
    );

    INSERT INTO Direccion (
      idPersona, provincia, canton, distrito, otrasSenas
    )
    VALUES (
      @personId, @province, @canton, @district, @otherSigns
    );

    INSERT INTO Usuario (
      idPersonaFisica, nickname, contrasena
    )
    VALUES (
      @personId, @username,
      HASHBYTES('SHA2_512', CONVERT(VARCHAR(100), @password))
    );

    COMMIT;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
      ROLLBACK;

    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
    DECLARE @ErrorState INT = ERROR_STATE();

    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
  END CATCH
END
GO