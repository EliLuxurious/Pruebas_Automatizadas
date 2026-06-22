@NuevaVenta
Feature: AjusteComprobante

  Cobertura de Ajuste de Comprobante: Notas de Debito y Notas de Credito.
  Se crean las ventas como precondicion y luego se aplica el ajuste desde Ver Ventas.

  NOTA: Tras crear la venta, al acceder a "Ver Ventas" el comprobante
  aparece primero en la lista sin necesidad de filtrar por fecha.

Background:
    Given el usuario ingresa al ambiente 'https://alpha2.newfrontdev-qa.sigesonline.com/sales/new-sales'
    When el usuario inicia sesión con usuario 'admin.ti@tsol.com' y contraseña 'calidad'
    And se descarta aviso de contrasena de Chrome si aparece
    And el usuario accede al módulo 'Ventas'
#    Given existe el concepto item comercial para ventas:
#      | Familia | TipoFamilia | TratamientoIGVFamilia | CodigoFamilia | CategoriaFamilia | Codigo    | Sufijo            | UMComercial | UMedida | Rol            | Modulo  | Marca | Presentacion | Cantidad | UnidadMedida | Tarifa     | Precio |
#      | Gaseosa | Bien        | Exoneracion IGV       | QA-GASEOSA    | SIN CATEGORÍA    | 123456789 | Gaseosa Inca kola | UN          | UN      | Item Comercial | MOD0001 |       | BOTELLAS     | 1        | UN           | POR UNIDAD | 2.30   |
#
#    Given Navego al módulo de 'Adquisición'
#    And Entro al submódulo específico de 'Nueva Adquisición'
#    When Se configuran los datos de 'Facturación':
#      | Campo                 | Valor                    |
#      | Documento             | FACTURA ELECTRONICA      |
#      | Serie                 | F001                     |
#      | Correlativo           | 00009991                 |
#      | Fecha de emisión      | 04/03/2026               |
#      | Proveedor             | 10759012017              |
#      | Información Adicional | Precondición Inka Kola   |
#    And Se selecciona el tipo de entrega 'Inmediata'
#    And Se configuran los datos de Entrega de Ventas:
#      | Campo           | Valor                    |
#      | Rol             | Item Comercial           |
#      | Establecimiento | SIGES - CENTRAL          |
#      | Almacén         | SIGES - CASTILLO GRANDE  |
#    And Se selecciona y configura el producto de ventas a adquirir:
#      | Producto                     | Cantidad | V. U |
#      | 123456789\|Gaseosa Inca kola | 15       | 2.00 |
#    Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
#    And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

# ============================================================================
# NOTAS DE DEBITO - precondicion fija: FACTURA / Inmediata / Contado
# ============================================================================

@NotaDebito
Scenario Outline: ND por intereses por mora - <caso>
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'FACTURA ELECTRONICA' 'F002' '20542245671'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    When el usuario accede al submodulo 'Ver Ventas'
    # NOTA: La idea original era tomar el ultimo comprobante emitido filtrando por fecha.
    # Sin embargo, Ver Ventas muestra registros de diferentes fechas mezclados y los ultimos
    # comprobantes no siempre aparecen primero, por lo que el filtro de fecha no es confiable.
    # Se opta por filtrar por tipo de documento "FAC" para acotar la tabla solo a Facturas
    # y que el primer resultado visible corresponda al tipo correcto para aplicar el ajuste.
    And filtra por tipo de documento "FAC"
    And abre el modal ajustes de comprobante
    And selecciona la opcion "Nota de débito" del modal ajustes de comprobante
    And selecciona tipo de nota de debito "INTERESES POR MORA"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "<motivo>"
    And ingresa monto del interes "<interes>"
    And expande la seccion "Pago" del ajuste
    And selecciona tipo de pago "Credito" en la seccion pago del ajuste
    And ingresa monto inicial "<montoInicial>" en el ajuste
    And selecciona medio de pago "<medioPago>" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema genera el comprobante de ajuste exitosamente

    Examples:
      | caso | motivo                       | interes | montoInicial | medioPago |
      | ND01 | Interes por mora de prueba   | 5.00    | 2.00         | Efectivo  |
      | ND02 | Interes por mora sin inicial | 5.00    | -            | -         |

