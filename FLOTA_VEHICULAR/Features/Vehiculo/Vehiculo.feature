@Vehiculo
Feature: Gestión de Vehículos

@RegistroVehiculo
Scenario: Registrar nuevo vehículo
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"

    And Se ingresa al módulo "Vehículo"
    And Se selecciona "+Nuevo"

    When Se ingresan los datos del vehículo:
    | Campo            | Valor            |
    | PLACA            | ANTONI           |
    | AREA ASIGNADA    | DPAM             |
    | PROPIETARIO      | MIMP             |
    | MARCA            | KIA              | 
    | MODELO           | RIO              |
    | AÑO              | 2026             |
    | TIPO DE VEHICULO | AUTOMOVIL        |
    | CLASIFICADOR     | ALTA             |
    | COLOR            | ROJO             |
    | NUMERO MOTOR     | ENG998877        |
    | TIPO COMBUSTIBLE | G-90             |
    | TIPO MOTOR       | COMBUSTIBLE      |
    | RANGO CONSUMO    | 45               |
    | NUMERO SERIE     | ABCD123456789012A|

    Then Se procede a "GUARDAR" el vehículo



    @BajaVehiculo
Scenario: Dar de baja a un vehículo existente
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"

    And Se ingresa al módulo "Vehículo"
    
    When Se busca el vehículo por placa "EGM303"
    And Se hace clic en ver vehículo
    And Se hace clic en dar de baja
    And Se ingresan las observaciones "Vehículo en mal estado técnico, se procede a dar de baja definitiva."
    Then Se confirma la baja del vehículo


    @EditarVehiculo
    Scenario: Editar un vehículo existente
        Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
        When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"

        And Se ingresa al módulo "Vehículo"
        
        # 1. Buscamos y entramos al detalle
        When Se busca el vehículo por placa "EGG337"
        And Se hace clic en ver vehículo
        
        # 2. Clic en el icono de lápiz
        And Se hace clic en editar
        
        # 3. Reutilizamos tu paso maestro (sin la fila PLACA)
        When Se ingresan los datos del vehículo:
        | Campo            | Valor            |
        | AREA ASIGNADA    | DPAM             |
        | PROPIETARIO      | PCM              |
        | MARCA            | KIA              | 
        | MODELO           | RIO              |
        | AÑO              | 2025             |
        | TIPO DE VEHICULO | AUTOMOVIL        |
        | CLASIFICADOR     | MEDIA            |
        | COLOR            | AZUL             |
        | NUMERO MOTOR     | NUEVOENG123      |
        | TIPO COMBUSTIBLE | G-95             |
        | TIPO MOTOR       | BI-COMBUSTIBLE   |
        | RANGO CONSUMO    | 50               |
        | NUMERO SERIE     | NUEVOSERIE9876   |

        # 4. Guardamos los cambios
        Then Se procede a "GUARDAR" el vehículo

     
@FlujoCompletoPlacaDeBaja
Scenario: Registrar nuevo vehículo con placa existente en estado DEBAJA
    # PASO 1: Registrar el vehículo con la placa que luego daremos de baja
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
    And Se ingresa al módulo "Vehículo"
    And Se selecciona "+Nuevo"
    When Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | OLC009            |
        | AREA ASIGNADA    | DPAM              |
        | PROPIETARIO      | MIMP              |
        | MARCA            | DAEWOO            | 
        | MODELO           | RIO               |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | ALTA              |
        | COLOR            | ROJO              |
        | NUMERO MOTOR     | MOTOR-TEMP-01     |
        | TIPO COMBUSTIBLE | G-90              |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 45                |
        | NUMERO SERIE     | SERIETEMP001FF    |
    And Se procede a "GUARDAR" el vehículo

    # PASO 2: Dar de baja la placa OLD001
    And Se busca el vehículo por placa "OLC009"
    And Se hace clic en ver vehículo
    And Se hace clic en dar de baja
    And Se ingresan las observaciones "Baja técnica para probar reasignación de placa."
    And Se confirma la baja del vehículo
    And Se cierra la ventana de detalles

    # PASO 3: Registrar el vehículo definitivo reasignando la placa OLD001
    And Se selecciona "+Nuevo"
    When se ingresa la placa "OLC009" y se valida que no exista el error de duplicado
    And Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | OLD009            |
        | AREA ASIGNADA    | DPAM              |
        | PROPIETARIO      | MIMP              |
        | MARCA            | HYUNDAI           | 
        | MODELO           | SONATA            |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | MEDIA             |
        | COLOR            | BLANCO            |
        | NUMERO MOTOR     | MTRNUEVO02        |
        | TIPO COMBUSTIBLE | G-90              |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 405               |
        | NUMERO SERIE     | SERIE123456789    |

    Then el botón GUARDAR debe estar habilitado para permitir la reasignación
    And Se procede a "GUARDAR" el vehículo


