Feature: ViewItems

Filtros generales de Concepto

Background:
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Ver Conceptos


Scenario: Filtrado de conceptos por el modo de Familia y Categoria
When el usuario selecciona el filtro Familia "BALANZA"
And el usuario selecciona el filtro Categoria "SIN CATEGORÍA"
And el usuario presiona el botón Buscar
Then el sistema no muestra conceptos asociados


Scenario: Búsqueda fallida de los conceptos por la equivocación del usuario
When el usuario ingresa la palabra clave "cuaaderno"
Then el sistema no muestra conceptos asociados


Scenario: Limpieza de filtros y listado de todos los conceptos
When el usuario selecciona el filtro Familia "INDIVIDUAL"
And el usuario selecciona el filtro Categoria "HERRAMIENTA DE COCINA"
And el usuario presiona el botón Buscar
And el usuario restablece los filtros
And el usuario presiona el botón Buscar
Then el sistema muestra conceptos asociados


Scenario: Filtrar conceptos con la partición positiva con respecto al campo nombre de concepto 
When el usuario ingresa la palabra clave "GAS"
Then el sistema muestra conceptos asociados


Scenario: Editar el concepto Instalación NodeJs
When el usuario cierra el sidebar
And el usuario edita el concepto "Instalación NodeJs"
And el usuario ingresa el Precio "6"
Then Guardar concepto


Scenario: Eliminar el concepto Plan_Auto_0204_0926
When el usuario cierra el sidebar
And el usuario elimina el concepto "Plan_Auto_0204_0926"
Then el sistema muestra un mensaje de confirmacion