@NuevaAdquisicionMultiMedioPago
Feature: NuevaAdquisicionMultiMedioPagos
@RegistroAdquisicionMultipagoAlContado
Scenario: Registro Adquisicion CONTADO Multipago Efectivo - Tarjeta de Crédito
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor                       |
	| Documento             | BOLETA DE VENTA ELECTRONICA |
	| Serie                 | B002                        |
	| Correlativo           | 00000020                    |
	| Fecha de emisión      | 17/04/2026                  |
	| Proveedor             | 10759012017                 |
	| Información Adicional | Compra Multipago con Efectivo y Tarjeta de Credito      |

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
	| Campo | Valor   |
	| Tipo  | Contado |

	And Se activa la opción de 'Multipago'
	
	And Se agregan los siguientes medios de pago fraccionados:
	| Método             | Monto  | Observación			  | Tarjeta |
	| Efectivo           | 314.20 | Pago parcial efectivo |         |
	| Tarjeta de crédito | 500.00 | Saldo completo		  | VISA	|

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro Adquisicion CONTADO Multipago Tarjeta Debito - Crédito
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor                       |
	| Documento             | BOLETA DE VENTA ELECTRONICA |
	| Serie                 | B002                        |
	| Correlativo           | 00000020                    |
	| Fecha de emisión      | 17/04/2026                  |
	| Proveedor             | 10759012017                 |
	| Información Adicional | Compra Multipago Con Tarjeta de Credito y Debito      |

	And Se selecciona el tipo de entrega 'Inmediata'
	And Se configuran los datos de 'Entrega':
	| Campo           | Valor                    |
	| Rol             | Item Comercial           |
	| Establecimiento | RECSA - CENTRAL          |
	| Almacén         | CENTRO COMERCIAL CENTRAL |

	And Se selecciona y configura el producto a adquirir:
	| Producto                    | Cantidad | V. U |
	| 7751234001115\|Azúcar Rubia | 100      | 6.9  |
	| 7751234001122\|Azúcar Blanca| 200      | 6.7  |
	
	And Se configuran los datos de 'Pago':
	| Campo | Valor   |
	| Tipo  | Contado |

	And Se activa la opción de 'Multipago'
	
	And Se agregan los siguientes medios de pago fraccionados:
	| Método             | Monto   | Observación       | Tarjeta		|
	| Tarjeta de crédito | 1500.00 | Saldo restante    | VISA			|
	| Tarjeta de debito  | 895.40  | Saldo completo    | MASTER CARD	|

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

@RegistroAdquisicionMultipagoCredito
Scenario: Registro Adquisicion CREDITO Multipago Efectivo - Tarjeta de Crédito
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor                       |
	| Documento             | BOLETA DE VENTA ELECTRONICA |
	| Serie                 | B002                        |
	| Correlativo           | 00000020                    |
	| Fecha de emisión      | 17/04/2026                  |
	| Proveedor             | 10759012017                 |
	| Información Adicional | Compra Multipago con Efectivo y Tarjeta de Credito      |

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
	| Campo				| Valor    |
	| Tipo              | Crédito  |
	| Monto Inicial     | 250      |
	| Cuotas            | 6        |
	| Frecuencia (Días) | 60       |

	And Se activa la opción de 'Multipago'
	
	And Se agregan los siguientes medios de pago fraccionados:
	| Método             | Monto  | Observación			  | Tarjeta |
	| Efectivo           | 150.00 | Pago parcial efectivo |         |
	| Tarjeta de crédito | 100.00 | Saldo completo		  | VISA	|

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro Adquisicion CREDITO Multipago Tarjeta Debito - Crédito
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor                       |
	| Documento             | BOLETA DE VENTA ELECTRONICA |
	| Serie                 | B002                        |
	| Correlativo           | 00000020                    |
	| Fecha de emisión      | 17/04/2026                  |
	| Proveedor             | 10759012017                 |
	| Información Adicional | Compra Multipago Con Tarjeta de Credito y Debito      |

	And Se selecciona el tipo de entrega 'Inmediata'
	And Se configuran los datos de 'Entrega':
	| Campo           | Valor                    |
	| Rol             | Item Comercial           |
	| Establecimiento | RECSA - CENTRAL          |
	| Almacén         | CENTRO COMERCIAL CENTRAL |

	And Se selecciona y configura el producto a adquirir:
	| Producto                    | Cantidad | V. U |
	| 7751234001115\|Azúcar Rubia | 100      | 6.9  |
	| 7751234001122\|Azúcar Blanca| 200      | 6.7  |
	
	And Se configuran los datos de 'Pago':
	| Campo				| Valor    |
	| Tipo              | Crédito  |
	| Monto Inicial     | 950      |
	| Cuotas            | 12       |
	| Frecuencia (Días) | 15       |

	And Se activa la opción de 'Multipago'
	
	And Se agregan los siguientes medios de pago fraccionados:
	| Método             | Monto   | Observación       | Tarjeta		|
	| Tarjeta de crédito | 500.00  | Saldo restante    | VISA			|
	| Tarjeta de debito  | 450.00  | Saldo completo    | MASTER CARD	|

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'