use InfinipayDB;
go

ALTER TABLE [DeduccionAPago]
DROP CONSTRAINT CHK_DeduccionAPago_Tipo;
GO

ALTER TABLE DeduccionAPago
DROP COLUMN [tipo];
GO

ALTER TABLE DeduccionAPago
ADD tipo VARCHAR(26) NOT NULL;
GO

ALTER TABLE DeduccionAPago
ADD CONSTRAINT CHK_DeduccionAPago_tipo
CHECK (
		tipo IN (
					'empleado_ccss_sem', 'empleado_ccss_ivm', 'empleado_lpt_bpop', 'empleado_beneficio', 'empleado_renta',
					'empleador_ccss_sem', 'empleador_ccss_ivm', 'empleador_otras_bpop', 'empleador_otras_familiares',
					'empleador_otras_imas', 'empleador_otras_ina', 'empleador_lpt_bpop', 'empleador_lpt_fcl',
					'empleador_lpt_opc', 'empleador_lpt_ins'
				)
	  );
go