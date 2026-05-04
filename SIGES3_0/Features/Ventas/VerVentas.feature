@ignore
Feature: VerVentas

Cobertura base para acciones disponibles en ver ventas.

Background:
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'

@CanjearComprobante
Scenario: Canjear comprobante
    When abre el flujo de ventas "Ver Ventas"
    And Ingresar fecha inicial "27/01/2025"
    And Ingresar fecha final "20/02/2025"
    And Click en consultar ventas
    And Buscar venta 'NV02-53'
    And Activar canje
    And Seleccionar venta
    And Click en el boton canjear
    And Seleccionar el tipo de comprobante "BOLETA DE VENTA ELECTRONICA"
    And Click en el boton aceptar
    Then Ver comprobante

@NotaDebito
Scenario: Emitir nota de debito con aumento en el valor
    When abre el flujo de ventas "Ver Ventas"
    And Ingresar fecha inicial "27/01/2025"
    And Ingresar fecha final "19/02/2025"
    And Click en consultar ventas
    And Buscar venta 'B002-27905'
    And Ver venta buscada
    And Elegir tipo de nota 'DEBITO'
    And Seleccionar el tipo de nota "AUMENTO EN EL VALOR"
    And Seleccionar el documento "NOTA DE DEBITO"
    And Escribir el motivo de la nota "Aumento el valor"
    And Ingresar el aumento de valor de la nota '60'
    And Guardar nota
    Then Ver comprobante

@EnviarComprobante
Scenario: Enviar comprobante
    When abre el flujo de ventas "Ver Ventas"
    And Ingresar fecha inicial "27/01/2025"
    And Ingresar fecha final "13/02/2025"
    And Click en consultar ventas
    And Buscar venta 'B002-27909'
    And Ver venta buscada
    And Click en el boton enviar
    And Ingresar correo 'kevinsanchezcabrerakevin@gmail.com'
    And Click en el boton agregar el correo
    Then Enviar comprobante de venta
