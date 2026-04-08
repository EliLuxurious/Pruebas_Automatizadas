Feature: AjusteComprobante

  Cobertura de Ajuste de Comprobante: Notas de Débito y Notas de Crédito.
  Se crean las ventas como precondición y luego se aplica el ajuste desde Ver Ventas.

  NOTA: Tras crear la venta, al acceder a "Ver Ventas" el comprobante
  aparece primero en la lista sin necesidad de filtrar por fecha.


Background:
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
    And el usuario accede al módulo 'Ventas'

# ═══════════════════════════════════════════════════════════════════════
# NOTAS DE DÉBITO — precondición fija: FACTURA / Inmediata / Contado
# ═══════════════════════════════════════════════════════════════════════

@NotaDebito
Scenario Outline: ND por intereses por mora - <caso>
    Given el usuario accede al submodulo 'Nueva Venta'
    And selecciona familia "gaseosa" y concepto "7753234003313"
    And actualiza la cantidad "50"
    And ingresa el documento del cliente "20542245671"
    And selecciona comprobante "FACTURA ELECTRONICA" con serie "F002"
    And selecciona tipo de entrega "Inmediata"
    And selecciona tipo de pago "Contado"
    And hace clic en Guardar
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    And selecciona "Nota de débito" en el modal de ajuste
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
      | ND01 | Interés por mora de prueba   | 5.00    | 2.00         | Efectivo  |
      | ND02 | Interés por mora sin inicial | 5.00    | -            | -         |

@NotaDebito
Scenario Outline: ND por aumento en el valor - <caso>
    Given el usuario accede al submodulo 'Nueva Venta'
    And selecciona familia "gaseosa" y concepto "7753234003313"
    And actualiza la cantidad "50"
    And ingresa el documento del cliente "20542245671"
    And selecciona comprobante "FACTURA ELECTRONICA" con serie "F002"
    And selecciona tipo de entrega "Inmediata"
    And selecciona tipo de pago "Contado"
    And hace clic en Guardar
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    And selecciona "Nota de débito" en el modal de ajuste
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
      | caso | motivo                        | aumento | observacion     | verificacion                                 |
      | ND03 | Aumento en el valor de prueba | 3.00    | Pago contado ND | genera el comprobante de ajuste exitosamente |

    @Inconsistencia
    Examples: Casos invalidos
      | caso | motivo                       | aumento  | observacion     | verificacion                                 |
      | ND04 | Ajuste inválido de prueba    | -        | -               | bloquea el guardado del ajuste               |

# ═══════════════════════════════════════════════════════════════════════════════════
# ################################## NOTAS DE CRÉDITO ###############################
# ═══════════════════════════════════════════════════════════════════════════════════

  # ⚠️ MÓDULO ALMACÉN: no existe aún. Validaciones de stock pendientes.


# ═══════════════════════════════════════════════════════════════════════
# NOTAS DE CRÉDITO - ANULACIÓN / DEVOLUCIÓN TOTAL
# precondición variable: entrega y tipo de pago según caso
# ═══════════════════════════════════════════════════════════════════════

