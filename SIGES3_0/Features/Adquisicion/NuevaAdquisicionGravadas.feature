@NuevaAdquisicionGravada
Feature: Nueva Adquisicion - Gravadas

@RegistroAdquisicionExitosa-GravadaG
Scenario: Registro exitoso con Gravada G
    Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    And Navego al módulo de 'Adquisición'
    And Entro al submódulo específico de 'Nueva Adquisición'
    
    When Se configuran los datos de 'Facturación':
    | Campo                 | Valor                         |
    | Documento             | BOLETA DE VENTA ELECTRONICA   |
    | Serie                 | B002                          |
    | Correlativo           | 00000011                      |
    | Fecha de emisión      | 04/03/2026                    |
    | Proveedor             | 10759012017                   |
    | Información Adicional | Boleta Exitosa                |

    And Se selecciona el tipo de entrega 'Diferida'
    And Se configuran los datos de 'Entrega':
    | Campo           | Valor                    |
    | Rol             | Item Comercial           |
    | Establecimiento | RECSA - CENTRAL          |
    | Almacén         | CENTRO COMERCIAL CENTRAL |
    
    # Especificamos 'G' para que el Step sepa qué botón clickear
    And Se selecciona el tipo de compra 'G'

    And Se selecciona y configura el producto a adquirir:
    | Producto                    | Cantidad   | V. U |
    | 7751234001115\|Azúcar Rubia |  701       | 6.9  |
    
    Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
    And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

@RegistroAdquisicionExitosa-GravadaNG
Scenario: Registro exitoso con Gravada con NG
    Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    And Navego al módulo de 'Adquisición'
    And Entro al submódulo específico de 'Nueva Adquisición'
    
    When Se configuran los datos de 'Facturación':
    | Campo                 | Valor                         |
    | Documento             | BOLETA DE VENTA ELECTRONICA   |
    | Serie                 | B003                          |
    | Correlativo           | 00000012                      |
    | Fecha de emisión      | 04/03/2026                    |
    | Proveedor             | 10759012017                   |
    | Información Adicional | Boleta Exitosa                |

    And Se selecciona el tipo de entrega 'Diferida'
    And Se configuran los datos de 'Entrega':
    | Campo           | Valor                    |
    | Rol             | Item Comercial           |
    | Establecimiento | RECSA - CENTRAL          |
    | Almacén         | CENTRO COMERCIAL CENTRAL |
    
    # Especificamos 'NG' para que el Step sepa qué botón clickear
    And Se selecciona el tipo de compra 'NG'

    And Se selecciona y configura el producto a adquirir:
    | Producto                    | Cantidad   | V. U |
    | 7751234001115\|Azúcar Rubia |  750       | 6.9  |
    
    Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
    And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

@RegistroAdquisicionExitosa-GravadaGyNG
Scenario: Registro exitoso con Gravada G y NG
    Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    And Navego al módulo de 'Adquisición'
    And Entro al submódulo específico de 'Nueva Adquisición'
    
    When Se configuran los datos de 'Facturación':
    | Campo                 | Valor                         |
    | Documento             | BOLETA DE VENTA ELECTRONICA   |
    | Serie                 | B004                          |
    | Correlativo           | 00000013                      |
    | Fecha de emisión      | 04/03/2026                    |
    | Proveedor             | 10759012017                   |
    | Información Adicional | Boleta Exitosa                |

    And Se selecciona el tipo de entrega 'Diferida'
    And Se configuran los datos de 'Entrega':
    | Campo           | Valor                    |
    | Rol             | Item Comercial           |
    | Establecimiento | RECSA - CENTRAL          |
    | Almacén         | CENTRO COMERCIAL CENTRAL |
    
    # Especificamos 'G y NG' para que el Step sepa qué botón clickear
    And Se selecciona el tipo de compra 'G y NG'

    And Se selecciona y configura el producto a adquirir:
    | Producto                    | Cantidad   | V. U |
    | 7751234001115\|Azúcar Rubia |  800       | 6.9  |
    
    Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
    And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'