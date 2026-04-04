@GuiaRemision
Feature: Guia de Remision Remitente

Como usuario del sistema
Quiero emitir una guia de remision
Para validar el traslado de productos

Background:
	Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
	And el usuario accede al módulo 'Pedidos'
	And el usuario accede al submodulo 'Ver Pedidos'

@EmitirGuia
Scenario Outline: Validar emision de guia de remision
	Given existe un pedido base para emitir guia con familia 'Azúcar' concepto '7751234001115' cantidad '20' cliente '75971755' entrega 'diferida'
	When el usuario abre el flujo de guia de remision con comprobante 'boleta de venta electronica' serie 'B002' cliente '75971755' entrega 'inmediata'
	And el usuario valida el destinatario autocompletado
	And el usuario ingresa fecha de traslado '<fecha_de_inicio_traslado>'
	And el usuario ingresa peso bruto '<peso_bruto>'
	And el usuario ingresa numero de bultos '<cantidad_bultos>'
	And el usuario selecciona transporte '<tipo_transporte>'
	And el usuario ingresa RUC transportista '<transportista_ruc>'
	And el usuario ingresa licencia '<numero_licencia>'
	And el usuario ingresa placa '<numero_placa>'
	And el usuario selecciona direccion de origen '<direccion_origen>'
	And el usuario selecciona detalle de direccion de origen '<detalle_origen>'
	And el usuario selecciona direccion de destino '<direccion_destino>'
	And el usuario selecciona detalle de direccion de destino '<detalle_destino>'
	And el usuario emite la guia
	Then el sistema valida el resultado de la guia '<resultado_esperado>'

Examples:
	| caso | fecha_de_inicio_traslado | peso_bruto | cantidad_bultos | tipo_transporte | transportista_ruc | numero_licencia | numero_placa | direccion_origen           | detalle_origen | direccion_destino        | detalle_destino | resultado_esperado                  |
	|    1 | Hoy                      |        100 |              10 | Publico         |       20602945589 | NA              | NA           | Huanuco-Leoncio-Rupa Rupa  | Av amazonas C9 | Lima-Lima-Lima           | Av San Juna C1  | Guia emitida correctamente          |
	|    2 | Hoy                      |         80 |               5 | Publico         |       ninguno     | NA              | NA           | Huanuco-Leoncio-Rupa Rupa  | Av amazonas C9 | Lima-Lima-Lima           | Av San Juna C1  | Transportista debe tener RUC valido |
	#|    3 | Ninguno                  |         50 |               2 | Publico         |       20123456789 | NA              | NA           | Lima-Lima-Lima             | Av amazonas C9 | Huanuco-Leoncio-Rupa Rup | Av San Juna C1  | Registre la fecha de inicio         |
	|    4 | Hoy                      | Ninguno    | Ninguno         | Publico         |       20123456789 | NA              | NA           | Huanuco-Leoncio-Rupa Rupa  | Av amazonas C9 | Lima-Lima-Lima           | Av San Juna C1  | Falta peso y numero de bultos       |
	|    5 | Hoy                      |        100 |              10 | Privado         | NA                | M-71310154      | 2770XS       | Arequipa-Arequipa-Arequipa | Av amazonas C9 | Lima-Lima-Lima           | Av San Juna C1  | Guia emitida correctamente          |
	|    6 | Hoy                      |         10 |               2 | Privado         | NA                | NA              | NA           | Huanuco-Leoncio-Rupa Rupa  | Av amazonas C9 | Lima-Lima-Lima           | Av San Juna C1  | Identifique al conductor con DNI    |
	|    7 | Hoy                      |         10 |               2 | Privado         | NA                | Ninguno         | 2770XS       | Arequipa-Arequipa-Arequipa | Av amazonas C9 | Lima-Lima-Lima           | Av San Juna C1  | Ingrese numero de licencia          |
	|    8 | Hoy                      |         10 |               2 | Privado         | NA                | M-71310154      | Ninguno      | Huanuco-Leoncio-Rupa Rupa  | Av amazonas C9 | Lima-Lima-Lima           | Av San Juna C1  | Ingrese numero de placa             |



