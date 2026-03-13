@Soat
Feature: Gestión de SOAT

    Background: Iniciar sesión en el sistema
        Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
        When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"

    # ==========================================
    # CASOS DE PRUEBA: CREAR VEHÍCULO + REGISTRO SOAT (EXITOSO)
    # ==========================================
    @RegistroSoat @RegistroExitoso
    Scenario Outline: <Caso> - <Descripcion>
        # ------------------------------------------
        # FASE 1: CREACIÓN DEL VEHÍCULO
        # ------------------------------------------
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"

        When Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | <Placa>           |
        | AREA ASIGNADA    | DPAM              |
        | PROPIETARIO      | MIMP              |
        | MARCA            | DAEWOO            | 
        | MODELO           | TICO SL           |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | ALTA              |
        | COLOR            | NEGRO             |
        | NUMERO MOTOR     | ENG554433         |
        | TIPO COMBUSTIBLE | G-90              |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 45                |
        | NUMERO SERIE     | XYZ9876543210987A |

        Then Se procede a "GUARDAR" el vehículo

        # ------------------------------------------
        # FASE 2: REGISTRO DEL SOAT
        # ------------------------------------------
        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        
        # 1. Datos del Vehículo
        And Se ingresa la placa "<Placa>" y se busca en SOAT
        
        # 2. Datos de la Póliza
        And Se selecciona el proveedor "<Proveedor>"
        And Se ingresa la póliza "<Poliza>"
        And Se selecciona la fecha DESDE el día "<DiaDesde>" y HASTA el día "<DiaHasta>" del próximo año
        
        # 3. Datos del Contratante
        And Se ingresa el RUC "<Ruc>" y se busca
        And Se selecciona la fecha del contratante el día "<DiaContratante>"
        And Se ingresa la hora de emisión "<Hora>" y el importe "<Importe>"
        
        # 4. Documento Adjunto
        And Se adjunta el documento "<RutaArchivo>"
        
        Then Se guarda el SOAT

        Examples:
            | Caso       | Descripcion                 | Placa  | Proveedor | Poliza    | DiaDesde | DiaHasta | Ruc         | DiaContratante | Hora  | Importe | RutaArchivo                                |
            | CP-SOAT-05 | Flujo Correcto              | 823PL1 | RIMAC     | 222-3421  | 14       | 14       | 20604915351 | 14             | 10:30 | 300     | C:\Users\MANUEL\Pictures\goleto adidas.jpg |
            | CP-SOAT-13 | Registro con proveedor OTRO | 467UI1 | OTRO      | 4455-6671 | 25       | 24       | 20552103816 | 25             | 08:00 | 220     | C:\Users\MANUEL\Pictures\goleto adidas.jpg |




            # ==========================================
    # CASOS DE PRUEBA: INTENTOS DE REGISTRO FALLIDOS (BOTÓN DESHABILITADO)
    # ==========================================
    
    @RegistroSoat @RegistroFallido @CP-SOAT-02
    Scenario: CP-SOAT-02 - Registro de SOAT sin cargar el documento adjunto
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | 7845K1            |
        | AREA ASIGNADA    | DPAM              |
        | PROPIETARIO      | MIMP              |
        | MARCA            | DAEWOO            | 
        | MODELO           | TICO SL           |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | ALTA              |
        | COLOR            | NEGRO             |
        | NUMERO MOTOR     | ENG554433         |
        | TIPO COMBUSTIBLE | G-90              |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 45                |
        | NUMERO SERIE     | XYZ9876543210987A |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "7845K1" y se busca en SOAT
        And Se selecciona el proveedor "LA POSITIVA"
        And Se ingresa la póliza "1242-651"
        And Se selecciona la fecha DESDE el día "10" y HASTA el día "10" del próximo año
        And Se ingresa el RUC "20538856674" y se busca
        And Se selecciona la fecha del contratante el día "10"
        And Se ingresa la hora de emisión "17:00" y el importe "200.00"
        # OMITIMOS EL PASO DE ADJUNTAR DOCUMENTO
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado

    @RegistroSoat @RegistroFallido @CP-SOAT-10
    Scenario: CP-SOAT-10 - Intento de registro sin buscar datos del vehículo (sin lupa)
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | 718VB1            |
        | AREA ASIGNADA    | DPAM              |
        | PROPIETARIO      | MIMP              |
        | MARCA            | DAEWOO            | 
        | MODELO           | TICO SL           |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | ALTA              |
        | COLOR            | NEGRO             |
        | NUMERO MOTOR     | ENG554433         |
        | TIPO COMBUSTIBLE | G-90              |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 45                |
        | NUMERO SERIE     | XYZ9876543210987A |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        # PASO CLAVE: Ingresamos placa pero NO buscamos
        And Se ingresa la placa "718VB1" sin buscar en SOAT
        And Se selecciona el proveedor "MAPFRE"
        And Se ingresa la póliza "5544-3321"
        And Se selecciona la fecha DESDE el día "12" y HASTA el día "12" del próximo año
        And Se ingresa el RUC "20604915351" y se busca
        And Se selecciona la fecha del contratante el día "12"
        And Se ingresa la hora de emisión "16:00" y el importe "250.00"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado

    @RegistroSoat @RegistroFallido @CP-SOAT-11
    Scenario: CP-SOAT-11 - Intento de registro sin buscar datos del contratante (sin lupa RUC)
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | 892RT1            |
        | AREA ASIGNADA    | DPAM              |
        | PROPIETARIO      | MIMP              |
        | MARCA            | DAEWOO            | 
        | MODELO           | TICO SL           |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | ALTA              |
        | COLOR            | NEGRO             |
        | NUMERO MOTOR     | ENG554433         |
        | TIPO COMBUSTIBLE | G-90              |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 45                |
        | NUMERO SERIE     | XYZ9876543210987A |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "892RT1" y se busca en SOAT
        And Se selecciona el proveedor "PROTECTA"
        And Se ingresa la póliza "7788-9901"
        And Se selecciona la fecha DESDE el día "15" y HASTA el día "14" del próximo año
        # PASO CLAVE: Ingresamos RUC pero NO buscamos
        And Se ingresa el RUC "20553856451" sin buscar
        And Se selecciona la fecha del contratante el día "15"
        And Se ingresa la hora de emisión "09:30" y el importe "175.00"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado






        # ==========================================
    # CASOS DE PRUEBA: REGLAS DE NEGOCIO (FECHAS Y MONTOS INVÁLIDOS)
    # ==========================================

    @RegistroSoat @RegistroFallido @CP-SOAT-03
    Scenario: CP-SOAT-03 - Validación de Integridad Financiera (Prima con letras E110)
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | 912MN1            |
        | AREA ASIGNADA    | DPAM              |
        | PROPIETARIO      | MIMP              |
        | MARCA            | DAEWOO            | 
        | MODELO           | TICO SL           |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | ALTA              |
        | COLOR            | NEGRO             |
        | NUMERO MOTOR     | ENG554433         |
        | TIPO COMBUSTIBLE | G-90              |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 45                |
        | NUMERO SERIE     | XYZ9876543210987A |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "912MN1" y se busca en SOAT
        And Se selecciona el proveedor "LA POSITIVA"
        And Se ingresa la póliza "353-451"
        And Se selecciona la fecha DESDE el día "10" y HASTA el día "10" del próximo año
        And Se ingresa el RUC "20605100016" y se busca
        And Se selecciona la fecha del contratante el día "10"
        And Se ingresa la hora de emisión "18:00" y el importe "E110"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        # El sistema debería bloquear el botón por el formato inválido "E110"
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado


    @RegistroSoat @RegistroFallido @CP-SOAT-07
    Scenario: CP-SOAT-07 - Vigencia de póliza con fecha HASTA anterior a fecha DESDE
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | 289HJ1            |
        | AREA ASIGNADA    | DPAM              |
        | PROPIETARIO      | MIMP              |
        | MARCA            | DAEWOO            | 
        | MODELO           | TICO SL           |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | ALTA              |
        | COLOR            | NEGRO             |
        | NUMERO MOTOR     | ENG554433         |
        | TIPO COMBUSTIBLE | G-90              |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 45                |
        | NUMERO SERIE     | XYZ9876543210987A |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "289HJ1" y se busca en SOAT
        And Se selecciona el proveedor "RIMAC"
        And Se ingresa la póliza "3344-551"
        # PASO CLAVE: Seleccionamos el 15 en DESDE para verificar el bloqueo
        And Se selecciona solo la fecha DESDE el día "15"
        Then Se verifica que el día "14" está deshabilitado en el calendario HASTA


    @RegistroSoat @RegistroConMensajeError @CP-SOAT-09
    Scenario: CP-SOAT-09 - Registro de SOAT con importe de prima = 0
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | 365ZX1            |
        | AREA ASIGNADA    | DPAM              |
        | PROPIETARIO      | MIMP              |
        | MARCA            | DAEWOO            | 
        | MODELO           | TICO SL           |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | ALTA              |
        | COLOR            | NEGRO             |
        | NUMERO MOTOR     | ENG554433         |
        | TIPO COMBUSTIBLE | G-90              |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 45                |
        | NUMERO SERIE     | XYZ9876543210987A |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "365ZX1" y se busca en SOAT
        And Se selecciona el proveedor "INTERSEGURO"
        And Se ingresa la póliza "1122-3341"
        And Se selecciona la fecha DESDE el día "10" y HASTA el día "9" del próximo año
        And Se ingresa el RUC "20552103816" y se busca
        And Se selecciona la fecha del contratante el día "10"
        And Se ingresa la hora de emisión "11:00" y el importe "0"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT
        # AQUÍ ESTÁ EL CAMBIO CON EL MENSAJE REAL DEL SISTEMA
        And Se verifica el mensaje de error del SOAT "Los datos ingresados no son correctos!"










        # ==========================================
    # CASOS DE PRUEBA: EDICIÓN DE SOAT
    # ==========================================

    @EditarSoat @CP-SOAT-14
    Scenario: CP-SOAT-14 - Edición de SOAT cambiando proveedor
        When Se ingresa al módulo SOAT
        # PON AQUÍ UNA PLACA QUE SÍ EXISTA EN TU TABLA (Ejemplo: ONO123)
        And Se busca el SOAT por placa "MAN111"
        And Se hace clic en ver SOAT
        And Se hace clic en editar SOAT
        And Se selecciona el proveedor "RIMAC"
        Then Se guarda el SOAT

    @EditarSoat @CP-SOAT-22
    Scenario: CP-SOAT-22 - Edición de SOAT cambiando solo el documento adjunto
        When Se ingresa al módulo SOAT
        # PON OTRA PLACA QUE EXISTA AQUÍ
        And Se busca el SOAT por placa "823PLW"
        And Se hace clic en ver SOAT
        And Se hace clic en editar SOAT
        And Se elimina el documento adjunto
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\adidas 2.png"
        Then Se guarda el SOAT

    @EditarSoat @CP-SOAT-23
    Scenario: CP-SOAT-23 - Intento de edición sin modificar ningún campo (Validando comportamiento actual)
        When Se ingresa al módulo SOAT
        # ASEGÚRATE DE USAR TU PLACA VÁLIDA
        And Se busca el SOAT por placa "467UIO" 
        And Se hace clic en ver SOAT
        And Se hace clic en editar SOAT
        # Como sabemos que el botón no se bloquea, le damos a Guardar
        Then Se guarda el SOAT
        # Validamos el mensaje verde que arroja el sistema actualmente
        And Se verifica el mensaje de error del SOAT "Se actualizo el SOAT Correctamente"



        # ==========================================
    # CASOS DE PRUEBA: EDICIÓN AVANZADA Y BÚSQUEDA BÁSICA
    # ==========================================

    @RegistroSoat @RegistroFallido @CP-SOAT-15
    Scenario: CP-SOAT-15 - Eliminación de documento adjunto sin agregar uno nuevo antes de guardar
        # 1. Creamos el vehículo
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | 649LK2            |
        | AREA ASIGNADA    | DPAM              |
        | PROPIETARIO      | MIMP              |
        | MARCA            | DAEWOO            | 
        | MODELO           | TICO SL           |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | ALTA              |
        | COLOR            | NEGRO             |
        | NUMERO MOTOR     | ENG554433         |
        | TIPO COMBUSTIBLE | G-90              |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 45                |
        | NUMERO SERIE     | XYZ9876543210987A |
        Then Se procede a "GUARDAR" el vehículo

        # 2. Registramos el SOAT (Aquí aplicamos tu caso de prueba exacto)
        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "649LK2" y se busca en SOAT
        And Se selecciona el proveedor "PACIFICO"
        And Se ingresa la póliza "3344-1120"
        And Se selecciona la fecha DESDE el día "5" y HASTA el día "4" del próximo año
        And Se ingresa el RUC "20538856674" y se busca
        And Se selecciona la fecha del contratante el día "5"
        And Se ingresa la hora de emisión "17:00" y el importe "165"
        
        # Subimos el PDF
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        
        # LO ELIMINAMOS EN EL MISMO FORMULARIO DE REGISTRO
        And Se elimina el documento adjunto
        
        # Verificamos que el sistema detecte que falta el archivo y bloquee el botón
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado

    @FiltrosSoat @CP-SOAT-24
    Scenario: CP-SOAT-24 - Búsqueda de SOATs sin aplicar ningún filtro
        When Se ingresa al módulo SOAT
        # Reutilizamos el paso de hacer clic en buscar que hicimos para Odómetro
        And Se hace clic en el boton Buscar Filtros
        # Verificamos que la tabla devuelva resultados
        Then Se verifica que la grilla de SOAT muestra resultados


        @FiltrosSoat @CP-SOAT-17
    Scenario: CP-SOAT-17 - Busqueda de SOAT por multiples aseguradoras simultaneamente
        When Se ingresa al módulo SOAT
        And Se abre el filtro de "Aseguradoras"
        And Se desmarca la opcion TODAS
        And Se seleccionan las siguientes aseguradoras:
          | Aseguradora |
          | LA POSITIVA |
          | RIMAC       |
          | PACIFICO    |
        # Asumiendo que las fechas se ingresan en campos de texto (Desde - Hasta)
        And Se ingresa la fecha de vencimiento DESDE "01/01/2026" y HASTA "31/12/2026" en los filtros
        And Se hace clic en el boton Buscar Filtros
        Then Se verifica que la grilla de SOAT muestra resultados






        @FiltrosSoat @CP-SOAT-16
    Scenario: CP-SOAT-16 - Búsqueda de SOATs proximos a vencer
        When Se ingresa al módulo SOAT
        And Se abre el filtro de "Estado"
        And Se desmarca la opcion TODAS
        And Se seleccionan las siguientes opciones en el filtro:
          | Opcion           |
          | PRÓXIMO A VENCER |
        # Usamos la caja de texto de Días para Vencer
        And Se ingresa "30" en dias para vencer
        And Se ingresa la fecha de vencimiento DESDE "05/02/2026" y HASTA "05/03/2026" en los filtros
        Then Se verifica que la grilla de SOAT muestra resultados

    @FiltrosSoat @CP-SOAT-18
    Scenario: CP-SOAT-18 - Búsqueda de SOATs caducados por area especifica
        When Se ingresa al módulo SOAT
        And Se abre el filtro de "Estado"
        And Se desmarca la opcion TODAS
        And Se seleccionan las siguientes opciones en el filtro:
          | Opcion   |
          | CADUCADO |
        And Se abre el filtro de "Áreas"
        And Se desmarca la opcion TODAS
        And Se seleccionan las siguientes opciones en el filtro:
          | Opcion                  |
          | UPE LIMA NORTE - CALLAO |
        And Se ingresa la fecha de vencimiento DESDE "04/02/2026" y HASTA "04/02/2026" en los filtros
        And Se hace clic en el boton Buscar Filtros
        Then Se verifica que la grilla de SOAT muestra resultados

    @HistorialSoat @CP-SOAT-19
    Scenario: CP-SOAT-19 - Consulta de historial de SOAT
        When Se ingresa al módulo SOAT
        And Se hace clic en el boton Historial
        # Reutilizamos el paso que ya tienes creado para ingresar la placa y darle a la lupa
        And Se ingresa la placa "MAN111" y se busca en SOAT
        Then Se verifica que la grilla de SOAT muestra resultados
        And Se cierra el historial del SOAT




        # ==========================================
    # BLOQUE 2: REGLAS DE NEGOCIO Y VALIDACIONES DE FECHA
    # ==========================================

    @RegistroSoat @RegistroFallido @CP-SOAT-04
    Scenario: CP-SOAT-04 - Fecha de contratante posterior a la vigencia
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | 111AAA            |
        | AREA ASIGNADA    | DPAM              |
        | PROPIETARIO      | MIMP              |
        | MARCA            | DAEWOO            | 
        | MODELO           | TICO SL           |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | ALTA              |
        | COLOR            | NEGRO             |
        | NUMERO MOTOR     | ENG554433         |
        | TIPO COMBUSTIBLE | G-90              |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 45                |
        | NUMERO SERIE     | XYZ9876543210987A |
        Then Se procede a "GUARDAR" el vehículo
        
        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "111AAA" y se busca en SOAT
        And Se selecciona el proveedor "PACIFICO"
        And Se ingresa la póliza "0404-0000"
        # Inicio el día 10
        And Se selecciona la fecha DESDE el día "10" y HASTA el día "9" del próximo año
        And Se ingresa el RUC "20538856674" y se busca
        # Contratante posterior al inicio (día 15)
        And Se selecciona la fecha del contratante el día "15"
        And Se ingresa la hora de emisión "10:00" y el importe "100.00"
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado


    @RegistroSoat @RegistroFallido @CP-SOAT-12
    Scenario: CP-SOAT-12 - Fecha contratante exactamente igual al fin de vigencia
        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        # Reutilizamos la misma placa para los siguientes para hacer la prueba más rápida
        And Se ingresa la placa "111AAA" y se busca en SOAT
        And Se selecciona el proveedor "RIMAC"
        And Se ingresa la póliza "1212-0000"
        # Digamos que el fin de vigencia es el día 20 del próximo año
        And Se selecciona la fecha DESDE el día "21" y HASTA el día "20" del próximo año
        And Se ingresa el RUC "20604915351" y se busca
        # Contratante en el futuro (día 20) -> Esto suele ser inválido en seguros
        And Se selecciona la fecha del contratante el día "20"
        And Se ingresa la hora de emisión "10:00" y el importe "120.00"
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado


    @RegistroSoat @RegistroFallido @CP-SOAT-20
    Scenario: CP-SOAT-20 - Fecha contratante anterior al inicio de vigencia
        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "111AAA" y se busca en SOAT
        And Se selecciona el proveedor "LA POSITIVA"
        And Se ingresa la póliza "2020-0000"
        # Inicio el día 25
        And Se selecciona la fecha DESDE el día "25" y HASTA el día "24" del próximo año
        And Se ingresa el RUC "20552103816" y se busca
        # Contratante muy anterior (día 5)
        And Se selecciona la fecha del contratante el día "5"
        And Se ingresa la hora de emisión "10:00" y el importe "150.00"
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado


    @RegistroSoat @RegistroExitoso @CP-SOAT-21
    Scenario: CP-SOAT-21 - Registro de SOAT con vigencia de exactamente 365 días
        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "111AAA" y se busca en SOAT
        And Se selecciona el proveedor "MAPFRE"
        And Se ingresa la póliza "2121-0000"
        # Del 10 al 9 del próximo año = 365 días exactos
        And Se selecciona la fecha DESDE el día "10" y HASTA el día "9" del próximo año
        And Se ingresa el RUC "20538856674" y se busca
        And Se selecciona la fecha del contratante el día "10"
        And Se ingresa la hora de emisión "15:00" y el importe "200.00"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT


    @RegistroSoat @RegistroFallido @CP-SOAT-25
    Scenario: CP-SOAT-25 - Registro de SOAT con vigencia menor a 30 dias
        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "111AAA" y se busca en SOAT
        And Se selecciona el proveedor "PROTECTA"
        And Se ingresa la póliza "2525-0000"
        # OJO AQUÍ: Usamos un paso nuevo para NO cambiar de año (Mismo mes)
        And Se selecciona la fecha DESDE el día "10" y HASTA el día "25" del mismo mes
        And Se ingresa el RUC "20538856674" y se busca
        And Se selecciona la fecha del contratante el día "10"
        And Se ingresa la hora de emisión "09:00" y el importe "50.00"
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado


    @RegistroSoat @RegistroFallido @CP-SOAT-06
    Scenario: CP-SOAT-06 - Registro de SOAT con vigencia vencida (Año pasado)
        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "111AAA" y se busca en SOAT
        And Se selecciona el proveedor "INTERSEGURO"
        And Se ingresa la póliza "0606-0000"
        And Se selecciona la fecha DESDE el día "10" y HASTA el día "11" del año pasado
        And Se ingresa el RUC "20538856674" y se busca
        And Se selecciona la fecha del contratante el día "10"
        And Se ingresa la hora de emisión "09:00" y el importe "100.00"
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado








        # ==========================================
    # BLOQUE 3: CASOS ESPECIALES Y CONFLICTOS
    # ==========================================

    @RegistroSoat @RegistroFallido @CP-SOAT-01
    Scenario: CP-SOAT-01 - Registro de SOAT sin cargar datos del contratante
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | 345XAB            |
        | AREA ASIGNADA    | DPAM              |
        | PROPIETARIO      | MIMP              |
        | MARCA            | DAEWOO            | 
        | MODELO           | TICO SL           |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | ALTA              |
        | COLOR            | NEGRO             |
        | NUMERO MOTOR     | ENG554433         |
        | TIPO COMBUSTIBLE | G-90              |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 45                |
        | NUMERO SERIE     | XYZ9876543210987A |
        Then Se procede a "GUARDAR" el vehículo
        
        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "345XAB" y se busca en SOAT
        And Se selecciona el proveedor "PACIFICO"
        And Se ingresa la póliza "555-343"
        And Se selecciona la fecha DESDE el día "27" y HASTA el día "27" del próximo año
        
        # 🔥 PASO CLAVE: Usamos tu método "sin buscar" para que el RUC quede inválido
        And Se ingresa el RUC "20553856451" sin buscar
        
        And Se selecciona la fecha del contratante el día "27"
        And Se ingresa la hora de emisión "4:00" y el importe "120.00"
        # Omitimos el documento para ver si el botón se bloquea de todas formas
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado


   # 🐛 BUG DETECTADO: El sistema no valida superposición y permite guardar.
    # Pendiente de corrección por el equipo de Desarrollo.
    @RegistroSoat @Bug_Activo @CP-SOAT-08
    Scenario: CP-SOAT-08 - Intento de registro de SOATs superpuestos (Vehículo ya tiene SOAT)
        # 1. Creamos el vehículo
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | 948JKH            |
        | AREA ASIGNADA    | DPAM              |
        | PROPIETARIO      | MIMP              |
        | MARCA            | DAEWOO            | 
        | MODELO           | TICO SL           |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | ALTA              |
        | COLOR            | NEGRO             |
        | NUMERO MOTOR     | ENG554433         |
        | TIPO COMBUSTIBLE | G-90              |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 45                |
        | NUMERO SERIE     | XYZ9876543210987A |
        Then Se procede a "GUARDAR" el vehículo

        # 2. Registramos el PRIMER SOAT (Válido y Exitoso)
        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "948JKH" y se busca en SOAT
        And Se selecciona el proveedor "MAPFRE"
        And Se ingresa la póliza "4884-2331"
        And Se selecciona la fecha DESDE el día "1" y HASTA el día "31" del próximo año
        And Se ingresa el RUC "20600439368" y se busca
        And Se selecciona la fecha del contratante el día "5"
        And Se ingresa la hora de emisión "10:30" y el importe "120.00"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT

        # 3. Intentamos registrar un SEGUNDO SOAT para la misma placa
        When Se selecciona Nuevo SOAT
        And Se ingresa la placa "948JKH" y se busca en SOAT
        And Se selecciona el proveedor "PACIFICO"
        And Se ingresa la póliza "9988-7761"
        And Se selecciona la fecha DESDE el día "1" y HASTA el día "31" del próximo año
        And Se ingresa el RUC "20600439368" y se busca
        And Se selecciona la fecha del contratante el día "15"
        And Se ingresa la hora de emisión "14:30" y el importe "180.00"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT

        # BUG, DEBERÍA SALIR ASÍ EN EL SISTEMA POR VIGENCIAS SUPERPUESTAS:
       # And Se verifica el mensaje de error del SOAT "El vehículo ya cuenta con poliza de SOAT en curso."