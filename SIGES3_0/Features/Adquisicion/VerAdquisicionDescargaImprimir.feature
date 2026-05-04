@VerAdquisicionDescargarImprimir
Feature: Ver Adquicision Especifica Descargar
Scenario: Ver adquisicion especifica Descargar
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Ver Adquisición'
	When Se configuran los filtros de búsqueda:
      | Campo         | Valor                            |
      | Fecha Inicial | 01/01/2026                       |
      | Fecha Final   | 25/03/2026                       |
      | Proveedor     | KRISTELL VALERIA-FALCON-VILLEGAS |
    And Se hace clic en el botón de buscar
    And Selecciono el primer registro de la tabla para ver su detalle
    And Cambio el formato del documento a 'A4'
    Then Ejecuto la acción de 'Descargar' el documento
    And Valido que la acción de 'Descargar' se ejecutó correctamente

Scenario: Ver adquisicion especifica Imprimir
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Ver Adquisición'
	When Se configuran los filtros de búsqueda:
      | Campo         | Valor                            |
      | Fecha Inicial | 01/01/2026                       |
      | Fecha Final   | 25/03/2026                       |
      | Proveedor     | KRISTELL VALERIA-FALCON-VILLEGAS |
    And Se hace clic en el botón de buscar
    And Selecciono el primer registro de la tabla para ver su detalle
    And Cambio el formato del documento a 'TICKET'
    Then Ejecuto la acción de 'Imprimir' el documento