@RevisionTecnica
Feature: Gestión de Revisión Técnica

    Background: Iniciar sesión en el sistema
        Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
        When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"

    # CASOS DE PRUEBA: REGISTRO DE REVISIÓN TÉCNICA (HAPPY PATHS)
    
    @RegistroRevTecnica @RegistroExitoso @ProximoAno
    Scenario Outline: Registro con vencimiento al próximo año - <Caso> - <Descripcion>
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
        When Se navega al módulo "Revisión Técnica"
        And Se selecciona "+Nuevo" en Revisión Técnica
        And Se ingresa la placa "<Placa>" y se busca en Revisión Técnica
        And Se ingresa el N de certificado "<Certificado>"
        And Se selecciona el proveedor de revisión "<Proveedor>"
        And Se selecciona la fecha de revisión el día "<DiaRev>" y vencimiento el día "<DiaVenc>" del próximo año
        And Se adjunta el documento de revisión "<RutaArchivo>"
        Then Se guarda la Revisión Técnica

        Examples:
            | Caso     | Descripcion                           | Placa  | Certificado| Proveedor   | DiaRev | DiaVenc | RutaArchivo                                |
            | CP-RT-99 | Registro Exitoso                      | RVT099 | RT2026099  | CHECK S.A.C | 28     | 28      | C:\Users\MANUEL\Pictures\goleto adidas.jpg |
            | CP-RT-98 | Registro Exitoso                      | RVT098 | RT2026098  | CHECK S.A.C | 28     | 28      | C:\Users\MANUEL\Pictures\goleto adidas.jpg |
            | CP-RT-01 | Registro Exitoso (Happy Path)         | PPP011 | PPP021821  | CHECK S.A.C | 28     | 28      | C:\Users\MANUEL\Pictures\goleto adidas.jpg |
            | CP-RT-13 | Transición Inicial -> VIGENTE         | RVT013 | RT2026013  | CHECK S.A.C | 13     | 13      | C:\Users\MANUEL\Pictures\goleto adidas.jpg |
           

    @RegistroRevTecnica @RegistroExitoso @MismoAno
    Scenario Outline: Registro con vencimiento en el mismo año - <Caso> - <Descripcion>
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
        When Se navega al módulo "Revisión Técnica"
        And Se selecciona "+Nuevo" en Revisión Técnica
        And Se ingresa la placa "<Placa>" y se busca en Revisión Técnica
        And Se ingresa el N de certificado "<Certificado>"
        And Se selecciona el proveedor de revisión "<Proveedor>"
        And Se selecciona la fecha de revisión el día "<DiaRev>" y vencimiento el día "<DiaVenc>" del mismo año
        And Se adjunta el documento de revisión "<RutaArchivo>"
        Then Se guarda la Revisión Técnica

        Examples:
            | Caso     | Descripcion                           | Placa  | Certificado| Proveedor   | DiaRev | DiaVenc | RutaArchivo                                |
            | CP-RT-06 | Cálculo de Vigencia - VIGENTE         | RVT006 | RT2026006  | CHECK S.A.C | 20     | 28      | C:\Users\MANUEL\Pictures\goleto adidas.jpg |
            | CP-RT-07 | Cálculo Vigencia - PRÓXIMO A VENCER   | RVT007 | RT2026007  | CHECK S.A.C | 20     | 27      | C:\Users\MANUEL\Pictures\goleto adidas.jpg |
            | CP-RT-08 | Cálculo de Vigencia - CADUCADO        | RVT008 | RT2026008  | CHECK S.A.C | 20     | 19      | C:\Users\MANUEL\Pictures\goleto adidas.jpg |



   
    # CASOS DE PRUEBA: BLOQUEOS Y VALIDACIONES NEGATIVAS

    @BloqueoRevTecnica @CP-RT-02
    Scenario: CP-RT-02 - Bloqueo por Campos Obligatorios Incompletos (Falta Proveedor)
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        When Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | RVT002            |
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
        When Se navega al módulo "Revisión Técnica"
        And Se selecciona "+Nuevo" en Revisión Técnica
        And Se ingresa la placa "RVT002" y se busca en Revisión Técnica
        And Se ingresa el N de certificado "RT2026002"
        And Se selecciona la fecha de revisión el día "26" y vencimiento el día "26" del próximo año
        And Se adjunta el documento de revisión "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda la Revisión Técnica

    @BloqueoRevTecnica @CP-RT-03
    Scenario: CP-RT-03 - Bloqueo por Falta de Documento Adjunto
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        When Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | RVT003            |
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
        When Se navega al módulo "Revisión Técnica"
        And Se selecciona "+Nuevo" en Revisión Técnica
        And Se ingresa la placa "RVT003" y se busca en Revisión Técnica
        And Se ingresa el N de certificado "RT2026003"
        And Se selecciona el proveedor de revisión "CHECK S.A.C."
        And Se selecciona la fecha de revisión el día "28" y vencimiento el día "28" del próximo año
        Then Se guarda la Revisión Técnica

    @BloqueoRevTecnica @CP-RT-04
    Scenario: CP-RT-04 - Bloqueo por Placa No Registrada
        When Se navega al módulo "Revisión Técnica"
        And Se selecciona "+Nuevo" en Revisión Técnica
        And Se ingresa la placa "RVT004" y se busca en Revisión Técnica
        Then Se valida el mensaje de error "Vehículo no Registrado La placa ingresada no corresponde a ningun vehículo"

    @BloqueoRevTecnica @CP-RT-05
    Scenario: CP-RT-05 - Bloqueo por Incoherencia de Fechas (Rev > Venc)
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        When Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | RVT005            |
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
        When Se navega al módulo "Revisión Técnica"
        And Se selecciona "+Nuevo" en Revisión Técnica
        And Se ingresa la placa "RVT005" y se busca en Revisión Técnica
        And Se ingresa el N de certificado "RT2026005"
        And Se selecciona el proveedor de revisión "CHECK S.A.C."
        And Se selecciona la fecha de revisión el día "28" y vencimiento el día "01" del mismo año
        And Se adjunta el documento de revisión "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda la Revisión Técnica
        And Se valida que el botón Guardar esté deshabilitado

    @BloqueoRevTecnica @CP-RT-DUPLICADO
    Scenario: CP-RT-DUPLICADO - Bloqueo por Certificado Ya Existente
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        When Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | RVT001            |
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
        When Se navega al módulo "Revisión Técnica"
        And Se selecciona "+Nuevo" en Revisión Técnica
        And Se ingresa la placa "RVT001" y se busca en Revisión Técnica
        And Se ingresa el N de certificado "RT2026001"
        And Se selecciona el proveedor de revisión "CHECK S.A.C."
        And Se selecciona la fecha de revisión el día "28" y vencimiento el día "28" del próximo año
        And Se adjunta el documento de revisión "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda la Revisión Técnica
        And Se valida el mensaje de error "Registro de Revisión Técnica Fallido! El N° del certificado ingresado ya existe"



    #CASOS DE PRUEBA: EDICIÓN DE REVISIÓN TÉCNICA

