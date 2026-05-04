Feature: TablesofDecisionEditCharacteristicDistinctiveFeature1

Edición válida e inválida de Característica Propia utilizando la técnica de Tablas de Decisiones

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opción Características
And el usuario selecciona el tipo de Caracteristica "Propia"

Scenario: Editar Característica Propia a la característica propia PROCESADOR
When el usuario edita la característica propia "PROCESADOR"
And el usuario ingresa el codigo de caracteristica propia "7"
Then el usuario guarda los cambios al editar caracteristica propia