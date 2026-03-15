@ignore
@RegistroClientes
Feature: Gestión de Clientes en Facturación Cíclica

@RegistroRuc10
Scenario: Registrar nuevo cliente con tipo de documento RUC (10) con Factura
    Given Inicio de sesión con usuario 'administrador' y contraseña 'calidad' en 'https://sigesdev.newfront-dev-qa.sigesonline.com/'
    And Se ingresa al módulo 'Facturación Cíclica'
    And Se selecciona la opción 'Nuevo Cliente'
    And Se expande la sección 'Datos Generales' 
    And Se selecciona el tipo de documento 'REGISTRO NACIONAL DE CONTRIBUYENTES'
    When Se ingresan los datos de identidad:
    | Campo                | Valor                        |
    | Numero documento     | 10123456789                  |
    | Nombres              | Luis                         |
    | Apellido Paterno     | Nakamura                     |
    | Apellido Materno     | Pontorielo                   |
    | Nombre Comercial     | AGRINOVATE DEL PERU          |
    
    And Se selecciona el Ubigeo 'LIMA - LIMA - MIRAFLORES'
    And Se ingresa la dirección 'Jr. Rio de Janeiro Nro. 382' con detalle '-'
    And Se ingresa el correo electrónico 'agricola@gmail.com'
    And Se expande la sección de 'Facturación'
    And Se ingresa el número de teléfono '937584269'
    And Se completan los datos de facturación:
    | Campo                | Valor                        |
    | Tipo comprobante     | FACTURA ELECTRONICA          |
    | Ciclo facturacion    | MENSUAL                      |
    | Forma de pago        | VENCIDO                      |
    | Fecha inicio         | 02/28/2026                   |
    | Plan                 | PLAN BASICO                  |

    Then Se procede a 'GUARDAR' el registro del cliente
