<template>
	<div class="container my-5">
		<div class="text-center mb-4">
			<img src="../assets/images/logo.png" alt="Company logo" class="img-fluid"
				style="max-width: 350px;">
		</div>

		<header class="mb-5 custom-header">
			<nav class="navbar navbar-expand-lg rounded custom-navbar">
				<div class="container-fluid">
					<div class="d-flex">
						<div class="btn btn-outline-primary me-2 custom-navbar-btn"
							style="color: #758694; border: transparent; ">
							Iniciar sesión
						</div>
						<div class="btn btn-outline-primary me-2 custom-navbar-btn"
							style="color: #758694; border: transparent; ">
							Registrá tu empresa
						</div>
						<div class="btn btn-outline-primary me-2 custom-navbar-btn"
							style="color: #758694; border: transparent; ">
							Beneficios
						</div>
						<div class="btn btn-outline-primary me-2 custom-navbar-btn"
							style="color: #758694; border: transparent; ">
							Empleados
						</div>
					</div>
				</div>
			</nav>
		</header>

		<div class="text-center mb-5">
			<h2 class="fw-bold"> [Nombre Empresa] </h2>
			<h3 class="text-muted"> Empleados Supervisados </h3>
		</div>

		<div class="mb-5 text-start">
			<label for="search" class="me-2 fw-semibold"
				style="font-size: 20px"> Buscar:
			</label>
			<input type="text" id="search" class="form-control d-inline-block"
				style="max-width: 300px; width: auto; height: 30px;
					background: transparent; border-radius: 10px"
			/>
		</div>

		<div v-for="(employee, index) in employees" :key="index" class="mb-2">
			<button
				class="btn btn-outline-secondary w-100 text-start d-flex
				justify-content-between align-items-center"
				@click="toggleEmployee(index);"
			>
				<span>{{employee.name}}</span>
				<span :class="{'rotate-180': employee.open,
							'rotate--180': !employee.open}">⌄</span>
			</button>

			<div v-if="employee.open" class="border rounded p-3 mt-2">
				<div class="row">
					<div class="col-md-6 mb-2">
						<label class="fw-semibold"> ¿Reporta Horas? </label>
						<input type="text" class="form-control"
							:value="employee.reportaHoras" readonly/>
					</div>
					<div class="col-md-6 mb-2">
						<label class="fw-semibold"> Fecha de Contratación </label>
						<input type="text" class="form-control"
							:value="employee.fechaContratacion" readonly/>
					</div>
					<div class="col-md-6 mb-2">
						<label class="fw-semibold"> Salario Bruto </label>
						<input type="text" class="form-control"
							:value="employee.salarioBruto" readonly />
					</div>
					<div class="col-md-6 mb-2">
						<label class="fw-semibold"> Horas Reportadas </label>
						<input type="text" class="form-control"
							:value="employee.horasReportadas" readonly />
					</div>
					<div class="col-md-6 mb-2"> </div>
					<div class="col-md-6 mb-2">
						<div class="d-flex gap-2 mt-2">
							<button class="btn btn-outline-success w-100" @click="toggleEmployee(index);">
								Aprobar
							</button>
							<button class="btn btn-outline-danger w-100" @click="toggleEmployee(index);">
								Denegar
							</button>
						</div>
					</div>
				</div>
			</div>
		</div>

		<div class="col-md-6 mb-2">
		</div>
			<button class="btn btn-outline-primary w-10">
				<span>{{ '<' }}</span>
			</button>
				<span class="ms-2 me-2">{{10/2}}/10</span>
			<button class="btn btn-outline-primary w-10">
				<span>{{ '>' }}</span>
			</button>
	</div>

	<footer class="py-5 custom-footer">
		<div class="container">
			<div class="row">
				<div class="col-md-3 mb-3">
					<p class="h5">Infinipay CO.</p>
					<div>
						<a href="#" class="fa fa-facebook"></a>
						<a href="#" class="fa fa-linkedin"></a>
						<a href="#" class="fa fa-youtube"></a>
						<a href="#" class="fa fa-instagram"></a>
					</div>
				</div>
				<div class="col-md-3 mb-3">
					<p class="h5">Empresa y equipo</p>
					<a href="#">Sobre nosotros</a>
				</div>
				<div class="col-md-3 mb-3">
					<p class="h5">Recursos</p>
					<a href="#">¿Cómo registro mi empresa?</a><br />
					<a href="#">¿Cómo registro empleados a mi empresa?</a><br />
					<a href="#">¿Cómo accedo a mi perfil?</a>
				</div>
				<div class="col-md-3 mb-3">
					<p class="h5">Contacto</p>
					<p>
						<i class="pi pi-phone" style="color:#405D72;"></i>
						+506 2000-0000
					</p>
					<p>
						<i class="pi pi-home" style="color:#405D72;"></i>
						San José, Montes de Oca, San Pedro
					</p>
				</div>
			</div>
		</div>
	</footer>
</template>

<script>
	export default {
		data() {
			return {
				employees: [
					{
						name: 'Juan David Perez Hidalgo',
						reportaHoras: 'Sí',
						salarioBruto: '₡600,000',
						fechaContratacion: '2024-04-26',
						horasReportadas: 12,
						open: false,
					},
					{
						name: 'Rosalina Guimaraes Azofeifa',
						reportaHoras: 'Sí',
						salarioBruto: '₡600,000',
						fechaContratacion: '2024-04-26',
						horasReportadas: 12,
						open: false,
					},
				],
				activeItem: null,
			};
		},
		methods: {
			toggleEmployee(index) {
				this.employees[index].open = !this.employees[index].open;
			},
		},
	};
</script>

<style>
  .custom-navbar-btn:hover {
    background-color: #F7E7DC !important;
  }

  .rotate-180 {
    transform: rotate(180deg);
    transition: transform 0.5s ease-out;
  }

	.rotate--180 {
    transform: rotate(-90deg);
    transition: transform 0.3s ease-out;
  }

	@import '../assets/css/HeaderFooter.css';
</style>
