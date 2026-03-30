Feature: ViewItems

Filtros generales de Concepto

Scenario: Filtrado de conceptos por el modo de Familia y Categoria
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Ver Conceptos
And el usuario selecciona el filtro Familia "BALANZA"
And el usuario selecciona el filtro Categoria "SIN CATEGORÍA"
And el usuario presiona el botón Buscar
Then el sistema no muestra conceptos asociados


Scenario: Búsqueda fallida de los conceptos por la equivocación del usuario
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Ver Conceptos
And el usuario ingresa la palabra clave "cuaaderno"
Then el sistema no muestra conceptos asociados


Scenario: Limpieza de filtros y listado de todos los conceptos
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Ver Conceptos
And el usuario selecciona el filtro Familia "INDIVIDUAL"
And el usuario selecciona el filtro Categoria "HERRAMIENTA DE COCINA"
And el usuario presiona el botón Buscar
And el usuario restablece los filtros
And el usuario presiona el botón Buscar
Then el sistema muestra conceptos asociados


Scenario: Filtrar conceptos con la partición positiva con respecto al campo nombre de concepto 
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Ver Conceptos
And el usuario ingresa la palabra clave "GAS"
Then el sistema muestra conceptos asociados