@ValidarPlacaDuplicadaActiva
Scenario: No permitir el registro de un vehículo con placa ya existente y ACTIVA
    # PASO 1: Registrar un vehículo inicial (para que la placa quede ACTIVA)
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
    And Se ingresa al módulo "Vehículo"
    And Se selecciona "+Nuevo"
    When Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | ACT011            |
        | AREA ASIGNADA    | DPAM              |
        | PROPIETARIO      | MIMP              |
        | MARCA            | TOYOTA            | 
        | MODELO           | RIO               |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | ALTA              |
        | COLOR            | NEGRO             |
        | NUMERO MOTOR     | MOTOR-ACT-01      |
        | TIPO COMBUSTIBLE | G-90              |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 50                |
        | NUMERO SERIE     | SERIEACT018888    |
    And Se procede a "GUARDAR" el vehículo

    # PASO 2: Intentar registrar otro vehículo con la misma placa
    And Se selecciona "+Nuevo"
    
    # LLAMADA AL NUEVO MÉTODO DE ERROR
    When se ingresa la placa "ACT011" y el sistema debe mostrar error de duplicado por estar ACTIVO
    
    # Validamos que no se pueda guardar
    Then el botón GUARDAR debe estar deshabilitado


    @FlujoCompletoMotorDeBaja
Scenario: Registrar nuevo vehículo con número de motor de un vehículo dado de baja
    # PASO 1: Registrar el vehículo original
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
    And Se ingresa al módulo "Vehículo"
    And Se selecciona "+Nuevo"
    When Se ingresan los datos del vehículo:
      | Campo            | Valor             |
      | PLACA            | REU558            |
      | AREA ASIGNADA    | DPAM              |
      | PROPIETARIO      | MIMP              |
      | MARCA            | DAEWOO            | 
      | MODELO           | TICO SL           |
      | AÑO              | 2026              |
      | TIPO DE VEHICULO | AUTOMOVIL         |
      | CLASIFICADOR     | ALTA              |
      | COLOR            | ROJO              |
      | NUMERO MOTOR     | MOTOR-REUTILIZAR  |
      | TIPO COMBUSTIBLE | G-90              |
      | TIPO MOTOR       | COMBUSTIBLE       |
      | RANGO CONSUMO    | 45                |
      | NUMERO SERIE     | SERIE9988776655AA |
    And Se procede a "GUARDAR" el vehículo
    
    # PASO 2: Dar de baja el vehículo que acabamos de crear
    And Se busca el vehículo por placa "REU558"
    And Se hace clic en ver vehículo
    And Se hace clic en dar de baja
    And Se ingresan las observaciones "Baja técnica para prueba de reutilización de motor."
    And Se confirma la baja del vehículo
    And Se cierra la ventana de detalles
    # PASO 3: Registrar un nuevo vehículo usando el mismo motor
    And Se selecciona "+Nuevo"
    When Se ingresan los datos del vehículo:
      | Campo            | Valor             |
      | PLACA            | NVA999            |
      | AREA ASIGNADA    | DPAM              |
      | PROPIETARIO      | INCAUTADO         |
      | MARCA            | KIA               |
      | MODELO           | RIO               |
      | AÑO              | 2026              |
      | TIPO DE VEHICULO | AUTOMOVIL         |
      | CLASIFICADOR     | MEDIA             |
      | COLOR            | AZUL              |
      | NUMERO MOTOR     | MOTOR-REUTILIZAR  |
      | TIPO COMBUSTIBLE | G-95              |
      | TIPO MOTOR       | COMBUSTIBLE       |
      | RANGO CONSUMO    | 50                |
      | NUMERO SERIE     | SERIENUEVA1234    |
    Then Se procede a "GUARDAR" el vehículo
    And El sistema valida que el motor "MOTOR-REUTILIZAR" es aceptado por estar DE BAJA

    #aña
    @ValidarMotorDuplicadoActivo
