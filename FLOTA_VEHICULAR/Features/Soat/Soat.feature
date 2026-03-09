@Soat
Feature: Gestión de SOAT

    @RegistroSoat
    Scenario: Registrar un nuevo SOAT para un vehículo
        Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
        When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"

        And Se ingresa al módulo SOAT
        And Se selecciona Nuevo SOAT
        
        # 1. Datos del Vehículo
        When Se ingresa la placa "ONO123" y se busca en SOAT
        
        # 2. Datos de la Póliza
        And Se selecciona el proveedor "LA POSITIVA"
        And Se ingresa la póliza "12345-6789"
        And Se selecciona la fecha DESDE el día "10" y HASTA el día "28"
        
        # 3. Datos del Contratante (RUC/DNI en tu imagen es 72211766)
        And Se ingresa el RUC "72211766" y se busca
        And Se selecciona la fecha del contratante el día "9"
        # ¡DEBES PONER LOS DOS PUNTOS (:) EN LA HORA!
        And Se ingresa la hora de emisión "11:25" y el importe "200.00"
        
        # 4. Documento Adjunto (Usamos tu ruta exacta)
        And Se adjunta el documento "C:\Users\MANUEL\Pictures\goleto adidas.jpg"
        
        Then Se guarda el SOAT




