@NuevaAdquisicionB_ConNuevoConcepto
Feature: NuevaAdquisicionNewConcepto

Scenario: Regsitro exitoso de Nueva Adquisicion con Nuevo Concepto
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'

	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor                         |
	| Documento             | BOLETA DE VENTA ELECTRONICA   |
	| Serie                 | B001                          |
	| Correlativo           | 00000010                      |
	| Fecha de emisión      | 04/03/2026                    |
	| Proveedor             | 10759012017                   |
	| Información Adicional | Compra con concepto nuevo     |

	And Se selecciona el tipo de entrega 'Inmediata'
	And Se configuran los datos de 'Entrega':
	| Campo           | Valor                    |
	| Rol             | Item Comercial           |
	| Establecimiento | RECSA - CENTRAL          |
	| Almacén         | CENTRO COMERCIAL CENTRAL |

	And Se abre el modal de 'Nuevo Concepto' en la sección de productos
	And Se registran los datos del nuevo concepto en el modal:
	| familia | codigo  | sufijo         | marca      | presentacion | tarifa     | precio | 
	| HISOPO  | INS0111 | PARA USO MEDICO| TextilPeru | SP           | POR UNIDAD | 10     |


	And Se guarda el concepto desde el modal

	And Se selecciona y configura el producto a adquirir:
  | Producto | Cantidad | V. U |
  | INS0111  | 100      | 6.9  |
	
	And Se configuran los datos de 'Pago':
	| Campo       | Valor    |
	| Tipo        | Contado  |
	| Método      | Efectivo |
	| Observación | NINGUNO  |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'