Scenario: Registrar vehiculo con motor ya existente en estado ACTIVO
    # PASO 1: Registrar un vehículo inicial (Vehículo A)
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
    And Se ingresa al módulo "Vehículo"
    And Se selecciona "+Nuevo"
    When Se ingresan los datos del vehículo:
      | Campo            | Valor             |
      | PLACA            | BOL123            |
      | AREA ASIGNADA    | DPAM              |
      | PROPIETARIO      | MIMP              |
      | MARCA            | KIA               | 
      | MODELO           | RIO               |
      | AÑO              | 2026              |
      | TIPO DE VEHICULO | AUTOMOVIL         |
      | CLASIFICADOR     | MEDIA             |
      | COLOR            | NEGRO             |
      | NUMERO MOTOR     | MTR999            |
      | TIPO COMBUSTIBLE | G-90              |
      | TIPO MOTOR       | COMBUSTIBLE       |
      | RANGO CONSUMO    | 100               |
      | NUMERO SERIE     | 1GNEK1234567890AB |
    And Se procede a "GUARDAR" el vehículo

    # PASO 2: Intentar registrar otro vehículo con el MISMO motor (MTR999)
    And Se selecciona "+Nuevo"
    When Se ingresan los datos del vehículo:
      | Campo            | Valor             |
      | PLACA            | XAZ557            |
      | AREA ASIGNADA    | DPAM              |
      | PROPIETARIO      | MIMP              |
      | MARCA            | KIA               | 
      | MODELO           | RIO               |
      | AÑO              | 2026              |
      | TIPO DE VEHICULO | AUTOMOVIL         |
      | CLASIFICADOR     | MEDIA             |
      | COLOR            | NEGRO             |
      | NUMERO MOTOR     | MTR999            |
      | TIPO COMBUSTIBLE | G-90              |
      | TIPO MOTOR       | COMBUSTIBLE       |
      | RANGO CONSUMO    | 100               |
      | NUMERO SERIE     | 2GNEK0000000000XX |
    Then el botón GUARDAR debe permanecer inhabilitado


    @ValidarMotorDuplicadoActivo
Scenario: Registro de  vehículo con motor ya existente en estado ACTIVO
    # PASO 1: Registrar el vehículo original (Quedará en estado ACTIVO)
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
    And Se ingresa al módulo "Vehículo"
    And Se selecciona "+Nuevo"
    When Se ingresan los datos del vehículo:
      | Campo            | Valor             |
      | PLACA            | MTR-116           |
      | AREA ASIGNADA    | DPAM              |
      | PROPIETARIO      | MIMP              |
      | MARCA            | NISSAN            | 
      | MODELO           | RIO               |
      | AÑO              | 2026              |
      | TIPO DE VEHICULO | AUTOMOVIL         |
      | CLASIFICADOR     | MEDIA             |
      | COLOR            | PLATA             |
      | NUMERO MOTOR     | MOTOR-BLOQUEADO   |
      | TIPO COMBUSTIBLE | G-90              |
      | TIPO MOTOR       | COMBUSTIBLE       |
      | RANGO CONSUMO    | 40                |
      | NUMERO SERIE     | SERIEUNICA0100    |
    And Se procede a "GUARDAR" el vehículo
    
    # PASO 2: Intentar registrar un nuevo vehículo con el mismo motor
    And Se selecciona "+Nuevo"
    And Se ingresan los datos del vehículo:
      | Campo            | Valor             |
      | PLACA            | MTR-222           |
      | AREA ASIGNADA    | DPAM              |
      | PROPIETARIO      | MIMP              |
      | MARCA            | NISSAN            |
      | MODELO           | RIO               |
      | AÑO              | 2026              |
      | TIPO DE VEHICULO | AUTOMOVIL         |
      | CLASIFICADOR     | MEDIA             |
      | COLOR            | PLATA             |
      | NUMERO MOTOR     | MOTOR-BLOQUEADO   |
      | TIPO COMBUSTIBLE | G-90              |
      | TIPO MOTOR       | COMBUSTIBLE       |
      | RANGO CONSUMO    | 40                |
      | NUMERO SERIE     | SERIEUNICA0001    |
     
      
    # LLAMADA AL MÉTODO DE VALIDACIÓN DE BUG
    When se ingresa el motor "MOTOR-BLOQUEADO" y el sistema debe impedir el registro por estar asociado a un vehículo ACTIVO
    
    Then el botón GUARDAR debe estar deshabilitado


    @ValidarEdicionCompleta
