@VerAdquisicionNotaCredito
Feature: Ver Adquicision NotaCredito
Scenario: Ver adquisicion Nota de Credito
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
    And Se hace click en el boton de 'Nota de Credito'
    