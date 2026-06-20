@Soat
Feature: Gestión de SOAT

    Background: Iniciar sesión en el sistema
        Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
        When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"

    #CASOS DE PRUEBA: CREAR VEHÍCULO + REGISTRO SOAT (EXITOSO)
    @RegistroSoat @RegistroExitoso
    Scenario Outline: <Caso> - <Descripcion>
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
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
            | NUMERO MOTOR     | <Motor>           |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | <Serie>           |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "<Placa>" y se busca en SOAT
        And Se selecciona el proveedor "<Proveedor>"
        And Se ingresa la póliza "<Poliza>"
        And Se configuran las fechas dinámicas sumando "<Dias>" dias para un SOAT "<EstadoEsperado>"
        And Se ingresa el RUC "<Ruc>" y se busca
        And Se ingresa la hora de emisión "<Hora>" y el importe "<Importe>"
        And Se adjunta el documento "<RutaArchivo>"
        Then Se guarda el SOAT

        Examples:
            | Caso       | Descripcion                 | Placa  | Motor    | Serie              | Proveedor | Poliza    | Dias | EstadoEsperado   | Ruc         | Hora  | Importe | RutaArchivo                                  |
            | CP-SOAT-05 | Flujo Correcto Vigente      | SAA005 | ENG0505A | SERIE0505A2026X1Z | RIMAC     | 2605-5001 | 25   | VIGENTE          | 20604915351 | 10:30 | 300     | C:\Users\MANUEL\Pictures\goleto adidas.jpg |
            | CP-SOAT-13 | Registro con proveedor OTRO | SAA013 | ENG1313A | SERIE1313A2026X1Z | OTRO      | 2613-5001 | 15   | PROXIMO A VENCER | 20552103816 | 08:00 | 220     | C:\Users\MANUEL\Pictures\goleto adidas.jpg |

    #CASOS DE PRUEBA: INTENTOS DE REGISTRO FALLIDOS
    @RegistroSoat @RegistroFallido @CP-SOAT-02
    Scenario: CP-SOAT-02 - Registro de SOAT sin cargar el documento adjunto
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA002            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG0202A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE0202A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA002" y se busca en SOAT
        And Se selecciona el proveedor "LA POSITIVA"
        And Se ingresa la póliza "2602-5001"
        And Se configuran las fechas dinámicas sumando "25" dias para un SOAT "VIGENTE"
        And Se ingresa el RUC "20538856674" y se busca
        And Se ingresa la hora de emisión "17:00" y el importe "200.00"
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado

    @RegistroSoat @RegistroFallido @CP-SOAT-10
    Scenario: CP-SOAT-10 - Intento de registro sin buscar datos del vehículo (sin lupa)
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA010            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG1010A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE1010A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA010" sin buscar en SOAT
        Then Se verifica que el SOAT no permite continuar sin buscar la placa

    @RegistroSoat @RegistroFallido @CP-SOAT-11
    Scenario: CP-SOAT-11 - Intento de registro sin buscar datos del contratante (sin lupa RUC)
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA011            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG1111A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE1111A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA011" y se busca en SOAT
        And Se selecciona el proveedor "PROTECTA"
        And Se ingresa la póliza "2611-5001"
        And Se configuran las fechas dinámicas sumando "25" dias para un SOAT "VIGENTE"
        And Se ingresa el RUC "20553856451" sin buscar
        And Se ingresa la hora de emisión "09:30" y el importe "175.00"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado

    #CASOS DE PRUEBA: REGLAS DE NEGOCIO Y VALIDACIONES DE FECHA
    @RegistroSoat @RegistroFallido @CP-SOAT-03
    Scenario: CP-SOAT-03 - Validación de Integridad Financiera (Prima con letras E110)
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA003            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG0303A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE0303A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA003" y se busca en SOAT
        And Se selecciona el proveedor "LA POSITIVA"
        And Se ingresa la póliza "2603-5001"
        And Se configuran las fechas dinámicas sumando "25" dias para un SOAT "VIGENTE"
        And Se ingresa el RUC "20605100016" y se busca
        And Se ingresa la hora de emisión "18:00" y el importe "E110"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado

    @RegistroSoat @RegistroFallido @CP-SOAT-07
    Scenario: CP-SOAT-07 - Vigencia de póliza con fecha HASTA anterior a fecha DESDE
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA007            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG0707A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE0707A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA007" y se busca en SOAT
        And Se selecciona el proveedor "RIMAC"
        And Se ingresa la póliza "2607-5001"
        And Se selecciona la fecha DESDE del SOAT sumando "15" dias
        Then Se verifica que la fecha anterior al DESDE está deshabilitada en el calendario HASTA

    @RegistroSoat @RegistroFallido @CP-SOAT-04
    Scenario: CP-SOAT-04 - Fecha de contratante posterior a la vigencia
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA004            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG0404A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE0404A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA004" y se busca en SOAT
        And Se selecciona el proveedor "PACIFICO"
        And Se ingresa la póliza "2604-5001"
        And Se configuran las fechas de vigencia del SOAT iniciando en "30" dias y con duracion de "365" dias
        And Se ingresa el RUC "20538856674" y se busca
        And Se selecciona la fecha de contratante "5" dias despues del HASTA del SOAT
        And Se ingresa la hora de emisión "10:00" y el importe "100.00"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado

    @RegistroSoat @RegistroFallido @CP-SOAT-25
    Scenario: CP-SOAT-25 - Registro de SOAT con vigencia menor a 30 dias
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA025            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG2525A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE2525A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA025" y se busca en SOAT
        And Se selecciona el proveedor "PROTECTA"
        And Se ingresa la póliza "2625-5001"
        And Se selecciona solo la fecha DESDE de vigencia del SOAT iniciando en "30" dias
        Then Se verifica que la fecha HASTA con duracion de "15" dias está deshabilitada
        And Se verifica que el boton Guardar del SOAT esta deshabilitado

    @RegistroSoat @RegistroConMensajeError @CP-SOAT-09
    Scenario: CP-SOAT-09 - Registro de SOAT con importe de prima = 0
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA009            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG0909A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE0909A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA009" y se busca en SOAT
        And Se selecciona el proveedor "INTERSEGURO"
        And Se ingresa la póliza "2609-5001"
        And Se configuran las fechas dinámicas sumando "25" dias para un SOAT "VIGENTE"
        And Se ingresa el RUC "20552103816" y se busca
        And Se ingresa la hora de emisión "11:00" y el importe "0"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT
        And Se verifica el mensaje de error del SOAT "Los datos ingresados no son correctos!"

    @RegistroSoat @RegistroFallido @CP-SOAT-01
    Scenario: CP-SOAT-01 - Registro de SOAT sin cargar datos del contratante asegurado
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA001            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG0101A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE0101A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA001" y se busca en SOAT
        And Se selecciona el proveedor "PACIFICO"
        And Se ingresa la póliza "2601-5001"
        And Se configuran las fechas dinámicas sumando "25" dias para un SOAT "VIGENTE"
        And Se ingresa la hora de emisión "16:00" y el importe "120"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se verifica que el boton Guardar del SOAT esta deshabilitado

    @RegistroSoat @RegistroFallido @CP-SOAT-06
    Scenario: CP-SOAT-06 - Registro de SOAT con vigencia vencida
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA006            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG0606A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE0606A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA006" y se busca en SOAT
        And Se selecciona el proveedor "LA POSITIVA"
        And Se ingresa la póliza "2606-5001"
        And Se configuran fechas vencidas del SOAT
        And Se ingresa el RUC "20553856451" y se busca
        And Se ingresa la hora de emisión "15:00" y el importe "150"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT

    @RegistroSoat @RegistroExitoso @CP-SOAT-12
    Scenario: CP-SOAT-12 - Registro con fecha de contratante igual a fecha fin de vigencia
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA012            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG1212A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE1212A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA012" y se busca en SOAT
        And Se selecciona el proveedor "CRECER"
        And Se ingresa la póliza "2612-5001"
        And Se configuran las fechas de vigencia del SOAT iniciando en "30" dias y con duracion de "365" dias
        And Se ingresa el RUC "20605100016" y se busca
        And Se selecciona la fecha de contratante igual al HASTA del SOAT
        And Se ingresa la hora de emisión "13:00" y el importe "190"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT

    @RegistroSoat @RegistroExitoso @CP-SOAT-20
    Scenario: CP-SOAT-20 - Registro de SOAT con fecha de contratante anterior a fecha inicio de vigencia
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA020            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG2020A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE2020A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA020" y se busca en SOAT
        And Se selecciona el proveedor "INTERSEGURO"
        And Se ingresa la póliza "2620-5001"
        And Se configuran las fechas de vigencia del SOAT iniciando en "30" dias y con duracion de "365" dias
        And Se ingresa el RUC "20604915351" y se busca
        And Se selecciona la fecha de contratante "5" dias antes del DESDE del SOAT
        And Se ingresa la hora de emisión "09:00" y el importe "180.00"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT

    @RegistroSoat @RegistroExitoso @CP-SOAT-21
    Scenario: CP-SOAT-21 - Registro de SOAT con vigencia exactamente de 365 dias
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA021            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG2121A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE2121A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA021" y se busca en SOAT
        And Se selecciona el proveedor "MAPFRE"
        And Se ingresa la póliza "2621-5001"
        And Se configuran las fechas de vigencia del SOAT iniciando en "30" dias y con duracion de "365" dias
        And Se ingresa el RUC "20553856451" y se busca
        And Se selecciona la fecha de contratante igual al DESDE del SOAT
        And Se ingresa la hora de emisión "12:00" y el importe "185"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT
        And Se verifica que el SOAT de la placa "SAA021" se registró correctamente

    # CASOS DE PRUEBA: FILTROS
    @FiltrosSoat @CP-SOAT-24
    Scenario: CP-SOAT-24 - Búsqueda de SOATs sin aplicar ningún filtro
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA024            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG2424A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE2424A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA024" y se busca en SOAT
        And Se selecciona el proveedor "RIMAC"
        And Se ingresa la póliza "2624-5001"
        And Se configuran las fechas dinámicas sumando "25" dias para un SOAT "VIGENTE"
        And Se ingresa el RUC "20604915351" y se busca
        And Se ingresa la hora de emisión "10:45" y el importe "205"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT

        When Se ingresa al módulo SOAT
        And Se hace clic en el boton Buscar Filtros
        Then Se verifica que la grilla de SOAT muestra resultados

    @FiltrosSoat @CP-SOAT-17
    Scenario: CP-SOAT-17 - Busqueda de SOAT por multiples aseguradoras simultaneamente
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAB171            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG1711A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE1711A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAB171" y se busca en SOAT
        And Se selecciona el proveedor "LA POSITIVA"
        And Se ingresa la póliza "2717-5001"
        And Se configuran las fechas dinámicas sumando "25" dias para un SOAT "VIGENTE"
        And Se ingresa el RUC "20538856674" y se busca
        And Se ingresa la hora de emisión "09:10" y el importe "210"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT

        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAB172            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG1712A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE1712A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAB172" y se busca en SOAT
        And Se selecciona el proveedor "RIMAC"
        And Se ingresa la póliza "2717-5002"
        And Se configuran las fechas dinámicas sumando "25" dias para un SOAT "VIGENTE"
        And Se ingresa el RUC "20604915351" y se busca
        And Se ingresa la hora de emisión "09:20" y el importe "215"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT

        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAB173            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG1713A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE1713A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAB173" y se busca en SOAT
        And Se selecciona el proveedor "PACIFICO"
        And Se ingresa la póliza "2717-5003"
        And Se configuran las fechas dinámicas sumando "25" dias para un SOAT "VIGENTE"
        And Se ingresa el RUC "20552103816" y se busca
        And Se ingresa la hora de emisión "09:30" y el importe "220"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT

        When Se ingresa al módulo SOAT
        And Se abre el filtro de "Aseguradoras"
        And Se desmarca la opcion TODAS
        And Se seleccionan las siguientes aseguradoras:
            | Aseguradora |
            | LA POSITIVA |
            | RIMAC       |
            | PACIFICO    |
        And Se hace clic en el boton Buscar Filtros
        Then Se verifica que la grilla de SOAT muestra resultados

    @FiltrosSoat @CP-SOAT-16
    Scenario: CP-SOAT-16 - Búsqueda de SOATs proximos a vencer
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA016            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG1616A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE1616A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA016" y se busca en SOAT
        And Se selecciona el proveedor "PROTECTA"
        And Se ingresa la póliza "2616-5001"
        And Se configuran las fechas dinámicas sumando "15" dias para un SOAT "PROXIMO A VENCER"
        And Se ingresa el RUC "20553856451" y se busca
        And Se ingresa la hora de emisión "10:15" y el importe "230"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT

        When Se ingresa al módulo SOAT
        And Se abre el filtro de "Estado"
        And Se desmarca la opcion TODAS
        And Se seleccionan las siguientes opciones en el filtro:
            | Opcion           |
            | PRÓXIMO A VENCER |
        And Se ingresa "30" en dias para vencer
        And Se hace clic en el boton Buscar Filtros
        Then Se verifica que la grilla de SOAT muestra resultados

    @FiltrosSoat @CP-SOAT-18
    Scenario: CP-SOAT-18 - Búsqueda de SOATs caducados por área específica
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor                    |
            | PLACA            | SAA018                   |
            | AREA ASIGNADA    | UPE LIMA NORTE - CALLAO  |
            | PROPIETARIO      | MIMP                     |
            | MARCA            | DAEWOO                   |
            | MODELO           | TICO SL                  |
            | AÑO              | 2026                     |
            | TIPO DE VEHICULO | AUTOMOVIL                |
            | CLASIFICADOR     | ALTA                     |
            | COLOR            | NEGRO                    |
            | NUMERO MOTOR     | ENG1818A                 |
            | TIPO COMBUSTIBLE | G-90                     |
            | TIPO MOTOR       | COMBUSTIBLE              |
            | RANGO CONSUMO    | 45                       |
            | NUMERO SERIE     | SERIE1818A2026X1Z        |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA018" y se busca en SOAT
        And Se selecciona el proveedor "LA POSITIVA"
        And Se ingresa la póliza "2618-5001"
        And Se configuran fechas vencidas del SOAT
        And Se ingresa el RUC "20538856674" y se busca
        And Se ingresa la hora de emisión "14:30" y el importe "200"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT

        When Se ingresa al módulo SOAT
        And Se abre el filtro de "Estado"
        And Se desmarca la opcion TODAS
        And Se seleccionan las siguientes opciones en el filtro:
            | Opcion   |
            | CADUCADO |
        And Se abre el filtro de "Area"
        And Se desmarca la opcion TODAS
        And Se seleccionan las siguientes areas:
            | Area                    |
            | UPE LIMA NORTE - CALLAO |
        And Se hace clic en el boton Buscar Filtros
        Then Se verifica que la grilla de SOAT muestra resultados

    @HistorialSoat @CP-SOAT-19
    Scenario: CP-SOAT-19 - Consulta de historial de SOAT
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
            | Campo            | Valor             |
            | PLACA            | SAA019            |
            | AREA ASIGNADA    | DPAM              |
            | PROPIETARIO      | MIMP              |
            | MARCA            | DAEWOO            |
            | MODELO           | TICO SL           |
            | AÑO              | 2026              |
            | TIPO DE VEHICULO | AUTOMOVIL         |
            | CLASIFICADOR     | ALTA              |
            | COLOR            | NEGRO             |
            | NUMERO MOTOR     | ENG1919A          |
            | TIPO COMBUSTIBLE | G-90              |
            | TIPO MOTOR       | COMBUSTIBLE       |
            | RANGO CONSUMO    | 45                |
            | NUMERO SERIE     | SERIE1919A2026X1Z |
        Then Se procede a "GUARDAR" el vehículo

        When Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        And Se ingresa la placa "SAA019" y se busca en SOAT
        And Se selecciona el proveedor "MAPFRE"
        And Se ingresa la póliza "2619-5001"
        And Se configuran las fechas de vigencia del SOAT iniciando en "30" dias y con duracion de "365" dias
        And Se ingresa el RUC "20553856451" y se busca
        And Se selecciona la fecha de contratante igual al DESDE del SOAT
        And Se ingresa la hora de emisión "12:00" y el importe "185"
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda el SOAT

        When Se ingresa al módulo SOAT
        And Se hace clic en el boton Historial
        And Se ingresa la placa "SAA019" y se busca en SOAT
        Then Se verifica que la grilla de SOAT muestra resultados
        And Se cierra el historial del SOAT
