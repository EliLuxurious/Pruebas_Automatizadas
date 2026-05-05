Feature: TablesofDecisionRegisterCharacteristicDistinctiveFeature1

Registro válido e inválido de Característica Propia utilizando la técnica de Tablas de Decisiones

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opción Características
And el usuario selecciona el tipo de Caracteristica "Propia"

Scenario: Registrar nueva Característica Propia
When el usuario ingresa el codigo de caracteristica propia "2026"
And el usuario ingresa el nombre de caracteristica propia "Modelo"
And el usuario selecciona el tipo de dato "Texto"
Then se guarda el registro


Scenario: Registro inválido de Característica Propia (CODIGO DE CARACTERÍSTICA PROPIA EXISTENTE)
When el usuario ingresa el codigo de caracteristica propia "1243525"
And el usuario ingresa el nombre de caracteristica propia "KLK"
And el usuario selecciona el tipo de dato "Texto"
Then no se guarda el registro