@NotaCredito
Scenario Outline: NC por anulacion / devolucion total - <caso>
    Given el usuario accede al submodulo 'Nueva Venta'
    And selecciona familia "gaseosa" y concepto "7753234003313"
    And actualiza la cantidad "50"
    And ingresa el documento del cliente "75893616"
    And selecciona comprobante "BOLETA DE VENTA ELECTRONICA" con serie "B002"
    And selecciona tipo de entrega "<entregaVenta>"
    And selecciona tipo de pago "<tipoPagoVenta>"
    And ingresa monto inicial del pago "<montoInicialVenta>"
    And hace clic en Guardar
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    And selecciona "Nota de crédito" en el modal de ajuste
    And selecciona tipo de nota de credito "<tipoNC>"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "<motivo>"
    And selecciona entrega "<entregaAjuste>" en el ajuste
    And selecciona devolucion "<devolucion>" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema <verificacion>

    Examples: Casos validos
      | caso | entregaVenta | tipoPagoVenta | montoInicialVenta | tipoNC                    | motivo                                                | entregaAjuste | devolucion | verificacion                                 |
      | NC01 | Diferida     | Credito       | -                 | ANULACIÓN DE LA OPERACIÓN | Anulación total de la operación por error de registro | -             | -          | genera el comprobante de ajuste exitosamente |
      | NC03 | Inmediata    | Contado       | -                 | ANULACIÓN DE LA OPERACIÓN | Anulación de la operación con devolución pendiente    | Diferida      | Contado    | genera el comprobante de ajuste exitosamente |
      | NC04 | Inmediata    | Contado       | -                 | ANULACIÓN DE LA OPERACIÓN | Anulación de la operación con devolución a crédito    | Inmediata     | Credito    | genera el comprobante de ajuste exitosamente |
      | NC13 | Diferida     | Credito       | 30                | DEVOLUCIÓN TOTAL          | Devolución total de ítems no entregados               | Diferida      | Contado    | genera el comprobante de ajuste exitosamente |

    @Inconsistencia
    Examples: Casos invalidos
      | caso  | entregaVenta | tipoPagoVenta | montoInicialVenta | tipoNC                    | motivo | entregaAjuste | devolucion | verificacion                   |
      | INC02 | Diferida     | Credito       | -                 | ANULACIÓN DE LA OPERACIÓN | -      | -             | -          | bloquea el guardado del ajuste |
    # NC02 — NC anulación con entrega mixta: ❌ NO REALIZABLE — solo TODOS entregados o TODOS diferidos

# ═══════════════════════════════════════════════════════════════════════
# NOTAS DE CRÉDITO - DESCUENTO GLOBAL
# ═══════════════════════════════════════════════════════════════════════

@NotaCredito
Scenario Outline: NC por descuento global - <caso>
    Given el usuario accede al submodulo 'Nueva Venta'
    And selecciona familia "gaseosa" y concepto "7753234003313"
    And actualiza la cantidad "50"
    And ingresa el documento del cliente "75893616"
    And selecciona comprobante "BOLETA DE VENTA ELECTRONICA" con serie "B002"
    And selecciona tipo de entrega "Inmediata"
    And selecciona tipo de pago "<tipoPagoVenta>"
    And ingresa monto inicial del pago "<montoInicialVenta>"
    And hace clic en Guardar
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    And selecciona "Nota de crédito" en el modal de ajuste
    And selecciona tipo de nota de credito "DESCUENTO GLOBAL"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "<motivo>"
    And ingresa importe NC "<importeNC>"
    And selecciona devolucion "<devolucion>" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema <verificacion>

    Examples: Casos validos
      | caso | tipoPagoVenta | montoInicialVenta | motivo                                         | importeNC | devolucion | verificacion                                 |
      | NC05 | Credito       | 1                 | Descuento global aplicado posterior a la venta | 10.00     | Contado    | genera el comprobante de ajuste exitosamente |
    # NC07 — Descuento global importe ≤ cuotas (Credito | 1): 🐛 PENDIENTE DE BUG — sección Pago no se oculta cuando la NC absorbe las cuotas
    # NC09 — Descuento global sin cash pagado (Credito | -): 🐛 PENDIENTE DE BUG — sección Pago no se oculta aunque no hay efectivo que devolver

    @Inconsistencia
    Examples: Casos invalidos
      | caso  | tipoPagoVenta | montoInicialVenta | motivo                            | importeNC | devolucion | verificacion                            |
      | INC01 | Contado       | -                 | Ajuste de descuento mal ingresado | 99999.00  | -          | muestra mensaje de monto mayor al total |

