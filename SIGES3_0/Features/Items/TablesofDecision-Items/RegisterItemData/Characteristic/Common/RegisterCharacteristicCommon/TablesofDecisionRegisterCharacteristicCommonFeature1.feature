Feature: TablesofDecisionRegisterCharacteristicCommonFeature1

Registro de Caracteristica Comun utilizando la técnica de Tablas de Decisiones

Background: 

Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opción Características
And el usuario selecciona el tipo de Caracteristica "Comun"

Scenario: Registrar nueva Característica Común
When el usuario ingresa el nombre de Caracteristica Comun "Hecho"
Then se guarda el registro

