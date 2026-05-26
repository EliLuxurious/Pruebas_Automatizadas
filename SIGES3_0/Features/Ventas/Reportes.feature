@NuevaVenta
Feature: Reportes
  Como usuario administrador
  Quiero acceder al menu de Reportes de Ventas
  Para validar la funcionalidad de generacion de reportes por Comprobantes y Conceptos

Background:
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
    And el usuario accede al módulo 'Ventas'

@Reportes
@FiltroFechas
Scenario Outline: Validar filtro de fechas en reporte de ventas
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '7753234003313'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Comprobantes"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    Then el sistema valida el comportamiento esperado de la fecha final "<resultadoEsperado>"

    Examples:
      | Caso   | fechaHoraInicial       | fechaHoraFinal         | resultadoEsperado                         |
      | CPF001 | ayer 01:15 am          | hace 2 dias 10:45 pm   | No permite aplicar el filtro Inhabilitado |
      | CPF002 | hace 20 dias 01:15 am  | hace 5 dias 10:45 pm   | Aplica el filtro correctamente            |
      | CPF003 | hace 10 dias 10:15 pm  | hace 10 dias 10:45 pm  | Aplica el filtro correctamente            |

@Reportes
@PorComprobante
Scenario Outline: <Caso> Generar reporte por comprobante
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '7753234003313'
    And usuario ingresa la cantidad '1'
    And configura la facturacion '<TipoComprobante>' '<Serie>' '<ClienteVenta>'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "<ResultadoVenta>"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Comprobantes"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el tipo de comprobante "<TipoComprobante>"
    And selecciona la serie "<Serie>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Comprobante"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | TipoComprobante             | Serie | ClienteVenta | ResultadoVenta       | fechaHoraInicial      | fechaHoraFinal       |
      | CP0000 | BOLETA DE VENTA ELECTRONICA | B002  | 75893616     | guarda exitosamente  | ayer 01:15 am | hoy 10:45 pm |
      | CP0000 | FACTURA ELECTRONICA         | F002  | 20542245671  | guarda exitosamente  | ayer 01:15 am | hoy 10:45 pm |
      | CP0000 | NOTA DE VENTA(INTERNA)      | NV02  | 00000000     | guarda exitosamente  | ayer 01:15 am | hoy 10:45 pm |

  @Reportes
  @PorComprobante
  @NotaCredito
  @CP082
  Scenario Outline: <Caso> Generar reporte por comprobante de nota de credito
    # Precondiciones
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '<ConceptoVenta>'
    And usuario ingresa la cantidad '1'
    And configura la facturacion '<ComprobanteVenta>' '<SerieVenta>' '<ClienteVenta>'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Ver Ventas'
    And abre el modal ajustes de comprobante
    And selecciona la opcion "Nota de credito" del modal ajustes de comprobante
    And selecciona tipo de nota de credito "ANULACIÓN DE LA OPERACIÓN"
    And selecciona serie "<SerieReporte>" en el ajuste
    And ingresa motivo o sustento "Precondicion reporte nota de credito"
    And selecciona entrega "Diferida" en el ajuste
    And selecciona devolucion "Contado" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema genera el comprobante de ajuste exitosamente

    # Flujo principal
    When el usuario accede al submodulo 'Reportes'
    And selecciona la vista "Comprobantes"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el tipo de comprobante "<TipoComprobanteReporte>"
    And selecciona la serie "<SerieReporte>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Comprobante"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | ConceptoVenta | ComprobanteVenta            | SerieVenta | ClienteVenta | TipoComprobanteReporte | SerieReporte | fechaHoraInicial | fechaHoraFinal |
      | CP082 | 7753234003320 | BOLETA DE VENTA ELECTRONICA | B002       | 75893616     | NOTA DE CREDITO         | B002         | ayer 01:15 am    | hoy 10:45 pm   |

  @Reportes
  @PorComprobante
  @NotaDebito
  @CP083
  Scenario Outline: <Caso> Generar reporte por comprobante de nota de debito
    # Precondiciones
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '<ConceptoVenta>'
    And usuario ingresa la cantidad '1'
    And configura la facturacion '<ComprobanteVenta>' '<SerieVenta>' '<ClienteVenta>'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Ver Ventas'
    And abre el modal ajustes de comprobante
    And selecciona la opcion "Nota de debito" del modal ajustes de comprobante
    And selecciona tipo de nota de debito "INTERESES POR MORA"
    And selecciona serie "<SerieReporte>" en el ajuste
    And ingresa motivo o sustento "Precondicion reporte nota de debito"
    And ingresa monto del interes "5.00"
    And expande la seccion "Pago" del ajuste
    And selecciona tipo de pago "Contado" en la seccion pago del ajuste
    And selecciona medio de pago "Efectivo" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema genera el comprobante de ajuste exitosamente

    # Flujo principal
    When el usuario accede al submodulo 'Reportes'
    And selecciona la vista "Comprobantes"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el tipo de comprobante "<TipoComprobanteReporte>"
    And selecciona la serie "<SerieReporte>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Comprobante"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | ConceptoVenta | ComprobanteVenta    | SerieVenta | ClienteVenta | TipoComprobanteReporte | SerieReporte | fechaHoraInicial | fechaHoraFinal |
      | CP083 | 7753234003313 | FACTURA ELECTRONICA | F002       | 20542245671  | NOTA DE DEBITO          | B002         | ayer 01:15 am    | hoy 10:45 pm   |

