Feature: NuevaVenta

CP001: Factura con cliente DNI sin RUC - Flujo paso a paso con selectores de QA.
CP002: Factura con cliente RUC - Flujo paso a paso con selectores de QA.

Background:
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'

@NuevaVenta
@VentaNormal
Scenario: CP001 - Factura con cliente DNI sin RUC
    When abre el flujo de ventas "Nueva Venta"
    And ejecuta el flujo de nueva venta "CP001"
    Then valida el resultado esperado de la venta:
        | Campo       | Valor         |
        | SaveEnabled | NO            |
        | ExecuteSave | NO            |

@NuevaVenta
@VentaNormal
Scenario: CP002 - Factura con cliente RUC 20542245671
    When abre el flujo de ventas "Nueva Venta"
    And ejecuta el flujo de nueva venta "CP002"
    Then valida el resultado esperado de la venta:
        | Campo       | Valor                    |
        | SaveEnabled | SI                       |
        | ExecuteSave | SI                       |
        | Mensaje     | Se registro correctamente |
