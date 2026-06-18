@Cotizacion
Feature: Cotizacion

Como usuario del sistema
Quiero registrar cotizaciones
Para gestionar cotizaciones de clientes

Background:
	Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
	And el usuario accede al módulo 'Cotización'

@RegistrarCotizacion
Scenario Outline: Registro de nueva cotización - Casos variados

	When el usuario selecciona la opción 'Nueva Cotización'
	#And el usuario selecciona la familia '<familia>'
	#And el usuario selecciona el concepto '<concepto>'
	#And el usuario ingresa la cantidad '<cantidad>'
	And el usuario prepara producto para cotizacion con familia '<familia>' concepto '<concepto>' cantidad '<cantidad>' resultado '<resultado_esperado>'
	And el usuario activa IGV '<igv>'
	And el usuario configura descuento '<descuento>' '<tipo_descuento>' '<modo_descuento>' '<valor_descuento>'
	And el usuario busca el cliente '<cliente>'
	And el usuario ingresa la fecha final '<fecha_final>'
	And el usuario registra la cotizacion
	Then el sistema valida el resultado de la cotizacion '<resultado_esperado>'

Examples:
	| caso | familia | concepto      | cantidad | igv   | descuento | tipo_descuento | modo_descuento | valor_descuento | cliente  | fecha_final         | resultado_esperado                    |
	|    1 | Gaseosa | 7753234003320 |       10 | true  | true      | item           | %              |               5 | 00000000 | 20/05/2026 12:00:am | la cotizacion se guardo correctamente |
	|    2 | ninguno | ninguno       |        0 | false | false     | NA             | NA             |               0 | 00000000 | 24/05/2026 01:00:am | Debe seleccionar un producto o servicio |
	|    3 | Gaseosa | 7753234003320 |999999990 | true  | true      | global         | %              |              10 | 75971755 | 19/05/2026 02:00:am | Cantidad debe ser menor al stock      |
	|    4 | Azúcar  | 7751234001115 |       20 | false | true      | item           | $              |               1 | 00000000 | 20/03/2026 12:30:am | Boton de fechas  deshabilitado        |
	|    5 | Gaseosa | 7753234003320 |       10 | false | true      | global         | %              |               5 | 75971755 | 15/05/2026 12:10:am | la cotizacion se guardo correctamente |
	|    6 | Azúcar  | 7751234001115 |       10 | true  | true      | item           | %              |               5 | 00000000 | 20/05/2026 12:00:am | la cotizacion se guardo correctamente |


@EditarCotizacion
Scenario Outline: Editar cotización - Casos variados

	Given existe una cotizacion editable con familia 'Azúcar' concepto '7751234001115' cantidad '10' cliente '00000000' fecha '29/05/2026 12:00:am'
	When el usuario selecciona editar la cotizacion
	And el usuario selecciona la familia '<familia>'
	And el usuario selecciona el concepto '<concepto>'
	And el usuario ingresa la cantidad '<cantidad>'
	And el usuario activa IGV '<igv>'
	And el usuario configura descuento '<descuento>' '<tipo_descuento>' '<modo_descuento>' '<valor_descuento>'
	And el usuario busca el cliente '<cliente>'
	And el usuario ingresa la fecha final '<fecha_final>'
	And el usuario actualiza la cotizacion
	Then el sistema valida el resultado de la cotizacion '<resultado_esperado>'

	Examples:
	| caso | familia   | concepto      | cantidad  | igv       | descuento | tipo_descuento | modo_descuento | valor_descuento | cliente   | fecha_final         | resultado_esperado                 |
	| 1    | NO_CAMBIO | NO_CAMBIO     | 15        | NO_CAMBIO | false     | NA             | NA             | 0               | NO_CAMBIO | NO_CAMBIO           | se registro correctamente          |
	| 2    | NO_CAMBIO | NO_CAMBIO     | NO_CAMBIO | NO_CAMBIO | false     | NA             | NA             | 0               | NO_CAMBIO | NO_CAMBIO           | debe realizar alguna modificacion  |

	@PregenerarVenta
Scenario Outline: Pregenerar venta desde cotizacion - Casos variados

	# 1. Reutilizamos el escudo inteligente para asegurar que haya una cotización en la grilla
	Given existe una cotizacion editable con familia 'Azúcar' concepto '7751234001115' cantidad '10' cliente '00000000' fecha '29/05/2026 12:00:am'
	
	# 2. Hacemos clic en el icono de la canasta (Pregenerar Venta)
	When el usuario hace clic en el icono pregenerar venta
	
	# 3. REUTILIZAMOS LOS STEPS DEL MÓDULO DE VENTAS
	And selecciona el modo de venta '<ModoVenta>'
	And selecciona el punto de venta '<PuntoVenta>'
	And selecciona el vendedor '<Vendedor>'
	And ingresa la fecha de emision '<FechaEmision>'
	And hace clic en Guardar
	
	# 4. Validamos con el consolidador de Cotización
	Then el sistema valida el resultado de venta "<ResultadoEsperado>"

Examples:
	| caso | ModoVenta              | PuntoVenta               | Vendedor                  | FechaEmision  | ResultadoEsperado             |
	|    1 | VENTA MODO CAJA        | ALMACEN CENTRAL          | PAMELA GLORIA TONE RECUAY | NA            | guarda exitosamente           |
	#|    2 | VENTA MODO CAJA        | NA                       | PAMELA GLORIA TONE RECUAY | NA            | Debe completar los campos requeridos |
	#|    3 | VENTA MODO CAJA        | ALMACEN CENTRAL          | NA                        | NA            | Debe completar los campos requeridos |
	|    4 | VENTA POR CONTINGENCIA | NA                       | NA                        | 05/05/2026    | guarda exitosamente           |
	#|    5 | VENTA POR CONTINGENCIA | NA                       | NA                        | 20/04/2026    | Debe completar los campos requeridos |
