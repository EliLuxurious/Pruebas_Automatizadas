Feature: RegisterItemDataFeature

Registro de Datos de Concepto 


Scenario: Registro exitoso de familia por el Tipo BIEN
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opción Familia
And el usuario selecciona el tipo "Bien"
And el usuario selecciona el tipo de tratamiento "Exoneracion IGV"
And el usuario ingresa el código de familia "PRU-001"
And el usuario ingresa el nombre de familia "PRUEBA"
And el usuario selecciona la categoria "SIN CATEGORÍA"
Then se guarda el registro


Scenario: Registro exitoso de familia por el Tipo SERVICIO
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opción Familia
And el usuario selecciona el tipo "Servicio"
And el usuario selecciona el tipo de tratamiento "IGV Restaurantes"
And el usuario selecciona la opción Detracción
And el usuario selecciona el tipo de detracción "RECURSOS HIDROBIOLÓGICOS 003"
And el usuario ingresa el código de familia "PRU-002"
And el usuario ingresa el nombre de familia "PRUEBA2"
And el usuario selecciona la categoria "SERVICIOS TÉCNICOS"
Then se guarda el registro


Scenario: Registro inválido de Familia por el no cumplimiento de campos obligatorios -  Porcentaje de detracción faltante, Categoria faltante
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opción Familia
And el usuario selecciona el tipo "Bien"
And el usuario selecciona el tipo de tratamiento "Exoneracion IGV"
And el usuario selecciona la opción Detracción
And el usuario ingresa el código de familia "PRU-003"
And el usuario ingresa el nombre de familia "Mesa"
Then no se guarda el registro



Scenario: Registro exitoso de Categoría con categoría padre
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opción Categoría
And el usuario ingresa el nombre de categoría "ENSALADAS FRESCAS"
And el usuario ingresa la descripcion de categoría "ENSALADAS FRESCAS Y SALUDABLES"
And el usuario selecciona la categoria padre "SIN CATEGORÍA"
Then se guarda el registro



Scenario: Registro exitoso de Categoría sin categoría padre
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opción Categoría
And el usuario ingresa el nombre de categoría "ELECTRODOMÉSTICOS"
And el usuario ingresa la descripcion de categoría "PRODUCTOS ELÉCTRICOS"
Then se guarda el registro



Scenario: Registro inválido de Categoría por el no cumplimiento de campos obligatorios - Descripción faltante
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opción Categoría
And el usuario ingresa el nombre de categoría "PRUEBA CATEGORIA"
And el usuario ingresa la descripcion de categoría " "
Then no se guarda el registro



Scenario: Registro exitoso de Presentación por el cumplimiento de campos obligatorios
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opcion Presentación
And el usuario ingresa el codigo de presentación "PRE010"
And el usuario ingresa el nombre de presentación "Blister"
And el usuario ingresa la descripcion de presentación "Presentación en empaque plástico sellado"
Then se guarda el registro



Scenario: Registro exitoso de Característica Propia por el cumplimiento de campos obligatorios
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opción Características
And el usuario selecciona el tipo de Caracteristica "Propia"
And el usuario ingresa el codigo de caracteristica propia "CAR001"
And el usuario ingresa el nombre de caracteristica propia "VOLTAJE"
And el usuario selecciona el tipo de dato "Numérico"
Then se guarda el registro



Scenario: Registro inválido de Característica Propia por el no cumplimiento de campos obligatorios - Tipo de Texto faltante
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opción Características
And el usuario selecciona el tipo de Caracteristica "Propia"
And el usuario ingresa el codigo de caracteristica propia "CAR003"
And el usuario ingresa el nombre de caracteristica propia "COLOR"
Then no se guarda el registro



Scenario: Registro exitoso de Característica Común por el cumplimiento de campos obligatorios
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opción Características
And el usuario selecciona el tipo de Caracteristica "Comun"
And el usuario ingresa el nombre de Caracteristica Comun "OLOR"
Then se guarda el registro



Scenario: Registro exitoso de valor de Característica Común
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opcion Valor de Caracteristica
And el usuario selecciona la caracteristica comun "COLOR"
And el usuario ingresa el valor de caracteristica comun "AGUAMARINA"
And el usuario guarda el valor de caracteristica comun
Then el sistema muestra un mensaje de confirmacion



Scenario: Registro exitoso de asignar valor de característica  a Familia
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Registrar Datos de Concepto
And el usuario selecciona la opcion Asignar Valor de Caracteristica
And el usuario selecciona la Familia a asignar "Cuaderno"
And el usuario ingresa el valor a asignar "PRUEBA3"
And el usuario arrastra el valor "PRUEBA3"
Then se guarda la asignacion

