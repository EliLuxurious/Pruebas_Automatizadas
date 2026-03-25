@GuiaRemision
Feature: Guia de Remision Remitente

Como usuario del sistema
Quiero emitir una guia de remision
Para validar el traslado de productos

Background:
	Given el usuario accede al modulo correspondiente

@EmitirGuia
Scenario Outline: Validar emision de guia de remision

	When el usuario valida el destinatario autocompletado
	And el usuario ingresa fecha de traslado '<fecha_de_inicio_traslado>'
	And el usuario ingresa peso bruto '<peso_bruto>'
	And el usuario ingresa numero de bultos '<cantidad_bultos>'
	And el usuario selecciona transporte '<tipo_transporte>'
	And el usuario ingresa RUC transportista '<transportista_ruc>'
	And el usuario ingresa DNI conductor '<dni_conductor>'
	And el usuario ingresa licencia '<numero_licencia>'
	And el usuario ingresa placa '<numero_placa>'
	And el usuario selecciona direccion de origen '<direccion_origen>'
	And el usuario selecciona direccion de destino '<direccion_destino>'
	And el usuario emite la guia
	Then el sistema valida el resultado de la guia '<resultado_esperado>'

Examples:
	| caso | fecha_de_inicio_traslado | peso_bruto | cantidad_bultos | tipo_transporte | transportista_ruc | dni_conductor | numero_licencia | numero_placa | direccion_origen           | direccion_destino        | resultado_esperado                  |
	|    1 | Hoy                      |        100 |              10 | Publico         |       20602945589 | NA            | NA              | NA           | Huanuco-Leoncio-Rupa Rupa  | Lima-Lima-Lima           | Guia emitida correctamente          |
	|    2 | Hoy                      |         80 |               5 | Publico         |       00000000000 | NA            | NA              | NA           | Huanuco-Leoncio-Rupa Rupa  | Lima-Lima-Lima           | Transportista debe tener RUC valido |
	|    3 | Ninguno                  |         50 |               2 | Publico         |       20123456789 | NA            | NA              | NA           | Lima-Lima-Lima             | Huanuco-Leoncio-Rupa Rup | Registre la fecha de inicio         |
	|    4 | Hoy                      | Ninguno    | Ninguno         | Publico         |       20123456789 | NA            | NA              | NA           | Huanuco-Leoncio-Rupa Rupa  | Lima-Lima-Lima           | Falta peso y numero de bultos       |
	|    5 | Hoy                      |        100 |              10 | Privado         | NA                |      71310154 | M-71310154      | 2770XS       | Arequipa-Arequipa-Arequipa | Lima-Lima-Lima           | Guia emitida correctamente          |
	|    6 | Hoy                      |         10 |               2 | Privado         | NA                | Ninguno       | NA              | NA           | Huanuco-Leoncio-Rupa Rupa  | Lima-Lima-Lima           | Identifique al conductor con DNI    |
	|    7 | Hoy                      |         10 |               2 | Privado         | NA                |      71310154 | Ninguno         | 2770XS       | Arequipa-Arequipa-Arequipa | Lima-Lima-Lima           | Ingrese numero de licencia          |
	|    8 | Hoy                      |         10 |               2 | Privado         | NA                |      71310154 | M-71310154      | Ninguno      | Huanuco-Leoncio-Rupa Rupa  | Lima-Lima-Lima           | Ingrese numero de placa             |
