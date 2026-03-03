Feature: Registrar una venta

Registrar una venta normal, venta en modo caja y venta por contigencia, cada uno con sus distintos escenarios.

Background:
	Given Inicio de sesion con usuario 'admin@plazafer.com' y contrasena 'calidad'

@NuevaVenta

Scenario: Registro de una nueva venta con pago al contado
	When Seleccionar Venta y luego "Nueva Venta"
	And Activar IGV 'SI'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Nueva Venta"
	And Seleccionar tipo de pago "contado"
	And Seleccionar el medio de pago 'TDEB'
	And Rellene datos de la tarjeta 'BBVA' , 'MASTER' y '206556' en el módulo de "Nueva Venta"
	Then Guardar venta

Scenario: Registro de una nueva venta con pago al crédito rápido
	When Seleccionar Venta y luego "Nueva Venta"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Nueva Venta"
	And Seleccionar tipo de pago "credito rapido"
	And Ingresar monto inicial de crédito rapido '20' en el módulo de "Nueva Venta"
	And Seleccionar el medio de pago 'EF'
	And Rellene datos de la tarjeta '' , '' y '20' en el módulo de "Nueva Venta"
	Then Guardar venta

Scenario: Registro de una nueva venta con pago al crédito rápido sin inicial
	When Seleccionar Venta y luego "Nueva Venta"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Nueva Venta"
	And Seleccionar tipo de pago "credito rapido"
	Then Guardar venta

Scenario: Registro de una nueva venta con pago al crédito configurado
	When Seleccionar Venta y luego "Nueva Venta"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Nueva Venta"
	And Seleccionar tipo de pago "credito configurado"
	And Ingresar monto inicial '10'
	And Ingresar el número de coutas '3'
	And Ingresar fecha '1 de cada mes'
	And Click en generar coutas
	And Click en Aceptar
	And Seleccionar el medio de pago 'TDEB'
	And Rellene datos de la tarjeta 'BBVA' , 'MASTER' y '206556' en el módulo de "Nueva Venta"
	Then Guardar venta

Scenario: Registro de una nueva venta con pago al crédito configurado sin inicial
	When Seleccionar Venta y luego "Nueva Venta"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Nueva Venta"
	And Seleccionar tipo de pago "credito configurado"
	And Ingresar el número de coutas sin inicial '3'
	And Ingresar fecha '1 de cada mes'
	And Click en generar coutas 
	And Click en Aceptar
	Then Guardar venta

@VentaModoCaja

Scenario: Registro de una venta modo caja con pago al contado
	When Seleccionar Venta y luego "Venta Modo Caja"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar un punto de venta 'PRINCIPAL'
	And Seleccionar un vendedor 'KETHY'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Venta Modo Caja"
	And Seleccionar tipo de pago "contado"
	And Seleccionar el medio de pago 'TDEB'
	And Rellene datos de la tarjeta 'BBVA' , 'MASTER' y '206556' en el módulo de "Venta Modo Caja"
	Then Guardar venta

Scenario: Registro de una venta modo caja con pago al crédito rápido
	When Seleccionar Venta y luego "Venta Modo Caja"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar un punto de venta 'PRINCIPAL'
	And Seleccionar un vendedor 'KETHY'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Venta Modo Caja"
	And Seleccionar tipo de pago "credito rapido"
	And Ingresar monto inicial de crédito rapido '20' en el módulo de "Venta Modo Caja"
	And Seleccionar el medio de pago 'TDEB'
	And Rellene datos de la tarjeta 'BBVA' , 'MASTER' y '206556' en el módulo de "Venta Modo Caja"
	Then Guardar venta

Scenario: Registro de una venta modo caja con pago al crédito rápido sin inicial
	When Seleccionar Venta y luego "Venta Modo Caja"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar un punto de venta 'PRINCIPAL'
	And Seleccionar un vendedor 'KETHY'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Venta Modo Caja"
	And Seleccionar tipo de pago "credito rapido"
	Then Guardar venta

