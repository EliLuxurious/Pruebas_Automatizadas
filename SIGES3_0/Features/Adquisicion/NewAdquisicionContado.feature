@NuevaAdquisicion
Feature: Nueva Adquicision - Pago Contado
@RegistroAdquisicionPagoContado
Scenario: Registro exitoso con Pago Contado Efectivo
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor                         |
	| Documento             | BOLETA DE VENTA ELECTRONICA   |
	| Serie                 | B200                          |
	| Correlativo           | 00000010                      |
	| Fecha de emisión      | 04/03/2026                    |
	| Proveedor             | 10759012017                   |
	| Información Adicional | Boleta Exitosa                |

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
	| Campo       | Valor    |
	| Tipo        | Contado  |
	| Método      | Efectivo |
	| Observación | NINGUNO  |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro Adquisicion Pago Contado Billetera Digital
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor                         |
	| Documento             | BOLETA DE VENTA ELECTRONICA   |
	| Serie                 | B201                          |
	| Correlativo           | 00000010                      |
	| Fecha de emisión      | 04/03/2026                    |
	| Proveedor             | 10759012017                   |
	| Información Adicional | Boleta Exitosa                |

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
	| Campo       | Valor             |
	| Tipo        | Contado           |
	| Método      | Billetera digital |
	| Billetera   | Yape              | 
	| Código      | 123456789         |
	| Observación | NINGUNO           |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro Adquisicion Pago Contado Tarjeta de Credito
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor                         |
	| Documento             | BOLETA DE VENTA ELECTRONICA   |
	| Serie                 | B202                          |
	| Correlativo           | 00000010                      |
	| Fecha de emisión      | 04/03/2026                    |
	| Proveedor             | 10759012017                   |
	| Información Adicional | Boleta Exitosa                |

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
    | Campo       | Valor              |
    | Tipo        | Contado            |
    | Método      | Tarjeta de credito | 
	| Tarjeta     | VISA               |
    | Información | NINGUNO            |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro Adquisicion Pago Contado Tarjeta de Debito
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor                         |
	| Documento             | BOLETA DE VENTA ELECTRONICA   |
	| Serie                 | B203                          |
	| Correlativo           | 00000010                      |
	| Fecha de emisión      | 04/03/2026                    |
	| Proveedor             | 10759012017                   |
	| Información Adicional | Boleta Exitosa                |

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
	| Campo       | Valor              |
	| Tipo        | Contado            |
	| Método      | Tarjeta de DǸbito  |
	| Tarjeta     | DINERS CLUB        |
	| Información | NINGUNO            |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro Adquisicion Pago Contado Transferencia en Cuenta
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor                         |
	| Documento             | BOLETA DE VENTA ELECTRONICA   |
	| Serie                 | B204                          |
	| Correlativo           | 00000010                      |
	| Fecha de emisión      | 04/03/2026                    |
	| Proveedor             | 10759012017                   |
	| Información Adicional | Boleta Exitosa                |

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
	| Campo							| Valor				   |
	| Tipo							| Contado			   |
	| Método						|Transferecia en Cuenta|
	| Cuenta Bancaria Propia		| TSOL123456789		   |
	| Cuenta bancaria del proveedor | 1110987654321        |
	| Información					| NINGUNO              |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

Scenario: Registro Adquisicion Pago Contado Deposito en Cuenta
	Given Inicio de sesión en el módulo de Adquisición con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Navego al módulo de 'Adquisición'
	And Entro al submódulo específico de 'Nueva Adquisición'
	
	When Se configuran los datos de 'Facturación':
	| Campo                 | Valor                         |
	| Documento             | BOLETA DE VENTA ELECTRONICA   |
	| Serie                 | B205                          |
	| Correlativo           | 00000010                      |
	| Fecha de emisión      | 04/03/2026                    |
	| Proveedor             | 10759012017                   |
	| Información Adicional | Boleta Exitosa                |

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
	| Campo									  | Valor				 |
	| Tipo									  | Contado				 |
	| Método								  | Deposito en Cuenta   |
	| Caja									  | TSOL123456789		 |
	| Número de cuenta bancaria del proveedor | 1110987654321        |
	| Información							  | NINGUNO              |

	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'