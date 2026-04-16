Feature: VerVentas

Cobertura base para acciones disponibles en ver ventas.

Background:
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
    And el usuario accede al módulo 'Ventas'
    And el usuario accede al submodulo 'Ver Ventas'

@CanjearNV
Scenario Outline: <Caso> Canjear <CantidadNV> NV con comprobante <Comprobante> cliente
    When el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And hace clic en consultar ventas
    And activa el modo canje
    And selecciona <CantidadNV> notas de venta
    And hace clic en el boton Canjear
    And selecciona el comprobante "<Comprobante>" en el modal de canje
    And selecciona la serie "<Serie>" en el modal de canje
    And confirma el canje
    Then el sistema genera el canje exitosamente

    Examples:
      | Caso  | TipoCliente | CantidadNV | Comprobante                 | Serie | fechaHoraInicial    | fechaHoraFinal      |
      | CP039 | VARIOS      | 2          | BOLETA DE VENTA ELECTRONICA | B002  | 05/03/2026 12:00 am | 25/03/2026 11:59 pm |
      | CP040 | DNI         | 2          | BOLETA DE VENTA ELECTRONICA | B002  | 05/03/2026 12:00 am | 25/03/2026 11:59 pm |
      | CP041 | RUC         | 1          | FACTURA ELECTRONICA         | F001  | 05/03/2026 12:00 am | 25/03/2026 11:59 pm |

@CanjearNV
@SinSeleccion
Scenario Outline: <Caso> Verificar que el boton Canjear esta deshabilitado sin NVs seleccionadas
    When el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And hace clic en consultar ventas
    And activa el modo canje
    Then el boton Canjear permanece deshabilitado

    Examples:
      | Caso  | fechaHoraInicial    | fechaHoraFinal      |
      | CP042 | 05/03/2026 12:00 am | 25/03/2026 11:59 pm |

@CanjearNV
@MismoCliente
Scenario Outline: <Caso> Canjear NV con comprobante cliente sin restriccion de mismo cliente
    When el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And hace clic en consultar ventas
    And activa el modo canje
    And selecciona <CantidadNV> notas de venta
    And hace clic en el boton Canjear
    And selecciona el comprobante "<Comprobante>" en el modal de canje
    And selecciona la serie "<Serie>" en el modal de canje
    And confirma el canje
    Then el sistema genera el canje exitosamente

    Examples:
      | Caso  | TipoCliente | CantidadNV | Comprobante                 | Serie | fechaHoraInicial    | fechaHoraFinal      |
      | CP043 | RUC         | 2          | FACTURA ELECTRONICA         | F002  | 05/03/2026 12:00 am | 25/03/2026 11:59 pm |
      | CP044 | DNI         | 2          | BOLETA DE VENTA ELECTRONICA | B002  | 05/03/2026 12:00 am | 25/03/2026 11:59 pm |

@CanjearNV
@Inconsistencia
Scenario Outline: <Caso> Canjear <CantidadNV> NV con comprobante <Comprobante> genera inconsistencia
    When el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
    And hace clic en consultar ventas
    And activa el modo canje
    And selecciona <CantidadNV> notas de venta
    And hace clic en el boton Canjear
    And selecciona el comprobante "<Comprobante>" en el modal de canje
    And selecciona la serie "<Serie>" en el modal de canje
    Then el sistema muestra una advertencia de inconsistencia
    And el boton Aceptar permanece deshabilitado

    Examples:
      | Caso  | CantidadNV | Comprobante                 | Serie | fechaHoraInicial    | fechaHoraFinal      |
      | CP045 | 2          | BOLETA DE VENTA ELECTRONICA | B002  | 05/03/2026 12:00 am | 25/03/2026 11:59 pm |
      | CP046 | 2          | FACTURA ELECTRONICA         | F001  | 05/03/2026 12:00 am | 25/03/2026 11:59 pm |
