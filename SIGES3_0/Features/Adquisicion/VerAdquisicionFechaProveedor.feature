@VerAdquisicion
Feature: Ver Adquicision 
Scenario: Ver Adquisicion Fecha Proveedor
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Ver Adquisición'
	When Se configuran los filtros de búsqueda:
      | Campo         | Valor                          |
      | Fecha Inicial | 01/01/2026                     |
      | Fecha Final   | 27/02/2026                     |
      | Proveedor     | LEONARDO OSWALDO LOPEZ CONDEZO |
    And Se hace clic en el botón de buscar
    Then El sistema actualiza la tabla mostrando los registros correspondientes al proveedor 'LEONARDO OSWALDO LOPEZ CONDEZO'

Scenario: Ver Adquisicion Fecha
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Ver Adquisición'
	When Se configuran los filtros de búsqueda:
      | Campo         | Valor                          |
      | Fecha Inicial | 01/01/2026                     |
      | Fecha Final   | 27/03/2026                     |
    And Se hace clic en el botón de buscar

Scenario: Ver Adquisicion Proveedor
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Ver Adquisición'
	When Se configuran los filtros de búsqueda:
      | Campo         | Valor                            |
      | Proveedor     | KRISTELL VALERIA-FALCON-VILLEGAS |
    And Se hace clic en el botón de buscar
    Then El sistema actualiza la tabla mostrando los registros correspondientes al proveedor 'KRISTELL VALERIA-FALCON-VILLEGAS'
