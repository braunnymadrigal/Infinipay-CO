﻿USE [InfinipayDB];
GO

CREATE PROCEDURE sp_CreateNewEmployee
  @idNumber NVARCHAR(50),
  @phoneNumber NVARCHAR(50),
  @email NVARCHAR(100),
  @firstName NVARCHAR(50),
  @secondName NVARCHAR(50),
  @firstLastName NVARCHAR(50),
  @secondLastName NVARCHAR(50),
  @gender NVARCHAR(10),
  @province NVARCHAR(50),
  @canton NVARCHAR(50),
  @district NVARCHAR(50),
  @otherSigns NVARCHAR(255),
  @username NVARCHAR(50),
  @password NVARCHAR(128),
  @birthDay INT,
  @birthMonth INT,
  @birthYear INT,
  @hireDay INT,
  @hireMonth INT,
  @hireYear INT,
  @role NVARCHAR(50),
  @creationDay INT,
  @creationMonth INT,
  @creationYear INT,
  @reportsHours BIT,
  @salary DECIMAL(18,2),
  @typeContract NVARCHAR(50),
  @loggedPersonId UNIQUEIDENTIFIER
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

    DECLARE @loggedUsername NVARCHAR(50);
    SELECT TOP 1 @loggedUsername = nickname
    FROM Usuario
    WHERE idPersonaFisica = @loggedPersonId;

    IF @loggedUsername IS NULL
      THROW 51004, 'USUARIO_LOGUEADO_INVALIDO', 1;

    DECLARE @auditId UNIQUEIDENTIFIER = NEWID();
    INSERT INTO Auditoria (id, usuarioCreador)
    VALUES (@auditId, @loggedUsername);

    DECLARE @personId UNIQUEIDENTIFIER = NEWID();
    DECLARE @birthDate DATE = DATEFROMPARTS(@birthYear, @birthMonth, @birthDay);

    INSERT INTO Persona (id, identificacion, numeroTelefono, correoElectronico,
                         tipoIdentificacion, idAuditoria, fechaNacimiento)
    VALUES (@personId, @idNumber, @phoneNumber, @email,
            'fisica', @auditId, @birthDate);

    INSERT INTO PersonaFisica (id, primerNombre, segundoNombre, primerApellido,
                                segundoApellido, genero)
    VALUES (@personId, @firstName, @secondName, @firstLastName,
            @secondLastName, @gender);

    INSERT INTO Direccion (idPersona, provincia, canton, distrito, otrasSenas)
    VALUES (@personId, @province, @canton, @district, @otherSigns);

    INSERT INTO Usuario (idPersonaFisica, nickname, contrasena)
    VALUES (@personId, @username,
        HASHBYTES('SHA2_512', CONVERT(VARCHAR(100), @password)));

    DECLARE @hireDate DATE = DATEFROMPARTS(@hireYear, @hireMonth, @hireDay);
    INSERT INTO Empleado (idPersonaFisica, rol, fechaContratacion, idEmpleadorContratador)
    VALUES (@personId, @role, @hireDate, @loggedPersonId);

    DECLARE @creationDate DATE = DATEFROMPARTS(@creationYear, @creationMonth, @creationDay);
    INSERT INTO Contrato (reportaHoras, fechaCreacion, salarioBruto, tipoContrato, idEmpleado)
    VALUES (@reportsHours, @creationDate, @salary, @typeContract, @personId);

    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    ROLLBACK TRANSACTION;
    DECLARE @errMsg NVARCHAR(4000), @errSeverity INT, @errState INT;
    SELECT @errMsg = ERROR_MESSAGE(), @errSeverity = ERROR_SEVERITY(), @errState = ERROR_STATE();
    RAISERROR(@errMsg, @errSeverity, @errState);
  END CATCH
END;

select * from persona