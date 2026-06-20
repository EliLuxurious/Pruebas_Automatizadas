Feature: Flujo End-to-End (E2E) 

    Background: Iniciar sesión en el sistema
        Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
        When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"

    @FlujoE2E
    Scenario Outline: CP-COMB-<ID> - Registro E2E para el combustible: <Concepto>

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
        When Se ingresa al módulo "Contrato" y submódulo "Contratos de Abastecimientos"
        And Se selecciona el boton Nuevo
        And Se ingresa el numero de contrato "<ContratoParaCrear>"
        And Se selecciona la fecha del contrato DESDE el dia "1" y HASTA el dia "1" dentro de "2" anos
        And Se selecciona el tipo "Abastecimiento", concepto "<Concepto>" y area "<AreaParaCrear>"
        And Se ingresa la cantidad "100000" y precio unitario "15.50"
        And Se ingresa el RUC "20542259117" del proveedor y se busca
        And Se ingresa la direccion "Sede Central", correo "prov@test.com", telefono "999888777" y clasificacion "SAC"
        Then Se guarda el registro 

        When Se ingresa al módulo "Combustible" y submódulo "Ver abastecimientos"
        And Se recarga la página
        And Se selecciona el boton Nuevo
        And Se ingresa la placa "<Placa>" en abastecimiento y se busca
        And Se ingresa la nota de despacho "<NotaDespacho>"
        And Se selecciona la fecha de registro el dia "<DiaRegistro>"
        And Se selecciona el conductor "<ConductorExistente>" en abastecimiento
        And Se ingresa la hora de despacho "<HoraDespacho>" y odometro "<Odometro>"
        And Se selecciona el area "<AreaParaAbastecer>" y contrato "<ContratoExistente>"
        And Se adjunta el archivo "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        And Se selecciona el concepto "<Concepto>" y cantidad "<Cantidad>"
        Then Se verifica que el resultado del guardado sea "<ResultadoEsperado>"

   Examples:
    | ID  | Concepto   | Placa  | DniParaCrear | ContratoParaCrear | ConductorExistente                  | ContratoExistente | NotaBase  | NotaDespacho | AreaParaCrear  | AreaParaAbastecer | DiaRegistro | HoraBase | HoraDespacho | OdometroBase | Odometro    | CantidadBase | Cantidad | ResultadoEsperado           |
    | 01  | G-95 - GLP | PPA101 | 72211711     | CTR20262301       | GIULIANNO RENATO MEDINA SALDAÑA     | CTR26002          | 202603001 | 202603101    | OAS-TRANSPORTE | PNP               | 15          | 08:00    | 10:30        | 129000       | 130000      | 10           | 20       | EXITO                       |
    | 02  | G-95 - GLP | PPA102 | 72211712     | CTR20262302       | ANGELA RUBI GONZALES CARRANZA       | CTR26002          | 202603002 | 202603102    | OAS-TRANSPORTE | PNP               | 16          | 08:10    | 14:45        | 129100       | 130350      | 10           | 18       | EXITO                       |
    | 03  | G-95 - GLP | PPA103 | 72211713     | CTR20262303       | JULIO OMAR MONCAYO CHAVEZ           | CTR26002          | 202603003 | 202603103    | OAS-TRANSPORTE | PNP               | 18          | 08:20    | 09:15        | 129200       | 130800      | 10           | 22       | EXITO                       |
    | 17a | G-95 - GLP | PPA117 | 72211714     | CTR20262317       | DENIS OMAR ALVARADO IRIGOIN         | CTR26002          | 202603017 | 202603117    | OAS-TRANSPORTE | PNP               | 25          | 08:30    | 09:00        | 129500       | 130000      | 10           | 20       | EXITO                       |
    | 17b | G-95 - GLP | PPA118 | 72211714     | CTR20262318       | DENIS OMAR ALVARADO IRIGOIN         | CTR26002          | 202603018 | 202603118    | OAS-TRANSPORTE | PNP               | 25          | 08:40    | 09:00        | 130000       | 129999      | 10           | 20       | ERROR_ODOMETRO_MENOR        |
    | 18a | G-95 - GLP | PPA119 | 72211715     | CTR20262319       | JENNY LIZBETH DIAZ SALDAÑA          | CTR26002          | 202603019 | 202603119    | OAS-TRANSPORTE | PNP               | 20          | 08:50    | 11:00        | 129700       | 130000      | 10           | 18       | EXITO                       |
    | 18b | G-95 - GLP | PPA120 | 72211716     | CTR20262320       | AMALIA ISABEL VILLALOBOS DIAZ       | CTR26002          | 202603020 | 202603020    | OAS-TRANSPORTE | PNP               | 20          | 09:00    | 11:00        | 129800       | 130500      | 10           | 18       | ERROR_NO_GUARDA             |
    | 19  | G-95 - GLP | PPA121 | 72211717     | CTR20262321       | JHON MILTON DIAZ CUBAS              | CTR26002          | 202603021 | 202603121    | OAS-TRANSPORTE | PNP               | 26          | 09:10    | 09:20        | 126500       | 127000      | 10           | -5       | BOTON_GUARDAR_DESHABILITADO |
    | 20a | G-95 - GLP | PPA122 | 72211719     | CTR20262322       | JHEYSY MARISOL DELGADO SANCHEZ      | CTR26002          | 202603022 | 202603122    | OAS-TRANSPORTE | PNP               | 27          | 09:20    | 08:30        | 127000       | 9999999999  | 10           | 15       | EXITO                       |
    | 20b | G-95 - GLP | PPA123 | 72211766     | CTR20262323       | MANUEL ANTONIO BERROSPI RAMIREZ     | CTR26002          | 202603023 | 202603123    | OAS-TRANSPORTE | PNP               | 27          | 09:30    | 08:40        | 127100       | 10000000000 | 10           | 15       | BOTON_GUARDAR_DESHABILITADO |
    | 21  | G-95 - GLP | PPA124 | 72211766     | CTR20262324       | MANUEL ANTONIO BERROSPI RAMIREZ     | CTR26002          | 202603024 |              | OAS-TRANSPORTE | PNP               | 28          | 09:40    | 10:00        | 127000       | 127200      | 10           | 17       | BOTON_GUARDAR_DESHABILITADO |




       @EdicionAbastecimiento @CP-COMB-07
    Scenario Outline: <ID_Caso> - Edición de Registro de Abastecimiento

        When Se ingresa al módulo "Combustible" y submódulo "Ver abastecimientos"
        And Se recarga la página
        And Se filtra la tabla usando el selector de Placa "<PlacaFiltro>"
        And Se hace clic en el boton BUSCAR de la grilla
        And Se hace clic en la Lupa del primer registro de la tabla
        And Se hace clic en el boton Editar abastecimiento
        And Se ingresa la hora de despacho "<NuevaHora>" y odometro "<NuevoOdometro>"
        And Se modifica la cantidad por "<NuevaCantidad>"
        Then Se verifica que el resultado de la actualizacion sea "<ResultadoEsperado>"

    Examples:
        | ID_Caso     | PlacaFiltro | NuevaHora | NuevoOdometro | NuevaCantidad | ResultadoEsperado           |
        | CP-COMB-07a | COM012      | 14:00     | 130000        | 25            | EXITO_ACTUALIZACION         |
        | CP-COMB-07b | COM013      | 15:00     | 110000        | 15            | ERROR_ODOMETRO_MENOR        |




        @AnularAbastecimiento 
    Scenario Outline: <ID_Caso> - Creación y Anulación de Registro de Abastecimiento
        
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

        When Se ingresa al módulo "Contrato" y submódulo "Contratos de Abastecimientos"
        And Se selecciona el boton Nuevo
        And Se ingresa el numero de contrato "<ContratoParaCrear>"
        And Se selecciona la fecha del contrato DESDE el dia "1" y HASTA el dia "1" dentro de "2" anos
        And Se selecciona el tipo "Abastecimiento", concepto "<Concepto>" y area "<AreaParaCrear>"
        And Se ingresa la cantidad "100000" y precio unitario "15.50"
        And Se ingresa el RUC "20542259117" del proveedor y se busca
        And Se ingresa la direccion "Sede Central", correo "prov@test.com", telefono "999888777" y clasificacion "SAC"
        Then Se guarda el registro 
        When Se ingresa al módulo "Combustible" y submódulo "Ver abastecimientos"
        And Se recarga la página
        And Se selecciona el boton Nuevo
        And Se ingresa la placa "<Placa>" en abastecimiento y se busca
        And Se ingresa la nota de despacho "<NotaDespacho>"
        And Se selecciona la fecha de registro el dia "<DiaRegistro>"
        And Se selecciona el conductor "<ConductorExistente>" en abastecimiento
        And Se ingresa la hora de despacho "<HoraDespacho>" y odometro "<Odometro>"
        And Se selecciona el area "<AreaParaAbastecer>" y contrato "<ContratoExistente>"
        And Se adjunta el archivo "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        And Se selecciona el concepto "<Concepto>" y cantidad "<Cantidad>"
        Then Se verifica que el resultado del guardado sea "<ResultadoEsperado>"

        When Se recarga la página
        And Se filtra la tabla usando el selector de Placa "<Placa>"
        And Se hace clic en el boton BUSCAR de la grilla
        And Se hace clic en la Lupa del primer registro de la tabla
        And Se hace clic en el boton Anular abastecimiento
        And Se ingresan las observaciones de baja "<Observacion>" y se guarda
        Then Se verifica que el resultado de la anulacion sea "EXITO_ANULACION"

    Examples:
        | ID_Caso     | Concepto   | Placa  | DniParaCrear | ContratoParaCrear | ConductorExistente              | ContratoExistente | NotaDespacho | AreaParaCrear  | AreaParaAbastecer | DiaRegistro | HoraDespacho | Odometro | Cantidad | ResultadoEsperado | Observacion                     |
        | CP-COMB-BAJ | G-95 - GLP | BAJAAN | 23015637     | CTR20260599       | MANUEL ANTONIO BERROSPI RAMIREZ | CTR26002          | 20262199     | OAS-TRANSPORTE | PNP               | 25          | 10:00        | 999999   | 10       | EXITO             | Prueba automatizada de baja QA. |
