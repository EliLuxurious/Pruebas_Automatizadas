Feature: Configuración de medios de pago

  Como usuario del sistema
  Quiero configurar los medios de pago
  Para validar las reglas de negocio del bloque de pago en cualquier módulo

  @MediosPago @Reutilizable
  Scenario Outline: Validar configuración de medios de pago

	Given existe un pedido listo para confirmar
	When el usuario selecciona tipo de pago '<Tipo_pago>'
	And el usuario marca el check de multipago '<Check_Multipago>'
	And el usuario selecciona medio de pago '<Medios_pago>'
	And el usuario configura cuotas '<Cuotas>'
	And el usuario selecciona banco '<Tipo_de_banco>'
	And el usuario selecciona tarjeta '<Tipo_de_tarjeta>'
	And el usuario selecciona cuenta bancaria '<Cuenta_bancaria>'
# numero de operacion se ingresa en tarjeta de debito, tarjeta de credito, transferencia de fondos y deposito en cuenta
	And el usuario ingresa numero de operacion '<Numero_operacion>'
	And el usuario identifica cliente '<cliente>'
	And el usuario tiene puntos '<puntos_suficientes>'
	And el usuario tiene credito '<nota_de_credito_suficiente>'
	And el usuario ingresa monto '<monto_cubre_total>'
	And el usuario confirma el pedido
	Then el sistema valida el resultado del pedido '<resultado_esperado>'
	

Examples:
	| caso | Tipo_pago | Check_Multipago | Medios_pago                                      | Cuotas | Tipo_de_banco | Tipo_de_tarjeta | Cuenta_bancaria | Numero_operacion       | cliente  | puntos_suficientes | nota_de_credito_suficiente | monto_cubre_total | resultado_esperado                                                   |
	|    1 | contado   | false           | efectivo                                         | NA     | NA            | NA              | NA              | NA                     | 00000000 | NA                 | NA                         | true              | Pago Exitoso                                                |
	|    2 | contado   | false           | efectivo                                         | NA     | NA            | NA              | NA              | NA                     | 00000000 | NA                 | NA                         | false             | Monto Insuficiente                                          |
	|    3 | contado   | false           | tarjeta de debito                                | NA     | INTERVANK     | VISA            | NA              |                 458962 | 00000000 | NA                 | NA                         | true              | Pago Exitoso                                                |
	|    4 | contado   | false           | tarjeta de debito                                | NA     | ninguno       | ninguno         | NA              | ninguno                | 00000000 | NA                 | NA                         | false             | seleccione entidad bancaria: "Este campo es obligatorio"    |
	|    5 | contado   | false           | transferencia de fondo                           | NA     | NA            | NA              | BCP             |                 458962 | 00000000 | NA                 | NA                         | true              | Pago Exitoso                                                |
	|    6 | contado   | false           | deposito en cuenta                               | NA     | NA            | NA              | ninguno         | ninguno                | 00000000 | NA                 | NA                         | false             | Seleccione una cuenta bancaria "Este campos es obligatorio" |
	|    7 | contado   | false           | puntos                                           | NA     | NA            | NA              | NA              | NA                     | 75971755 | true               | NA                         | true              | Pago Exitoso                                                |
	|    8 | contado   | false           | puntos                                           | NA     | NA            | NA              | NA              | NA                     | 75971751 | false              | NA                         | false             | Puntos insuficiente                                         |
	|    9 | contado   | false           | puntos                                           | NA     | NA            | NA              | NA              | NA                     | 00000000 | NA                 | NA                         | NA                | Para el pago con puntos debe identificar al cliente         |
	|   10 | contado   | false           | nota de credito                                  | NA     | NA            | NA              | NA              | NA                     | 75971755 | NA                 | true                       | true              | Pago exitoso                                                |
	|   11 | contado   | false           | nota de credito                                  | NA     | NA            | NA              | NA              | NA                     | 75971755 | NA                 | false                      | false             | Nota de credito insuficiente                                |
	|   12 | contado   | false           | nota de credito                                  | NA     | NA            | NA              | NA              | NA                     | 00000000 | NA                 | NA                         | false             | Completar los datos requeridos                              |
	|   13 | contado   | true            | efectivo, tarjeta de credito, deposito en cuenta | NA     | YAPE          | VISA            | BCP             | 31004542, 000744226861 | 00000000 | NA                 | NA                         | true              | Pago Exitoso                                                |
	|   14 | contado   | true            | transferencia de fondos, puntos, nota de credito | NA     | SI            | SI              | BCP             |              000204202 | 75971751 | true               | true                       | true              | Pago Exitoso                                                |
	|   15 | contado   | true            | puntos, nota de credito                          | NA     | NA            | NA              | NA              | SI                     | 00000000 | NA                 | NA                         | false             | Identifique al cliente                                      |
	|   16 | credito   | false           | efectivo                                         |      2 | NA            | NA              | NA              | NA                     | 75971755 | NA                 | SI                         | NA                | Pago Exitoso                                                |
	|   17 | credito   | false           | efectivo                                         |      3 | NA            | NA              | NA              | NA                     | 00000000 | NA                 | NO                         | NA                | Para dar a credito debe identificar al cliente              |
	|   18 | credito   | true            | tarjeta de credito, puntos                       |      4 | INTERBANK     | VISA            | NA              |                0030281 | 75971755 | NA                 | NO                         | OK                | Pago Exitoso                                                |
