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
    | NUMERO SERIE     | ABCD123456789012A |

    Then Se procede a "GUARDAR" el vehículo


    @BajaVehiculo
Scenario: Dar de baja a un vehículo existente
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"

    And Se ingresa al módulo "Vehículo"
    
    When Se busca el vehículo por placa "HOLAAA"
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
        When Se busca el vehículo por placa "688IKJ"
        And Se hace clic en ver vehículo
        
        # 2. Clic en el icono de lápiz
        And Se hace clic en editar
        
        # 3. Reutilizamos tu paso maestro (sin la fila PLACA)
        When Se ingresan los datos del vehículo:
        | Campo            | Valor            |
        | AREA ASIGNADA    | UPE LIMA ESTE    |
        | PROPIETARIO      | PCM              |
        | MARCA            | KIA              | 
        | MODELO           | RIO              |
        | AÑO              | 2025             |
        | TIPO DE VEHICULO | CAMIONETA RURAL  |
        | CLASIFICADOR     | MEDIA            |
        | COLOR            | AZUL             |
        | NUMERO MOTOR     | NUEVOENG123      |
        | TIPO COMBUSTIBLE | G-95             |
        | TIPO MOTOR       | BI-COMBUSTIBLE   |
        | RANGO CONSUMO    | 50               |
        | NUMERO SERIE     | NUEVOSERIE98765A |

        # 4. Guardamos los cambios
        Then Se procede a "GUARDAR" el vehículo