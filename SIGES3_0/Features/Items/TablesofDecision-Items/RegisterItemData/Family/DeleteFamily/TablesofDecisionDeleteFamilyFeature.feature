Feature: TablesofDecisionDeleteFamilyFeature

Dar de Baja a las familias de forma exitosa o inválida


Background:
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario va a la opcion Familia
And el usuario cierra el sidebar


Scenario: Dar de Baja Familia a la Familia  ANTICIPO
When el usuario da de baja la familia "ANTICIPO"
And el usuario reasigna los conceptos a la nueva familia "Cuaderno"
And el usuario selecciona la característica "VERDES"
And el usuario guarda los cambios de reasignacion
And el usuario confirma la reasignación
Then el sistema muestra un mensaje de confirmacion



Scenario: Dar de Baja Familia a la Familia Barra de Ensaladas ExoIgv
When el usuario da de baja la familia "Barra de Ensaladas ExoIgv"
And el usuario elimina todos los conceptos de la tabla
And el usuario confirma la reasignación
Then el sistema muestra un mensaje de confirmacion



Scenario: Dar de Baja Familia a la Familia Maquillajes
When el usuario da de baja la familia "Maquillajes"
And el usuario reasigna los conceptos a la nueva familia "prueba"
And el usuario guarda los cambios de reasignacion
When el usuario confirma la reasignación
Then el sistema muestra un mensaje de confirmacion


Scenario: Dar de Baja Familia a la Familia vocefala Igv10
When el usuario da de baja la familia "vocefala Igv10"
Then el sistema muestra un mensaje de confirmacion


Scenario: Dar de Baja Familia a la Familia slas
Then el usuario desactiva la familia "slas"



Scenario: Dar de Baja Familia inválida a la Familia Azúcar
Then el usuario desactiva la familia "Azúcar"


Scenario: Dar de Baja Familia inválida a la Familia Bebidas Frías
When el usuario da de baja la familia "Bebidas Frías"
And el usuario confirma la reasignación
Then el sistema muestra un mensaje de error 