@NotaDebito
Scenario Outline: ND por aumento en el valor - <caso>
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'FACTURA ELECTRONICA' 'F002' '20542245671'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    When el usuario accede al submodulo 'Ver Ventas'
    # NOTA: La idea original era tomar el ultimo comprobante emitido filtrando por fecha.
    # Sin embargo, Ver Ventas muestra registros de diferentes fechas mezclados y los ultimos
    # comprobantes no siempre aparecen primero, por lo que el filtro de fecha no es confiable.
    # Se opta por filtrar por tipo de documento "FAC" para acotar la tabla solo a Facturas
    # y que el primer resultado visible corresponda al tipo correcto para aplicar el ajuste.
    And filtra por tipo de documento "FAC"
    And abre el modal ajustes de comprobante
    And selecciona la opcion "Nota de débito" del modal ajustes de comprobante
    And selecciona tipo de nota de debito "AUMENTO EN EL VALOR"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "<motivo>"
    And ingresa total aumento del valor "<aumento>"
    And expande la seccion "Pago" del ajuste
    And selecciona tipo de pago "Contado" en la seccion pago del ajuste
    And selecciona medio de pago "Efectivo" en el ajuste
    And ingresa observacion "<observacion>" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema <verificacion>

    Examples: Casos validos
      | caso | motivo                      | aumento | observacion     | verificacion                                 |
      | ND03 | Aumento en el valor de prueba | 3.00  | Pago contado ND | genera el comprobante de ajuste exitosamente |

    @Inconsistencia
    Examples: Casos invalidos
      | caso | motivo                    | aumento | observacion | verificacion                   |
      | ND04 | Ajuste invalido de prueba | -       | -           | bloquea el guardado del ajuste |

# ============================================================================
# NOTAS DE CREDITO
# ============================================================================

# MODULO ALMACEN: no existe aun. Validaciones de stock pendientes.

# ============================================================================
# NOTAS DE CREDITO - ANULACION / DEVOLUCION TOTAL
# precondicion variable: entrega y tipo de pago segun caso
# ============================================================================

@NotaCredito
Scenario Outline: NC por anulacion / devolucion total - <caso>
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '5'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega '<entregaVenta>' 'false'
    And el usuario configura los medios de pago '<tipoPagoVenta>' 'false' '<medioPagoVenta>' 'NA' 'NA' 'NA' 'NA' 'NA' 'NA' '<montoInicialVenta>' 'NA'
    And hace clic en Guardar
    When el usuario accede al submodulo 'Ver Ventas'
    # NOTA: La idea original era tomar el ultimo comprobante emitido filtrando por fecha.
    # Sin embargo, Ver Ventas muestra registros de diferentes fechas mezclados y los ultimos
    # comprobantes no siempre aparecen primero, por lo que el filtro de fecha no es confiable.
    # Se filtra por "BV" para acotar la tabla a Boletas y tomar el primero visible correctamente.
    And filtra por tipo de documento "BV"
    And abre el modal ajustes de comprobante
    And selecciona la opcion "Nota de crédito" del modal ajustes de comprobante
    And selecciona tipo de nota de credito "<tipoNC>"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "<motivo>"
    And selecciona entrega "<entregaAjuste>" en el ajuste
    And selecciona devolucion "<devolucion>" en el ajuste
    And opcionalmente ingresa monto inicial "<montoInicialAjuste>" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema <verificacion>

    Examples: Casos validos
      | caso  | entregaVenta | tipoPagoVenta | medioPagoVenta | montoInicialVenta | tipoNC                    | motivo                                                | entregaAjuste | devolucion | montoInicialAjuste | verificacion                                 |
      # cantidad=5, precio=S/2.30 → total=S/11.50. montoInicialVenta=1 (< total) deja cuotas pendientes para el credito.
      | NC001 | Diferida     | Credito       | NA             | 1                | ANULACIÓN DE LA OPERACIÓN | Anulacion total de la operacion por error de registro | -             | -          | -                  | genera el comprobante de ajuste exitosamente |
      | NC003 | Inmediata    | Contado       | efectivo       | NA               | ANULACIÓN DE LA OPERACIÓN | Anulacion de la operacion con devolucion pendiente    | Diferida      | Contado    | -                  | genera el comprobante de ajuste exitosamente |
      | NC004 | Inmediata    | Contado       | efectivo       | NA               | ANULACIÓN DE LA OPERACIÓN | Anulacion de la operacion con devolucion a credito    | Inmediata     | Credito    | 1                  | genera el comprobante de ajuste exitosamente |

    @Inconsistencia
    Examples: Casos invalidos
      | caso  | entregaVenta | tipoPagoVenta | medioPagoVenta | montoInicialVenta | tipoNC                    | motivo | entregaAjuste | devolucion | montoInicialAjuste | verificacion                   |
      # cantidad=5, precio=S/2.30 → total=S/11.50. montoInicialVenta=1 (< total). Se espera bloqueo porque el motivo va vacio.
      | NC015 | Diferida     | Credito       | NA             | 1                 | ANULACIÓN DE LA OPERACIÓN | -      | -             | -          | -                  | bloquea el guardado del ajuste |
        ##Se cambio NC015 para que en la automatizacion se integre un monto inicial a la precondicion de credito##

    # NC002 y NC013 - con entrega mixta: NO REALIZABLE - solo TODOS entregados o TODOS diferidos

