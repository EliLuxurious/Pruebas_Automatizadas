Feature: CreacionCliente

  CP001: Creación de cliente con DNI - Persona Natural con búsqueda RENIEC.
  CP002: Creación de cliente con RUC 10 - Persona Natural con búsqueda SUNAT.
  CP003: Creación de cliente con RUC 20 - Persona Jurídica con búsqueda SUNAT.

Background:
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
    And el usuario accede al módulo 'Ventas'
    And el usuario accede al submodulo 'Nueva Venta'

@CreacionCliente
@ClienteDNI
Scenario Outline: Creación de cliente con DNI - <Caso>
    When abre el modal de creación de cliente
    And selecciona tipo de documento '<TipoDocumento>' con número '<NumeroDocumento>'
    And completa datos generales con género '<Genero>', estado civil '<EstadoCivil>', correo '<Correo>' y teléfono '<Telefono>'
    And ingresa dirección '<Direccion>'
    And guarda y confirma el cliente
    Then el cliente '<NumeroDocumento>' queda registrado en la venta

    Examples:
      | Caso  | TipoDocumento              | NumeroDocumento | Genero    | EstadoCivil | Correo              | Telefono  | Direccion                                            |
      | CP001 | DOC. NACIONAL DE IDENTIDAD | 46326331       | MASCULINO | Casado(a)   | caterno99@gmail.com | 927027827 | Brisas del huallaga comite 7 mz y lt 17, tingo maria |

@CreacionCliente
@ClienteRUC10
Scenario Outline: Creación de cliente con RUC 10 - Persona Natural - <Caso>
    When abre el modal de creación de cliente
    And selecciona tipo de documento '<TipoDocumento>' con número '<NumeroDocumento>'
    And completa datos generales con género '<Genero>', estado civil '<EstadoCivil>', correo '<Correo>' y teléfono '<Telefono>'
    And ingresa dirección '<Direccion>'
    And guarda y confirma el cliente
    Then el cliente '<NumeroDocumento>' queda registrado en la venta

    Examples:
      | Caso  | TipoDocumento                | NumeroDocumento | Genero    | EstadoCivil | Correo              | Telefono  | Direccion                                            |
      | CP002 | REG. UNICO DE CONTRIBUYENTES | 10273622786     | MASCULINO | Casado(a)   | caterno99@gmail.com | 927027827 | Brisas del huallaga comite 7 mz y lt 17, tingo maria |

@CreacionCliente
@ClienteRUC20
Scenario Outline: Creación de cliente con RUC 20 - Persona Jurídica - <Caso>
    When abre el modal de creación de cliente
    And selecciona tipo de documento '<TipoDocumento>' con número '<NumeroDocumento>'
    And completa datos de empresa con correo '<Correo>' y teléfono '<Telefono>'
    And ingresa dirección '<Direccion>'
    And guarda y confirma el cliente
    Then el cliente '<NumeroDocumento>' queda registrado en la venta

    Examples:
      | Caso  | TipoDocumento                | NumeroDocumento | Correo              | Telefono  | Direccion                                            |
      | CP003 | REG. UNICO DE CONTRIBUYENTES | 20270286675     | caterno99@gmail.com | 927027827 | Brisas del huallaga comite 7 mz y lt 17, tingo maria |