Scenario: Registrar y luego validar que edición sin cambios mantenga el botón bloqueado
    # PASO 1: Registro previo para asegurar data limpia
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
    And Se ingresa al módulo "Vehículo"
    And Se selecciona "+Nuevo"
    When Se ingresan los datos del vehículo:
      | Campo            | Valor             |
      | PLACA            | EAI77S            |
      | AREA ASIGNADA    | DPAM              |
      | PROPIETARIO      | MIMP              |
      | MARCA            | NISSAN            | 
      | MODELO           | RIO               |
      | AÑO              | 2026              |
      | TIPO DE VEHICULO | AUTOMOVIL         |
      | CLASIFICADOR     | MEDIA             |
      | COLOR            | NEGRO             |
      | NUMERO MOTOR     | MTR-EDIT-99       |
      | TIPO COMBUSTIBLE | G-90              |
      | TIPO MOTOR       | COMBUSTIBLE       |
      | RANGO CONSUMO    | 100               |
      | NUMERO SERIE     | SERIEYEDITT99HH   |
    And Se procede a "GUARDAR" el vehículo

    # PASO 2: Validar CP-006 (Edición sin cambios)
    And Se busca el vehículo por placa "EAI77S"
    And Se hace clic en ver vehículo
    And Se hace clic en editar
    Then el botón GUARDAR debe permanecer inhabilitado por falta de cambios
    
  @EditarVehiculo_CP007
Scenario: Editar campos de vehiculo en estado ACTIVO 
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
    And Se ingresa al módulo "Vehículo"
    
    # 1. Buscamos y entramos al detalle
    And Se busca el vehículo por placa "EAI77S"
    And Se hace clic en ver vehículo
    And Se hace clic en editar
    
    # 2. Modificamos un dato 
    When Se ingresan los datos del vehículo:
      | Campo         | Valor         |
      | COLOR         | BLANCO        |
    
    # 3. Tu validación estrella
    Then el botón GUARDAR debe habilitarse al detectar cambios en el formulario
    And Se procede a "GUARDAR" el vehículo

    #Actualmente falla aunque no debería ser así ya que el sistema no permite editar vehiculos con estado AVERIADO.
@EditarVehiculoAveriado_CP014 
Scenario: Editar vehiculo con estado de registro ACTIVO y AVERIADO
  # =======================================================
  # FASE 1: REGISTRO DEL VEHÍCULO (Nace ACTIVO y OPERATIVO)
  # =======================================================
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo "Vehículo"
  And Se selecciona "+Nuevo"
  And Se ingresan los datos del vehículo:
    | Campo            | Valor             |
    | PLACA            | AVEE77            |
    | AREA ASIGNADA    | DPAM              |
    | PROPIETARIO      | MIMP              |
    | MARCA            | TOYOTA            | 
    | MODELO           | RIO               |
    | AÑO              | 2026              |
    | TIPO DE VEHICULO | AUTOMOVIL         |
    | CLASIFICADOR     | ALTA              |
    | COLOR            | NEGRO             |
    | NUMERO MOTOR     | MTR-AVEE-77       |
    | TIPO COMBUSTIBLE | G-90              |
    | TIPO MOTOR       | COMBUSTIBLE       |
    | RANGO CONSUMO    | 50                |
    | NUMERO SERIE     | SERIEAVEEUU77X    |
  And Se procede a "GUARDAR" el vehículo

  # =======================================================
  # FASE 2: CAMBIAR ESTADO A AVERIADO (Precondición)
  # =======================================================
  And Se busca el vehículo por placa "AVEE77"
  And Se hace clic en ver vehículo
  And se hace clic en reportar avería
  And se ingresan las observaciones de avería "Vehículo averiado para prueba de edición CP014."
  And se confirma el reporte de la avería
  And Se cierra la ventana de detalles

  # =======================================================
  # FASE 3: INTENTAR EDITAR EL VEHÍCULO (Donde saltará el BUG)
  # =======================================================
  When Se busca el vehículo por placa "AVEE77"
  And Se hace clic en ver vehículo
  # Aquí es donde Selenium no encontrará el botón y lanzará tu Exception
  And Se hace clic en editar
  
  # =======================================================
  # FASE 4: MODIFICAR Y GUARDAR (Para cuando arreglen el bug)
  # =======================================================
  And Se ingresan los datos del vehículo:
    | Campo | Valor  |
    | COLOR | BLANCO |
  Then el botón GUARDAR debe habilitarse al detectar cambios en el formulario
  And Se procede a "GUARDAR" el vehículo



    @SeguridadEstadoBaja_CP008
