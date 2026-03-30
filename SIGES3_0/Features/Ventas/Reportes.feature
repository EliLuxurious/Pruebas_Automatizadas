Feature: Reportes
  Como usuario administrador
  Quiero acceder al menu de Reportes de Ventas
  Para validar la funcionalidad de generacion de reportes por Comprobantes y Conceptos

Background: 
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
    And el usuario accede al módulo 'Ventas'
    And el usuario accede al submodulo 'Reportes'

@Reportes 
@FiltroFechas
Scenario Outline: Validar filtro de fechas en reporte de ventas
	When selecciona la vista "Comprobantes"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And selecciona el tipo de comprobante "<TipoComprobante>"
    And selecciona la serie "<Serie>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Comprobante"
    Then el sistema muestra el resultado esperado del reporte "<resultadoEsperado>"

    Examples:
	
      | Caso   | TipoComprobante             | Serie | fechaHoraInicial   | fechaHoraFinal      | resultadoEsperado                         |
      | CP0000 | BOLETA DE VENTA ELECTRONICA | B002  |10/03/2026 12:00 am | 09/03/2026 11:59 pm | No permite aplicar el filtro Inhabilitado |
      | CP0000 | FACTURA ELECTRONICA         | F002  |09/03/2026 12:00 am | 23/03/2026 11:59 pm | Aplica el filtro correctamente            |
      | CP0000 | NOTA DE VENTA(INTERNA)      | NV02  |01/03/2026 11:00 pm | 01/03/2026 12:59 pm | Aplica el filtro correctamente            |
      
@Reportes 
@PorComprobante
Scenario Outline: <Caso> Generar reporte por comprobante 
    When selecciona la vista "Comprobantes"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And selecciona el tipo de comprobante "<TipoComprobante>"
    And selecciona la serie "<Serie>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Comprobante"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | TipoComprobante             | Serie | fechaHoraInicial    | fechaHoraFinal      |
      | CP0000 | BOLETA DE VENTA ELECTRONICA | B002  |05/03/2026 12:00 am  | 25/03/2026 11:59 pm |
      | CP0000 | FACTURA ELECTRONICA         | F002  |05/03/2026 12:00 am  | 25/03/2026 11:59 pm |
      | CP0000 | NOTA DE VENTA(INTERNA)      | NV02  |05/03/2026 12:00 am  | 25/03/2026 11:59 pm |
      | CP0000 | NOTA DE CREDITO             | B002  |05/03/2026 12:00 am  | 25/03/2026 11:59 pm |
      | CP0000 | NOTA DE DEBITO              | B002  |05/03/2026 12:00 am  | 25/03/2026 11:59 pm |


@Reportes
@PorSerie
Scenario Outline: <Caso> Generar reporte por serie
    When selecciona la vista "Series"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And selecciona el comprobante y serie "<ComprobanteSerie>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Serie"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | ComprobanteSerie | fechaHoraInicial    | fechaHoraFinal      |
      | CP0000 | 03 : B002            | 05/03/2026 12:00 am | 25/03/2026 11:59 pm |

@Reportes
@PorConceptos
@Sinfiltro
Scenario Outline: <Caso> Generar reporte por conceptos (sin filtro adicional)
    When selecciona la vista "Conceptos"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And selecciona el punto de venta "<PuntoVenta>"
    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | PuntoVenta                 | Tarjeta                             | fechaHoraInicial    | fechaHoraFinal      |
      | CP086  | CENTRO COMERCIAL CENTRAL   | Por Familia y Serie                 | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |
      | CP087  | CENTRO COMERCIAL CENTRAL   | Por Categoría y Serie               | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |
      | CP088  | CENTRO COMERCIAL CENTRAL   | Según Horario                       | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |
      | CP0000 | CENTRO COMERCIAL CENTRAL   | Por Comprobante con ICBPER          | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |

@Reportes
@PorConceptos
@PorFamilia
Scenario Outline: <Caso> Generar reporte por familia en la vista Conceptos
    When selecciona la vista "Conceptos"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And selecciona el punto de venta "<PuntoVenta>"
    And selecciona la familia "<Familia>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Familia"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | PuntoVenta                 | Familia  | fechaHoraInicial    | fechaHoraFinal      |
      | CP070  | CENTRO COMERCIAL CENTRAL   | Gaseosa  | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |

