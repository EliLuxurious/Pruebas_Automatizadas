Feature: TablesofDecisionDeleteCharacteristicValueFeature1

Eliminar Valor de Caracteristica Comun utilizando la técnica de Tablas de Decisiones

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opcion Valor de Caracteristica


Scenario: Dar de Baja  Valor de Caracteristica Común sin conceptos asociados
When el usuario selecciona la caracteristica comun "MODALIDAD"
And el usuario elimina el valor de característica común "SEMI-PRESENCIAL"
