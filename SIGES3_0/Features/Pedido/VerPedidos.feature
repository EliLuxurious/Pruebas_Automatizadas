@Pedido
Feature: VerPedidos

Como usuario del sistema
Quiero registrar pedidos
Para gestionar pedidos de clientes

Background:
	Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
	#And el usuario accede al módulo 'Pedidos'
	#And el usuario accede al submodulo 'Ver Pedidos'
	


@RegistrarPedido
Scenario Outline: Registro de nuevo pedido - Casos variados

#	Given Navego al módulo de 'Adquisición'
#	And Entro al submódulo específico de 'Nueva Adquisición'
#	
#	When Se configuran los datos de 'Facturación':
#	| Campo                 | Valor               |
#	| Documento             | FACTURA ELECTRONICA |
#	| Serie                 | F001                |
#	| Correlativo           | 00000010            |
#	| Fecha de emisión      | 04/03/2026          |
#	| Proveedor             | 10759012017         |
#	| Información Adicional | Factura Exitosa     |
#
#	And Se selecciona el tipo de entrega 'Inmediata'
#	And Se configuran los datos de 'Entrega':
#	| Campo           | Valor                    |
#	| Rol             | Item Comercial           |
#	| Establecimiento | RECSA - CENTRAL          |
#	| Almacén         | CENTRO COMERCIAL CENTRAL |
#
#	And Se selecciona y configura el producto a adquirir:
#	| Producto										| Cantidad  | V. U |
#	| 7753234003320\|Coca-Cola Gaseosa Botella 1.5L |  130      | 6.9  |
#	| 7753234003313\|Inca Kola Gaseosa Botella 1.5L |  130      | 7.1  |
#	| 7751234001115\|Azúcar Rubia				    |  30       | 3.2  |
#
#	
#	Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
#	And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

	When el usuario accede al módulo 'Pedidos'
	And el usuario accede al submodulo 'Ver Pedidos'
	And el usuario selecciona la opción 'Nuevo Pedido'
	And el usuario prepara producto para pedido con familia '<familia>' concepto '<concepto>' cantidad '<cantidad>' resultado '<resultado_esperado>'
	And el usuario activa IGV '<igv>'
	And el usuario activa DET.UNIF '<det_unif>'
	And el usuario configura descuento '<descuento>' '<tipo_descuento>' '<modo_descuento>' '<valor_descuento>'
	And el usuario abre la sección 'Facturación'
	And el usuario busca el cliente '<cliente>'
	And el usuario abre la sección 'Entrega'
	And el usuario selecciona tipo de entrega '<tipo_entrega>'
	And el usuario registra el pedido
	Then el sistema valida el resultado del pedido '<resultado_esperado>'

Examples:
	| caso | familia | concepto      | cantidad | igv   | det_unif | descuento | tipo_descuento | modo_descuento | valor_descuento | cliente     | tipo_entrega | resultado_esperado                |
	|    1 | Gaseosa | 7753234003320 |      100 | false | false    | false     | NA             | NA             |               0 |    00000000 | inmediata    | el pedido se guardo correctamente |
	|    2 | ninguno | ninguno       |        0 | false | false    | false     | NA             | NA             |               0 |    75971755 | diferida     | Ningún producto seleccionado      |
	|    3 | Gaseosa | 7753234003313 |       12 | true  | true     | true      | item           | $              |               1 |    75971755 | inmediata    | el pedido se guardo correctamente |
	|    4 | Azúcar  | 7751234001115 |       20 | false | false    | true      | global         | %              |              10 | 20542245671 | diferida     | el pedido se guardo correctamente |
	|    5 | Gaseosa | 7753234003313 |900500000 | false | false    | false     | NA             | NA             |               0 |    75971755 | inmediata    | Cantidad debe ser menor al stock  |

@EditarPedido
Scenario Outline: Ver pedido - editar pedido
	
	When el usuario accede al módulo 'Pedidos'
	And el usuario accede al submodulo 'Ver Pedidos'
	Given existe un pedido en estado registrado para invalidar con familia 'Gaseosa' concepto '7753234003320' cantidad '10' cliente '75971755' entrega 'inmediata'
	When el usuario selecciona la opción 'Editar pedido'
	And el usuario actualiza el pedido con familia '<familia>' concepto '<concepto>' cantidad '<cantidad>' igv '<igv>' detUnif '<detUnif>' descuento '<descuentoActivo>' tipoDescuento '<tipoDescuento>' modoDescuento '<modoDescuento>' valorDescuento '<valorDescuento>' cliente '<cliente>' entrega '<tipoEntrega>'
	And el usuario guarda la edición del pedido
	Then el sistema valida el resultado del pedido '<resultadoEsperado>'

