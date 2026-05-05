@VerAdquisicionEspecifica
Feature: Ver Adquicision Especifica
Scenario: Ver adquisicion especifica con documento 80MM
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
    And Cambio el formato del documento a '80MM'
    Then Verifico que el botón 'Descargar' esté disponible en el visor

Scenario: Ver adquisicion especifica con documento TICKET
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
    Then Verifico que el botón 'Descargar' esté disponible en el visor

Scenario: Ver adquisicion especifica con documento A4
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
    Then Verifico que el botón 'Descargar' esté disponible en el visor