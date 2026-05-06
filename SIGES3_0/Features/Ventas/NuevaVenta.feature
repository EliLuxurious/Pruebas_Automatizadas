Feature: Nueva Venta 

    # Casos CP-V1 a CP-V9 derivados de la tabla de decisión de Venta.
    # Técnicas: Tabla de Decisión + Partición de Equivalencia + Valores Límite.
    #
    # Condiciones evaluadas:
    #   - Modo venta: Normal, Modo Caja, Por Contingencia
    #   - Fecha de emisión (aplica a Modo Caja y Contingencia)
    #   - Vendedor (aplica a Modo Caja)
    #   - IGV / Detalle Unificado
    #   - Tipo de comprobante: Factura / Boleta / Nota de Venta
    #   - Cliente: DNI / RUC / VARIOS
    #   - Importe Total > 700 (Cantidad 150 × ~S/7 ≈ S/1 050)
    #   - Tipo de entrega: Inmediata / Diferida
    #   - Pago: Contado completo / Incompleto
    #
    # Acciones verificadas (columna ResultadoEsperado):
    #   "guarda exitosamente"                      → Guardar habilitado y ejecutado
    #   "inconsistencia: ruc requerido"             → Guardar deshabilitado (Factura + DNI)
    #   "inconsistencia: identificar cliente"       → Guardar deshabilitado (Boleta + VARIOS + total > 700)
    #   "pago no completado"                        → Guardar deshabilitado (pago insuficiente)
    #   "inconsistencia: contingencia fuera plazo"  → Guardar deshabilitado (contingencia expirada)

    Background:
        Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
        When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
        And el usuario accede al módulo 'Ventas'
        And el usuario accede al submodulo 'Nueva Venta'

    @NuevaVenta 
    @VentaNormalCaja
    Scenario Outline: Registro de venta Modo Normal o Modo Caja
        When selecciona el modo de venta "<ModoVenta>"
        And configura IGV "<IGV>" y Detalle Unificado "<DetUnificado>"
        And el usuario selecciona la familia 'Gaseosa'
        And el usuario selecciona el concepto '7753234003313'
        And el usuario ingresa la cantidad '<Cantidad>'
        And configura la facturacion '<Comprobante>' '<Serie>' '<Cliente>'
        And selecciona el punto de venta '<PuntoVenta>'
        And selecciona el vendedor '<Vendedor>'
        And el usuario configura la entrega '<Entrega>' '<GuiaRemision>'
        And configura el pago "<Pago>"
        And hace clic en Guardar
        Then el sistema valida el resultado de venta "<ResultadoEsperado>"
        Examples:
            | Caso  | ModoVenta       | IGV | DetUnificado | Cantidad | PuntoVenta               | Vendedor                  | Comprobante                 | Serie | Cliente     | Entrega   | GuiaRemision | Pago       | ResultadoEsperado                   |
            | CP-V1 | VENTA NORMAL    | Y   | Y            | 1        | -                        | -                         | FACTURA ELECTRONICA         | F002  | 75893616    | Inmediata | false        | Contado    | inconsistencia: ruc requerido       |
            | CP-V2 | VENTA MODO CAJA | Y   | Y            | 1        | ALMACEN CENTRAL          | PAMELA GLORIA TONE RECUAY | FACTURA ELECTRONICA         | F002  | 20542245671 | Inmediata | false        | Contado    | guarda exitosamente                 |
            | CP-V3 | VENTA NORMAL    | N   | N            | 150      | -                        | -                         | BOLETA DE VENTA ELECTRONICA | B002  | 00000000    | Inmediata | false        | Contado    | inconsistencia: identificar cliente |
            | CP-V4 | VENTA NORMAL    | N   | N            | 1        | -                        | -                         | BOLETA DE VENTA ELECTRONICA | B002  | 00000000    | Inmediata | false        | Contado    | guarda exitosamente                 |
            | CP-V5 | VENTA NORMAL    | N   | Y            | 150      | -                        | -                         | BOLETA DE VENTA ELECTRONICA | B002  | 75893616    | Diferida  | false        | Contado    | guarda exitosamente                 |
            | CP-V6 | VENTA MODO CAJA | N   | Y            | 150      | CENTRO COMERCIAL CENTRAL | PAMELA GLORIA TONE RECUAY | NOTA DE VENTA(INTERNA)      | NV02  | 00000000    | Inmediata | false        | Contado    | guarda exitosamente                 |
            | CP-V7 | VENTA NORMAL    | N   | N            | 150      | -                        | -                         | NOTA DE VENTA(INTERNA)      | NV02  | 75893616    | Inmediata | false        | Incompleto | pago no completado                  |

    @NuevaVenta
    @VentaConGuia
    Scenario Outline: <Caso> Registro de venta con Guia de Remision activa — Modo Normal/Caja
        When selecciona el modo de venta "<ModoVenta>"
        And configura IGV "<IGV>" y Detalle Unificado "<DetUnificado>"
        And el usuario selecciona la familia 'Gaseosa'
        And el usuario selecciona el concepto '7753234003313'
        And el usuario ingresa la cantidad '<Cantidad>'
        And selecciona el punto de venta '<PuntoVenta>'
        And selecciona el vendedor '<Vendedor>'
        And configura la facturacion '<Comprobante>' '<Serie>' '<Cliente>'
        And el usuario configura la entrega '<Entrega>' '<GuiaRemision>'
        And el usuario ingresa fecha de traslado '<FechaTraslado>'
        And el usuario ingresa peso bruto '<PesoBruto>'
        And el usuario ingresa numero de bultos '<Bultos>'
        And el usuario selecciona transporte '<TipoTransporte>'
        And el usuario ingresa RUC transportista '<TransportistaRuc>'
        And el usuario ingresa licencia '<NumeroLicencia>'
        And el usuario ingresa placa '<NumeroPlaca>'
        And configura el pago "<Pago>"
        And hace clic en Guardar
        Then el sistema valida el resultado de venta "<ResultadoEsperado>"
        Examples:
            | Caso  | ModoVenta       | IGV | DetUnificado | Cantidad | PuntoVenta      | Vendedor                  | Comprobante         | Serie | Cliente     | Entrega   | GuiaRemision | FechaTraslado | PesoBruto | Bultos | TipoTransporte | TransportistaRuc | NumeroLicencia | NumeroPlaca | Pago    | ResultadoEsperado             |
            | CP-V1 | VENTA NORMAL    | Y   | Y            | 1        | -               | -                         | FACTURA ELECTRONICA | F002  | 75893616    | Inmediata | true         | NA            | NA        | NA     | NA             | NA               | NA             | NA          | Contado | inconsistencia: ruc requerido |
            | CP-V2 | VENTA MODO CAJA | Y   | Y            | 1        | ALMACEN CENTRAL | FRANKLIN MARTINEZ HURTADO | FACTURA ELECTRONICA | F002  | 20542245671 | Inmediata | true         | 01/03/2026    | 100       | 10     | Publico        | 20602945589      | NA             | NA          | Contado | guarda exitosamente           |

    @NuevaVenta  
    @VentaContingencia
    Scenario Outline: <Caso> Registro de venta por Contingencia
        When selecciona el modo de venta "VENTA POR CONTINGENCIA"
        And configura IGV "false" y Detalle Unificado "false"
        And el usuario selecciona la familia 'Gaseosa'
        And el usuario selecciona el concepto '7753234003313'
        And el usuario ingresa la cantidad '150'
        And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
        And ingresa la fecha de emision "<FechaEmision>"
        And el usuario configura la entrega '<Entrega>' '<GuiaRemision>'
        And configura el pago "Contado"
        And hace clic en Guardar
        Then el sistema valida el resultado de venta "<ResultadoEsperado>"
        Examples:
            | Caso  | FechaEmision | Entrega   | GuiaRemision | ResultadoEsperado                        |
            | CP-V8 | 01/01/2024   | Inmediata | false        | inconsistencia: contingencia fuera plazo |
            | CP-V9 | 08/04/2026   | Inmediata | false        | guarda exitosamente                      |

    @NuevaVenta  
    @GuiaDeRemision
    Scenario Outline: Guia de Remision desde Nueva Venta — <Descripcion>
        When selecciona el modo de venta "VENTA NORMAL"
        And configura IGV "N" y Detalle Unificado "N"
        And el usuario selecciona la familia 'Gaseosa'
        And el usuario selecciona el concepto '7753234003313'
        And el usuario ingresa la cantidad '20'
        And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
        And el usuario configura la entrega 'Inmediata' '<GuiaRemision>'
        And el usuario ingresa fecha de traslado '<FechaTraslado>'
        And el usuario ingresa peso bruto '<PesoBruto>'
        And el usuario ingresa numero de bultos '<Bultos>'
        And el usuario selecciona transporte '<TipoTransporte>'
        And el usuario ingresa RUC transportista '<TransportistaRuc>'
        And el usuario ingresa licencia '<NumeroLicencia>'
        And el usuario ingresa placa '<NumeroPlaca>'
        And configura el pago "Contado"
        And hace clic en Guardar
        Then el sistema valida el resultado de venta "<ResultadoEsperado>"
        Examples:
            | Caso  | Descripcion                           | GuiaRemision | FechaTraslado | PesoBruto | Bultos | TipoTransporte | TransportistaRuc | NumeroLicencia | NumeroPlaca | ResultadoEsperado                    |
            | CP031 | Transporte Publico completo           | true         | Hoy           | 100       | 10     | Publico        | 20602945589      | NA             | NA          | guarda exitosamente                  |
            | CP032 | Transporte Publico sin transportista  | true         | Hoy           | 100       | 10     | Publico        | NA               | NA             | NA          | identifique al transportista con ruc |
            | CP033 | Transporte Publico sin fecha          | true         | NA            | 100       | 10     | Publico        | 20602945589      | NA             | NA          | registre la fecha de inicio          |
            | CP034 | Transporte Publico sin peso ni bultos | true         | Hoy           | NA        | NA     | Publico        | 20602945589      | NA             | NA          | guarda exitosamente                  |
            | CP035 | Transporte Privado completo           | true         | Hoy           | 100       | 10     | Privado        | 75971759         | M-71310154     | 2770XS      | guarda exitosamente                  |
            | CP036 | Transporte Privado sin conductor      | true         | Hoy           | 100       | 10     | Privado        | NA               | NA             | 2770XS      | identifique al conductor con dni     |
            | CP037 | Transporte Privado sin licencia       | true         | Hoy           | 100       | 10     | Privado        | 75971759         | NA             | 2770XS      | ingrese numero de licencia           |
            | CP038 | Transporte Privado sin placa          | true         | Hoy           | 100       | 10     | Privado        | 75971759         | M-71310154     | NA          | ingrese numero de placa              |

    