####################################################################
##################### SERIES
####################################################################
@Reportes
@PorSerie
Scenario Outline: Generar reporte por serie
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '7753234003313'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Series"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el comprobante y serie "<ComprobanteSerie>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Serie"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | ComprobanteSerie | fechaHoraInicial      | fechaHoraFinal       |
      | CP0000 | Todos            | hace 8 dias 01:15 am | hoy 10:45 pm |

####################################################################
##################### CONCEPTOS
####################################################################
@Reportes
@PorConceptos
@Sinfiltro
Scenario Outline: <Caso> Generar reporte por conceptos (sin filtro adicional)
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia '<FamiliaVenta>'
    And usuario selecciona el concepto '<ConceptoVenta>'
    And usuario ingresa la cantidad '<CantidadVenta>'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '<ClienteVenta>'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Conceptos"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el punto de venta "<PuntoVenta>"
    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | PuntoVenta               | Tarjeta                    | FamiliaVenta  | ConceptoVenta | CantidadVenta | ClienteVenta | fechaHoraInicial      | fechaHoraFinal  |
      | CP086  | CENTRO COMERCIAL CENTRAL | Por Familia y Serie        | Gaseosa       | 7753234003313 | 1             | 75893616     | ayer 01:15 am         | hoy 10:45 pm    |
      | CP087  | CENTRO COMERCIAL CENTRAL | Por Categoría y Serie      | Gaseosa       | 7753234003313 | 1             | 75893616     | ayer 01:15 am         | hoy 10:45 pm    |
      | CP088  | CENTRO COMERCIAL CENTRAL | Según Horario              | Gaseosa       | 7753234003313 | 1             | 75893616     | ayer 01:15 am         | hoy 10:45 pm    |
      | CP0000 | CENTRO COMERCIAL CENTRAL | Por Comprobante con ICBPER | Bolsa Plastica| 895674556789  | 1             | 00000000     | ayer 01:15 am         | hoy 10:45 pm    |

@Reportes
@PorConceptos
@PorFamilia
Scenario Outline: <Caso> Generar reporte por familia en la vista Conceptos
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia '<Familia>'
    And usuario selecciona el concepto '7753234003313'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Conceptos"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el punto de venta "<PuntoVenta>"
    And selecciona la familia "<Familia>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Familia"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | PuntoVenta               | Familia | fechaHoraInicial      | fechaHoraFinal       |
      | CP070 | CENTRO COMERCIAL CENTRAL | Gaseosa | ayer 01:15 am | hoy 10:45 pm |

@Reportes
@PorConceptos
@PorCaracteristica
Scenario Outline: <Caso> Generar reporte por característica en la vista Conceptos
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '7753234003313'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Conceptos"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el punto de venta "<PuntoVenta>"
    And selecciona la caracteristica "<Caracteristica>" en la tarjeta "<Tarjeta>"
    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | PuntoVenta               | Caracteristica | Tarjeta                                       | fechaHoraInicial      | fechaHoraFinal       |
      | CP084 | CENTRO COMERCIAL CENTRAL | MARCA          | POR CONCEPTO, CARACTERISTICAS Y FORMA DE PAGO | ayer 01:15 am | hoy 10:45 pm |
      | CP085 | CENTRO COMERCIAL CENTRAL | TAMAÑO         | POR CARACTERISTICAS                           | ayer 01:15 am | hoy 10:45 pm |

