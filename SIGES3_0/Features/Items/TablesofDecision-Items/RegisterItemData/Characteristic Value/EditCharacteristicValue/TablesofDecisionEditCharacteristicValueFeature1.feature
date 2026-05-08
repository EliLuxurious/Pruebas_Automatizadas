Feature: TablesofDecisionEditCharacteristicValueFeature1

Edición de Valor de Característica Común utilizando la técnica de Tablas de Decisiones

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opcion Valor de Caracteristica
And el usuario cierra el sidebar

Scenario: Editar Valor de Caracteristica Común (SE ACTUALIZA EL NOMBRE DEL CONCEPTO)
When el usuario selecciona la caracteristica comun "MARCA"
And el usuario edita el valor de característica común "Flor Blanca"
And el usuario ingresa el valor de caracteristica comun " 1"
And el usuario guarda los cambios al editar valor de caracteristica comun
And el usuario acepta actualizar el concepto con el valor de caracteristica comun
Then el sistema muestra un mensaje de confirmacion


Scenario: Editar Valor de Caracteristica Común (SE ELIMINA EL CONCEPTO)
When el usuario selecciona la caracteristica comun "MARCA"
And el usuario edita el valor de característica común "TIO NACHO"
And el usuario ingresa el valor de caracteristica comun " n"
And el usuario guarda los cambios al editar valor de caracteristica comun
And el usuario elimina los siguientes conceptos:
  | NombreConcepto                                     |
  | Shampoo ANTICASPA TIO NACHO n CAJA 1 L             |
And el usuario acepta actualizar el concepto con el valor de caracteristica comun
Then el sistema muestra un mensaje de confirmacion


Scenario: Editar Valor de Caracteristica Común (NO TIENE CONCEPTOS REGISTRADOS)
When el usuario selecciona la caracteristica comun "COLOR"
And el usuario edita el valor de característica común "PRUEBA"
And el usuario ingresa el valor de caracteristica comun " 1"
And el usuario guarda los cambios al editar valor de caracteristica comun
Then el sistema muestra un mensaje de confirmacion



Scenario: Edición inválida de  Valor de Caracteristica Común (NO MODIFICÓ NADA) EN ESPERA
When el usuario selecciona la caracteristica comun "MATERIAL"
And el usuario edita el valor de característica común "Metal"
And el usuario guarda los cambios al editar valor de caracteristica comun
Then el sistema muestra un mensaje de error