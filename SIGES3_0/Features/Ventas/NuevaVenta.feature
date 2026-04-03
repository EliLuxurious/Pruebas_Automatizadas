Feature: NuevaVenta

CP001: Factura con cliente DNI sin RUC - Flujo paso a paso con selectores de QA.
CP002: Factura con cliente RUC - Flujo paso a paso con selectores de QA.
CP003: Boleta con cliente VARIOS y total mayor a 700 para validar inconsistencia.
CP004: Boleta con cliente RUC.

Background:
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
    And el usuario accede al módulo 'Ventas'
    And el usuario accede al submodulo 'Nueva Venta'

@NuevaVenta
@VentaNormal
Scenario Outline: Flujo de ventas con Venta Normal 
    When selecciona familia "<Familia>" y concepto "<Concepto>"
    And actualiza la cantidad "<Cantidad>"
    And ingresa el documento del cliente "<Documento>"
    And selecciona comprobante "<Comprobante>" con serie "<Serie>"
    And selecciona tipo de entrega "<Entrega>"
    And configura el pago "<Pago>"
    And hace clic en Guardar
    Then el sistema muestra el mensaje "<Mensaje>"
    Examples:
      | Caso  | Familia | Concepto      | Cantidad | Documento   | Comprobante                 | Serie | Entrega   | Pago       | Mensaje                   |
      | CP001 | gaseosa | 7753234003320 |          | 75893616    | FACTURA ELECTRONICA         | F002  | Inmediata | Completo   |                           |
      | CP002 | gaseosa | 7753234003320 |          | 20542245671 | FACTURA ELECTRONICA         | F002  | Inmediata | Completo   | Se registro correctamente |
      | CP003 | gaseosa | 7753234003320 | 150      | 00000000    | BOLETA DE VENTA ELECTRONICA | B002  | Inmediata | Completo   | total es mayor a 700      |
      | CP004 | gaseosa | 7753234003320 |          | 20542245671 | BOLETA DE VENTA ELECTRONICA | B002  | Inmediata | Completo   | Se registro correctamente |
      | CP005 | gaseosa | 7753234003313 | 50       | 75893616    | BOLETA DE VENTA ELECTRONICA | B002  | Diferida  | Completo   | Se registro correctamente |
      | CP006 | gaseosa | 7753234003313 | 150      | 00000000    | NOTA DE VENTA(INTERNA)      | NV02  | Inmediata | Completo   | Se registro correctamente |
      | CP007 | gaseosa | 7753234003313 | 150      | 75893616    | NOTA DE VENTA(INTERNA)      | NV02  | Inmediata | Incompleto | insuficiente              |
