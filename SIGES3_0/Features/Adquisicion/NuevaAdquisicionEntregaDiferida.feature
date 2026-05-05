@NuevaAdquisicion
Feature: Nueva Adquicision - Tipo de Entrega Diferido

@RegistroAdquisicionExitosa-EntregaDiferida
Scenario: Registro exitoso con Entrega Diferida
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor               |
	| Documento             | FACTURA ELECTRONICA |
	| Serie                 | F002                |
	| Correlativo           | 00000011            |
	| Fecha de emisión      | 04/03/2026          |
	| Proveedor             | 10759012017         |
	| Información Adicional | Factura Exitosa     |

	And Se selecciona el tipo de entrega 'Diferida'
	And Se configuran los datos de 'Entrega':
	| Campo           | Valor                    |
	| Rol             | Item Comercial           |
	| Establecimiento | RECSA - CENTRAL          |
	| Almacén         | CENTRO COMERCIAL CENTRAL |

	And Se selecciona y configura el producto a adquirir:
	| Producto                    | Cantidad  | V. U |
	| 7751234001115\|Azúcar Rubia |  13       | 6.9  |
	
	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro exitoso con Entrega Diferida con Varios Almacen 
    Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    And Navego al módulo de 'Adquisición'
    And Entro al submódulo específico de 'Nueva Adquisición'
    
    When Se configuran los datos de 'Facturación':
    | Campo                 | Valor               |
    | Documento             | FACTURA ELECTRONICA |
    | Serie                 | F003                |
    | Correlativo           | 00000012            |
    | Fecha de emisión      | 04/03/2026          |
    | Proveedor             | 10759012017         |
    | Información Adicional | Factura Exitosa     |

    # 1. Cambiamos el nombre para que el código detecte 'Diferida' y 'Varios'
    And Se selecciona el tipo de entrega 'Diferida'
    And Se activa la opción de 'Varios'
    # 3. ¡Lo más importante! Agregamos las columnas de Rol y Almacén aquí
    And Se selecciona y configura el producto a adquirir:
    | Producto                    | Rol            | Almacén                  | Cantidad | V. U |
    | 7751234001115\|Azúcar Rubia | Item Comercial | CENTRO COMERCIAL CENTRAL | 13       | 6.9  |
    | 7751234001122\|Azúcar Blanca| Item Comercial | ALMACEN CENTRAL          | 17       | 6.7  |
    
    Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
    And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'