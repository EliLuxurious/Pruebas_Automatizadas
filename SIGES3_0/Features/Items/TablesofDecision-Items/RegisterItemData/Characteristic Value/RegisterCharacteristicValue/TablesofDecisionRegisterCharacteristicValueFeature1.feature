Feature: TablesofDecisionRegisterCharacteristicValueFeature1

Registro válido e inválido de Valor de Característica utilizando la técnica de Tablas de Decisiones

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opcion Valor de Caracteristica

Scenario: Registrar Valor de Caracteristica Común
When el usuario selecciona la caracteristica comun "COLOR"
And el usuario ingresa el valor de caracteristica comun "LIMA LIMÓN"
And el usuario guarda el valor de caracteristica comun
Then el sistema muestra un mensaje de confirmacion



Scenario: Registro inválido de Valor de Caracteristica Común (VALOR DE CARACTERISTICA EXISTENTE)
When el usuario selecciona la caracteristica comun "COLOR"
And el usuario ingresa el valor de caracteristica comun "AGUAMARINA"
And el usuario guarda el valor de caracteristica comun
Then el sistema muestra un mensaje de error
