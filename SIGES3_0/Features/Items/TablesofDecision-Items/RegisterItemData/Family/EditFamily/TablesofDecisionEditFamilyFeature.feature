Feature: TablesofDecisionEditFamilyFeature

Editar las familias con tablas de decisiones

Scenario: Editar Familia a la Familia BARRA DE ENSALADA
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
When el usuario va a la opcion Familia
And el usuario edita la familia "Barra de Ensaladas"
And el usuario selecciona el tipo "Servicio"
And el usuario selecciona el tipo de tratamiento "IGV Restaurantes"
And el usuario guarda los cambios de familia
And el usuario elimina el concepto a editar familia
Then el usuario aplica los cambios de familia


Scenario: Editar Familia a la Familia PLANTAS
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
When el usuario va a la opcion Familia
And el usuario edita la familia "PLANTAS"
And el usuario selecciona el tipo de tratamiento "Exoneracion IGV"
And el usuario selecciona la categoria "SIN CATEGORÍA"
And el usuario guarda los cambios de familia
Then el usuario aplica los cambios de familia


Scenario: Editar Familia a la Familia Platos de Fondos
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
When el usuario va a la opcion Familia
And el usuario edita la familia "Platos de Fondos"
And el usuario ingresa el nombre de familia " Ricos"
And el usuario guarda los cambios de familia



Scenario: Edición inválida de Familia a la Familia SLAS
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
When el usuario va a la opcion Familia
And el usuario edita la familia "slas"
And el usuario guarda los cambios de familia