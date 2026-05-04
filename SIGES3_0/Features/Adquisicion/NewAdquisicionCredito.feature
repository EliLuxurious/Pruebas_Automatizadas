@NuevaAdquisicion
Feature: Nueva Adquisicion - Pago Crédito

@RegistroAdquisicionPagoCredito
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
	| Cuotas            | 5        |
	| Frecuencia (Días) | 30       |
	| Método			| Efectivo |
	| Observación		| NINGUNO  |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro Adquisicion Pago Crédito Efectivo
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
	| Monto Inicial     | 150      |
	| Cuotas            | 5        |
	| Frecuencia (Días) | 30       |
	| Método			| Efectivo |
	| Observación		| NINGUNO  |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro Adquisicion Pago Crédito BilleteraDigital
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
	| 7751234001115\|Azúcar Rubia | 150        | 6.9  |
	| 7751234001122\|Azúcar Blanca| 270        | 6.7  |
	
	And Se configuran los datos de 'Pago':
	| Campo             | Valor    |
	| Tipo              | Crédito  |
	| Monto Inicial     | 750      |
	| Cuotas            | 5        |
	| Frecuencia (Días) | 45       |
	| Método			| Billetera digital |
	| Billetera			| Yape              | 
	| Código			| 123456789         |
	| Observación		| NINGUNO           |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro Adquisicion Pago Crédito Tarjeta de Credito
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
	| 7751234001115\|Azúcar Rubia | 150        | 6.9  |
	| 7751234001122\|Azúcar Blanca| 270        | 6.7  |
	
	And Se configuran los datos de 'Pago':
	| Campo             | Valor    |
	| Tipo              | Crédito  |
	| Monto Inicial     | 750      |
	| Cuotas            | 5        |
	| Frecuencia (Días) | 45       |
    | Método			| Tarjeta de credito | 
	| Tarjeta			| VISA               |
    | Información		| NINGUNO            |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro Adquisicion Pago Crédito Tarjeta de Debito
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
	| 7751234001115\|Azúcar Rubia | 150        | 6.9  |
	| 7751234001122\|Azúcar Blanca| 270        | 6.7  |
	
	And Se configuran los datos de 'Pago':
	| Campo             | Valor			|
	| Tipo              | Crédito		|
	| Monto Inicial     | 750			|
	| Cuotas            | 5				|
	| Frecuencia (Días) | 45			|
	| Método			| Tarjeta de DǸbito  |
	| Tarjeta			| DINERS CLUB        |
	| Información		| NINGUNO            |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro Adquisicion Pago Crédito Transferencia en Cuenta
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
	| 7751234001115\|Azúcar Rubia | 150        | 6.9  |
	| 7751234001122\|Azúcar Blanca| 270        | 6.7  |
	
	And Se configuran los datos de 'Pago':
	| Campo             | Valor			|
	| Tipo              | Crédito		|
	| Monto Inicial     | 750			|
	| Cuotas            | 5				|
	| Frecuencia (Días) | 45			|
	| Método						|Transferecia en Cuenta|
	| Cuenta Bancaria Propia		| TSOL123456789		   |
	| Cuenta bancaria del proveedor | 1110987654321        |
	| Información					| NINGUNO              |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro Adquisicion Pago Crédito Deposito en Cuenta
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
	| 7751234001115\|Azúcar Rubia | 150        | 6.9  |
	| 7751234001122\|Azúcar Blanca| 270        | 6.7  |
	
	And Se configuran los datos de 'Pago':
	| Campo             | Valor			|
	| Tipo              | Crédito		|
	| Monto Inicial     | 750			|
	| Cuotas            | 5				|
	| Frecuencia (Días) | 45			|
	| Método								  | Deposito en Cuenta   |
	| Caja									  | TSOL123456789		 |
	| Número de cuenta bancaria del proveedor | 1110987654321        |
	| Información							  | NINGUNO              |
	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'