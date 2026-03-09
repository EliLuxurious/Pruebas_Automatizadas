@Odometro
Feature: Gestión de Odómetro

    @FlujoCompletoOdometro
    Scenario: Registrar vehículo nuevo y luego registrar su lectura de odómetro
        # ==========================================
        # FASE 0: INICIO DE SESIÓN
        # ==========================================
        Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
        When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"

        # ==========================================
        # FASE 1: CREACIÓN DEL VEHÍCULO
        # ==========================================
        And Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"

        When Se ingresan los datos del vehículo:
        | Campo            | Valor            |
        | PLACA            | ONO123           |
        | AREA ASIGNADA    | DPAM             |
        | PROPIETARIO      | MIMP             |
        | MARCA            | DAEWOO           | 
        | MODELO           | TICO SL          |
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

        # ==========================================
        # FASE 2: REGISTRO DEL ODÓMETRO
        # ==========================================
        # Directamente después de guardar, el robot va al otro menú
        When Se ingresa al módulo Odómetro
        And Se selecciona Nuevo Odómetro
        
        # Usamos la MISMA placa que creamos arriba
        And Se ingresa la placa "ONO123" y se cargan los datos
        
        And Se ingresa la lectura del odómetro "15500"
        And Se selecciona la fecha de lectura día "15"
        
        Then Se procede a Guardar el odómetro


     @EditarOdometro
        Scenario: Editar una lectura de Odómetro existente
        Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
        When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"

        And Se ingresa al módulo Odómetro
        
        When Se busca el odómetro por placa "ONO123"
        And Se hace clic en ver odómetro
        And Se hace clic en editar odómetro
        
        # Reutilizamos el paso de escritura para actualizar el valor
        And Se ingresa la lectura del odómetro "16800"
        And Se selecciona la fecha de lectura día "19"
        Then Se guarda la edición del odómetro


        @BajaOdometro
    Scenario: Dar de baja a una lectura de Odómetro existente
        Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
        When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"

        And Se ingresa al módulo Odómetro
        
        # Reutilizamos la búsqueda en la grilla
        When Se busca el odómetro por placa "ONO123"
        
        # Nuevos pasos
        And Se hace clic en dar de baja odómetro
        Then Se confirma la baja del odómetro



        @FiltrosOdometro
    Scenario: Buscar registros de Odómetro usando los filtros
        Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
        When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"

        And Se ingresa al módulo Odómetro
        
        # Como "TODAS" viene marcado por defecto, al seleccionar un área se desmarca solo automáticamente
        When Se seleccionan las áreas en el filtro "DPAM, UPE LIMA ESTE"
        And Se selecciona el origen en el filtro "Odómetro"
        
        And Se hace clic en Buscar filtros
        
        # (Opcional) Podemos simular que el usuario se arrepiente y vuelve a marcar "TODAS"
        # And Se marca la opción TODAS en "ÁREAS"
        # And Se hace clic en Buscar filtros