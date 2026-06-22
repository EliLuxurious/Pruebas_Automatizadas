@NuevaAdquisicionDescuento
Feature: Nueva Adquisicion - Descuento

@RegistroAdquisicion-DescuentoItem
Scenario: Registro exitoso con Descuento por Item
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
    When Se configuran los datos de 'Facturación':
    |  Campo                | Valor                         |
    | Documento             | BOLETA DE VENTA ELECTRONICA   |
    | Serie                 | B005                          |
    | Correlativo           | 00000014                      |
    | Fecha de emisión      | 04/03/2026                    |
    | Proveedor             | 75901201                      |
    | Información Adicional | Boleta Exitosa                |

	And Se selecciona el tipo de entrega 'Diferida'
	And Se configuran los datos de 'Entrega':
	| Campo           | Valor                    |
	| Rol             | Item Comercial           |
	| Establecimiento | RECSA - CENTRAL          |
	| Almacén         | CENTRO COMERCIAL CENTRAL |
	
	And Se selecciona y configura el producto con descuento por item:
	| Producto                    | Cantidad | V. U | Descuento |
	| 7751234001115\|Azúcar Rubia | 13       | 6.9  | 11.5       | 
    | 7751234001122\|Azúcar Blanca| 17       | 6.7  | 10.5       |
	
	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'
 
 @RegistroAdquisicion-DescuentoGlobal
  Scenario: Registro exitoso con Descuento Global
    Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    And Navego al módulo de 'Adquisición'
    And Entro al submódulo específico de 'Nueva Adquisición'
    
    When Se configuran los datos de 'Facturación':
    |  Campo                | Valor                         |
    | Documento             | BOLETA DE VENTA ELECTRONICA   |
    | Serie                 | B005                          |
    | Correlativo           | 00000014                      |
    | Fecha de emisión      | 04/03/2026                    |
    | Proveedor             | 75901201                      |
    | Información Adicional | Boleta Exitosa                |

   And Se selecciona el tipo de entrega 'Diferida'
	And Se configuran los datos de 'Entrega':
	| Campo           | Valor                    |
	| Rol             | Item Comercial           |
	| Establecimiento | RECSA - CENTRAL          |
	| Almacén         | CENTRO COMERCIAL CENTRAL |
    And Se selecciona y configura el producto a adquirir:
    | Producto                    |  Cantidad | V. U |
    | 7751234001115\|Azúcar Rubia |  13       | 6.9  |
    | 7751234001122\|Azúcar Blanca|  17       | 6.7  |

	And Se habilita la sección de descuento:
    | Descuento                   |
    | 110                         |
	
	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'