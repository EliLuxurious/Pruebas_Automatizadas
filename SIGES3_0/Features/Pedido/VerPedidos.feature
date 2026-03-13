@Pedido
Feature: VerPedidos

Como usuario del sistema
Quiero registrar pedidos
Para gestionar pedidos de clientes

Background:
	Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
	And el usuario accede al módulo 'Pedidos'
	And el usuario accede al submodulo 'Ver Pedidos'
	


@RegistrarPedido
Scenario Outline: Registro de nuevo pedido - Casos variados

	And el usuario selecciona la opción 'Nuevo Pedido'

# Selección de producto

	When el usuario selecciona la familia '<familia>'
	And el usuario selecciona el concepto '<concepto>'
	And el usuario ingresa la cantidad '<cantidad>'

# Opciones del pedido

	And el usuario activa IGV '<igv>'
	And el usuario activa DET.UNIF '<det_unif>'

# Configuración de descuento

	And el usuario configura descuento '<descuento>' '<tipo_descuento>' '<modo_descuento>' '<valor_descuento>'

# Facturación

	And el usuario abre la sección 'Facturación'
	And el usuario busca el cliente '<cliente>'

# Entrega

	And el usuario abre la sección 'Entrega'
	And el usuario selecciona tipo de entrega '<tipo_entrega>'

# Registro

	And el usuario registra el pedido
	Then el sistema valida '<resultado_esperado>'


Examples:
	| caso | familia | concepto      | cantidad | igv   | det_unif | descuento | tipo_descuento | modo_descuento | valor_descuento | cliente  | tipo_entrega | resultado_esperado                          |
	|    1 | Gaseosa | 7753234003320 |       10 | false | false    | false     | NA             | NA             |               0 | 00000000 | inmediata    | el pedido se guardo correctamente           |
	|    2 | ninguno | ninguno       |        0 | false | false    | false     | NA             | NA             |               0 | 75971755 | diferida     | Ningún producto seleccionado                |
	|    3 | Gaseosa | 7753234003313 |       12 | true  | true     | true      | item           | $              |               1 | 00000000 | inmediata    | el pedido se guardo correctamente           |
	|    4 | Azúcar  | 7751234001115 |       20 | false | false    | true      | global         | %              |              10 | 75893616 | diferida     | el pedido se guardo correctamente           |
	|    5 | Gaseosa | 7753234003313 |     5000 | false | false    | false     | NA             | NA             |               0 | 75971755 | inmediata    | La cantidad debe ser menor o igual al stock |


@InvalidarPedido
Scenario Outline: Invalidar pedido - Casos variados

	When el usuario selecciona la opcion 'Invalidar pedido'
	And el usuario ingresa el motivo '<motivo>'
	And el usuario confirma '<accion>'
	Then el sistema valida '<resultado>'

Examples:
	| caso | motivo           | accion | resultado                              |
	|    1 | Producto agotado | SI     | el pedido se Invalido correctamente    |
	|    2 | ninguno          | SI     | Boton SI deshabilitado                 |


