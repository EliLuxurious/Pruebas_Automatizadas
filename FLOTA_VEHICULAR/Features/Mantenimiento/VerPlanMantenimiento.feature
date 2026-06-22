@ModuloVerPlanMantenimiento
Feature: Gestión de Ver Plan Mantenimientos
  Como administrador del sistema
  Quiero acceder y gestionar los planes de mantenimientos
  Para llevar el control y la planificación de los servicios vehiculares

  @NavegacionPlanMantenimiento_CP001
  Scenario: Ingresar exitosamente al submódulo Ver Plan Mantenimientos
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
    
    # NUESTRO NUEVO PASO DE NAVEGACIÓN
    And Se ingresa al módulo Mantenimiento y submódulo Ver Plan Mantenimientos


    @RegistroPlanMantenimiento_CP066
  Scenario: Registrar plan de mantenimiento exitosamente
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
    And Se ingresa al módulo Mantenimiento y submódulo Ver Plan Mantenimientos
    
    # Inicia el registro
    When Se hace clic en el botón Nuevo Plan de Mantenimiento
    And Se ingresa el RUC "10759012017" y se busca la Razón Social
    And Se ingresa la dirección "JR.VIOLETAS 423"
    And Se ingresa el número de contrato "CONTRATO9999"
    
    # Manejo de los dos calendarios
    And Se selecciona la fecha Desde "06/05/2026"
    And Se selecciona la fecha Hasta "07/05/2026"
    
    # Documento y Guardado
    And Se adjunta el documento del plan "factura.pdf"
    Then Se guarda el registro del plan de mantenimiento


    @RegistroPlanMantenimiento_CP078
Scenario: No registrar plan de mantenimiento por número de contrato duplicado
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Plan Mantenimientos
  
  When Se hace clic en el botón Nuevo Plan de Mantenimiento
  And Se ingresa el RUC "10759012017" y se busca la Razón Social
  And Se ingresa la dirección "JR.VIOLETAS 423"
  And Se ingresa el número de contrato "CONTRATITO777"
  
  And Se selecciona la fecha Desde "06/05/2026"
  And Se selecciona la fecha Hasta "07/05/2026"
  
  And Se adjunta el documento del plan "factura.pdf"
  Then Se guarda el registro del plan de mantenimiento
  And el sistema debe mostrar el mensaje de error de plan "Numero de contrato existente"


    

    @AprobarPlanMantenimiento_CP067
Scenario: Aprobar plan de mantenimiento exitosamente
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Plan Mantenimientos

  When Se busca el plan de mantenimiento por número de contrato "CONTRATO99988"
  And Se abre el detalle del plan de mantenimiento encontrado
  And Se agregan vehículos al plan de mantenimiento
  And Se aprueba el plan de mantenimiento
  Then Se busca el plan de mantenimiento por número de contrato "CONTRATO99988"
  And el sistema debe mostrar el plan con estado "APROBADO"

  @AprobarPlanSinVehiculos_CP068
Scenario: No aprobar plan de mantenimiento sin vehículos ni mantenimientos asociados
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Plan Mantenimientos

  When Se busca el plan de mantenimiento por número de contrato "CONTRATO99999"
  And Se abre el detalle del plan de mantenimiento encontrado
  And Se intenta aprobar el plan de mantenimiento sin vehículos asociados
  Then el sistema debe mostrar mensaje de validación al aprobar plan sin vehículos




  @CrearMantenimientoPreventivo_CP055
Scenario: Crear mantenimiento preventivo válido
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Plan Mantenimientos

  # Crear plan base
  When Se hace clic en el botón Nuevo Plan de Mantenimiento
  And Se ingresa el RUC "10759012017" y se busca la Razón Social
  And Se ingresa la dirección "JR.VIOLETAS 423"
  And Se ingresa el número de contrato "CONTRATOCP0635"
  And Se selecciona la fecha Desde "01/01/2026"
  And Se selecciona la fecha Hasta "31/12/2026"
  And Se adjunta el documento del plan "factura.pdf"
  Then Se guarda el registro del plan de mantenimiento

  # Buscar plan creado
  When Se busca el plan de mantenimiento por número de contrato "CONTRATOCP0635"
  And Se abre el detalle del plan de mantenimiento encontrado

  # Agregar vehículo al plan
  And Se agregan vehículos al plan de mantenimiento

  # Crear mantenimiento preventivo
  And Se crea mantenimiento preventivo con clase "A" y fecha de ejecución "01/02/2026"
  Then el sistema debe mostrar mensaje de mantenimiento registrado exitosamente


  @CrearMantenimientoPreventivo_CP055
Scenario: Crear mantenimiento preventivo válido con vehículos específicos
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo Mantenimiento y submódulo Ver Plan Mantenimientos

  # Crear plan base
  When Se hace clic en el botón Nuevo Plan de Mantenimiento
  And Se ingresa el RUC "10759012017" y se busca la Razón Social
  And Se ingresa la dirección "JR.VIOLETAS 423"
  And Se ingresa el número de contrato "CONTRATOCP0605"
  And Se selecciona la fecha Desde "01/01/2026"
  And Se selecciona la fecha Hasta "31/12/2026"
  And Se adjunta el documento del plan "factura.pdf"
  Then Se guarda el registro del plan de mantenimiento

  # Buscar plan creado
  When Se busca el plan de mantenimiento por número de contrato "CONTRATOCP0605"
  And Se abre el detalle del plan de mantenimiento encontrado

  # Agregar vehículos específicos al plan
  And Se agregan los vehículos con placas "0496KA" y "0642LA" al plan de mantenimiento

  # Crear mantenimiento preventivo
  And Se crea mantenimiento preventivo con clase "A" y fecha de ejecución "29/05/2026"
  Then el sistema debe mostrar mensaje de mantenimiento registrado exitosamente