@NotaCredito
Scenario Outline: NC por descuento por item - <caso>
    Given el usuario accede al submodulo 'Nueva Venta'
    And selecciona familia "gaseosa" y concepto "7753234003313"
    And actualiza la cantidad "50"
    And ingresa el documento del cliente "75893616"
    And selecciona comprobante "BOLETA DE VENTA ELECTRONICA" con serie "B002"
    And selecciona tipo de entrega "Inmediata"
    And selecciona tipo de pago "<tipoPagoVenta>"
    And ingresa monto inicial del pago "<montoInicialVenta>"
    And hace clic en Guardar
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    And selecciona "Nota de crédito" en el modal de ajuste
    And selecciona tipo de nota de credito "DESCUENTO POR ÍTEM"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "<motivo>"
    And ingresa importe detalle "<importeDetalle>" para el item
    And selecciona devolucion "<devolucion>" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema genera el comprobante de ajuste exitosamente

    Examples:
      | caso | tipoPagoVenta | montoInicialVenta | motivo                                              | importeDetalle | devolucion |
      | NC06 | Credito       | 1                 | Descuento aplicado por ajuste comercial posterior   | 6.00           | Credito    |
      | NC08 | Contado       | -                 | Descuento por ajuste comercial en un ítem facturado | 3.00           | Contado    |

# ═══════════════════════════════════════════════════════════════════════
# NOTAS DE CRÉDITO - DEVOLUCIÓN POR ÍTEM
# ═══════════════════════════════════════════════════════════════════════

@NotaCredito
Scenario Outline: NC por devolucion por item - <caso>
    Given el usuario accede al submodulo 'Nueva Venta'
    And selecciona familia "gaseosa" y concepto "7753234003313"
    And actualiza la cantidad "50"
    And ingresa el documento del cliente "75893616"
    And selecciona comprobante "BOLETA DE VENTA ELECTRONICA" con serie "B002"
    And selecciona tipo de entrega "<entregaVenta>"
    And selecciona tipo de pago "<tipoPagoVenta>"
    And ingresa monto inicial del pago "<montoInicialVenta>"
    And hace clic en Guardar
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    And selecciona "Nota de crédito" en el modal de ajuste
    And selecciona tipo de nota de credito "DEVOLUCIÓN POR ÍTEM"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "<motivo>"
    And selecciona entrega "<entregaAjuste>" en el ajuste
    And ingresa cantidad a devolver "<cantDevolver>"
    And selecciona devolucion "<devolucion>" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema genera el comprobante de ajuste exitosamente

    Examples:
      | caso | entregaVenta | tipoPagoVenta | montoInicialVenta | motivo                                             | entregaAjuste | cantDevolver | devolucion |
      | NC11 | Inmediata    | Credito       | 1                 | Devolución parcial (se aplica a cuotas pendientes) | Diferida      | 1            | -          |
      | NC12 | Diferida     | Contado       | -                 | Anulación de ítem no entregado                     | -             | -            | Credito    |
    # NC02 — NC devolución por ítem con entrega mixta (Inmediata+Diferida): ❌ NO REALIZABLE — solo TODOS entregados o TODOS diferidos
    # NC10 — NC devolución parcial por ítem (mitad/mitad): ❌ NO REALIZABLE — misma restricción que NC02

# ═══════════════════════════════════════════════════════════════════════
# PRUEBAS NO REALIZADAS
# ═══════════════════════════════════════════════════════════════════════
#
# ❌ DEFINITIVAMENTE NO REALIZABLES — funcionalidad no implementada:
#   NC02, NC10, INC04: requieren productos parcialmente entregados
#   (mitad/mitad). Solo se permite TODOS entregados o TODOS diferidos.
#
# 🐛 PENDIENTE DE BUG — realizables cuando se corrija el sistema:
#   NC07: Descuento global (Credito | 1), importe ≤ cuotas pendientes.
#         El sistema no oculta la sección Pago aunque toda la NC
#         se absorbe en cuotas y no hay efectivo que devolver.
#   NC09: Descuento global (Credito | -), sin pagos al contado.
#         El sistema no oculta la sección Pago aunque no existe
#         ningún pago en efectivo registrado que devolver.
#   INC03: Segunda NC sobre comprobante con importe vigente agotado.
#          El sistema no bloquea el intento aunque el importe disponible
#          ya es 0 tras la primera NC de anulación.
#
# ⚠️ MÓDULO ALMACÉN: no existe aún. Validaciones de stock, ingreso de
# mercadería y revocación de órdenes de salida quedan PENDIENTES.
# ═══════════════════════════════════════════════════════════════════════