Scenario: Editar vehiculos con estado de registro DE BAJA
    # PASO 1: Registro (Estado inicial ACTIVO)
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
    And Se ingresa al módulo "Vehículo"
    And Se selecciona "+Nuevo"
    And Se ingresan los datos del vehículo:
      | Campo            | Valor             |
      | PLACA            | BAJA01            |
      | AREA ASIGNADA    | DPAM              |
      | PROPIETARIO      | MIMP              |
      | MARCA            | NISSAN            | 
      | MODELO           | RIO               |
      | AÑO              | 2026              |
      | TIPO DE VEHICULO | AUTOMOVIL         |
      | CLASIFICADOR     | MEDIA             |
      | COLOR            | NEGRO             |
      | NUMERO MOTOR     | MTR-EDIT-99       |
      | TIPO COMBUSTIBLE | G-90              |
      | TIPO MOTOR       | COMBUSTIBLE       |
      | RANGO CONSUMO    | 100               |
      | NUMERO SERIE     | SERIEYEDITT99H    |
   
    And Se procede a "GUARDAR" el vehículo

    # PASO 2: Dar de baja (Estado final DE BAJA)
    And Se busca el vehículo por placa "BAJA01"
    And Se hace clic en ver vehículo
    And Se hace clic en dar de baja
    And Se ingresan las observaciones "Prueba de seguridad CP008."
    And Se confirma la baja del vehículo
    And Se cierra la ventana de detalles

    # PASO 3: Validación de seguridad (El núcleo del CP008)
    And Se busca el vehículo por placa "BAJA01"
    And Se hace clic en ver vehículo
    Then el sistema no debe mostrar la opción de editar para vehículos con estado de baja




@FlujoCompleto_ReportarAveria_CP009
Scenario: Rertortar avería en vehiculo con estado operativo 
  
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo "Vehículo"
  And Se selecciona "+Nuevo"
  And Se ingresan los datos del vehículo:
    | Campo            | Valor             |
    | PLACA            | AVE008            |
    | AREA ASIGNADA    | DPAM              |
    | PROPIETARIO      | MIMP              |
    | MARCA            | KIA               | 
    | MODELO           | RIO               |
    | AÑO              | 2026              |
    | TIPO DE VEHICULO | AUTOMOVIL         |
    | CLASIFICADOR     | MEDIA             |
    | COLOR            | NEGRO             |
    | NUMERO MOTOR     | MTRAVERIA005      |
    | TIPO COMBUSTIBLE | G-90              |
    | TIPO MOTOR       | COMBUSTIBLE       |
    | RANGO CONSUMO    | 100               |
    | NUMERO SERIE     | SERIEAVE005XJJ    |
  And Se procede a "GUARDAR" el vehículo

 
  And Se busca el vehículo por placa "AVE008"
  Then al buscar la placa "AVE008" en la bandeja, su estado debe ser "OPERATIVO"
  
  When Se hace clic en ver vehículo
  And se hace clic en reportar avería
  And se ingresan las observaciones de avería "Vehículo presenta fallas en el motor (Prueba QA)."
  And se confirma el reporte de la avería
  And Se cierra la ventana de detalles
  
  And Se busca el vehículo por placa "AVE008" 
  Then al buscar la placa "AVE008" en la bandeja, su estado debe ser "AVERIADO"


  @FlujoCompleto_Reparacion_CP010
Scenario: Registrar reparación y verificar cambio de estado de AVERIADO a OPERATIVO
  # =======================================================
  # FASE 1: CREACIÓN DEL VEHÍCULO (Nace OPERATIVO)
  # =======================================================
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo "Vehículo"
  And Se selecciona "+Nuevo"
  And Se ingresan los datos del vehículo:
    | Campo            | Valor             |
    | PLACA            | REP201            |
    | AREA ASIGNADA    | DPAM              |
    | PROPIETARIO      | MIMP              |
    | MARCA            | TOYOTA            | 
    | MODELO           | RIO               |
    | AÑO              | 2026              |
    | TIPO DE VEHICULO | AUTOMOVIL         |
    | CLASIFICADOR     | ALTA              |
    | COLOR            | BLANCO            |
    | NUMERO MOTOR     | MTR-REP-001       |
    | TIPO COMBUSTIBLE | G-90              |
    | TIPO MOTOR       | COMBUSTIBLE       |
    | RANGO CONSUMO    | 50                |
    | NUMERO SERIE     | SERIEREP001XAA    |
  And Se procede a "GUARDAR" el vehículo

  # =======================================================
  # FASE 2: REPORTAR AVERÍA (Cambia a AVERIADO)
  # =======================================================
  And Se busca el vehículo por placa "REP201"
  And Se hace clic en ver vehículo
  And se hace clic en reportar avería
  And se ingresan las observaciones de avería "Se reporta falla para iniciar prueba de reparación."
  And se confirma el reporte de la avería
  And Se cierra la ventana de detalles
  
  # Validamos que realmente se averió en la bandeja
  And Se busca el vehículo por placa "REP201"
  Then al buscar la placa "REP201" en la bandeja, su estado debe ser "AVERIADO"

  # =======================================================
  # FASE 3: REGISTRAR REPARACIÓN (Vuelve a OPERATIVO)
  # =======================================================
  When Se hace clic en ver vehículo
  And se hace clic en registrar reparación
  And se ingresan las observaciones de avería "Se arregló las fallas en el motor (Prueba QA)."
  And se confirma el reporte de la avería
  And Se cierra la ventana de detalles

  # =======================================================
  # FASE 4: VALIDACIÓN FINAL EN BANDEJA
  # =======================================================
  And Se busca el vehículo por placa "REP001"
  Then al buscar la placa "REP001" en la bandeja, su estado debe ser "OPERATIVO"


  @BloqueoMantenimiento_VehiculoDeBaja_CP011
  #Registrar avería/reparación para vehiculo en estado DEBAJA
