Feature: TestE2EFeature

Se realizará una prueba end-to-end para validar la correcta integración entre los
submódulos del módulo Conceptos y garantizar el flujo funcional completo del sistema.

Background: 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos


Scenario: Registrar un concepto mediante un flujo end-to-end completo
When el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opción Características
And el usuario selecciona el tipo de Caracteristica "Comun"
And el usuario ingresa el nombre de Caracteristica Comun "TIPO DE SOPORTE"
Then se guarda el registro
When el usuario selecciona el tipo de Caracteristica "Propia"
And el usuario ingresa el codigo de caracteristica propia "QA001"
And el usuario ingresa el nombre de caracteristica propia "TIEMPO DE ATENCION"
And el usuario selecciona el tipo de dato "Texto"
Then se guarda el registro
When el usuario selecciona la opcion Valor de Caracteristica
And el usuario selecciona la caracteristica comun "TIPO DE SOPORTE"
And el usuario ingresa el valor de caracteristica comun "REMOTO"
And el usuario guarda el valor de caracteristica comun
Then el sistema muestra un mensaje de confirmacion
When el usuario ingresa el valor de caracteristica comun "PRESENCIAL"
And el usuario guarda el valor de caracteristica comun
Then el sistema muestra un mensaje de confirmacion
When el usuario ingresa el valor de caracteristica comun "EMPRESARIAL"
And el usuario guarda el valor de caracteristica comun
Then el sistema muestra un mensaje de confirmacion
When el usuario selecciona la opcion Presentación
And el usuario ingresa el codigo de presentación "PRES-QA"
And el usuario ingresa el nombre de presentación "SOPORTE DIGITAL"
And el usuario ingresa la descripcion de presentación "No hay presentacion"
Then se guarda el registro
When el usuario selecciona la opción Categoría
And el usuario ingresa el nombre de categoría "SOPORTE TECNOLÓGICO"
And el usuario ingresa la descripcion de categoría "Soporte Tecnológico a Empresas"
Then se guarda el registro
When el usuario selecciona la opción Familia
And el usuario selecciona el tipo "Bien"
And el usuario selecciona el tipo de tratamiento "IGV Restaurantes"
And el usuario ingresa el código de familia "FAM-QA"
And el usuario ingresa el nombre de familia "SERVICIOS DE SOPORTE"
And el usuario selecciona la categoria "SOPORTE TECNOLÓGICO"
And el usuario ingresa la caracteristica comun "TIPO DE SOPORTE" y su estado "Activo"
Then se guarda el registro
When el usuario selecciona la opcion Asignar Valor de Caracteristica
And el usuario selecciona la Familia a asignar "SERVICIOS DE SOPORTE"
And el usuario arrastra el valor "REMOTO"
Then se guarda la asignacion
When el usuario selecciona Nuevo Concepto
And el usuario selecciona la Familia "SERVICIOS DE SOPORTE"
And el usuario selecciona Auto al Código
And el usuario ingresa el Sufijo "loco"
And el usuario selecciona el Rol "Item Comercial"
And el usuario selecciona el Módulo a Mostrar "MOD0001"
And el usuario selecciona la Marca "REMOTO"
And el usuario selecciona la Presentación "SOPORTE DIGITAL"
And el usuario ingresa la Cantidad "30"
And el usuario selecciona la tarifa "POR UNIDAD"
And el usuario ingresa el Precio "100"
Then Guardar concepto