# ═══════════════════════════════════════════════════════════════════════════════════════════════════════════════
# ############################################# INVALIDAR VENTA #################################################
# ═══════════════════════════════════════════════════════════════════════════════════════════════════════════════

@Invalidar
Scenario Outline: Invalidar venta dentro de plazo 
    Given el usuario accede al submodulo 'Nueva Venta'
    And selecciona familia "gaseosa" y concepto "7753234003313"
    And actualiza la cantidad "50"
    And ingresa el documento del cliente "<docCliente>"
    And selecciona comprobante "<comprobante>" con serie "<serie>"
    And selecciona tipo de entrega "<entregaVenta>"
    And selecciona tipo de pago "<tipoPagoVenta>"
    And ingresa monto inicial del pago "<montoInicialVenta>"
    And hace clic en Guardar
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    And selecciona "Invalidar" en el modal de ajuste
    And selecciona devolucion "<devolucionInvalidar>" en el modal de invalidacion
    And ingresa observacion de invalidacion "<observacion>"
    And hace clic en Invalidar en el modal de invalidacion
    Then el sistema procesa la invalidacion correctamente

    Examples:
      | caso  | docCliente  | comprobante                 | serie | entregaVenta | tipoPagoVenta | montoInicialVenta | devolucionInvalidar | observacion                                          |
      | CP064 | 20542245671 | FACTURA ELECTRONICA         | F002  | Inmediata    | Contado       | -                 | Inmediata           | Invalidación dentro de plazo con pago al contado     |
      | CP066 | 75893616    | BOLETA DE VENTA ELECTRONICA | B002  | Diferida     | Credito       | -                 | -                   | Invalidación de venta diferida sin pago              |
      | CP069 | 75893616    | BOLETA DE VENTA ELECTRONICA | B002  | Inmediata    | Credito       | 2                 | Diferida            | Invalidación dentro de plazo con devolución diferida |
    # CP065 — ❌ NO REALIZABLE — requiere venta con "productos entregados Y diferidos" (entrega mixta/parcial). Solo TODOS entregados (Inmediata) o TODOS diferidos.
    # CP066: 🐛 PENDIENTE — el modal debería ocultar las secciones Entrega y Pago (solo diferidos y sin pago al contado), pero actualmente las muestra.

@Invalidar
@Inconsistencia
Scenario: CP067 - No permitir invalidar venta sin observacion obligatoria
    Given el usuario accede al submodulo 'Ver Ventas'
    And filtra ventas de 8 dias atras
    And accede a las opciones del comprobante recien registrado
    And selecciona "Invalidar" en el modal de ajuste
    And selecciona devolucion "Inmediata" en el modal de invalidacion
    Then el sistema no activa el boton Invalidar

# CP068 — Mostrar mensaje fuera de plazo (>7 días): ❌ NO REALIZABLE AUTOMÁTICAMENTE.
# Requiere una venta fuera de plazo (>7 días), diferida, con pago registrado y cuotas pendientes,
# que no puede crearse dentro del flujo automatizado (la venta siempre es "hoy").
# Ejecución manual: buscar en Ver Ventas una venta con fecha anterior (diferida, con pago y cuotas),
# abrir Ajuste de Comprobante > Invalidar, ingresar observación y hacer clic en Invalidar.
# Resultado esperado: el sistema muestra "Fuera de plazo (usar Nota de Crédito)".
# 🐛 PENDIENTE — el modal actualmente muestra la sección Entrega para ventas fuera de plazo; debería ocultarse.
