Feature: TablesofDecisionRegisterCategoryFeature1

Registro válido e inválido de Categoría con la técnica de Tablas de Decisiones

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opción Categoría

Scenario: Registrar nueva Categoria sin cateogría padre
When el usuario ingresa el nombre de categoría "PICKLE"
And el usuario ingresa la descripcion de categoría "MANÍ"
Then se guarda el registro


Scenario: Registro inválido de Categoria (CATEGORIA EXISTENTE)
When el usuario ingresa el nombre de categoría "holi"
And el usuario ingresa la descripcion de categoría "boli"
Then no se guarda el registro

