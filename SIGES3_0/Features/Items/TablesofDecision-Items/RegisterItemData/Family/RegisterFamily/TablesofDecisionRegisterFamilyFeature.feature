Feature: Registrar, Editar y Dar de Baja Familia usando tablas de decisión

Background:
  Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
  When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
  And el usuario accede al módulo Conceptos
  And el usuario selecciona Registrar Datos de Concepto
  And el usuario selecciona la opción Familia

Scenario Outline: Registro exitoso de familia
  When el usuario selecciona el tipo "<tipo_familia>"
  And el usuario selecciona el tipo de tratamiento "<tratamiento_igv>"
  And el usuario establece la detracción en "<estado_detraccion>"
  And el usuario selecciona el tipo de detracción "<tipo_detraccion>"
  And el usuario ingresa el código de familia "<codigo>"
  And el usuario ingresa el nombre de familia "<familia>"
  And el usuario selecciona la categoria "<categoria>"
  And el usuario ingresa la caracteristica comun "<caract_comun>" y su estado "<estado_comun>"
  And el usuario ingresa la caracteristica propia "<caract_propia>" y su estado "<estado_propia>"
  Then se guarda el registro

  Examples:
	| tipo_familia | tratamiento_igv                 | estado_detraccion | tipo_detraccion              | codigo   | familia     | categoria               | caract_comun | caract_propia | estado_comun | estado_propia |
	| Bien         | Exoneración de IGV (Ley de IGV) | Inactivo          |                              | PA0001   | Pan         | PANADERÍA               | PRECIOS      | FORMA         | Activo       | Inactivo      |
	| Servicio     | IGV para Restaurantes y Hoteles | Inactivo          |                              | IT0001   | Instalación | SERVICIOS TÉCNICOS      | TIPO         |               | Inactivo     |               |
	| Bien         | Exoneración de IGV (Ley de IGV) | Activo            | RECURSOS HIDROBIOLÓGICOS 003 | LE0001   | Leche       | LÁCTEOS                 |              |               | Activo       |               |
	| Servicio     | IGV para Restaurantes y Hoteles | Activo            | RECURSOS HIDROBIOLÓGICOS 003 | PREB-001 | PRUEBA      | SIN CATEGORÍA           | TIPO         |               | Activo       |               |
    


Scenario Outline: Registro inválido de familia por falta de campos
  When el usuario selecciona el tipo "<tipo_familia>"
  And el usuario selecciona el tipo de tratamiento "<tratamiento_igv>"
  And el usuario establece la detracción en "<estado_detraccion>"
  And el usuario selecciona el tipo de detracción "<tipo_detraccion>"
  And el usuario ingresa el código de familia "<codigo>"
  And el usuario ingresa el nombre de familia "<familia>"
  And el usuario selecciona la categoria "<categoria>"
  And el usuario ingresa la caracteristica comun "<caract_comun>" y su estado "<estado_comun>"
  Then no se guarda el registro

  Examples:
	| tipo_familia | tratamiento_igv                 | estado_detraccion | tipo_detraccion | codigo  | familia      | categoria                 | caract_comun | estado_comun |
	| Bien         | Exoneración de IGV (Ley de IGV) | Activo            |                 |         | Fideos       | PASTAS                    | MARCA        | Activo       |
	| Servicio     | IGV para Restaurantes y Hoteles | Activo            |                 |         | Capacitación | SERVICIOS ADMINISTRATIVOS |              | Inactivo     |
	| Bien         | Exoneración de IGV (Ley de IGV) | Activo            |                 | ANA0001 | Analgésico   | MEDICAMENTOS              |              | Activo       |
	| Servicio     | IGV para Restaurantes y Hoteles | Inactivo          |                 | MON0001 | Monitoreo    | SERVICIOS DE SEGURIDAD    | MODALIDAD    | Inactivo     |

