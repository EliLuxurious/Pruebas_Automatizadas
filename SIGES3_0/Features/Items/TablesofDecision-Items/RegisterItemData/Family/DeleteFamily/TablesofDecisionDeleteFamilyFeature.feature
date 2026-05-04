Feature: TablesofDecisionDeleteFamilyFeature

Dar de Baja a las familias de forma exitosa o inválida


Background:
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto

Scenario: Dar de Baja Familia a la Familia  ANTICIPO
When el usuario va a la opcion Familia
And el usuario da de baja la familia "ANTICIPO"
And el usuario reasigna los conceptos a la nueva familia "Cuaderno"
And el usuario selecciona la característica "VERDE"
And el usuario guarda los cambios de reasignacion
Then el usuario confirma la reasignación


Scenario: Dar de Baja Familia a la Familia Barra de Ensaladas
When el usuario va a la opcion Familia
And el usuario da de baja la familia "Barra de Ensaladas"
And el usuario elimina el concepto a dar de baja
Then el usuario confirma la reasignación



Scenario: Dar de Baja Familia a la Familia agua
When el usuario va a la opcion Familia
And el usuario da de baja la familia "agua"
And el usuario reasigna los conceptos a la nueva familia "prueba"
And el usuario guarda los cambios de reasignacion
Then el usuario confirma la reasignación


Scenario: Dar de Baja Familia a la Familia 12345
When el usuario va a la opcion Familia
And el usuario da de baja la familia "12345"
Then el sistema muestra un mensaje de confirmación de baja de familia


Scenario: Dar de Baja Familia a la Familia slas
When el usuario va a la opcion Familia
And el usuario desactiva la familia "slas"



Scenario: Dar de Baja Familia a la Familia Azúcar
When el usuario va a la opcion Familia
And el usuario desactiva la familia "Azúcar"


Scenario: Dar de Baja Familia a la Familia Bebidas Frías
When el usuario va a la opcion Familia
And el usuario da de baja la familia "Bebidas Frías"
Then el sistema muestra un mensaje de error al dar de baja