# ============================================================================
# NOTAS DE CREDITO - DESCUENTO GLOBAL
# ============================================================================

@NotaCredito
Scenario Outline: NC por descuento global - <caso>
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And el usuario configura los medios de pago '<tipoPagoVenta>' 'false' '<medioPagoVenta>' 'NA' 'NA' 'NA' 'NA' 'NA' 'NA' '<montoInicialVenta>' 'NA'
    And hace clic en Guardar
    When el usuario accede al submodulo 'Ver Ventas'
    And filtra por tipo de documento "BV"
    And abre el modal ajustes de comprobante
    And selecciona la opcion "Nota de crédito" del modal ajustes de comprobante
    And selecciona tipo de nota de credito "DESCUENTO GLOBAL"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "<motivo>"
    And ingresa importe NC "<importeNC>"
    And selecciona devolucion "<devolucion>" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema <verificacion>

    Examples: Casos validos
      | caso | tipoPagoVenta | medioPagoVenta | montoInicialVenta | motivo                                         | importeNC | devolucion | verificacion                                 |
      # montoInicialVenta=1 e importeNC=1.50 ajustados (precio=S/2.30, total=S/2.30; ambos deben ser < total)
      | NC05 | Credito       | NA             | 1                 | Descuento global aplicado posterior a la venta | 1.50      | Contado    | genera el comprobante de ajuste exitosamente |
    # NC07 - Descuento global importe <= cuotas (Credito | 1): PENDIENTE DE BUG; aqui no se devuelve nada
    # NC09 - Descuento global sin cash pagado (Credito | NA): PENDIENTE DE BUG; aqui no se devuelve nada

    @Inconsistencia
    Examples: Casos invalidos
      | caso  | tipoPagoVenta | medioPagoVenta | montoInicialVenta | motivo                            | importeNC | devolucion | verificacion                            |
      | NC014 | Contado       | efectivo       | NA                | Ajuste de descuento mal ingresado | 999       | -          | muestra mensaje de monto mayor al total |

@NotaCredito
Scenario Outline: NC por descuento por item - <caso>
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '2'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And el usuario configura los medios de pago '<tipoPagoVenta>' 'false' '<medioPagoVenta>' 'NA' 'NA' 'NA' 'NA' 'NA' 'NA' '<montoInicialVenta>' 'NA'
    And hace clic en Guardar
    When el usuario accede al submodulo 'Ver Ventas'
    And filtra por tipo de documento "BV"
    And abre el modal ajustes de comprobante
    And selecciona la opcion "Nota de crédito" del modal ajustes de comprobante
    And selecciona tipo de nota de credito "DESCUENTO POR ÍTEM"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "<motivo>"
    And ingresa importe detalle "<importeDetalle>" para el item
    And selecciona devolucion "<devolucion>" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema genera el comprobante de ajuste exitosamente

    Examples:
      | caso | tipoPagoVenta | medioPagoVenta | montoInicialVenta | motivo                                              | importeDetalle  | devolucion |
      # montoInicialVenta=3 e importeDetalle=2.00 ajustados (cantidad 2, precio=S/2.30, total=S/4.60; ambos deben ser < total)
      | NC06 | Credito       | NA             | 3                 | Descuento aplicado por ajuste comercial posterior   | 2.00            | Credito    |
      | NC08 | Contado       | efectivo       | NA                | Descuento por ajuste comercial en un item facturado | 3.00            | Contado    |