Scenario: Registrar avería para vehículo DE BAJA y OPERATIVO
  
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo "Vehículo"
  And Se selecciona "+Nuevo"
  And Se ingresan los datos del vehículo:
    | Campo            | Valor             |
    | PLACA            | BAJA99            |
    | AREA ASIGNADA    | DPAM              |
    | PROPIETARIO      | MIMP              |
    | MARCA            | TOYOTA            | 
    | MODELO           | RIO               |
    | AÑO              | 2026              |
    | TIPO DE VEHICULO | AUTOMOVIL         |
    | CLASIFICADOR     | ALTA              |
    | COLOR            | NEGRO             |
    | NUMERO MOTOR     | MTR-BAJA-999      |
    | TIPO COMBUSTIBLE | G-90              |
    | TIPO MOTOR       | COMBUSTIBLE       |
    | RANGO CONSUMO    | 50                |
    | NUMERO SERIE     | SERIE0BAJA0999    |
  And Se procede a "GUARDAR" el vehículo

  And Se busca el vehículo por placa "BAJA99"
  And Se hace clic en ver vehículo
  And Se hace clic en dar de baja
  And Se ingresan las observaciones "Baja técnica para prueba de bloqueo de mantenimiento."
  And Se confirma la baja del vehículo
  And Se cierra la ventana de detalles

  When Se busca el vehículo por placa "BAJA99"
  And Se hace clic en ver vehículo
  # El paso definitivo que valida 
  Then no deben mostrarse ni habilitarse los botones de flujo de mantenimiento


  @BloqueoReparacion_VehiculoOperativo_CP012
Scenario: Registrar reparación en vehiculo con estado OPERATIVO y ACTIVO
  # =======================================================
  # FASE 1: CREACIÓN DEL VEHÍCULO (Nace OPERATIVO por defecto)
  # =======================================================
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo "Vehículo"
  And Se selecciona "+Nuevo"
  And Se ingresan los datos del vehículo:
    | Campo            | Valor             |
    | PLACA            | OPER97            |
    | AREA ASIGNADA    | DPAM              |
    | PROPIETARIO      | MIMP              |
    | MARCA            | NISSAN            | 
    | MODELO           | RIO               |
    | AÑO              | 2026              |
    | TIPO DE VEHICULO | AUTOMOVIL         |
    | CLASIFICADOR     | MEDIA             |
    | COLOR            | PLATA             |
    | NUMERO MOTOR     | MTR-OPER-99       |
    | TIPO COMBUSTIBLE | G-90              |
    | TIPO MOTOR       | COMBUSTIBLE       |
    | RANGO CONSUMO    | 40                |
    | NUMERO SERIE     | SERIE7OPER799X    |
  And Se procede a "GUARDAR" el vehículo

  # =======================================================
  # FASE 2: VALIDACIÓN DE LÓGICA DE INTERFAZ (CP012)
  # =======================================================
  And Se busca el vehículo por placa "OPER97"
  And Se hace clic en ver vehículo
  Then no debe mostrarse el botón Registrar reparación y solo debe mostrarse Reportar avería



  @BloqueoAveria_VehiculoAveriado_CP013