####################################################################
##################### VENDEDOR
####################################################################
@Reportes
@PorVendedor
Scenario Outline: <Caso> Generar reporte por vendedor en la vista Vendedor
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA MODO CAJA"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia '<Familia>'
    And usuario selecciona el concepto '7753234003313'
    And usuario ingresa la cantidad '1'
    And selecciona el punto de venta 'CENTRO COMERCIAL CENTRAL'
    And selecciona el vendedor '<Vendedor>'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Vendedor"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el vendedor "<Vendedor>"
    And selecciona "<Familia>" en el filtro "Familias" de la tarjeta "Por Vendedor"
    And selecciona "<Concepto>" en el filtro "Conceptos" de la tarjeta "Por Vendedor"
    And hace clic en "VER REPORTE" en la tarjeta "Por Vendedor"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | Vendedor                  | Familia | Concepto  | fechaHoraInicial      | fechaHoraFinal       |
      | CP090 | FRANKLIN MARTINEZ HURTADO | Gaseosa | Inka Kola | ayer 01:15 am | hoy 10:45 pm |

@Reportes
@PorVendedor
@PorModalidadConcepto
Scenario Outline: <Caso> Generar reporte por modalidad y concepto en la vista Vendedor
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "<Modalidad>"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '7753234003313'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Vendedor"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el vendedor "<Vendedor>"
    And selecciona "<Modalidad>" en el filtro "Modalidad" de la tarjeta "Por Modalidad y Concepto"
    And hace clic en "VER REPORTE" en la tarjeta "Por Modalidad y Concepto"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | Vendedor                  | Modalidad    | fechaHoraInicial      | fechaHoraFinal       |
      | CP091 | FRANKLIN MARTINEZ HURTADO | VENTA NORMAL | ayer 01:15 am | hoy 10:45 pm |

@Reportes
@PorVendedor
@PorFamiliaVendedor
Scenario Outline: <Caso> Generar reporte por familia y vendedor en la vista Vendedor
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA MODO CAJA"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '7753234003313'
    And usuario ingresa la cantidad '1'
    And selecciona el punto de venta 'CENTRO COMERCIAL CENTRAL'
    And selecciona el vendedor '<Vendedor>'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Vendedor"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el vendedor "<Vendedor>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Familia y Vendedor"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | Vendedor                  | fechaHoraInicial      | fechaHoraFinal       |
      | CP092 | FRANKLIN MARTINEZ HURTADO | ayer 01:15 am | hoy 10:45 pm |

      # Falta caso de prueba por Por centro de atención y Serie

####################################################################
##################### GRUPOS
####################################################################
@Reportes
@PorGrupos
Scenario Outline: <Caso> Generar reporte en la vista Grupos
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA MODO CAJA"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '7753234003313'
    And usuario ingresa la cantidad '1'
    And selecciona el punto de venta '<PuntoVenta>'
    And selecciona el vendedor 'FRANKLIN MARTINEZ HURTADO'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Grupos"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el establecimiento "<Establecimiento>"
    And selecciona el punto de venta "<PuntoVenta>"
    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | Establecimiento | PuntoVenta               | Tarjeta             | fechaHoraInicial      | fechaHoraFinal       |
      | CP093 | RECSA - CENTRAL | RECSA - CENTRAL          | Por Grupo           | ayer 01:15 am         | hoy 10:45 pm |
      | CP094 | RECSA - CENTRAL | RECSA - CENTRAL          | Por Familia y Grupo | ayer 01:15 am         | hoy 10:45 pm |
      | CP095 | RECSA - CENTRAL | RECSA - CENTRAL          | Por Grupo Detallado | ayer 01:15 am         | hoy 10:45 pm |

@Reportes
@PorExcepciones
Scenario Outline: <Caso> Generar reporte en la vista Excepciones
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '7753234003313'
    And usuario ingresa la cantidad '1'
    And configura la facturacion '<ComprobanteVenta>' '<SerieVenta>' '<ClienteVenta>'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Excepciones"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el punto de venta "<PuntoVenta>"
    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | PuntoVenta               | Tarjeta              | ComprobanteVenta            | SerieVenta | ClienteVenta | fechaHoraInicial      | fechaHoraFinal       |
      | CP096 | CENTRO COMERCIAL CENTRAL | Por Notas de Credito | BOLETA DE VENTA ELECTRONICA | B002       | 75893616     | ayer 01:15 am | hoy 10:45 pm |
      | CP097 | CENTRO COMERCIAL CENTRAL | Por Invalidaciones   | BOLETA DE VENTA ELECTRONICA | B002       | 75893616     | ayer 01:15 am | hoy 10:45 pm |
      | CP098 | CENTRO COMERCIAL CENTRAL | Por Notas de Debito  | FACTURA ELECTRONICA         | F002       | 20542245671  | ayer 01:15 am | hoy 10:45 pm |