Examples:
	| familia | concepto | cantidad   | igv  | detUnif | descuentoActivo | tipoDescuento | modoDescuento | valorDescuento | cliente | tipoEntrega | resultadoEsperado                |
	| NA      | NA       |         20 | true | NA      | NA              | NA            | NA            | NA             | NA      | NA          | el pedido se edito correctamente |
	| NA      | NA       | sin_cambio | NA   | NA      | NA              | NA            | NA            | NA             | NA      | NA          | Boton deshabilitado              |



@InvalidarPedido
Scenario Outline: Invalidar pedido - Casos variados
	
	When el usuario accede al módulo 'Pedidos'
	And el usuario accede al submodulo 'Ver Pedidos'
	Given existe un pedido en estado registrado para invalidar con familia 'Gaseosa' concepto '7753234003320' cantidad '10' cliente '75971755' entrega 'inmediata'
	When el usuario selecciona la opción 'Invalidar pedido'
	And el usuario ingresa el motivo '<motivo>'
	And el usuario confirma '<accion>'
	Then el sistema valida el resultado del pedido '<resultado_esperado>'

Examples:
	| caso | motivo           | accion | resultado_esperado                  |
	|    1 | Producto agotado | SI     | el pedido se Invalido correctamente |
	|    2 | ninguno          | SI     | Boton SI deshabilitado              |




@ConfirmarPedido
Scenario Outline: Confirmar pedido Comprobantes - Casos variados

	When el usuario accede al módulo 'Pedidos'
	And el usuario accede al submodulo 'Ver Pedidos'
	Given existe un pedido base registrado para confirmar con total mayor a 700 '<total_mayor_700>' familia '<familia_base>' concepto '<concepto_base>' cantidad '<cantidad_base>' cliente '<cliente_base>' entrega '<entrega_base>'
	When el usuario selecciona la opción 'Confirmar pedido'
	And el usuario configura la facturacion '<tipo_comprobante>' '<serie>' '<cliente>'
	And el usuario configura la entrega '<tipo_entrega>' '<guia_remision>'
	And el usuario completa la guia de remision '<guia_remision>' '<fecha_de_inicio_traslado>' '<peso_bruto>' '<cantidad_bultos>' '<tipo_transporte>' '<transportista_ruc>' '<numero_licencia>' '<numero_placa>' '<direccion_origen>' '<detalle_origen>' '<direccion_destino>' '<detalle_destino>'
	And el usuario configura el pago 'efectivo' '<monto_cubre_total>'
	And el usuario confirma el pedido preparado
	Then el sistema valida el resultado del pedido '<resultado_esperado>'

