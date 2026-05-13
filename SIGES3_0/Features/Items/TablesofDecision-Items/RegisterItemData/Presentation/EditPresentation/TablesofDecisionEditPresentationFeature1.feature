Feature: TablesofDecisionEditPresentationFeature1

Editar la Presentación de forma válida e inválida usando la técnica de Tablas de Decisiones

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario va a la opcion Presentacion
And el usuario cierra el sidebar



Scenario: Editar Presentación a la presentación Frasco
When el usuario edita la presentacion "Frasco"
And el usuario ingresa el codigo de presentación " 1"
And el usuario guarda los cambios al editar presentacion
And el usuario elimina los siguientes conceptos:
  | NombreConcepto                          |
  | Gaseosa de Naranja INKA COLA Frasco 3 L |
And el usuario aplica los cambios al editar presentacion
Then el sistema muestra un mensaje de confirmacion


Scenario: Editar Presentación a la presentación BOTELLAS
When el usuario edita la presentacion "BOTELLAS"
And el usuario ingresa la descripcion de presentación " 1"
And el usuario guarda los cambios al editar presentacion
And el usuario aplica los cambios al editar presentacion
Then el sistema muestra un mensaje de confirmacion


Scenario: Editar Presentación a la presentación 0123
When el usuario edita la presentacion "0123"
And el usuario ingresa la descripcion de presentación "es"
And el usuario guarda los cambios al editar presentacion
Then el sistema muestra un mensaje de confirmacion


Scenario: Editar Presnetación inválida a la presentación locazos
When el usuario edita la presentacion "locazos"
And el usuario guarda los cambios al editar presentacion
Then el sistema muestra un mensaje de error
