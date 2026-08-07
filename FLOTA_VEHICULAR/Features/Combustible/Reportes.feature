Feature: Generacion de Reportes de Combustible

    Background: Iniciar sesión en el sistema
        Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
        When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"


    @Reportes
    Scenario Outline: <ID_Caso> - Validacion de generacion de reportes (<Tipo>)

        # Preparación de datos: creación de vehículo
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        And Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | <Placa>           |
        | AREA ASIGNADA    | <AreaParaCrear>   |
        | PROPIETARIO      | MIMP              |
        | MARCA            | DAEWOO            |
        | MODELO           | TICO SL           |
        | AÑO              | 2026              |
        | TIPO DE VEHICULO | AUTOMOVIL         |
        | CLASIFICADOR     | ALTA              |
        | COLOR            | ROJO              |
        | NUMERO MOTOR     | ENG<Placa>        |
        | TIPO COMBUSTIBLE | <Concepto>        |
        | TIPO MOTOR       | COMBUSTIBLE       |
        | RANGO CONSUMO    | 45                |
        | NUMERO SERIE     | SERIE000<Placa>   |
        Then Se procede a "GUARDAR" el vehículo

        # Preparación de datos: creación de conductor
        When Se ingresa al módulo "Conductor" y submódulo "Ver conductores"
        And Se selecciona el boton Nuevo
        And Se ingresa el DNI "<DniParaCrear>" del conductor y se busca
        And Se selecciona la fecha de nacimiento el dia "15" y ano "1990"
        And Se selecciona el genero "Masculino" y area "<AreaParaCrear>"
        And Se ingresa el correo "chofer<DniParaCrear>@mimp.gob.pe", telefono "987654321" y direccion "Av Lima 123"
        And Se ingresa la licencia "Q<DniParaCrear>", clase "A" y categoria "IIa"
        And Se selecciona la fecha de expedicion el dia "10" y vencimiento el dia "10" dentro de "3" anos
        And Se adjunta el archivo "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        And Se hace clic en el boton Agregar Licencia
        Then Se guarda el registro

        # Preparación de datos: registro de abastecimiento
        When Se ingresa al módulo "Combustible" y submódulo "Ver abastecimientos"
        And Se recarga la página
        And Se selecciona el boton Nuevo
        And Se ingresa la placa "<Placa>" en abastecimiento y se busca
        And Se ingresa la nota de despacho "<NotaDespacho>"
        And Se selecciona la fecha de registro el dia "<DiaRegistro>"
        And Se selecciona el conductor "<ConductorExistente>" en abastecimiento
        And Se ingresa la hora de despacho "<HoraDespacho>" y odometro "<Odometro>"
        And Se selecciona el area "<AreaParaAbastecer>" y contrato "<Contrato>"
        And Se adjunta el archivo "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        And Se selecciona el concepto "<Concepto>" y cantidad "<Cantidad>"
        Then Se guarda el registro

        # Validación del reporte
        When Se ingresa al módulo "Combustible" y submódulo "Reportes"
        And Se selecciona el tipo de reporte "<Tipo>" 
        And Se filtran las fechas del reporte desde "<FechaDesde>" hasta "<FechaHasta>"
        And Se selecciona el area "<AreaReporte>" para el reporte
        And Se selecciona el contrato "<ContratoReporte>" en reportes
        And Se hace clic en el boton Ver Reporte
        Then Se valida que el sistema genere la accion esperada para el resultado "<ResultadoEsperado>"

    Examples:
        | ID_Caso    | Tipo           | FechaDesde | FechaHasta | Concepto   | Placa  | DniParaCrear | ConductorExistente              | NotaDespacho | AreaParaCrear  | AreaParaAbastecer | AreaReporte    | Contrato | ContratoReporte | DiaRegistro | HoraDespacho | Odometro | Cantidad | ResultadoEsperado |
        | CP-COMB-13 | Valorizaciones | 01012026   | 24122026   | G-95 - GLP | PCR113 | 72211715     | JENNY LIZBETH DIAZ SALDAÑA      | 202605013    | OAS-TRANSPORTE | PNP               | OAS-TRANSPORTE | CTR26002 | CTR26002         | 15          | 10:00        | 128000   | 20       | REPORTE_CON_DATOS |
        | CP-COMB-38 | Control        | 01012026   | 24122026   | G-95 - GLP | PCR138 | 72211717     | JHON MILTON DIAZ CUBAS          | 202605038    | OAS-TRANSPORTE | PNP               | OAS-TRANSPORTE | CTR26002 | N/A              | 16          | 11:00        | 128500   | 18       | REPORTE_CON_DATOS |