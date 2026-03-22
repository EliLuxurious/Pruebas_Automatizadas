Feature: Reportes
  Como usuario administrador
  Quiero acceder al menu de Reportes de Ventas
  Para validar la funcionalidad de generacion de reportes por Comprobantes y Conceptos

Background: 
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'

@Reportes @Ventas @PorComprobante
Scenario Outline: <Caso> Generar reporte por comprobante 
    When ingresa al modulo de "Ventas" y selecciona "Reportes"
    And selecciona la vista "Comprobantes"
    And ingresa la fecha y hora inicial "05/03/2026 12:00 a. m." y final "05/03/2026 11:59 p. m."
    And selecciona el tipo de comprobante "<TipoComprobante>"
    And selecciona la serie "<Serie>"
    And hace clic en "VER REPORTE" en la tarjeta "POR COMPROBANTE"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | TipoComprobante             | Serie |
      | CP001A | BOLETA DE VENTA ELECTRONICA | B002  |
      | CP001B | FACTURA ELECTRONICA         | F002  |
      | CP001C | NOTA DE VENTA(INTERNA)      | NV02  |

@Reportes @Ventas @CP002
Scenario: CP002 Validar que no se habilite VER REPORTE en Comprobantes cuando solo se selecciona el tipo de comprobante y la serie queda pendiente
    When ingresa al modulo de "Ventas" y selecciona "Reportes"
    And selecciona la vista "Comprobantes"
    And ingresa la fecha y hora inicial "05/03/2026 12:00 a. m." y final "05/03/2026 11:59 p. m."
    And selecciona el tipo de comprobante "BOLETA DE VENTA ELECTRONICA"
    Then valida que el boton "VER REPORTE" en la tarjeta "POR COMPROBANTE" este deshabilitado

@Reportes @Ventas @CP003
Scenario: CP003 Generar reporte por familia en la vista Conceptos filtrando el punto de venta vigente CENTRO COMERCIAL CENTRAL para validar consulta exitosa
    When ingresa al modulo de "Ventas" y selecciona "Reportes"
    And selecciona la vista "Conceptos"
    And ingresa la fecha y hora inicial "05/03/2026 12:00 a. m." y final "05/03/2026 11:59 p. m."
    And selecciona el punto de venta "CENTRO COMERCIAL CENTRAL"
    And selecciona la familia "Gaseosa"
    And hace clic en "VER REPORTE" en la tarjeta "POR FAMILIA"
    Then el sistema genera el reporte exitosamente
