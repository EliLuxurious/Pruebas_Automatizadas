Feature: TablesofDecisionDeletePresentationFeature1

Dar de Baja Presentacion de forma válida e inválida utilizando la técnica de Tablas de Decisiones

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto

Scenario: Dar de Baja Presentación a la presentación GGG
When el usuario va a la opcion Presentacion
And el usuario elimina la presentacion "GGG"
And el usuario selecciona la presentacion a reasignar "Frasco"
Then el usuario reasigna y elimina la presentacion


Scenario: Dar de Baja Presentación a la presentación neymar
When el usuario va a la opcion Presentacion
And el usuario elimina la presentacion "neymar"
And el usuario elimina el concepto al eliminar la presentacion
Then el usuario reasigna y elimina la presentacion


Scenario: Dar de Baja Presentación a la presentación 18956131
When el usuario va a la opcion Presentacion
Then el usuario desactiva la presentación "locazos"


Scenario: Dar de Baja Presentació inválido a la presentación 369
When el usuario va a la opcion Presentacion
Then el usuario desactiva la presentación "Frasco"