------------------------------------------------------------------------------------------
create procedure sp_deleteCompany
	@mailEmployer varchar(100)
as
begin
	set nocount on;
	begin try
		set transaction isolation level serializable;
		begin transaction delete_all;
			declare @idEmployer uniqueidentifier; set @idEmployer = (select id from Persona where correoElectronico = @mailEmployer);
			declare @idCompany uniqueidentifier; set @idCompany = (select idPersonaJuridica from Empleador where idPersonaFisica = @idEmployer);

			declare @idEmployees   table(id uniqueidentifier primary key clustered); 
			declare @idBenefits	   table(id uniqueidentifier primary key clustered);
			declare @idDeductions  table(id uniqueidentifier primary key clustered, idFormula uniqueidentifier);
			declare @idFormulas    table(id uniqueidentifier primary key clustered);
			declare @idAudits      table(id uniqueidentifier primary key clustered);
			declare @idDedPayments table(id uniqueidentifier primary key clustered); 

			insert into @idEmployees    select idPersonaFisica from Empleado where idEmpleadorContratador = @idEmployer;
			insert into @idBenefits     select id from Beneficio where idPersonaJuridica = @idCompany;
			insert into @idDeductions   select id, idFormula from Deduccion where idPersonaJuridica = @idCompany;
			insert into @idFormulas     select f.id from Formula f where  f.id in (select idFormula from @idDeductions);
			insert into @idAudits       select a.id from Auditoria a inner join Persona p on p.idAuditoria = a.id where p.id in (select id from @idEmployees) or p.id = @idEmployer or p.id = @idCompany 
							    union 
							    select a.id from Auditoria a inner join Beneficio b on b.idAuditoria = a.id where b.id in (select id from @idBenefits) 
							    union 
							    select a.id from Auditoria a inner join Planilla p on p.idAuditoria = a.id where p.idPersonaJuridica = @idCompany 
							    union 
							    select a.id from Auditoria a inner join Deduccion d on d.idAuditoria = a.id where d.id in (select id from @idDeductions);
			insert into @idDedPayments  select d.id from DeduccionAPago d inner join DetallePago p on d.idDetallePago = p.id where p.idEmpleado in (select id from @idEmployees);

			if exists(select id from Planilla where idPersonaJuridica = @idCompany)
			begin
				update Direccion     set borrado = 1 where idPersona       in (select id from @idEmployees) or idPersona = @idEmployer or idPersona = @idCompany;
				update Usuario       set borrado = 1 where idPersonaFisica in (select id from @idEmployees) or idPersonaFisica = @idEmployer;
				update Persona       set borrado = 1 where id              in (select id from @idEmployees) or id = @idEmployer or id = @idCompany;
				update PersonaFisica set borrado = 1 where id              in (select id from @idEmployees) or id = @idEmployer;

				update Contrato             set borrado = 1 where idEmpleado in (select id from @idEmployees);
				update Horas                set borrado = 1 where idEmpleado in (select id from @idEmployees);
				update EmpleadoSupervision  set borrado = 1 where idEmpleado in (select id from @idEmployees);
				update EmpleadoDePlanilla   set borrado = 1 where idEmpleado in (select id from @idEmployees);
				update BeneficioPorEmpleado set borrado = 1 where idEmpleado in (select id from @idEmployees);
				update DetallePago          set borrado = 1 where idEmpleado in (select id from @idEmployees);

				update Empleado        set borrado = 1 where idEmpleadorContratador = @idEmployer;
				update PersonaJuridica set borrado = 1 where id = @idCompany;
				update Empleador       set borrado = 1 where idPersonaFisica = @idEmployer;
				update Beneficio       set borrado = 1 where idPersonaJuridica = @idCompany;
				update Planilla        set borrado = 1 where idPersonaJuridica = @idCompany;
				update Deduccion       set borrado = 1 where idPersonaJuridica = @idCompany;

				update Auditoria set borrado = 1 where id in (select id from @idAudits);

				update DeduccionAPago set borrado = 1 where id in (select id from @idDedPayments);

				update Formula    set borrado = 1 where id        in (select id from @idFormulas);
				update ApiExterna set borrado = 1 where idFormula in (select id from @idFormulas); 
			end
			else
			begin
				delete from Direccion            where idPersona       in (select id from @idEmployees) or idPersona = @idEmployer or idPersona = @idCompany;
				delete from Usuario              where idPersonaFisica in (select id from @idEmployees) or idPersonaFisica = @idEmployer;
				delete from Contrato             where idEmpleado      in (select id from @idEmployees);
				delete from Horas                where idEmpleado      in (select id from @idEmployees);
				delete from EmpleadoSupervision  where idEmpleado      in (select id from @idEmployees);
				delete from EmpleadoDePlanilla   where idEmpleado      in (select id from @idEmployees);
				delete from BeneficioPorEmpleado where idEmpleado      in (select id from @idEmployees);
				delete from DeduccionAPago       where id              in (select id from @idDedPayments);
				delete from ApiExterna           where idFormula       in (select id from @idFormulas);

				delete from DetallePago          where idEmpleado in (select id from @idEmployees);

				delete from Empleado             where idEmpleadorContratador = @idEmployer;
				delete from Planilla             where idPersonaJuridica = @idCompany;

				delete from Empleador            where idPersonaFisica = @idEmployer;
				delete from Deduccion            where idPersonaJuridica = @idCompany;

				delete from Formula              where id        in (select id from @idFormulas);
				delete from Beneficio            where idPersonaJuridica = @idCompany;
				delete from PersonaFisica        where id              in (select id from @idEmployees) or id = @idEmployer;

				delete from PersonaJuridica      where id = @idCompany;

				delete from Persona              where id              in (select id from @idEmployees) or id = @idEmployer or id = @idCompany;

				delete from Auditoria            where id in (select id from @idAudits);
			end

			commit transaction delete_all;
	end try
	begin catch
		if @@TRANCOUNT > 0
		begin
			rollback transaction delete_all;
			declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE();
			declare @ErrorSeverity int           = ERROR_SEVERITY();
			declare @ErrorState int              = ERROR_STATE();
			raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);
		end
	end catch
end
------------------------------------------------------------------------------------------
GO