Scenario: Reportar avería para vehiculo en estado ACTIVO y AVERIADO
  # =======================================================
  # FASE 1: CREACIÓN DEL VEHÍCULO (Nace OPERATIVO por defecto)
  # =======================================================
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo "Vehículo"
  And Se selecciona "+Nuevo"
  And Se ingresan los datos del vehículo:
    | Campo            | Valor             |
    | PLACA            | AVER11            |
    | AREA ASIGNADA    | DPAM              |
    | PROPIETARIO      | MIMP              |
    | MARCA            | TOYOTA            | 
    | MODELO           | RIO               |
    | AÑO              | 2026              |
    | TIPO DE VEHICULO | AUTOMOVIL         |
    | CLASIFICADOR     | ALTA              |
    | COLOR            | AZUL              |
    | NUMERO MOTOR     | MTR-AVER-11       |
    | TIPO COMBUSTIBLE | G-90              |
    | TIPO MOTOR       | COMBUSTIBLE       |
    | RANGO CONSUMO    | 50                |
    | NUMERO SERIE     | SERIEAVER11XHH    |
  And Se procede a "GUARDAR" el vehículo

  # =======================================================
  # FASE 2: REPORTAR AVERÍA (Cumplir la precondición del Excel)
  # =======================================================
  And Se busca el vehículo por placa "AVER11"
  And Se hace clic en ver vehículo
  And se hace clic en reportar avería
  And se ingresan las observaciones de avería "Se avería el vehículo para la prueba lógica."
  And se confirma el reporte de la avería
  And Se cierra la ventana de detalles

  # =======================================================
  # FASE 3: VALIDACIÓN DE LÓGICA DE INTERFAZ (El caso de prueba)
  # =======================================================
  When Se busca el vehículo por placa "AVER11"
  And Se hace clic en ver vehículo
  Then no debe mostrarse el botón Reportar avería y solo debe mostrarse Registrar reparación


  @DarDeBaja_CP015
Scenario: Dar de baja vehiculo con estado de registro ACTIVO y OPERATIVO
  # =======================================================
  # FASE 1: REGISTRO (Nace ACTIVO y OPERATIVO)
  # =======================================================
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo "Vehículo"
  And Se selecciona "+Nuevo"
  And Se ingresan los datos del vehículo:
    | Campo            | Valor             |
    | PLACA            | CP0015            |
    | AREA ASIGNADA    | DPAM              |
    | PROPIETARIO      | MIMP              |
    | MARCA            | NISSAN            | 
    | MODELO           | RIO               |
    | AÑO              | 2026              |
    | TIPO DE VEHICULO | AUTOMOVIL         |
    | CLASIFICADOR     | MEDIA             |
    | COLOR            | GRIS              |
    | NUMERO MOTOR     | MTR-CP015         |
    | TIPO COMBUSTIBLE | G-90              |
    | TIPO MOTOR       | COMBUSTIBLE       |
    | RANGO CONSUMO    | 50                |
    | NUMERO SERIE     | SERIE99900008H    |
  And Se procede a "GUARDAR" el vehículo

  # =======================================================
  # FASE 2: VALIDAR ESTADO INICIAL EN BANDEJA
  # =======================================================
  And Se busca el vehículo por placa "CP0015"
  Then al buscar la placa "CP0015" en la bandeja, su estado debe ser "OPERATIVO"

  # =======================================================
  # FASE 3: PROCEDIMIENTO PARA DAR DE BAJA
  # =======================================================
  When Se hace clic en ver vehículo
  And Se hace clic en dar de baja
  And Se ingresan las observaciones "Dar de baja caso de prueba CP0015"
  And Se confirma la baja del vehículo
  And Se cierra la ventana de detalles

  # =======================================================
  # FASE 4: VALIDACIÓN FINAL (El corazón del CP015)
  # =======================================================
  # Volvemos a buscar para refrescar la grilla y asegurar el cambio en BD
  And Se busca el vehículo por placa "CP0015"
  Then al buscar la placa "CP0015" en la bandeja, su estado debe ser "DE BAJA"


  #Actualmente falla porque el sistema no deja dar de baja a vehiculos con estado AVERIADO. Sin embargi esto es un bug y debe ser arreglado.
  @DarDeBajaAveriado_CP016 
