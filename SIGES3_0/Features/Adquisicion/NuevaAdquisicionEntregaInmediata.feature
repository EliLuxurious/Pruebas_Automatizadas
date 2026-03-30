@NuevaAdquisicion
Feature: Nueva Adquicision - Tipo de Entrega Imnmediata

@RegistroAdquisicionExitosa-EntregaInmediata
Scenario: Registro exitoso con Entrega Imnmediata
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor               |
	| Documento             | FACTURA ELECTRONICA |
	| Serie                 | F005                |
	| Correlativo           | 00000014            |
	| Fecha de emisión      | 04/03/2026          |
	| Proveedor             | 10759012017         |
	| Información Adicional | Factura Exitosa     |

	And Se selecciona el tipo de entrega 'Imnmediata'
	And Se configuran los datos de 'Entrega':
	| Campo           | Valor                    |
	| Rol             | Item Comercial           |
	| Establecimiento | RECSA - CENTRAL          |
	| Almacén         | ALMACEN CENTRAL          |

	And Se selecciona y configura el producto a adquirir:
	| Producto                    | Cantidad  | V. U |
	| 7751234001115\|Azúcar Rubia |  16       | 6.59  |
	
	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro exitoso con Entrega Inmediata con Varios Almacenes 
    Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    And Navego al módulo de 'Adquisición'
    And Entro al submódulo específico de 'Nueva Adquisición'
    
    When Se configuran los datos de 'Facturación':
    | Campo                 | Valor               |
    | Documento             | FACTURA ELECTRONICA |
    | Serie                 | F004                |
    | Correlativo           | 00000013            |
    | Fecha de emisión      | 09/03/2026          |
    | Proveedor             | 10759012017         |
    | Información Adicional | Prueba Varios Inme  |

    # 1. Activamos el modo 'Varios' en la sección de Entrega
    And Se selecciona el tipo de entrega 'Inmediata'
    And Se activa la opción de 'Varios'
    # 3. Definimos los 2 productos con sus respectivos almacenes en la tabla
    And Se selecciona y configura el producto a adquirir:
    | Producto                    | Rol            | Almacén                  | Cantidad | V. U |
    | 7751234001115\|Azúcar Rubia | Item Comercial | CENTRO COMERCIAL CENTRAL | 19       | 6.9  |
    | 7751234001122\|Azúcar Blanca| Item Comercial | ALMACEN CENTRAL          | 17       | 6.7  |
    
    Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
    And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'