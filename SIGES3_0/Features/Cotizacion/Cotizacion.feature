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
	And el usuario selecciona la familia '<familia>'
	And el usuario selecciona el concepto '<concepto>'
	And el usuario ingresa la cantidad '<cantidad>'
	And el usuario activa IGV '<igv>'
	And el usuario configura descuento '<descuento>' '<tipo_descuento>' '<modo_descuento>' '<valor_descuento>'
	And el usuario busca el cliente cotizacion '<cliente>'
	And el usuario ingresa la fecha final '<fecha_final>'
	And el usuario registra la cotizacion
	Then el sistema valida el resultado de la cotizacion '<resultado_esperado>'

Examples:
	| caso | familia | concepto      | cantidad | igv   | descuento | tipo_descuento | modo_descuento | valor_descuento | cliente  | fecha_final         | resultado_esperado                    |
	|    1 | Gaseosa | 7753234003320 |       10 | true  | true      | item           | %              |               5 | 00000000 | 30/04/2026 12:00:am | la cotizacion se guardo correctamente |
	|    2 | ninguno | ninguno       |        0 | false | false     | NA             | NA             |               0 | 00000000 | 30/04/2026 01:00:am | Debe seleccionar un producto o servicio          |
	|    3 | Gaseosa | 7753234003320 |999999990 | true  | true      | global         | %              |              10 | 75971755 | 20/04/2026 02:00:am | Cantidad debe ser menor al stock      |
	|    4 | Azúcar  | 7751234001115 |       20 | false | true      | item           | $              |               1 | 00000000 | 20/03/2026 12:30:am | Boton de fechas  deshabilitado        |
	|    5 | Gaseosa | 7753234003320 |       10 | false | true      | global         | %              |               5 | 75971755 | 21/04/2026 12:10:am | la cotizacion se guardo correctamente |

@EditarCotizacion
Scenario Outline: Editar cotización - Casos variados

	Given existe una cotizacion editable
	When el usuario selecciona editar la cotizacion
	And el usuario selecciona la familia '<familia>'
	And el usuario selecciona el concepto '<concepto>'
	And el usuario ingresa la cantidad '<cantidad>'
	And el usuario activa IGV '<igv>'
	And el usuario configura descuento '<descuento>' '<tipo_descuento>' '<modo_descuento>' '<valor_descuento>'
	And el usuario busca el cliente cotizacion '<cliente>'
	And el usuario ingresa la fecha final '<fecha_final>'
	And el usuario actualiza la cotizacion
	Then el sistema valida el resultado de la cotizacion '<resultado_esperado>'	

	Examples:
	| caso | familia   | concepto      | cantidad  | igv       | descuento | tipo_descuento | modo_descuento | valor_descuento | cliente   | fecha_final         | resultado_esperado                 |
	| 1    | NO_CAMBIO | NO_CAMBIO     | 15        | NO_CAMBIO | false     | NA             | NA             | 0               | NO_CAMBIO | NO_CAMBIO           | se registro correctamente          |
	| 2    | NO_CAMBIO | NO_CAMBIO     | NO_CAMBIO | NO_CAMBIO | false     | NA             | NA             | 0               | NO_CAMBIO | NO_CAMBIO           | debe realizar alguna modificacion  |