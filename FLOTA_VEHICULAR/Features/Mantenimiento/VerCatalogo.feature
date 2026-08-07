@ModuloMantenimiento
Feature: Gestión del Catálogo de Mantenimiento
  Como administrador del sistema
  Quiero acceder y gestionar el catálogo de mantenimiento
  Para mantener actualizados los servicios vehiculares

@NavegacionCatalogo_CP001
Scenario: Ingresar exitosamente al submódulo Ver Catálogos
  # ¡Estos dos pasos de Login se reutilizan automáticamente de tu VehiculoStepDefinitions!
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  
  # Y este es tu paso nuevo
  And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos


  @RegistroCatalogo_CP002
Scenario: Registro exitoso de un nuevo catálogo de mantenimiento
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos
  
  # Acción para abrir el modal
  When Se hace clic en el botón Nuevo Catálogo
  
  # Llenado de formulario
  And Se ingresan los datos del nuevo catálogo:
    | Campo                  | Valor                           |
    | CLASIFICADOR           | ALTA                            |
    | CLASE DE MANTENIMIENTO | A                               |
    | TIPO DE MOTOR          | COMBUSTIBLE                     |
    | FECHA DE INICIO        | 02/04/2026                      |
    | FECHA FIN              | 02/04/2027                      |
    | ACTIVIDADES            | Limpieza y regulación de frenos |
    
  Then Se procede a Guardar el catálogo



  @BloqueoCatalogoSolapado_CP028
Scenario: Registrar catalogo con fechas solapadas con catálogo en estado ACTIVO existente.
  # =======================================================
  # FASE 1: PRECONDICIÓN (Crear catálogo original)
  # =======================================================
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos
  
  When Se hace clic en el botón Nuevo Catálogo
  And Se ingresan los datos del nuevo catálogo:
    | Campo                  | Valor                           |
    | CLASIFICADOR           | ALTA                            |
    | CLASE DE MANTENIMIENTO | A                               |
    | TIPO DE MOTOR          | COMBUSTIBLE                     |
    | FECHA DE INICIO        | 01/01/2050                      |
    | FECHA FIN              | 31/12/2050                      |
    | ACTIVIDADES            | Limpieza y regulación de frenos |

  And Se procede a Guardar el catálogo
  
  
  # =======================================================
  # FASE 2: INTENTO DE SOLAPAMIENTO (El corazón del CP028)
  # =======================================================
  # Nota: Al guardar arriba, el modal debería cerrarse, así que volvemos a abrirlo.
  When Se hace clic en el botón Nuevo Catálogo
  And Se ingresan los datos del nuevo catálogo:
    | Campo                  | Valor                           |
    | CLASIFICADOR           | ALTA                            |
    | CLASE DE MANTENIMIENTO | A                               |
    | TIPO DE MOTOR          | COMBUSTIBLE                     |
    | FECHA DE INICIO        | 15/01/2026                      |
    | FECHA FIN              | 15/01/2027                     |
    | ACTIVIDADES            | Limpieza y regulación de frenos |
  And Se procede a Guardar el catálogo
  
  # =======================================================
  # FASE 3: VALIDACIÓN DE REGLA DE NEGOCIO
  # =======================================================
  Then el sistema debe impedir el registro y mostrar un error de fechas solapadas

  #
  @RegistroCatalogoSinSolapar_CP029