Scenario: Dar de baja vehiculo con estado de registro ACTIVO y AVERIADO
  # =======================================================
  # FASE 1: REGISTRO (Nace ACTIVO y OPERATIVO)
  # =======================================================
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo "Vehículo"
  And Se selecciona "+Nuevo"
  And Se ingresan los datos del vehículo:
    | Campo            | Valor             |
    | PLACA            | CP0016            |
    | AREA ASIGNADA    | DPAM              |
    | PROPIETARIO      | MIMP              |
    | MARCA            | TOYOTA            | 
    | MODELO           | RIO               |
    | AÑO              | 2026              |
    | TIPO DE VEHICULO | AUTOMOVIL         |
    | CLASIFICADOR     | ALTA              |
    | COLOR            | PLATA             |
    | NUMERO MOTOR     | MTR-CP016         |
    | TIPO COMBUSTIBLE | G-90              |
    | TIPO MOTOR       | COMBUSTIBLE       |
    | RANGO CONSUMO    | 50                |
    | NUMERO SERIE     | SERIEYYUUCP016    |
  And Se procede a "GUARDAR" el vehículo

  # =======================================================
  # FASE 2: PRECONDICIÓN (Cambiar estado a AVERIADO)
  # =======================================================
  And Se busca el vehículo por placa "CP0016"
  And Se hace clic en ver vehículo
  And se hace clic en reportar avería
  And se ingresan las observaciones de avería "Reportar avería caso de prueba CP0016"
  And se confirma el reporte de la avería
  And Se cierra la ventana de detalles

  # =======================================================
  # FASE 3: PROCEDIMIENTO PARA DAR DE BAJA (Aquí saltará el bug)
  # =======================================================
  When Se busca el vehículo por placa "CP0016"
  And Se hace clic en ver vehículo
  # En el siguiente paso Selenium no hallará la papelera y lanzará la Excepción
  And Se hace clic en dar de baja
  
  # -------------------------------------------------------
  # NOTA: Los siguientes pasos quedan listos para ejecutarse
  # el día que el equipo de Desarrollo solucione el bug.
  # -------------------------------------------------------
  And Se ingresan las observaciones "Dar de baja caso de prueba CP016"
  And Se confirma la baja del vehículo
  And Se cierra la ventana de detalles

  # =======================================================
  # FASE 4: VALIDACIÓN FINAL EN BANDEJA 
  # =======================================================
  And Se busca el vehículo por placa "CP0016"
  Then al buscar la placa "CP0016" en la bandeja, su estado debe ser "DE BAJA"


  # FILTRO

  @Filtros_VehiculoOperativo
Scenario: Listar vehiculo con estado OPERATIVO
  # 1. Precondiciones (Ingresar al módulo)
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo "Vehículo"
  
  # 2. Configurar los Filtros
  When se desmarcan los filtros adicionales dejando solo el estado de vehículo OPERATIVO
  And se hace clic en el botón Buscar de la sección de filtros
  
  # 3. Validación
  Then la grilla debe mostrar unicamente vehiculos con estado "OPERATIVO"



  @Filtros_VehiculoAveriado
Scenario: Listar vehiculos con estado AVERIADO
  # 1. Precondiciones (Ingresar al módulo)
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo "Vehículo"
  
  # 2. Configurar los Filtros
  When se desmarcan los filtros adicionales dejando solo el estado de vehículo AVERIADO
  And se hace clic en el botón Buscar de la sección de filtros
  
  # 3. Validación (¡Aprovechamos el validador dinámico!)
  Then la grilla debe mostrar unicamente vehiculos con estado "AVERIADO"



  @Filtros_RegistroActivo
Scenario: Filtrar bandeja para listar unicamente vehiculos en estado de registro ACTIVO
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo "Vehículo"
  
  When se configuran los filtros dejando solo el estado de registro ACTIVO
  And se hace clic en el botón Buscar de la sección de filtros
  
  Then la grilla debe mostrar unicamente vehiculos con estado de registro "ACTIVO"


  @Filtros_RegistroDeBaja
Scenario: Filtrar bandeja para listar unicamente vehiculos en estado de registro DE BAJA
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo "Vehículo"
  
  When se configuran los filtros dejando solo el estado de registro DE BAJA
  And se hace clic en el botón Buscar de la sección de filtros
  
  Then la grilla debe mostrar unicamente vehiculos con estado de registro "DE BAJA"


  @ExportarExcel_Vehiculos
Scenario: Exportar la lista a Excel con vehiculos en estado OPERATIVO
  Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
  When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
  And Se ingresa al módulo "Vehículo"
  
  # Filtramos usando los pasos que creaste anteriormente
  When se desmarcan los filtros adicionales dejando solo el estado de vehículo OPERATIVO
  And se hace clic en el botón Buscar de la sección de filtros
  Then la grilla debe mostrar unicamente vehiculos con estado "OPERATIVO"
  
  # Tu nuevo caso de prueba
  When se hace clic en el botón Exportar a Excel
  Then el sistema debe descargar un archivo Excel exitosamente