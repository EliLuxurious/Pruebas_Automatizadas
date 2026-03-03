Feature: LoginFeature

Prueba de Login Exitoso

@InicioSesion
Scenario: Inicio de sesion
    Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
    When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"
