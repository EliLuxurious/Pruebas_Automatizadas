Feature: TablesofDecisionRegisterPresentationFeature1

Registro válido e inválido de presentación utilizando la técnica de Tablas de Decisiones

Background:
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opcion Presentación

Scenario Outline: Registro exitoso de presentación
When el usuario ingresa el codigo de presentación "<codigo>"
And el usuario ingresa el nombre de presentación "<nombre>"
And el usuario ingresa la descripcion de presentación "<descripcion>"
Then se guarda el registro

Examples:
| codigo | nombre   | descripcion                                      |
| SA001  | Saco     | Presentación en saco                             |


Scenario Outline: Registro inválido de presentación
When el usuario ingresa el codigo de presentación "<codigo>"
And el usuario ingresa el nombre de presentación "<nombre>"
And el usuario ingresa la descripcion de presentación "<descripcion>"
Then no se guarda el registro

Examples:
| codigo | nombre   | descripcion                                      |	
| CA001  | CAJA     | Presentación en caja                             |
| 369    | Lata     | Presentación en lata                             |
| 369    | 369      | 369                                              |