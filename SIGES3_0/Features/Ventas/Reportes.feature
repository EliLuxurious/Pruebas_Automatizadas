@ignore
Feature: Reportes

Escenario base para reportes del sistema.

Background:
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'

@Reportes
Scenario: Generar reporte por tipo de comprobante
    When Ingresar a Compras - Reportes
    And Reporte por Tipo, Tipo de comprobante 'NO TRIBUTABLES' Fecha Inicial '01/01/2025' Fecha Final '01/02/2025'
    Then Generar reporte por 'TIPO'