Examples:
	| caso | total_mayor_700 | familia_base | concepto_base | cantidad_base | cliente_base | entrega_base | tipo_comprobante            | serie | cliente     | tipo_entrega | guia_remision | fecha_de_inicio_traslado | peso_bruto | cantidad_bultos | tipo_transporte | transportista_ruc | numero_licencia | numero_placa | direccion_origen           | detalle_origen | direccion_destino | detalle_destino | monto_cubre_total | resultado_esperado                                                                                         |
	|    1 | false           | Azúcar       | 7751234001115 |            10 |  20542245671 | diferida     | factura electronica         | NA    | 20542245671 | inmediata    | true          | Hoy                      |        100 |              10 | Publico         |       20602945589 | NA              | NA           | Huanuco-Leoncio-Rupa Rupa  | av amazonas    | Lima-Lima-Lima    | av brosil       | true              | Pedido confirmado correctamente                                                                            |
	|    2 | false           | Azúcar       | 7751234001115 |            10 |  20542245671 | diferida     | factura electronica         | NA    |    75971755 | inmediata    | false         | NA                       | NA         | NA              | NA              | NA                | NA              | NA           | NA                         | NA             | NA                | NA              | true              | Para emitir Factura Electrónica, el cliente debe tener RUC (11 dígitos)                                    |
	|    3 | false           | Azúcar       | 7751234001115 |            10 |  20542245671 | diferida     | factura electronica         | NA    | 20542245671 | diferida     | false         | NA                       | NA         | NA              | NA              | NA                | NA              | NA           | NA                         | NA             | NA                | NA              | true              | Pedido confirmado correctamente                                                                            |
	|    4 | false           | Azúcar       | 7751234001115 |            10 |  20542245671 | diferida     | factura electronica         | NA    | 20542245671 | inmediata    | false         | NA                       | NA         | NA              | NA              | NA                | NA              | NA           | NA                         | NA             | NA                | NA              | false             | Monto insuficiente                                                                                         |
	|    5 | true            | Gaseosa      | 7753234003320 |           110 |     00000000 | inmediata    | boleta de venta electronica | B004  |    75971635 | diferida     | false         | NA                       | NA         | NA              | NA              | NA                | NA              | NA           | NA                         | NA             | NA                | NA              | true              | Pedido confirmado correctamente                                                                            |
	|    6 | false           | Azúcar       | 7751234001115 |            10 |     00000000 | diferida     | boleta de venta electronica | B002  |    00000000 | inmediata    | true          | NA                       | NA         | NA              | NA              | NA                | NA              | NA           | NA                         | NA             | NA                | NA              | true              | Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI |
	|    7 | true            | Gaseosa      | 7753234003320 |           110 |     00000000 | inmediata    | boleta de venta electronica | B002  |    00000000 | inmediata    | false         | NA                       | NA         | NA              | NA              | NA                | NA              | NA           | NA                         | NA             | NA                | NA              | true              | Es necesario identificar al cliente, el total es mayor a S/.700                                            |
	|    8 | false           | Azúcar       | 7751234001115 |            10 |  20542245671 | diferida     | nota de venta               | NA    |    75971755 | diferida     | false         | NA                       | NA         | NA              | NA              | NA                | NA              | NA           | NA                         | NA             | NA                | NA              | true              | Pedido confirmado correctamente                                                                            |
	|    9 | false           | Azúcar       | 7751234001115 |            10 |  20542245671 | diferida     | nota de venta               | NA    | 20602945589 | inmediata    | false         | NA                       | NA         | NA              | NA              | NA                | NA              | NA           | NA                         | NA             | NA                | NA              | true              | Pedido confirmado correctamente                                                                            |
	|   10 | false           | Azúcar       | 7751234001115 |            10 |  20542245671 | diferida     | nota de venta               | NA    | 20602945589 | inmediata    | false         | NA                       | NA         | NA              | NA              |       20602945589 | NA              | NA           | NA                         | NA             | NA                | NA              | false             | Monto insuficiente                                                                                         |
	|   11 | true            | Gaseosa      | 7753234003320 |           110 |  20542245671 | diferida     | boleta de venta electronica | NA    |    75971635 | inmediata    | true          | ninguno                  |        100 |              10 | Privado         | NA                | M-71310154      | 2770XS       | AREQUIPA - LA UNIÒN - SAYLA| av arequipa    | PIURA             | av la mariana   | true              | Pedido confirmado correctamente                                                                            |

@ConfirmarPedidoMediosDePago
Scenario Outline: Confirmar pedido con medios de pago - Casos variados

	When el usuario accede al módulo 'Pedidos'
	And el usuario accede al submodulo 'Ver Pedidos'
	Given existe un pedido base registrado para confirmar con total mayor a 700 '<total_mayor_700>' familia '<familia_base>' concepto '<concepto_base>' cantidad '<cantidad_base>' cliente '<cliente_base>' entrega '<entrega_base>'
	When el usuario selecciona la opción 'Confirmar pedido'
	And el usuario configura la facturacion '<tipo_comprobante>' '<serie>' '<cliente>'
	And el usuario configura la entrega '<tipo_entrega>' 'false'
	And el usuario completa la guia de remision 'false' 'NA' 'NA' 'NA' 'NA' 'NA' 'NA' 'NA' 'NA' 'NA' 'NA' 'NA'
	And el usuario configura los medios de pago '<tipo_pago>' '<multipago>' '<medio_pago>' '<banco>' '<tarjeta>' '<cuenta_bancaria>' '<nro_operacion>' '<monto_por_medio>' '<nro_cuotas>' '<monto_inicial_credito>'
	And el usuario confirma el pedido preparado
	Then el sistema valida el resultado del pedido '<resultado_esperado>'

