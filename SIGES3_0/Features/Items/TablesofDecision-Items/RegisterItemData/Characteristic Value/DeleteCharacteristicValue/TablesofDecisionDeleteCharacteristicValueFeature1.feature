Feature: TablesofDecisionDeleteCharacteristicValueFeature1

Eliminar Valor de Caracteristica Comun utilizando la técnica de Tablas de Decisiones

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opcion Valor de Caracteristica
And el usuario cierra el sidebar


Scenario: Dar de Baja  Valor de Caracteristica Común sin conceptos asociados
When el usuario selecciona la caracteristica comun "MODALIDAD"
And el usuario elimina el valor de característica común "SEMI-PRESENCIAL"
Then el sistema muestra un mensaje de confirmacion


Scenario: Dar de Baja  Valor de Caracteristica Común con conceptos asociados (SE ELIMINA EL CONCEPTO REGISTRADO)
When el usuario selecciona la caracteristica comun "MARCA"
And el usuario elimina el valor de característica común "CLEVER"
And el usuario elimina los siguientes conceptos:
  | NombreConcepto                        |
  | BALANZA ELECTRONICA SP 30 KG          |
And el usuario acepta eliminar el concepto con el valor de caracteristica comun



Scenario: Dar de Baja  Valor de Caracteristica Común con conceptos asociados (SE REASIGNA NUEVO VALOR)
When el usuario selecciona la caracteristica comun "MARCA"
And el usuario elimina el valor de característica común "CLEVER"
And el usuario selecciona el nuevo valor de característica común "FANTA"
And el usuario acepta eliminar el concepto con el valor de caracteristica comun



Scenario: Dar de Baja  Valor de Caracteristica Común de forma inválida (NO REASIGNA UN NUEVO VALOR) EN ESPERA
When el usuario selecciona la caracteristica comun "MARCA"
And el usuario elimina el valor de característica común "ELVIVE"
And el usuario acepta eliminar el concepto con el valor de caracteristica comun