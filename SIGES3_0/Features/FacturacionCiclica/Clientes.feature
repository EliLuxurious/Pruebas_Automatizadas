Feature: Baja de Clientes en Facturación Cíclica

  Background:
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
    And navega al módulo 'Facturación Cíclica'
    And Se ingresa al submódulo 'Clientes'

  @baja_cliente_exitosa
  Scenario: Dar de baja un cliente correctamente
    Given existe un cliente en estado "Activo"
    When el usuario solicita dar de baja el cliente
    And confirma la operación de baja
    Then el cliente cambia a estado "Dado de Baja"

  @rechazo_baja_cliente
  Scenario: Cancelar la baja de un cliente
    Given existe un cliente en estado "Activo"
    When el usuario solicita dar de baja el cliente
    And cancela la operación de baja
    Then el cliente permanece en estado "Activo"
    And no se realiza la baja del cliente 

  @descarga_contrato_cliente
  Scenario: Descargar contrato de un cliente activo
    Given existe un cliente en estado "Activo"
    When el usuario solicita descargar el contrato del cliente
    Then se muestra la ventana de impresión del contrato