Scenario: Registro de catálogo con catálogo anterior en estado ACTIVO pero sin fechas SOLAPADAS
  # =======================================================
  # FASE 1: PRECONDICIÓN (Crear catálogo en el año 2025)
  # =======================================================
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos
  
  When Se hace clic en el botón Nuevo Catálogo
  And Se ingresan los datos del nuevo catálogo:
    | Campo                  | Valor                           |
    | CLASIFICADOR           | ALTA                            |
    | CLASE DE MANTENIMIENTO | C                               |
    | TIPO DE MOTOR          | COMBUSTIBLE                     |
    | FECHA DE INICIO        | 01/01/2029                      |
    | FECHA FIN              | 02/01/2029                      |
    | ACTIVIDADES            | Limpieza y regulación de frenos |
  And Se procede a Guardar el catálogo
  
  # =======================================================
  # FASE 2: PRUEBA REAL (Crear catálogo en el año 2028)
  # =======================================================
  # Como las fechas son del 2028, no chocan con las del 2025. El sistema DEBE permitirlo.
  When Se hace clic en el botón Nuevo Catálogo
  And Se ingresan los datos del nuevo catálogo:
    | Campo                  | Valor                           |
    | CLASIFICADOR           | ALTA                            |
    | CLASE DE MANTENIMIENTO | C                               |
    | TIPO DE MOTOR          | COMBUSTIBLE                     |
    | FECHA DE INICIO        | 03/02/2029                      |
    | FECHA FIN              | 04/02/2029                      |
    | ACTIVIDADES            | Limpieza y regulación de frenos |
  
  # Al usar este paso, el robot hará clic en guardar y no esperará ningún error,
  # validando exitosamente que el sistema lo dejó pasar.
  Then Se procede a Guardar el catálogo
  Then el catálogo debe guardarse exitosamente sin errores de solapamiento



  @RegistroCatalogoSolapadoConBaja_CP030
Scenario: Registrar catálogo con fechas solapadas pero con catlaogo anterior en estado DE BAJA
  # =======================================================
  # FASE 1: PRECONDICIÓN (Crear catálogo en 2040)
  # =======================================================
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos
  
  When Se hace clic en el botón Nuevo Catálogo
  And Se ingresan los datos del nuevo catálogo:
    | Campo                  | Valor                           |
    | CLASIFICADOR           | MEDIA                           |
    | CLASE DE MANTENIMIENTO | A                               |
    | TIPO DE MOTOR          | BI-COMBUSTIBLE                  |
    | FECHA DE INICIO        | 05/02/2040                      |
    | FECHA FIN              | 07/02/2040                      |
    | ACTIVIDADES            | Limpieza y regulación de frenos |
  And Se procede a Guardar el catálogo
  
  # =======================================================
  # FASE 2: DAR DE BAJA
  # =======================================================
  # Aquí asumimos que después de guardar, el sistema abre la vista detalle de la foto, 
  # o regresamos a la grilla y entramos al registro para borrarlo.
 
 When Se busca por fechas "05/02/2040" y "07/02/2040" y se da de baja el catálogo
 
  
  # =======================================================
  # FASE 3: PRUEBA REAL (Crear catálogo solapado en 2040)
  # =======================================================
  When Se hace clic en el botón Nuevo Catálogo
  And Se ingresan los datos del nuevo catálogo:
    | Campo                  | Valor                           |
    | CLASIFICADOR           | MEDIA                           |
    | CLASE DE MANTENIMIENTO | A                               |
    | TIPO DE MOTOR          | BI-COMBUSTIBLE                  |
    | FECHA DE INICIO        | 05/02/2040                      |
    | FECHA FIN              | 07/02/2040                      |
    | ACTIVIDADES            | Limpieza y regulación de frenos |
  And Se procede a Guardar el catálogo
  Then el catálogo debe guardarse exitosamente sin errores de solapamiento


  @CamposVacios_CP026
Scenario: Registrar catálogo con campos obligatorios vacíos (general)
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos
  
  # Abrimos el modal
  When Se hace clic en el botón Nuevo Catálogo

  
  # Omitimos el paso de llenar datos (Dejamos los campos obligatorios en blanco)
  
  # Validamos directamente que el sistema se defienda
  Then el botón Guardar debe estar deshabilitado para impedir el registro

  @EdicionCatalogoVigente_CP037
