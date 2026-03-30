Feature: TablesofDecisionEditPresentationFeature1

Editar la Presentación de forma válida e inválida usando la técnica de Tablas de Decisiones

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto


Scenario: Editar Presentación a la presentación 369
When el usuario va a la opcion Presentacion
And el usuario edita la presentacion "369"
And el usuario ingresa el codigo de presentación "0"
And el usuario guarda los cambios al editar presentacion
And el usuario elimina el concepto al editar presentacion
Then el usuario aplica los cambios al editar presentacion


Scenario: Editar Presentación a la presentación neymar
When el usuario va a la opcion Presentacion
And el usuario edita la presentacion "neymar"
And el usuario ingresa la descripcion de presentación "ista"
And el usuario guarda los cambios al editar presentacion
Then el usuario aplica los cambios al editar presentacion


Scenario: Editar Presentación a la presentación 0123
When el usuario va a la opcion Presentacion
And el usuario edita la presentacion "0123"
And el usuario ingresa la descripcion de presentación "es"
And el usuario guarda los cambios al editar presentacion


Scenario: Editar Presnetación inválida a la presentación martinez
When el usuario va a la opcion Presentacion
And el usuario edita la presentacion "martinez"
And el usuario guarda los cambios al editar presentacion