@EdicionRevTecnica @CP-RT-09
    Scenario: CP-RT-09 - Edición de N° de Certificado de una revisión técnica
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        When Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | RVT009            |
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
        When Se navega al módulo "Revisión Técnica"
        And Se selecciona "+Nuevo" en Revisión Técnica
        And Se ingresa la placa "RVT009" y se busca en Revisión Técnica
        And Se ingresa el N de certificado "RTV2026209"
        And Se selecciona el proveedor de revisión "CHECK S.A.C."
        And Se selecciona la fecha de revisión el día "17" y vencimiento el día "17" del próximo año
        And Se adjunta el documento de revisión "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda la Revisión Técnica
        When Se navega al módulo "Revisión Técnica"
        And Se busca la revisión técnica por N° de certificado "RTV2026209"
        And Se hace clic en ver Revisión Técnica
        And Se hace clic en editar Revisión Técnica
        And Se ingresa el N de certificado "RTV2026209EDIT"
        Then Se guarda la Revisión Técnica


@EdicionRevTecnica @CP-RT-009
    Scenario: CP-RT-009 - prueba de edición de certificado
        When Se navega al módulo "Revisión Técnica"
        And Se busca la revisión técnica por N° de certificado "RTV2026109"
        And Se hace clic en ver Revisión Técnica
        And Se hace clic en editar Revisión Técnica
        And Se ingresa el N de certificado "RTV2026109EDIT"
        Then Se guarda la Revisión Técnica






    @EdicionRevTecnica @TransicionesEstado @CP-RT-10
    Scenario: CP-RT-10 - Modificación de Fecha de Vencimiento que recalcula el Estado
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        When Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | RVT010            |
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
        When Se navega al módulo "Revisión Técnica"
        And Se selecciona "+Nuevo" en Revisión Técnica
        And Se ingresa la placa "RVT010" y se busca en Revisión Técnica
        And Se ingresa el N de certificado "RT2026010"
        And Se selecciona el proveedor de revisión "CHECK S.A.C."
        And Se selecciona la fecha de revisión el día "20" y vencimiento el día "20" del próximo año
        And Se adjunta el documento de revisión "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda la Revisión Técnica
        When Se navega al módulo "Revisión Técnica"
        And Se busca la revisión técnica por placa "RVT010"
        And Se hace clic en ver Revisión Técnica
        And Se hace clic en editar Revisión Técnica
        And Se editan las fechas seleccionando el día "20" para revisión y el día "30" para vencimiento del mismo año
        Then Se guarda la Revisión Técnica



        @EdicionRevTecnica @TransicionesEstado @CP-RT-14
    Scenario: CP-RT-14 - Transición VIGENTE → PRÓXIMO A VENCER
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        When Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | RVT014            |
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
        When Se navega al módulo "Revisión Técnica"
        And Se selecciona "+Nuevo" en Revisión Técnica
        And Se ingresa la placa "RVT014" y se busca en Revisión Técnica
        And Se ingresa el N de certificado "RTV2026014"
        And Se selecciona el proveedor de revisión "CHECK S.A.C."
        And Se configuran dinámicamente las fechas por calendario para el estado "VIGENTE"
        And Se adjunta el documento de revisión "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda la Revisión Técnica
        When Se navega al módulo "Revisión Técnica"
        And Se busca la revisión técnica por N° de certificado "RTV2026014"
        And Se hace clic en ver Revisión Técnica
        And Se hace clic en editar Revisión Técnica
        And Se configuran dinámicamente las fechas por calendario para el estado "PRÓXIMO A VENCER"
        Then Se guarda la Revisión Técnica





 
    #CASOS DE PRUEBA: EDICIÓN DE FECHAS Y TRANSICIÓN A CADUCADO
 
    @EdicionRevTecnica @TransicionesEstado @CP-RT-11
    Scenario: CP-RT-11 - Edición de N° de Certificado de una revisión técnica Caducado pero Activo
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        When Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | RVT011            |
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
        When Se navega al módulo "Revisión Técnica"
        And Se selecciona "+Nuevo" en Revisión Técnica
        And Se ingresa la placa "RVT011" y se busca en Revisión Técnica
        And Se ingresa el N de certificado "RTV202600011"
        And Se selecciona el proveedor de revisión "CHECK S.A.C."
        And Se seleccionan las fechas del año pasado el día "10" para revisión y el día "15" para vencimiento
        And Se adjunta el documento de revisión "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda la Revisión Técnica
        When Se navega al módulo "Revisión Técnica"
        And Se busca la revisión técnica por N° de certificado "RTV202600011"
        And Se hace clic en ver Revisión Técnica
        And Se hace clic en editar Revisión Técnica
        And Se ingresa el N de certificado "RTV202600011EDIT"
        Then Se guarda la Revisión Técnica


 @EdicionRevTecnica @TransicionesEstado @CP-RT-29
    Scenario: CP-RT-29 - Transición VIGENTE → CADUCADO
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        When Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | RVT029            |
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
        
        When Se navega al módulo "Revisión Técnica"
        And Se selecciona "+Nuevo" en Revisión Técnica
        And Se ingresa la placa "RVT029" y se busca en Revisión Técnica
        And Se ingresa el N de certificado "RTV20260000000029"
        And Se selecciona el proveedor de revisión "CHECK S.A.C."
        And Se selecciona la fecha de revisión el día "15" y vencimiento el día "15" del próximo año
        And Se adjunta el documento de revisión "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda la Revisión Técnica
        When Se navega al módulo "Revisión Técnica"
        And Se busca la revisión técnica por N° de certificado "RTV20260000000029"
        And Se hace clic en ver Revisión Técnica
        And Se hace clic en editar Revisión Técnica
        And Se edita la fecha de vencimiento al estándar caducado de inicio de año
        Then Se guarda la Revisión Técnica


    @EdicionRevTecnica @TransicionesEstado @CP-RT-30
  Scenario: CP-RT-30 - Transición PRÓXIMO A VENCER → CADUCADO
      When Se ingresa al módulo "Vehículo"
      And Se selecciona "+Nuevo"
      When Se ingresan los datos del vehículo:
      | Campo            | Valor             |
      | PLACA            | RVT030            |
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
      
      When Se navega al módulo "Revisión Técnica"
      And Se selecciona "+Nuevo" en Revisión Técnica
      And Se ingresa la placa "RVT030" y se busca en Revisión Técnica
      And Se ingresa el N de certificado "RTV2026030"
      And Se selecciona el proveedor de revisión "CHECK S.A.C."
      And Se selecciona la fecha de revisión el día "1" y vencimiento el día "5" del mismo año
      And Se adjunta el documento de revisión "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
      Then Se guarda la Revisión Técnica
      When Se navega al módulo "Revisión Técnica"
      And Se busca la revisión técnica por N° de certificado "RTV2026030"
      And Se hace clic en ver Revisión Técnica
      And Se hace clic en editar Revisión Técnica
      And Se edita la fecha de vencimiento al estándar caducado de inicio de año
      Then Se guarda la Revisión Técnica

   @BloqueoEdicion @CP-RT-12
    Scenario: CP-RT-12 - Bloqueo de Edición por Estado-Registro DE BAJA
       
        When Se ingresa al módulo "Vehículo"
        And Se selecciona "+Nuevo"
        When Se ingresan los datos del vehículo:
        | Campo            | Valor             |
        | PLACA            | RVT012            |
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
        
        When Se navega al módulo "Revisión Técnica"
        And Se selecciona "+Nuevo" en Revisión Técnica
        And Se ingresa la placa "RVT012" y se busca en Revisión Técnica
        And Se ingresa el N de certificado "RTV2026012"
        And Se selecciona el proveedor de revisión "CHECK S.A.C."
        And Se selecciona la fecha de revisión el día "15" y vencimiento el día "15" del próximo año
        And Se adjunta el documento de revisión "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        Then Se guarda la Revisión Técnica

        # ------------------------------------------
        # FASE 2: DAR DE BAJA EL REGISTRO
        # ------------------------------------------
        When Se navega al módulo "Revisión Técnica"
        And Se busca la revisión técnica por N° de certificado "RTV2026012"
        And Se hace clic en ver Revisión Técnica
        And Se hace clic en dar de baja Revisión Técnica
        And Se ingresan las observaciones de baja en Revisión Técnica "Pre-condición CP-12: Anulación por prueba de edición"
        Then Se guarda la baja de la Revisión Técnica

        # ------------------------------------------
        # FASE 3: VALIDACIÓN DEL BLOQUEO
        # ------------------------------------------
        When Se actualiza la página
        And Se navega al módulo "Revisión Técnica"
        And Se busca la revisión técnica por N° de certificado "RTV2026012"
        And Se hace clic en ver Revisión Técnica
        Then Se valida que la opción de editar está bloqueada u oculta