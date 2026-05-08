@VerAdquisicionNuevaAdquisicion
Feature: Ver Adquisicion Nueva Adquisicion
Scenario: Registrar nueva adquisición desde el modal en Ver Adquisición
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Ver Adquisición'
	When Hago clic en el botón superior de 'Nueva Compra'
	
	And Se configuran los datos de 'Facturación':
	| Campo                 | Valor                      |
	| Documento             | BOLETA DE VENTA ELECTRONICA|
	| Serie                 | B001                       |
	| Correlativo           | 00000010                   |
	| Fecha de emisión      | 04/03/2026                 |
	| Proveedor             | 10759012017                |
	| Información Adicional | Compra desde Modal         |

	And Se selecciona el tipo de entrega 'Inmediata'
	
	And Se configuran los datos de 'Entrega':
	| Campo           | Valor                    |
	| Rol             | Item Comercial           |
	| Establecimiento | RECSA - CENTRAL          |
	| Almacén         | CENTRO COMERCIAL CENTRAL |

	And Se selecciona y configura el producto a adquirir:
	| Producto                    | Cantidad | V. U |
	| 7751234001115\|Azúcar Rubia | 100      | 6.9  |
	
	And Se configuran los datos de 'Pago':
	| Campo       | Valor    |
	| Tipo        | Contado  |
	| Método      | Efectivo |
	| Observación | NINGUNO  |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'