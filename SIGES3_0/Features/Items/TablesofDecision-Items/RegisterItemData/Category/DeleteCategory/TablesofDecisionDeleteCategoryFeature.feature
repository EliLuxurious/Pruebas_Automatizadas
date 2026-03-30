Feature: TablesofDecisionDeleteCategoryFeature

Dar de Baja la categoría de forma válida e inválida  usando la técnica Tablas de Decisiones

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto

Scenario: Dar de Baja Categoria de la categoría adios
When el usuario va a la opcion categoria
Then el usuario elimina la categoria "adios"

