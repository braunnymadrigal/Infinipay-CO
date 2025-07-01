USE InfinipayDB;
ALTER TABLE BeneficioPorEmpleado ADD ElegibleParaPlanilla BIT;
----------------------------------------------------------------------------------
UPDATE BeneficioPorEmpleado set ElegibleParaPlanilla = 1;
----------------------------------------------------------------------------------
ALTER TABLE BeneficioPorEmpleado ALTER COLUMN ElegibleParaPlanilla BIT NOT NULL;
----------------------------------------------------------------------------------
ALTER TABLE BeneficioPorEmpleado ADD CONSTRAINT df_BeneficiosPorEmpleado DEFAULT 1 FOR ElegibleParaPlanilla;