Scenario: Registro de una venta modo caja con pago al crédito configurado
	When Seleccionar Venta y luego "Venta Modo Caja"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar un punto de venta 'PRINCIPAL'
	And Seleccionar un vendedor 'KETHY'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Venta Modo Caja"
	And Seleccionar tipo de pago "credito configurado"
	And Ingresar monto inicial '20'
	And Ingresar el número de coutas '3'
	And Ingresar fecha '1 de cada mes'
	And Click en generar coutas
	And Click en Aceptar
	And Seleccionar el medio de pago 'TDEB'
	And Rellene datos de la tarjeta 'BBVA' , 'MASTER' y '206556' en el módulo de "Venta Modo Caja"
	Then Guardar venta

Scenario: Registro de una modo caja con pago al crédito configurado sin inicial
	When Seleccionar Venta y luego "Venta Modo Caja"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar un punto de venta 'PRINCIPAL'
	And Seleccionar un vendedor 'KETHY'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Venta Modo Caja"
	And Seleccionar tipo de pago "credito configurado"
	And Ingresar el número de coutas sin inicial '3'
	And Ingresar fecha '1 de cada mes'
	And Click en generar coutas 
	And Click en Aceptar
	Then Guardar venta

@VentaContingencia

Scenario: Registro de una venta por contigencia con pago al contado
	When Seleccionar Venta y luego "Venta Por Contingencia"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Ingresar fecha de emisión de la venta '30/01/2025'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Venta por Contingencia"
	And Ingresar el número de documento 'B002-10'
	And Seleccionar tipo de pago "contado"
	And Seleccionar el medio de pago 'TDEB'
	And Rellene datos de la tarjeta 'BBVA' , 'MASTER' y '206556' en el módulo de "Venta por Contingencia"
	Then Guardar venta

Scenario: Registro de una venta por contigencia con pago al crédito rápido
	When Seleccionar Venta y luego "Venta Por Contingencia"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Ingresar fecha de emisión de la venta '1/01/2025'
	And Seleccionar tipo de comprobante 'BOLETA' en el módulo de "Venta por Contingencia"
	And Ingresar el número de documento '10'
	And Seleccionar tipo de pago "credito rapido"
	And Ingresar monto inicial de crédito rapido '20' en el módulo de "Venta por Contingencia"
	And Seleccionar el medio de pago 'TDEB'
	And Rellene datos de la tarjeta 'BBVA' , 'MASTER' y '206556' en el módulo de "Venta por Contingencia"
	Then Guardar venta

Scenario: Registro de una venta por contigencia con pago al crédito rápido sin inicial
	When Seleccionar Venta y luego "Venta Por Contingencia"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Ingresar fecha de emisión de la venta '30/01/2025'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Venta por Contingencia"
	And Ingresar el número de documento 'B002-10'
	And Seleccionar tipo de pago "credito rapido"
	Then Guardar venta

Scenario: Registro de una venta por contigencia con pago al crédito configurado
	When Seleccionar Venta y luego "Venta Por Contingencia"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Ingresar fecha de emisión de la venta '30/01/2025'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Venta por Contingencia"
	And Ingresar el número de documento '10'
	And Seleccionar tipo de pago "credito configurado"
	And Ingresar monto inicial '20'
	And Ingresar el número de coutas '3'
	And Ingresar fecha '1 de cada mes'
	And Click en generar coutas
	And Click en Aceptar
	And Seleccionar el medio de pago 'TDEB'
	And Rellene datos de la tarjeta 'BBVA' , 'MASTER' y '206556' en el módulo de "Venta por Contingencia"
	Then Guardar venta

Scenario: Registro de una venta por contigencia con pago al crédito configurado sin inicial
	When Seleccionar Venta y luego "Venta Por Contingencia"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Ingresar fecha de emisión de la venta '30/01/2025'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Venta por Contingencia"
	And Ingresar el número de documento '10'
	And Seleccionar tipo de pago "credito configurado"
	And Ingresar el número de coutas sin inicial '3'
	And Ingresar fecha '1 de cada mes'
	And Click en generar coutas 
	And Click en Aceptar
	Then Guardar venta

@VentaGuiaRemisión

