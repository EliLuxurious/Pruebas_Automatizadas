@ignore
Feature: LoginFeature

Prueba de Login Exitoso

@InicioSesion
Scenario: Inicio de sesion exitoso
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
