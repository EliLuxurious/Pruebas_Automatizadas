@NuevaAdquisicion
Feature: Nueva Adquisicion - Pago Crédito

@RegistroAdquisicionPagoCreditoMontoCero
Scenario: Registro Adquisicion Pago Crédito con Monto Inicial Cero
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                | Valor                    |
	| Documento            | NOTA DE COMPRA (INTERNA) |
	| Proveedor            | 10759012017              |
	| Información Adicional | Nota Crédito Exitosa     |

	And Se selecciona el tipo de entrega 'Inmediata'
	And Se configuran los datos de 'Entrega':
	| Campo           | Valor                    |
	| Rol             | Item Comercial           |
	| Establecimiento | RECSA - CENTRAL          |
	| Almacén         | CENTRO COMERCIAL CENTRAL |

	And Se selecciona y configura el producto a adquirir:
	| Producto                    | Cantidad   | V. U |
	| 7751234001115\|Azúcar Rubia |  150       | 6.9  |
	
	And Se configuran los datos de 'Pago':
	| Campo             | Valor    |
	| Tipo              | Crédito  |
	| Monto Inicial     | 0        |
	| Cuotas            | 4        |
	| Frecuencia (Días) | 30       |
	| Método			| Efectivo |
	| Observación		| NINGUNO  |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'