# ============================================================================
# NOTAS DE CREDITO - DEVOLUCION POR ITEM
# ============================================================================

@NotaCredito
Scenario Outline: NC por devolucion por item - <caso>
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '50'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega '<entregaVenta>' 'false'
    And el usuario configura los medios de pago '<tipoPagoVenta>' 'false' '<medioPagoVenta>' 'NA' 'NA' 'NA' 'NA' 'NA' 'NA' '<montoInicialVenta>' 'NA'
    And hace clic en Guardar
    When el usuario accede al submodulo 'Ver Ventas'
    And filtra por tipo de documento "BV"
    And abre el modal ajustes de comprobante
    And selecciona la opcion "Nota de crédito" del modal ajustes de comprobante
    And selecciona tipo de nota de credito "DEVOLUCIÓN POR ÍTEM"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "<motivo>"
    And selecciona entrega "<entregaAjuste>" en el ajuste
    And ingresa cantidad a devolver "<cantDevolver>"
    And selecciona devolucion "<devolucion>" en el ajuste
    And opcionalmente ingresa monto inicial "<montoInicialAjuste>" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema genera el comprobante de ajuste exitosamente

    Examples:
      | caso | entregaVenta | tipoPagoVenta | medioPagoVenta | montoInicialVenta | motivo                                           | entregaAjuste | cantDevolver | devolucion | montoInicialAjuste |
      | NC11 | Inmediata    | Credito       | NA             | 1                 | Devolucion parcial (se aplica a cuotas pendientes) | Diferida    | 1            | -          | -                  |
      # NC12: devolucion Credito requiere monto inicial para generar cuotas (precio 2.30 x 50 = S/115)
      | NC12 | Diferida     | Contado       | efectivo       | NA                | Anulacion de item no entregado                   | -             | -            | Credito    | 50                 |


# ============================================================================
# PRUEBAS NO REALIZADAS
# ============================================================================
#-----------------------------------------------------------------------------
# DEFINITIVAMENTE NO REALIZABLES - funcionalidad no implementada:
#   NC02, NC10, INC04: requieren productos parcialmente entregados
#   (mitad/mitad). Solo se permite TODOS entregados o TODOS diferidos.
#-----------------------------------------------------------------------------
# PENDIENTE DE BUG - realizables cuando se corrija el sistema:
#   NC07: Descuento global (Credito | 1), importe <= cuotas pendientes.
#         El sistema no oculta la seccion Pago aunque toda la NC
#         se absorbe en cuotas y no hay efectivo que devolver.
#   NC09: Descuento global (Credito | NA), sin pagos al contado.
#         El sistema no oculta la seccion Pago aunque no existe
#         ningun pago en efectivo registrado que devolver.
#   INC03: Segunda NC sobre comprobante con importe vigente agotado.
#          El sistema no bloquea el intento aunque el importe disponible
#          ya es 0 tras la primera NC de anulacion.
#------------------------------------------------------------------------------
# MODULO ALMACEN: no existe aun. Validaciones de stock, ingreso de
# mercaderia y revocacion de ordenes de salida quedan PENDIENTES.


# ============================================================================
# INVALIDAR VENTA
# ============================================================================

