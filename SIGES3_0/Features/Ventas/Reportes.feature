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
      | CP0000 | NOTA DE CRÉDITO             | B002  |05/03/2026 12:00 am  | 25/03/2026 11:59 pm |
      | CP0000 | NOTA DE DÉBITO              | B002  |05/03/2026 12:00 am  | 25/03/2026 11:59 pm |



#@Reportes @PorSerie
#Scenario: CP002 Validar que no se habilite VER REPORTE en Comprobantes cuando solo se selecciona el tipo de comprobante y la serie queda pendiente
#    When ingresa al modulo de "Ventas" y selecciona "Reportes"
#    And selecciona la vista "Comprobantes"
#    And ingresa la fecha y hora inicial "05/03/2026 12:00 a. m." y final "05/03/2026 11:59 p. m."
#    And selecciona el tipo de comprobante "BOLETA DE VENTA ELECTRONICA"
#    Then valida que el boton "VER REPORTE" en la tarjeta "POR COMPROBANTE" este deshabilitado
#
#@Reportes @Ventas @CP003
#Scenario: CP003 Generar reporte por familia en la vista Conceptos filtrando el punto de venta vigente CENTRO COMERCIAL CENTRAL para validar consulta exitosa
#    When ingresa al modulo de "Ventas" y selecciona "Reportes"
#    And selecciona la vista "Conceptos"
#    And ingresa la fecha y hora inicial "05/03/2026 12:00 a. m." y final "05/03/2026 11:59 p. m."
#    And selecciona el punto de venta "CENTRO COMERCIAL CENTRAL"
#    And selecciona la familia "Gaseosa"
#    And hace clic en "VER REPORTE" en la tarjeta "POR FAMILIA"
#    Then el sistema genera el reporte exitosamente

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