Examples:
	| caso | total_mayor_700 | familia_base | concepto_base | cantidad_base | cliente_base | entrega_base | tipo_comprobante            | serie | cliente  | tipo_entrega | tipo_pago | multipago | medio_pago                                | banco     | tarjeta | cuenta_bancaria         | nro_operacion          | monto_por_medio | nro_cuotas | monto_inicial_credito   | resultado_esperado                                  |
	|    1 | false           | Azúcar       | 7751234001115 |            10 |     00000000 | inmediata    | boleta de venta electronica | B002  | 00000000 | inmediata    | contado   | false     | efectivo                                  | NA        | NA      | NA                      | NA                     |            100  | NA         |   NA                    | Pedido confirmado correctamente                     |
	|    2 | false           | Azúcar       | 7751234001115 |            10 |     00000000 | inmediata    | boleta de venta electronica | B002  | 00000000 | inmediata    | contado   | false     | efectivo                                  | NA        | NA      | NA                      | NA                     |              10 | NA         |   NA                    | Monto insuficiente                                  |
	|    3 | false           | Azúcar       | 7751234001115 |            10 |     00000000 | inmediata    | boleta de venta electronica | B002  | 00000000 | inmediata    | contado   | false     | tarjeta_credito                           | INTERBANK | VISA    | NA                      |                 458962 | NA              | NA         |   NA                    | Pedido confirmado correctamente                     |
	|    4 | false           | Azúcar       | 7751234001115 |            10 |     00000000 | inmediata    | boleta de venta electronica | B002  | 00000000 | inmediata    | contado   | false     | tarjeta_debito                            | ninguno   | ninguno | NA                      | ninguno                | NA              | NA         |   NA                    | Seleccione una entidad bancaria                     |
	|    5 | false           | Azúcar       | 7751234001115 |            10 |     00000000 | inmediata    | boleta de venta electronica | B002  | 00000000 | inmediata    | contado   | false     | transferencia_fondos                      | NA        | NA      | BCP\|SOL\|1912490779081 |                 458962 | NA              | NA         |   NA                    | Pedido confirmado correctamente                     |
	|    6 | false           | Azúcar       | 7751234001115 |            10 |     00000000 | inmediata    | boleta de venta electronica | B002  | 00000000 | inmediata    | contado   | false     | deposito_cuenta                           | NA        | NA      | ninguno                 | ninguno                | NA              | NA         |   NA                    | Seleccione una cuenta bancaria                      |
	|    7 | false           | Azúcar       | 7751234001115 |            10 |     75971755 | inmediata    | boleta de venta electronica | B002  | 75971755 | inmediata    | contado   | false     | puntos                                    | NA        | NA      | NA                      | NA                     | NA              | NA         |   NA                    | Pedido confirmado correctamente                     |
	|    8 | true            | Gaseosa      | 7753234003320 |           110 |     75971751 | inmediata    | boleta de venta electronica | B002  | 75971751 | inmediata    | contado   | false     | puntos                                    | NA        | NA      | NA                      | NA                     | NA              | NA         |   NA                    | No hay suficientes puntos disponibles				  |
	|    9 | false           | Azúcar       | 7751234001115 |            10 |     00000000 | inmediata    | boleta de venta electronica | B002  | 00000000 | inmediata    | contado   | false     | puntos                                    | NA        | NA      | NA                      | NA                     | NA              | NA         |   NA                    | Para el pago con puntos debe identificar al cliente |
	|   10 | false           | Azúcar       | 7751234001115 |            10 |     00000000 | inmediata    | boleta de venta electronica | B002  | 00000000 | inmediata    | contado   | true      | tarjeta_debito, deposito_cuenta           | YAPE      | VISA    | BCP                     | 31004542, 000744226861 | 20, 12          | NA         |   NA                    | Pedido confirmado correctamente                     |
	|   11 | false           | Azúcar       | 7751234001115 |            10 |     75971755 | inmediata    | boleta de venta electronica | B002  | 75971755 | inmediata    | credito   | false     | efectivo                                  | NA        | NA      | NA                      | NA                     | 20              |          2 |   20                    | Pedido confirmado correctamente                     |
	|   12 | false           | Azúcar       | 7751234001115 |            10 |     00000000 | inmediata    | boleta de venta electronica | B002  | 00000000 | inmediata    | credito   | false     | efectivo                                  | NA        | NA      | NA                      | NA                     | NA              |          3 |   NA                    | Para dar a credito debe identificar al cliente      |
	|   13 | true            | Gaseosa      | 7753234003320 |           110 |     75971755 | inmediata    | boleta de venta electronica | B002  | 75971755 | inmediata    | credito   | true      | tarjeta_credito, transferencia_fondos     | INTERBANK | VISA    | BCP\|SOL\|1912490779081 | 0030281, 458962        | 45, 35          |          4 |   80                    | Pedido confirmado correctamente                     |
	|   14 | true            | Gaseosa      | 7753234003320 |           110 |     75971755 | inmediata    | boleta de venta electronica | B002  | 75971755 | inmediata    | credito   | true      | efectivo, tarjeta_credito                 | INTERBANK | VISA    | NA					  | NA, 0030281 	       | 45, 35          |          4 |   80                    | Pedido confirmado correctamente                     |



