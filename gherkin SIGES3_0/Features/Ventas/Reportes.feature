Feature: Reportes de Ventas
  Como usuario administrador
  Quiero validar la generacion de reportes de ventas
  Para garantizar que cada vista y tarjeta del submodulo funciona correctamente

Background:
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
    And el usuario accede al módulo 'Ventas'
    And el usuario accede al submodulo 'Reportes'

# ══════════════════════════════════════════════════════
# TAB: COMPROBANTES
# ══════════════════════════════════════════════════════
@Reportes @Comprobantes
Scenario Outline: <Caso> Generar reporte por comprobante
    When selecciona la vista "Comprobantes"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And selecciona el tipo de comprobante "<TipoComprobante>"
    And selecciona la serie "<Serie>"
    And hace clic en "VER REPORTE" en la tarjeta "POR COMPROBANTE"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | TipoComprobante             | Serie | fechaHoraInicial     | fechaHoraFinal      |
      | CP001A | BOLETA DE VENTA ELECTRONICA | B002  | 05/02/2026 12:00 am  | 05/03/2026 11:59 pm |
      | CP001B | FACTURA ELECTRONICA         | F002  | 05/02/2026 12:00 am  | 05/03/2026 11:59 pm |
      | CP001C | NOTA DE VENTA(INTERNA)      | NV02  | 05/02/2026 12:00 am  | 05/03/2026 11:59 pm |

#@Reportes @Comprobantes @Validacion
#Scenario: CP002 Validar que VER REPORTE queda deshabilitado sin serie seleccionada
#    When selecciona la vista "Comprobantes"
#    And el usuario ingresa la fecha y hora inicial "05/01/2026 12:00 am"
#    And el usuario ingresa la fecha y hora final "05/03/2026 11:59 pm"
#    And selecciona el tipo de comprobante "BOLETA DE VENTA ELECTRONICA"
#    Then valida que el boton "VER REPORTE" en la tarjeta "POR COMPROBANTE" este deshabilitado
#
# ══════════════════════════════════════════════════════
# TAB: SERIES
# Filtro "Comprobante y Serie" vive DENTRO de la tarjeta.
# Formato del valor: "Todos" | "XX : YYYY"  (ej: "01 : F002")
# ══════════════════════════════════════════════════════
@Reportes @Series
Scenario Outline: <Caso> Generar reporte por serie
    When selecciona la vista "Series"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And selecciona el comprobante y serie "<ComprobanteSerie>"
    And hace clic en "VER REPORTE" en la tarjeta "POR SERIE"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | fechaHoraInicial     | fechaHoraFinal      |
      | CP003A | 05/01/2026 12:00 am  | 05/03/2026 11:59 pm |

# ══════════════════════════════════════════════════════
# TAB: CONCEPTOS — punto de venta + familia
# ══════════════════════════════════════════════════════
@Reportes @Conceptos
Scenario Outline: <Caso> Generar reporte por familia en Conceptos
    When selecciona la vista "Conceptos"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And selecciona el punto de venta "<PuntoVenta>"
    And selecciona la familia "<Familia>"
    And hace clic en "VER REPORTE" en la tarjeta "POR FAMILIA"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | PuntoVenta               | Familia | fechaHoraInicial     | fechaHoraFinal      |
      | CP004A | CENTRO COMERCIAL CENTRAL | Gaseosa | 05/01/2026 12:00 am  | 05/03/2026 11:59 pm |

# ══════════════════════════════════════════════════════
# TAB: GRUPOS — 3 tarjetas, mismo step, distinto valor
# ══════════════════════════════════════════════════════
@Reportes @Grupos
Scenario Outline: <Caso> Generar reporte en la vista Grupos
    When selecciona la vista "Grupos"
    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | Tarjeta             | fechaHoraInicial     | fechaHoraFinal      |
      | CP005A | POR FAMILIA Y GRUPO | 05/01/2026 12:00 am  | 05/03/2026 11:59 pm |
      | CP005B | POR GRUPO           | 05/01/2026 12:00 am  | 05/03/2026 11:59 pm |
      | CP005C | POR GRUPO DETALLADO | 05/01/2026 12:00 am  | 05/03/2026 11:59 pm |

# ══════════════════════════════════════════════════════
# TAB: VENDEDOR — explorar DOM para agregar filtros
# ══════════════════════════════════════════════════════
#@Reportes @Vendedor
#Scenario Outline: <Caso> Generar reporte por vendedor
#    When selecciona la vista "Vendedor"
#    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
#    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
#    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
#    Then el sistema genera el reporte exitosamente
#    Examples:
#      | Caso   | Tarjeta     | fechaHoraInicial     | fechaHoraFinal      |
#      | CP006A | POR VENDEDOR| 05/01/2026 12:00 am  | 05/03/2026 11:59 pm |

# ══════════════════════════════════════════════════════
# TAB: EXCEPCIONES — explorar DOM para agregar filtros
# ══════════════════════════════════════════════════════
#@Reportes @Excepciones
#Scenario Outline: <Caso> Generar reporte por excepciones
#    When selecciona la vista "Excepciones"
#    And el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
#    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
#    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
#    Then el sistema genera el reporte exitosamente
#    Examples:
#      | Caso   | Tarjeta        | fechaHoraInicial     | fechaHoraFinal      |
#      | CP007A | POR EXCEPCION  | 05/01/2026 12:00 am  | 05/03/2026 11:59 pm |