@Reportes
@PorConceptos
@PorCaracteristica
 Scenario Outline: <Caso> Generar reporte por característica en la vista Conceptos
    When selecciona la vista "Conceptos"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And selecciona el punto de venta "<PuntoVenta>"
    And selecciona la característica "<Caracteristica>" en la tarjeta "<Tarjeta>"
    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | PuntoVenta                 | Caracteristica | Tarjeta                                            | fechaHoraInicial    | fechaHoraFinal      |
      | CP084  | CENTRO COMERCIAL CENTRAL   | MARCA          | POR CONCEPTO, CARACTERISTICAS Y FORMA DE PAGO      | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |
      | CP085  | CENTRO COMERCIAL CENTRAL   | TAMAÑO         | POR CARACTERISTICAS                                | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |

@Reportes
@PorVendedor
Scenario Outline: <Caso> Generar reporte por vendedor en la vista Vendedor
    When selecciona la vista "Vendedor"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And selecciona el vendedor "<Vendedor>"
    And selecciona "<Familia>" en el filtro "Familias" de la tarjeta "Por Vendedor"
    And selecciona "<Concepto>" en el filtro "Conceptos" de la tarjeta "Por Vendedor"
    And hace clic en "VER REPORTE" en la tarjeta "Por Vendedor"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | Vendedor                   | Familia | Concepto  | fechaHoraInicial    | fechaHoraFinal      |
      | CP090  | FRANKLIN MARTINEZ HURTADO  | Gaseosa | Inka Kola | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |

@Reportes
@PorVendedor
@PorModalidadConcepto
Scenario Outline: <Caso> Generar reporte por modalidad y concepto en la vista Vendedor
    When selecciona la vista "Vendedor"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And selecciona el vendedor "<Vendedor>"
    And selecciona "<Modalidad>" en el filtro "Modalidad" de la tarjeta "Por Modalidad y Concepto"
    And hace clic en "VER REPORTE" en la tarjeta "Por Modalidad y Concepto"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | Vendedor                   | Modalidad    | fechaHoraInicial    | fechaHoraFinal      |
      | CP091  | FRANKLIN MARTINEZ HURTADO  | VENTA NORMAL | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |

@Reportes
@PorVendedor
@PorFamiliaVendedor
Scenario Outline: <Caso> Generar reporte por familia y vendedor en la vista Vendedor
    When selecciona la vista "Vendedor"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And selecciona el vendedor "<Vendedor>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Familia y Vendedor"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | Vendedor                   | fechaHoraInicial    | fechaHoraFinal      |
      | CP092  | FRANKLIN MARTINEZ HURTADO  | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |

@Reportes
@PorGrupos
Scenario Outline: <Caso> Generar reporte en la vista Grupos
    When selecciona la vista "Grupos"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And selecciona el establecimiento "<Establecimiento>"
    And selecciona el punto de venta "<PuntoVenta>"
    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | Establecimiento | PuntoVenta               | Tarjeta             | fechaHoraInicial    | fechaHoraFinal      |
      | CP093  | RECSA - CENTRAL | CENTRO COMERCIAL CENTRAL | Por Grupo           | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |
      | CP094  | RECSA - CENTRAL | CENTRO COMERCIAL CENTRAL | Por Familia y Grupo | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |
      | CP095  | RECSA - CENTRAL | CENTRO COMERCIAL CENTRAL | Por Grupo Detallado | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |

@Reportes
@PorExcepciones
Scenario Outline: <Caso> Generar reporte en la vista Excepciones
    When selecciona la vista "Excepciones"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And selecciona el punto de venta "<PuntoVenta>"
    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | PuntoVenta               | Tarjeta               | fechaHoraInicial    | fechaHoraFinal      |
      | CP096  | CENTRO COMERCIAL CENTRAL | Por Notas de Credito  | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |
      | CP097  | CENTRO COMERCIAL CENTRAL | Por Invalidaciones    | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |
      | CP098  | CENTRO COMERCIAL CENTRAL | Por Notas de Debito   | 05/03/2026 12:00 am | 05/03/2026 11:59 pm |
