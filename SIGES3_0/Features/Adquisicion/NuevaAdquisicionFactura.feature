@NuevaAdquisicion
Feature: Nueva Adquicision - Tipo de Documento Factura

@RegistroAdquisicionExitosa-FACTURA
Scenario: Registro exitoso con factura electrónica
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor               |
	| Documento             | FACTURA ELECTRONICA |
	| Serie                 | F001                |
	| Correlativo           | 00000010            |
	| Fecha de emisión      | 04/03/2026          |
	| Proveedor             | 10759012017         |
	| Información Adicional | Factura Exitosa     |

	And Se selecciona el tipo de entrega 'Inmediata'
	And Se configuran los datos de 'Entrega':
	| Campo           | Valor                    |
	| Rol             | Item Comercial           |
	| Establecimiento | RECSA - CENTRAL          |
	| Almacén         | CENTRO COMERCIAL CENTRAL |

	And Se selecciona y configura el producto a adquirir:
	| Producto                    | Cantidad  | V. U |
	| 7751234001115\|Azúcar Rubia |  130      | 6.9  |
	
	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'
	
@RegistroAdquisicionExitosa-FACTURA
Scenario: Validar que el sistema exija RUC al elegir Factura Electrónica
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor               |
	| Documento             | FACTURA ELECTRONICA |
	| Serie                 | F002                |
	| Correlativo           | 00000011            |
	| Fecha de emisión      | 04/03/2026          |
	| Proveedor             | 75901201            |
	| Información Adicional | Factura con DNI     |

	And Se selecciona el tipo de entrega 'Inmediata'
	And Se configuran los datos de 'Entrega':
	| Campo           | Valor                    |
	| Rol             | Item Comercial           |
	| Establecimiento | RECSA - CENTRAL          |
	| Almacén         | CENTRO COMERCIAL CENTRAL |

	And Se selecciona y configura el producto a adquirir:
	| Producto                    | Cantidad  | V. U |
	| 7751234001115\|Azúcar Rubia |  13       | 6.9  |
	
	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And El sistema debe mostrar la alerta de validacion 'El proveedor debe tener un RUC válido'