Scenario: Editar catálogo con estado VIGENTE (CP037)(cp033)
  # =======================================================
  # FASE 1: PRECONDICIÓN (Crear catálogo en 2029 para editarlo)
  # =======================================================
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos
  
  When Se hace clic en el botón Nuevo Catálogo
  And Se ingresan los datos del nuevo catálogo:
    | Campo                  | Valor                           |
    | CLASIFICADOR           | MEDIA                           |
    | CLASE DE MANTENIMIENTO | A                               |
    | TIPO DE MOTOR          | COMBUSTIBLE                     |
    | FECHA DE INICIO        | 02/01/2042                      |
    | FECHA FIN              | 03/01/2042                      |
    | ACTIVIDADES            | Limpieza y regulación de frenos |
  And Se procede a Guardar el catálogo
  Then el catálogo debe guardarse exitosamente sin errores de solapamiento
  
  # =======================================================
  # FASE 2: BUSCAR Y ABRIR DETALLE
  # =======================================================
  When Se busca por fechas "02/01/2042" y "03/01/2042" y se abre el detalle
  
  # =======================================================
  # FASE 3: PRUEBA DE EDICIÓN
  # =======================================================
  # Le indicamos qué actividad nueva agregar y qué fila de la tabla eliminar (fila 1)
  When Se edita el catálogo agregando la actividad "LIMPIEZA DE BUJÍAS" y eliminando la fila 1
  And Se procede a Guardar el catálogo
  
  # Como es un Happy Path, el sistema debe permitir guardar los cambios exitosamente
  Then el catálogo debe guardarse exitosamente sin errores de solapamiento



  @EdicionCaducadoAVigente_CP039
Scenario: Editar fecha de catálogo CADUCADO para que cambie a VIGENTE (CP039-cp035)
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos
  
  # FASE 1: Buscar al catálogo muerto y abrir su edición
  When Se busca el primer catálogo en estado "CADUCADO" y se edita
  
  # FASE 2: La Resurrección (Reutilizando tu calendario)
  # Cambiamos las fechas a unas futuras para que el sistema lo reviva
  And Se actualizan las fechas del catálogo a inicio "01/01/2026" y fin "31/12/2026"
  And Se procede a Guardar el catálogo
  
  # FASE 3: Validaciones
  Then el catálogo debe guardarse exitosamente sin errores de solapamiento
  And el estado del catálogo editado debe cambiar a "VIGENTE"




  @EdicionCaducadoFechaPasada_CP038
Scenario: Editar catálogo CADUCADO con fecha pasada (CP038)(cp034)
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos
  
  # FASE 1: Buscar al catálogo muerto y abrir su edición (¡Reutilizado!)
  When Se busca el primer catálogo en estado "CADUCADO" y se edita
  
  # FASE 2: Intentar poner una fecha pasada (Ej. 2022) (¡Reutilizado!)
  And Se actualizan las fechas del catálogo a inicio "01/01/2022" y fin "31/12/2022"
  
  # FASE 3: Validación del bloqueo (Si el botón no se bloquea, el test falla atrapando el bug)
  Then el sistema debe impedir asignar una fecha pasada bloqueando el botón guardar



  #La función de guardar no sirve entonces al buscar no encuentra el catálogo y por tanto no puede terminar lo demás

  @ClonarCatalogoFechaMenor_CP031
Scenario: Clonar catálogo con nueva Fecha de Inicio menor a la del origen (CP031)(cp027)
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos
  When Se hace clic en el botón Nuevo Catálogo
  And Se ingresan los datos del nuevo catálogo:
    | Campo                  | Valor                           |
    | CLASIFICADOR           | ALTA                            |
    | CLASE DE MANTENIMIENTO | A                               |
    | TIPO DE MOTOR          | COMBUSTIBLE                     |
    | FECHA DE INICIO        | 15/02/2043                      |
    | FECHA FIN              | 31/12/2043                      |
    | ACTIVIDADES            | Limpieza y regulación de frenos |
  And Se procede a Guardar el catálogo
  Then el catálogo debe guardarse exitosamente sin errores de solapamiento
  
  # =======================================================
  # FASE 2: BUSCAR Y ABRIR PARA CLONAR
  # =======================================================
  When Se busca por fechas "15/02/2026" y "31/12/2026" y se abre el detalle
  And Se hace clic en el botón Clonar
  
  # =======================================================
  # FASE 3: INTENTAR CLONAR CON FECHA MENOR (Trampa)
  # =======================================================
  # Reutilizamos tu tabla para llenar el modal de clonación
  And Se ingresan los datos del nuevo catálogo:
    | Campo                  | Valor                           |
    | CLASIFICADOR           | ALTA                            |
    | CLASE DE MANTENIMIENTO | A                               |
    | FECHA DE INICIO        | 10/02/2043                      |
    | FECHA FIN              | 31/12/2043                      |
  And Se procede a Guardar el catálogo
  
  # =======================================================
  # FASE 4: VALIDAR EL BLOQUEO
  # =======================================================
  Then el sistema debe mostrar un error de solapamiento de fechas


  #Flujo completo
  @ClonarDeBaja_Ideal_CP032
