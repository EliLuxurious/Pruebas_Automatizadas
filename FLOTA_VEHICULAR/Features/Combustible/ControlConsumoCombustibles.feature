Feature: Control de Consumo de Combustibles y Cálculos Matemáticos

    Background: Iniciar sesión en el sistema
        Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
        When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"


    @ControlConsumo
    Scenario Outline: <ID_Caso> - Validacion de Limites y Tolerancia de Consumo

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

        When Se ingresa al módulo "Combustible" y submódulo "Ver abastecimientos"
        And Se recarga la página
        And Se selecciona el boton Nuevo
        And Se ingresa la placa "<Placa>" en abastecimiento y se busca
        And Se ingresa la nota de despacho "<NotaBase>"
        And Se selecciona la fecha de registro el dia "<DiaBase>"
        And Se selecciona el conductor "<ConductorExistente>" en abastecimiento
        And Se ingresa la hora de despacho "<HoraBase>" y odometro "<OdoInicial>"
        And Se selecciona el area "<AreaParaAbastecer>" y contrato "<ContratoExistente>"
        And Se adjunta el archivo "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        And Se selecciona el concepto "<Concepto>" y cantidad "<CantidadBase>"
        Then Se guarda el registro

        When Se recarga la página
        And Se selecciona el boton Nuevo
        And Se ingresa la placa "<Placa>" en abastecimiento y se busca
        And Se ingresa la nota de despacho "<NotaFinal>"
        And Se selecciona la fecha de registro el dia "<DiaFinal>"
        And Se selecciona el conductor "<ConductorExistente>" en abastecimiento
        And Se ingresa la hora de despacho "<HoraFinal>" y odometro "<OdoFinal>"
        And Se selecciona el area "<AreaParaAbastecer>" y contrato "<ContratoExistente>"
        And Se adjunta el archivo "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        And Se selecciona el concepto "<Concepto>" y cantidad "<CantidadFinal>"
        Then Se guarda el registro

        When Se ingresa al módulo "Combustible" y submódulo "Control Consumo Combustibles"
        And Se filtran las fechas desde el año "<AnoDesde>" hasta hoy
        And Se filtra por placa "<Placa>" en control de consumo
        And Se hace clic en el boton Buscar en la pantalla de control
        And Se hace clic en el icono de la Lupa del primer registro
        Then Se verifican los calculos y la regla de tolerancia del consumo
        And Se cierra el modal de detalle de consumo

    Examples:
        | ID_Caso    | AnoDesde | Concepto   | Placa  | DniParaCrear | ConductorExistente              | NotaBase  | NotaFinal | AreaParaCrear  | AreaParaAbastecer | ContratoExistente | DiaBase | DiaFinal | HoraBase | HoraFinal | OdoInicial | OdoFinal | CantidadBase | CantidadFinal |
        | CP-COMB-01 | 2026     | G-95 - GLP | PCC101 | 72211711     | GIULIANNO RENATO MEDINA SALDAÑA | 202604001 | 202604101 | OAS-TRANSPORTE | PNP               | CTR26002          | 15      | 16       | 08:00    | 10:30     | 126000     | 126180   | 15           | 20            |
        | CP-COMB-15 | 2026     | G-95 - GLP | PCC115 | 72211712     | ANGELA RUBI GONZALES CARRANZA   | 202604015 | 202604115 | OAS-TRANSPORTE | PNP               | CTR26002          | 15      | 16       | 08:10    | 11:00     | 130000     | 130001   | 15           | 15            |


    @FlujoE2E_SoloAbastecimientoYControl
    Scenario Outline: <ID_Caso> - Registro de Abastecimientos y Validacion BI-09 (<Limite>)

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

        When Se ingresa al módulo "Combustible" y submódulo "Ver abastecimientos"
        And Se recarga la página
        And Se selecciona el boton Nuevo
        And Se ingresa la placa "<Placa>" en abastecimiento y se busca
        And Se ingresa la nota de despacho "<NotaBase>"
        And Se selecciona la fecha de registro el dia "15"
        And Se selecciona el conductor "<ConductorExistente>" en abastecimiento
        And Se ingresa la hora de despacho "08:00" y odometro "<OdoInicial>"
        And Se selecciona el area "<AreaParaAbastecer>" y contrato "<ContratoExistente>"
        And Se adjunta el archivo "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        And Se selecciona el concepto "<Concepto>" y cantidad "15"
        Then Se guarda el registro

        When Se recarga la página
        And Se selecciona el boton Nuevo
        And Se ingresa la placa "<Placa>" en abastecimiento y se busca
        And Se ingresa la nota de despacho "<NotaLimite>"
        And Se selecciona la fecha de registro el dia "16"
        And Se selecciona el conductor "<ConductorExistente>" en abastecimiento
        And Se ingresa la hora de despacho "10:00" y odometro "<OdoFinal>"
        And Se selecciona el area "<AreaParaAbastecer>" y contrato "<ContratoExistente>"
        And Se adjunta el archivo "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        And Se selecciona el concepto "<Concepto>" y cantidad "15"
        Then Se guarda el registro

        When Se ingresa al módulo "Combustible" y submódulo "Control Consumo Combustibles"
        And Se filtran las fechas desde el año "2026" hasta hoy
        And Se filtra por placa "<Placa>" en control de consumo
        And Se hace clic en el boton Buscar en la pantalla de control
        And Se hace clic en el icono de la Lupa del primer registro
        Then Se verifican los calculos y la regla de tolerancia del consumo
        And Se cierra el modal de detalle de consumo

    Examples:
        | ID_Caso    | Limite | Concepto   | Placa  | DniParaCrear | ConductorExistente            | NotaBase  | NotaLimite | OdoInicial | OdoFinal | AreaParaCrear  | AreaParaAbastecer | ContratoExistente |
        | CP-COMB-15 | 0      | G-95 - GLP | PCC215 | 72211713     | JULIO OMAR MONCAYO CHAVEZ     | 202604215 | 202604315  | 126000     | 126000   | OAS-TRANSPORTE | PNP               | CTR26002          |
        | CP-COMB-16 | 1      | G-95 - GLP | PCC216 | 72211714     | DENIS OMAR ALVARADO IRIGOIN   | 202604216 | 202604316  | 120000     | 120001   | OAS-TRANSPORTE | PNP               | CTR26002          |
