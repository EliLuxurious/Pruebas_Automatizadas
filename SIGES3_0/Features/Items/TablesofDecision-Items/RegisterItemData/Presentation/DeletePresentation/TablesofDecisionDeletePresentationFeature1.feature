Feature: TablesofDecisionDeletePresentationFeature1

Dar de Baja Presentacion de forma válida e inválida utilizando la técnica de Tablas de Decisiones

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario va a la opcion Presentacion
And el usuario cierra el sidebar



Scenario: Dar de Baja Presentación a la presentación
When el usuario elimina la presentacion "BOTELLAS"
And el usuario selecciona la presentacion a reasignar "LATA"
And el usuario reasigna y elimina la presentacion
Then el sistema muestra un mensaje de confirmacion


Scenario: Dar de Baja Presentación a la presentación Bolsa de plástico
When el usuario elimina la presentacion "Bolsa de plástico"
And el usuario elimina todos los conceptos de la tabla
And el usuario reasigna y elimina la presentacion
Then el sistema muestra un mensaje de confirmacion


Scenario: Dar de Baja Presentación a la presentación SACO
When el usuario elimina la presentacion "SACO"
Then el sistema muestra un mensaje de confirmacion


Scenario: Dar de Baja Presentación a la presentación locazos
Then el usuario desactiva la presentación "locazos"


Scenario: Dar de Baja Presentació inválido a la presentación Frasco (EN ESPERA)
Then el usuario desactiva la presentación "Frasco"