@Invalidar
Scenario Outline: Invalidar venta dentro de plazo - <caso>
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion '<comprobante>' '<serie>' '<docCliente>'
    And el usuario configura la entrega '<entregaVenta>' 'false'
    And el usuario configura los medios de pago '<tipoPagoVenta>' 'false' '<medioPagoVenta>' 'NA' 'NA' 'NA' 'NA' 'NA' 'NA' '<montoInicialVenta>' 
    And hace clic en Guardar
    When el usuario accede al submodulo 'Ver Ventas'
    And abre el modal ajustes de comprobante
    And selecciona la opcion "Invalidar" del modal ajustes de comprobante
    And selecciona devolucion "<devolucionInvalidar>" en el modal de invalidacion
    And ingresa observacion de invalidacion "<observacion>"
    Then el sistema muestra el resultado esperado de la invalidacion "<resultadoEsperado>"

    Examples:
      | caso  | docCliente  | comprobante                 | serie | entregaVenta | tipoPagoVenta | medioPagoVenta | montoInicialVenta | devolucionInvalidar | observacion                                           | resultadoEsperado                     |
      | CP101 | 20542245671 | FACTURA ELECTRONICA         | F002  | Inmediata    | Contado       | efectivo       | NA                | Inmediata           | Invalidacion dentro de plazo con pago al contado      | procesa la invalidacion correctamente |
      | CP103 | 75893616    | BOLETA DE VENTA ELECTRONICA | B002  | Diferida     | Credito       | NA             | NA                | -                   | Invalidacion de venta diferida sin pago               | procesa la invalidacion correctamente |
      | CP104 | 20542245671 | FACTURA ELECTRONICA         | F002  | Inmediata    | Contado       | efectivo       | NA                | Inmediata           |                                                       | no activa el boton Invalidar          |
      | CP106 | 75893616    | BOLETA DE VENTA ELECTRONICA | B002  | Inmediata    | Credito       | NA             | 2                 | Diferida            | Invalidacion dentro de plazo con devolucion diferida  | procesa la invalidacion correctamente |
    # CP102 - NO REALIZABLE - requiere venta con entrega mixta/parcial
    # CP0103: PENDIENTE - el modal deberia ocultar las secciones Entrega y Pago

@Invalidar
@Inconsistencia
@FueraDePlazo
Scenario: CP105 Invalidar venta fuera de plazo
    When el usuario accede al submodulo 'Ver Ventas'
    And el usuario selecciona la fecha y hora inicial "hace 8 dias 12:00 am" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "hace 8 dias 11:59 pm" en el campo "Fecha y Hora Final"
    And hace clic en consultar ventas
    And abre el modal ajustes de comprobante
    Then el sistema no muestra la opcion "Invalidar" en el modal ajustes de comprobante

# ============================================================================
# CLONAR VENTA
# ============================================================================

@ClonarVenta
# Se mantiene como escenario independiente (no Outline) porque usa fecha relativa "ayer" para garantizar
# que siempre exista al menos una venta reciente en la tabla sin depender de fechas hardcodeadas.
# Valida que el sistema permita clonar una venta del dia anterior y la registre correctamente.
Scenario: CP107 Clonar una venta existente
    When el usuario accede al submodulo 'Ver Ventas'
    And el usuario selecciona la fecha y hora inicial "ayer" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "ayer" en el campo "Fecha y Hora Final"
    And hace clic en consultar ventas
    And abre el modal ajustes de comprobante
    And selecciona la opcion "Clonar" del modal ajustes de comprobante
    Then el sistema muestra el modal Clonar venta
    When hace clic en Clonar venta
    Then el sistema registra la venta clonada correctamente

# CP108 - No se automatiza por ahora porque el modulo de promociones aun no existe en este flujo.

@ClonarVenta
# Se mantiene como escenario independiente (no Outline) porque crea su propia venta como precondicion
# para garantizar que el cliente original sea conocido (75893616 - DNI).
# Valida que al clonar se puedan modificar cantidad, entrega y cambiar el cliente a un DNI diferente (48271935).
Scenario: CP109 Clonar una venta y modificar datos
    # Precondicion: crear venta con cliente 75893616 para tener un comprobante conocido que clonar
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    # Clonar y modificar datos
    When el usuario accede al submodulo 'Ver Ventas'
    And abre el modal ajustes de comprobante
    And selecciona la opcion "Clonar" del modal ajustes de comprobante
    Then el sistema muestra el modal Clonar venta
    And selecciona la pestaña "VENTA NORMAL" en el modal Clonar venta
    And modifica la cantidad a "2" en el modal Clonar venta
    And selecciona la entrega "Diferida" en el modal Clonar venta
    And modifica el cliente a "48271935" en el modal Clonar venta
    When hace clic en Clonar venta
    Then el sistema registra la venta clonada correctamente
