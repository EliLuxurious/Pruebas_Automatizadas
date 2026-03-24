Feature: Reporte de pedidos
  Como usuario del módulo Pedidos
  Quiero consultar reportes de pedidos usando filtros
  Para validar que el sistema permita o rechace combinaciones de fechas correctamente

Background:
	Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
	And el usuario accede al módulo 'Pedidos'
	And el usuario accede al submodulo de reportes

@ReportePedidos @FiltroFechas
Scenario Outline: Validar filtro de fechas en reporte de pedidos
	When el usuario selecciona el establecimiento "<establecimiento>"
	And el usuario selecciona el punto de venta "<puntoVenta>"
	And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
	And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
	And el usuario hace clic en ver reporte "<tipoReporte>"
	Then el sistema muestra el resultado esperado del reporte "<resultadoEsperado>"

Examples:
	| Caso | establecimiento | puntoVenta | fechaHoraInicial    | fechaHoraFinal      | tipoReporte | resultadoEsperado              |
	|    1 | Todos           | Todos      | 10/03/2026 12:00 am | 09/03/2026 11:59 pm | Invalidados | No permite aplicar el filtro   |
	|    2 | Todos           | Todos      | 09/03/2026 12:00 am | 23/03/2026 11:59 pm | Invalidados | Aplica el filtro correctamente |
	|    3 | Todos           | Todos      | 01/03/2026 12:00 am | 12/03/2026 11:59 pm | Invalidados | Aplica el filtro correctamente |
	|    4 | Todos           | Todos      | hoy                 | hoy                 | Invalidados | Aplica el filtro correctamente |
