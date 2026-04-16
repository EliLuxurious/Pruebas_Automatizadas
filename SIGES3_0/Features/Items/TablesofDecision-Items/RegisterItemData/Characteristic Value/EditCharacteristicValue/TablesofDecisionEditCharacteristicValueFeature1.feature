Feature: TablesofDecisionEditCharacteristicValueFeature1

Edición de Valor de Característica Común utilizando la técnica de Tablas de Decisiones

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opcion Valor de Caracteristica


Scenario: Editar Valor de Caracteristica Común (SE ACTUALIZA EL NOMBRE DEL CONCEPTO)
When el usuario selecciona la caracteristica comun "COLOR"
And el usuario edita el valor de característica común "ROJOS"
And el usuario ingresa el valor de caracteristica comun " VIVOS"
And el usuario guarda los cambios al editar valor de caracteristica comun
Then el sistema muestra un mensaje de confirmacion
When el usuario acepta actualizar el concepto con el valor de caracteristica comun


Scenario: Editar Valor de Caracteristica Común (SE ELIMINA EL CONCEPTO) EN ESPERA
When el usuario selecciona la caracteristica comun "COLOR"
And el usuario edita el valor de característica común "AZULES"
And el usuario ingresa el valor de caracteristica comun " CLAROS"
And el usuario guarda los cambios al editar valor de caracteristica comun
Then el sistema muestra un mensaje de confirmacion
When el usuario elimina los siguientes conceptos:
  | NombreConcepto                                     |
  | Cuaderno upipi AZULES Bolsa de plástico 1 UN       |
  | Cuaderno doble doble AZULES Bolsa de plástico 1 UN |

When el usuario acepta actualizar el concepto con el valor de caracteristica comun


Scenario: Editar Valor de Caracteristica Común (NO TIENE CONCEPTOS REGISTRADOS) EN ESPERA
When el usuario selecciona la caracteristica comun "COLOR"
And el usuario edita el valor de característica común "turquesa"
And el usuario ingresa el valor de caracteristica comun " b"
And el usuario guarda los cambios al editar valor de caracteristica comun
Then el sistema muestra un mensaje de confirmacion



Scenario: Edición inválida de  Valor de Caracteristica Común (NO MODIFICÓ NADA) EN ESPERA
When el usuario selecciona la caracteristica comun "COLOR"
And el usuario edita el valor de característica común "PRUEBA"
And el usuario guarda los cambios al editar valor de caracteristica comun
Then el sistema muestra un mensaje de error