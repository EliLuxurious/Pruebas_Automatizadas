Feature: TablesofDecisionEditFamilyFeature

Editar las familias con tablas de decisiones

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario va a la opcion Familia


Scenario: Editar Familia a la Familia Harina Igv18
When el usuario edita la familia "Harina Igv18"
And el usuario ingresa el código de familia " 1"
And el usuario guarda los cambios de familia
And el usuario elimina los siguientes conceptos:
  | NombreConcepto       |
  | Harina Para Hornear B l a n c a F l o r SP UN |
And el usuario aplica los cambios de familia
Then el sistema muestra un mensaje de confirmacion


Scenario: Editar Familia a la Familia PLANTASs ExoIgv
When el usuario edita la familia "PLANTASs ExoIgv"
And el usuario ingresa el código de familia " 1"
And el usuario guarda los cambios de familia
And el usuario aplica los cambios de familia
Then el sistema muestra un mensaje de confirmacion



Scenario: Editar Familia a la Familia KEKE Igv18
When el usuario edita la familia "KEKE Igv18"
And el usuario ingresa el código de familia " 1"
And el usuario guarda los cambios de familia
Then el sistema muestra un mensaje de confirmacion



Scenario: Edición inválida de Familia a la Familia SLAS
When el usuario edita la familia "slas"
And el usuario guarda los cambios de familia
Then el sistema muestra un mensaje de error
