alter table ApiExterna add borrado bit;
alter table Auditoria add borrado bit;
alter table Beneficio add borrado bit;
alter table BeneficioPorEmpleado add borrado bit;
alter table Contrato add borrado bit;
alter table Deduccion add borrado bit;
alter table DeduccionAPago add borrado bit;
alter table DetallePago add borrado bit;
alter table Direccion add borrado bit;
alter table Empleado add borrado bit;
alter table EmpleadoDePlanilla add borrado bit;
alter table Empleador add borrado bit;
alter table EmpleadoSupervision add borrado bit;
alter table Formula add borrado bit;
alter table Horas add borrado bit;
alter table Persona add borrado bit;
alter table PersonaFisica add borrado bit;
alter table PersonaJuridica add borrado bit;
alter table Planilla add borrado bit;
alter table Usuario add borrado bit;
------------------------------------------------------------------------------------------

update ApiExterna set borrado = 0;
update Auditoria set borrado = 0;
update Beneficio set borrado = 0;
update BeneficioPorEmpleado set borrado = 0;
update Contrato set borrado = 0;
update Deduccion set borrado = 0;
update DeduccionAPago set borrado = 0;
update DetallePago set borrado = 0;
update Direccion set borrado = 0;
update Empleado set borrado = 0;
update EmpleadoDePlanilla set borrado = 0;
update Empleador set borrado = 0;
update EmpleadoSupervision set borrado = 0;
update Formula set borrado = 0;
update Horas set borrado = 0;
update Persona set borrado = 0;
update PersonaFisica set borrado = 0;
update PersonaJuridica set borrado = 0;
update Planilla set borrado = 0;
update Usuario set borrado = 0;
------------------------------------------------------------------------------------------

alter table ApiExterna alter column borrado bit not null;
alter table Auditoria alter column borrado bit not null;
alter table Beneficio alter column borrado bit not null;
alter table BeneficioPorEmpleado alter column borrado bit not null;
alter table Contrato alter column borrado bit not null;
alter table Deduccion alter column borrado bit not null;
alter table DeduccionAPago alter column borrado bit not null;
alter table DetallePago alter column borrado bit not null;
alter table Direccion alter column borrado bit not null;
alter table Empleado alter column borrado bit not null;
alter table EmpleadoDePlanilla alter column borrado bit not null;
alter table Empleador alter column borrado bit not null;
alter table EmpleadoSupervision alter column borrado bit not null;
alter table Formula alter column borrado bit not null;
alter table Horas alter column borrado bit not null;
alter table Persona alter column borrado bit not null;
alter table PersonaFisica alter column borrado bit not null;
alter table PersonaJuridica alter column borrado bit not null;
alter table Planilla alter column borrado bit not null;
alter table Usuario alter column borrado bit not null;
------------------------------------------------------------------------------------------

alter table ApiExterna add constraint df_ApiExterna_borrado default 0 for borrado;
alter table Auditoria add constraint df_Auditoria_borrado default 0 for borrado;
alter table Beneficio add constraint df_Beneficio_borrado default 0 for borrado;
alter table BeneficioPorEmpleado add constraint df_BeneficioPorEmpleado_borrado default 0 for borrado;
alter table Contrato add constraint df_Contrato_borrado default 0 for borrado;
alter table Deduccion add constraint df_Deduccion_borrado default 0 for borrado;
alter table DeduccionAPago add constraint df_DeduccionAPago_borrado default 0 for borrado;
alter table DetallePago add constraint df_DetallePago_borrado default 0 for borrado;
alter table Direccion add constraint df_Direccion_borrado default 0 for borrado;
alter table Empleado add constraint df_Empleado_borrado default 0 for borrado;
alter table EmpleadoDePlanilla add constraint df_EmpleadoDePlanilla_borrado default 0 for borrado;
alter table Empleador add constraint df_Empleador_borrado default 0 for borrado;
alter table EmpleadoSupervision add constraint df_EmpleadoSupervision_borrado default 0 for borrado;
alter table Formula add constraint df_Formula_borrado default 0 for borrado;
alter table Horas add constraint df_Horas_borrado default 0 for borrado;
alter table Persona add constraint df_Persona_borrado default 0 for borrado;
alter table PersonaFisica add constraint df_PersonaFisica_borrado default 0 for borrado;
alter table PersonaJuridica add constraint df_PersonaJuridica_borrado default 0 for borrado;
alter table Planilla add constraint df_Planilla_borrado default 0 for borrado;
alter table Usuario add constraint df_Usuario_borrado default 0 for borrado;
------------------------------------------------------------------------------------------
GO