Scenario: Registro de una venta con guía de remisión con transporte público
	When Seleccionar Venta y luego "Nueva Venta"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Seleccionar tipo de comprobante 'BOLETA' en el módulo de "Nueva Venta"
	And Click en el botón Guía
	And Ingresar la fecha de inicio de traslado '10/02/2025'
	And Ingresar el peso bruto total '50'
	And Ingresar el número de bultos '3'
	And Ingresar el RUC del transportista '10614499015'
	And Seleccionar la modalidad del transporte "TRANSPORTE PÚBLICO"
	And Ingresar el ubigeo de la dirección de origen "HUANUCO - LEONCIO PRADO - HERMILIO VALDIZAN"
	And Ingresar el detalle de la dirección de origen "AV. AGRICULTURA"
	And Ingresar el ubigeo de la dirección de destino "HUANUCO - LEONCIO PRADO - DANIEL ALOMIA ROBLES"
	And Ingresar el detalle de la dirección de destino "JR. SVEN ERICSON N° 109"
	And Click en el botón aceptar guía de remisión
	And Seleccionar tipo de pago "contado"
	And Seleccionar el medio de pago 'TDEB'
	And Rellene datos de la tarjeta 'BBVA' , 'MASTER' y '206556' en el módulo de "Nueva Venta"
	Then Guardar venta

Scenario: Registro de una venta con guía de remisión con transporte privada
	When Seleccionar Venta y luego "Nueva Venta"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Seleccionar tipo de comprobante 'BOLETA' en el módulo de "Nueva Venta"
	And Click en el botón Guía
	And Ingresar la fecha de inicio de traslado '10/02/2025'
	And Ingresar el peso bruto total '50'
	And Ingresar el número de bultos '3'
	And Seleccionar la modalidad del transporte "TRANSPORTE PRIVADO"
	And Ingresar el DNI del conductor '71310154'
	And Ingresar la licencia del conductor 'M-71310154'
	And Ingresar la placa del vehículo '2232-8S'
	And Ingresar el ubigeo de la dirección de origen "HUANUCO - LEONCIO PRADO - HERMILIO VALDIZAN"
	And Ingresar el detalle de la dirección de origen "AV. AGRICULTURA"
	And Ingresar el ubigeo de la dirección de destino "HUANUCO - LEONCIO PRADO - DANIEL ALOMIA ROBLES"
	And Ingresar el detalle de la dirección de destino "JR. SVEN ERICSON N° 109"
	And Click en el botón aceptar guía de remisión
	And Seleccionar tipo de pago "contado"
	And Seleccionar el medio de pago 'TDEB'
	And Rellene datos de la tarjeta 'BBVA' , 'MASTER' y '206556' en el módulo de "Nueva Venta"
	Then Guardar venta

@VentaDetalleUnificado

Scenario: Registro de una venta con detalle unificado
	When Seleccionar Venta y luego "Nueva Venta"
	When Agregar los siguientes conceptos:
    | option | value |
    | BARRA  | 108300559 |
    | SELECCION  | 400001474 |
    | SELECCION  | 400000437 |
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Activar Detalle Unificado 'SI'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Nueva Venta"
	And Seleccionar tipo de pago "contado"
	And Seleccionar el medio de pago 'EF'
	And Rellene datos de la tarjeta '' , '' y '200' en el módulo de "Nueva Venta"
	Then Guardar venta

@TipoEntrega

Scenario: Registro de una nueva venta con entrega diferida o inmediata
	When Seleccionar Venta y luego "Nueva Venta"
	And Agregar concepto por 'barra' y valor '108300559'
	And Ingresa los siguientes datos del producto:
    | Campo            | Valor |
    | Cantidad         | 2     |
    | Precio Unitario  | 30    |
	And Activar IGV 'SI'
	And Seleccionar tipo de cliente 'DNI' '72380461'
	And Seleccionar tipo de comprobante 'NOTA' en el módulo de "Nueva Venta"
	And Seleccionar tipo de entrega 'INMEDIATA'
	And Hola ''
	And Seleccionar tipo de pago "contado"
	And Seleccionar el medio de pago 'TDEB'
	And Rellene datos de la tarjeta 'BBVA' , 'MASTER' y '206556' en el módulo de "Nueva Venta"
	Then Guardar venta