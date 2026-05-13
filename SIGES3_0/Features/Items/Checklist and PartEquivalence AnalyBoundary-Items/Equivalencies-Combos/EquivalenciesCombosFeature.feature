Feature: EquivalenciesCombosFeature

Registrar Equivalencias entre Productos y Combos 

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Equivalencia entre Productos y Combos


Scenario: Registrar Equivalencia Entre Producto
When el usuario selecciona la opcion Equivalencia entre Productos
And el usuario selecciona este producto "Azúcar Rubia"
And el usuario ingresa la cantidad "3"
And el usuario selecciona de este producto "Azúcar 4651 K G SP 156 UN"
Then el sistema agrega la equivalencia


Scenario: Registrar Combos
When el usuario selecciona la opcion Combos
And el usuario selecciona el concepto "Ajo Pelado KG"
And el usuario ingresa la cantidad del concepto "3"
And el usuario agrega el concepto al combo
And el usuario selecciona el producto final del combo "Ají de Gallina"
And el usuario guarda el combo
Then el sistema muestra un mensaje de confirmacion exitosa


Scenario: Editar Equivalencia Entre Producto
When el usuario edita la equivalencia "Azúcar Rubia"
And el usuario selecciona de este producto "Azúcar roja SP 1 UN"
And el usuario ingresa la cantidad "3"
Then el sistema agrega la equivalencia


Scenario: Editar Equivalencia Entre Producto y eliminar equivalencia
When el usuario edita la equivalencia "Azúcar Rubia"
And el usuario elimina la equivalencia "Azúcar roja SP 1 UN"
Then el sistema agrega la equivalencia