Feature: TablesofDecisionEditCategoryFeature1

Editar categoria con la técnica de Tabla de decisión

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto


Scenario: Editar Categoria de la categoría SAD
When el usuario va a la opcion categoria
And el usuario edita la categoria "sad"
And el usuario ingresa la descripcion de categoría "estoy triste"
Then el sistema guarda los cambios al editar categoria


Scenario: Edición inválida de Categoria de la categoría GOL
When el usuario va a la opcion categoria
And el usuario edita la categoria "gol"
Then el sistema no guarda los cambios de editar categoria

