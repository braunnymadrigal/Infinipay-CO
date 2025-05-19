-- needed to restrict Login access

USE [InfinipayDB];
GO

ALTER TABLE [Usuario]
ADD [numIntentos] TINYINT, [fechaExactaBloqueo] DATETIME2;

-- needed to restrict Login access