Scenario: Clonar un catálogo origen que está en estado DEBAJA(cp28).
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos
  
  # FASE 1: CREAR
  When Se hace clic en el botón Nuevo Catálogo
  And Se ingresan los datos del nuevo catálogo:
    | Campo                  | Valor                           |
    | CLASIFICADOR           | MEDIA                           |
    | CLASE DE MANTENIMIENTO | A                               |
    | FECHA DE INICIO        | 01/01/2047                      |
    | FECHA FIN              | 31/12/2047                      |
    | ACTIVIDADES            | Limpieza y regulación de frenos |
  And Se procede a Guardar el catálogo
  
  # FASE 2: DAR DE BAJA (Aquí el test muere actualmente por el bug)
  When Se busca por fechas "01/01/2047" y "31/12/2047" y se da de baja el catálogo
  
  # FASE 3: CLONAR
  When Se busca por fechas "01/01/2028" y "31/12/2028" y se abre el detalle
  And Se hace clic en el botón Clonar
  And Se ingresan los datos del nuevo catálogo:
    | Campo                  | Valor                           |
    | FECHA DE INICIO        | 01/01/2047                      |
    | FECHA FIN              | 31/12/2047                      |
  And Se modifica la lista de actividades agregando "LIMPIEZA DE BUJÍAS" y eliminando la fila 1
  And Se procede a Guardar el catálogo
  Then el catálogo debe guardarse exitosamente sin errores de solapamiento

  #Flujo solo clonación con el fin de pribar esto ya que el buscar no funciona para hacer le flujo completo 

  @ClonarDeBaja_Workaround_CP032
Scenario: v2 Clonar un catálogo origen que ya está en estado DE BAJA (CP032)
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos
  
  # FASE 1: Buscar directamente un registro DE BAJA en la tabla y abrir Clonación
  When Se busca el primer catálogo en estado "DE BAJA" y se clona
   
  # Eliminamos la actividad precargada y ponemos una nueva como pide el excel
  And Se modifica la lista de actividades agregando "LIMPIEZA DE BUJÍAS" y eliminando la fila 1
  
  And Se procede a Guardar el catálogo
  
  # FASE 3: Validación
  Then el catálogo debe guardarse exitosamente sin errores de solapamiento



  @ClonarCamposVacios_CP031
  Scenario: Clonar catálogo dejando campos obligatorios vacíos
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
    And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos
    
    # 1. Buscamos un catálogo cualquiera para clonarlo
    When Se busca el primer catálogo en estado "VIGENTE" y se clona
    
    # 2. Las fechas ya están vacías, así que solo eliminamos las actividades
    And Se eliminan todas las actividades precargadas
    
    # 3. Validamos que el botón de guardar no nos deje avanzar
    Then el botón Guardar debe estar deshabilitado para impedir el registro


    @FiltroEstadoVigente_CP074
  Scenario: Listar catálogo por estado VIGENTE (CP074)
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
    And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos
    
    # NUEVO PASO QUE HACE LA LIMPIEZA Y SELECCIÓN
    When Se limpian los filtros y se selecciona el estado "VIGENTE"
    And Se hace clic en el botón BUSCAR principal
    
    # Validamos la columna correcta
    Then todos los resultados en la columna "ESTADO-CATALOGO" deben coincidir con "VIGENTE"


  @FiltroEstadoCaducado_CP075
  Scenario: Listar catálogo por estado CADUCADO (CP075)
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
    And Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos
    
    # NUEVO PASO
    When Se limpian los filtros y se selecciona el estado "CADUCADO"
    And Se hace clic en el botón BUSCAR principal
    
    Then todos los resultados en la columna "ESTADO-CATALOGO" deben coincidir con "CADUCADO"