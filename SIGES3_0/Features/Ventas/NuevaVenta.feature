Feature: NuevaVenta

CP001: Factura con cliente DNI sin RUC - Flujo paso a paso con selectores de QA.
CP002: Factura con cliente RUC - Flujo paso a paso con selectores de QA.
CP003: Boleta con cliente VARIOS y total mayor a 700 para validar inconsistencia.
CP004: Boleta con cliente RUC.

Background:
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'

@NuevaVenta
@VentaNormal
Scenario Outline: Flujo de ventas con Nueva venta 
    When abre el flujo de ventas "Nueva Venta"
    And ejecuta el flujo de nueva venta con familia "<Familia>", concepto "<Concepto>", cantidad "<Cantidad>", documento "<Documento>", comprobante "<Comprobante>", serie "<Serie>", entrega "<Entrega>" y pago "<Pago>"
    Then valida que Guardar habilitado sea "<Habilitado>"
    And valida que Ejecutar guardado sea "<Ejecutar>"
    And verifica el mensaje de confirmacion "<Mensaje>"

    Examples:
      | Caso  | Familia | Concepto                   | Cantidad | Documento   | Comprobante                 | Serie | Entrega   | Pago       | Habilitado | Ejecutar | Mensaje                   |
      | CP001 | gaseosa | 7753234003320              |          | 75893616    | FACTURA ELECTRONICA         | F002  | Inmediata | Completo   | NO         | NO       |                           |
      | CP002 | gaseosa | 7753234003320              |          | 20542245671 | FACTURA ELECTRONICA         | F002  | Inmediata | Completo   | SI         | SI       | Se registro correctamente |
      | CP003 | gaseosa | 7753234003320              | 150      | 00000000    | BOLETA DE VENTA ELECTRONICA | B002  | Inmediata | Completo   | NO         | NO       | total es mayor a 700      |
      | CP004 | gaseosa | 7753234003320              |          | 20542245671 | BOLETA DE VENTA ELECTRONICA | B002  | Inmediata | Completo   | SI         | SI       | Se registro correctamente |
      | CP005 | gaseosa | 7753234003313              | 50       | 75893616    | BOLETA DE VENTA ELECTRONICA | B002  | Diferida  | Completo   | SI         | SI       | Se registro correctamente |
      | CP006 | gaseosa | 7753234003313              | 150      | 00000000    | NOTA DE VENTA(INTERNA)      | NV02  | Inmediata | Completo   | SI         | SI       | Se registro correctamente |
      | CP007 | gaseosa | 7753234003313              | 150      | 75893616    | NOTA DE VENTA(INTERNA)      | NV02  | Inmediata | Incompleto | SI         | SI       | insuficiente              |
