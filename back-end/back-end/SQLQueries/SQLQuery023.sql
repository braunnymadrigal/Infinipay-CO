USE [InfinipayDB];
GO

CREATE TABLE ApiExterna (
	id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
	idFormula UNIQUEIDENTIFIER NOT NULL,
	metodo VARCHAR(4) NOT NULL,
	headerUnoClave VARCHAR(50) NOT NULL,
	headerUnoValor VARCHAR(500) NOT NULL,
	paramUnoClave VARCHAR(50) NOT NULL,
	paramDosClave VARCHAR(50),
	paramTresClave VARCHAR(50),

	CONSTRAINT CHK_ApiExterna_metodo CHECK (metodo IN ('GET', 'POST')),
	CONSTRAINT UQ_ApiExterna_idFormula UNIQUE (idFormula),
	CONSTRAINT FK_ApiExterna_idFormula FOREIGN KEY (idFormula) REFERENCES Formula(id)
);
GO