# Casos CP031-CP038: Guia de Remision desde Nueva Venta (Tabla de Decisión).
    # Condiciones evaluadas:
    #   - Fecha de traslado: registrada / vacía
    #   - Peso bruto / Bultos: registrados / vacíos
    #   - Tipo de transporte: Público / Privado
    #   - Transportista RUC (Público) / Conductor DNI (Privado): registrado / vacío
    #   - Número de licencia (Privado): registrado / vacío
    #   - Número de placa (Privado): registrado / vacío
    #
    # Acciones verificadas:
    #   "guarda exitosamente"                         → Guardar habilitado y ejecutado
    #   "identifique al transportista con ruc"        → Guardar deshabilitado (Público sin RUC)
    #   "registre la fecha de inicio"                 → Guardar deshabilitado (fecha vacía)
    #   "identifique al conductor con dni"            → Guardar deshabilitado (Privado sin conductor)
    #   "ingrese numero de licencia"                  → Guardar deshabilitado (Privado sin licencia)
    #   "ingrese numero de placa"                     → Guardar deshabilitado (Privado sin placa)

    @NuevaVenta
@Descuentos
Scenario Outline: Validar descuentos en nueva venta

	# 🔹 REUTILIZADO (Pedidos)
	And el usuario selecciona la familia '<familia>'
	And el usuario selecciona el concepto '<concepto1>'
	And el usuario ingresa la cantidad '<cantidad1>'
	And el usuario selecciona el concepto '<concepto2>'
	And el usuario ingresa la cantidad '<cantidad2>'

	# 🔹 CONFIGURACIÓN
	And el usuario activa IGV '<igv>'

	# 🔹 REUTILIZADO (Cotización)
	And el usuario configura descuento '<descuento>' '<tipo_descuento>' '<modo_descuento>' '<valor_descuento>'

	# ❗ VALIDACIÓN SIN GUARDAR
	Then el sistema valida el resultado del descuento en venta '<resultado>'

Examples:
| caso | familia | concepto1      | cantidad1 | concepto2      | cantidad2 | igv   | descuento | tipo_descuento | modo_descuento | valor_descuento | resultado                                   |
| 1    | Gaseosa | 7753234003313 | 1         | 7753234003320 | 1         | false | true      | item           | $              | 1.00            | descuento item monto valido                  |
| 2    | Gaseosa | 7753234003313 | 1         | 7753234003320 | 1         | false | true      | global         | %              | 5               | descuento global porcentaje valido           |
| 3    | Gaseosa | 7753234003313 | 1         | 7753234003320 | 1         | false | true      | global         | $              | 20.00           | descuento global monto invalido              |
| 4    | Gaseosa | 7753234003313 | 1         | 7753234003320 | 1         | false | true      | item           | %              | 100             | descuento item porcentaje invalido           |
