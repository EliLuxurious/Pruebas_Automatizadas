Feature: VerVentas

Cobertura base para acciones disponibles en ver ventas.

Background:
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
    And el usuario accede al módulo 'Ventas'

@CanjearNV
Scenario Outline: <Caso> Canjear NV con comprobante destino <ComprobanteDestino>
    Given el usuario accede al submodulo 'Nueva Venta'
    And crea <CantidadNV> notas de venta con familia "<Familia>", concepto "<Concepto>", cantidad "<Cantidad>" y documento "<Documento>"
    When el usuario accede al submodulo 'Ver Ventas'
    And filtra ventas del dia de hoy
    And activa el modo canje
    And selecciona las primeras <CantidadNV> notas de venta
    And hace clic en el boton Canjear
    And selecciona el comprobante "<ComprobanteDestino>" en el modal de canje
    And selecciona la serie "<SerieDestino>" en el modal de canje
    And confirma el canje
    Then el sistema genera el canje exitosamente

    Examples:
      | Caso  | Familia | Concepto      | Cantidad | CantidadNV | Documento   | ComprobanteDestino          | SerieDestino |
      | CP039 | gaseosa | 7753234003313 | 50       | 2          | 00000000    | BOLETA DE VENTA ELECTRONICA | B002         |
      | CP040 | gaseosa | 7753234003313 | 100      | 2          | 75893616    | BOLETA DE VENTA ELECTRONICA | B002         |
      | CP041 | gaseosa | 7753234003313 | 50       | 1          | 20542245671 | FACTURA ELECTRONICA         | F001         |

@CanjearNV
@SinSeleccion
Scenario Outline: <Caso> Verificar que el boton Canjear esta deshabilitado sin NVs seleccionadas
    When el usuario accede al submodulo 'Ver Ventas'
    And el usuario ingresa la fecha y hora "<fechaHoraInicial>" en el campo "Fecha y hora de inicio"
    And el usuario ingresa la fecha y hora "<fechaHoraFinal>" en el campo "Fecha y hora de fin"
    And hace clic en consultar ventas
    And activa el modo canje
    Then el boton Canjear permanece deshabilitado

    Examples:
      | Caso  | fechaHoraInicial    | fechaHoraFinal      |
      | CP042 | 05/03/2026 12:00 am | 25/03/2026 11:59 pm |

# Pendiente: validación de inconsistencias del modal aún no implementada en la app