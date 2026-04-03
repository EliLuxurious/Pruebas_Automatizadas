Feature: AjusteComprobante

  Cobertura de Ajuste de Comprobante: Notas de Débito y Notas de Crédito.
  Se crean las ventas como precondición y luego se aplica el ajuste desde Ver Ventas.

  NOTA: Tras crear la venta, al acceder a "Ver Ventas" el comprobante
  aparece primero en la lista sin necesidad de filtrar por fecha.

  # ═══════════════════════════════════════════════════════════════════════
  # ANÁLISIS DE VIABILIDAD
  # ═══════════════════════════════════════════════════════════════════════
  #
  # ✅ REALIZABLES (17 pruebas):
  #   ND01/ND02 - ND por intereses por mora (con y sin monto inicial)
  #   ND03 - ND por aumento en el valor por ítem con pago contado
  #   ND04 - Validar bloqueo de ND por ajuste inválido
  #   NC01/NC03/NC04 - NC por anulación (diferida, contado, crédito)
  #   NC05/NC07/NC09 - NC por descuento global (contado, sin pago)
  #   NC06/NC08 - NC por descuento por ítem (crédito, contado)
  #   NC11/NC12 - NC por devolución por ítem (diferida, crédito)
  #   NC13 - NC por devolución total (solo ítems entregados; col.13 requiere entrega parcial, no realizable)
  #   INC01 - Inconsistencia: importe NC mayor al vigente
  #   INC02 - Inconsistencia: sin motivo o sustento
  #   INC03 - Inconsistencia: comprobante sin importe vigente
  #
  # ❌ NO REALIZABLES (3 pruebas) - Requieren productos parcialmente
  #   entregados (mitad/mitad). Solo se permite TODOS o NINGUNO:
  #   NC02, NC10, INC04
  #
  # ⚠️ MÓDULO ALMACÉN: no existe aún. Validaciones de stock pendientes.
  # ═══════════════════════════════════════════════════════════════════════

Background:
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
    And el usuario accede al módulo 'Ventas'

# ═══════════════════════════════════════════════════════════════════════
# NOTAS DE DÉBITO
# ═══════════════════════════════════════════════════════════════════════

@NotaDebito
Scenario Outline: ND por intereses por mora - <caso>
    Given el usuario accede al submodulo 'Nueva Venta'
    And crea una venta con familia "gaseosa", concepto "7753234003313", cantidad "50", documento "20542245671", comprobante "FACTURA ELECTRONICA", serie "F002", entrega "Inmediata", pago "Completo"
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    And selecciona "Nota de débito" en el modal de ajuste
    And selecciona tipo de nota de debito "INTERESES POR MORA"
    And selecciona comprobante destino "NOTA DE DEBITO"
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
    And crea una venta con familia "gaseosa", concepto "7753234003313", cantidad "50", documento "20542245671", comprobante "FACTURA ELECTRONICA", serie "F002", entrega "Inmediata", pago "Completo"
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    And selecciona "Nota de débito" en el modal de ajuste
    And selecciona tipo de nota de debito "AUMENTO EN EL VALOR"
    And selecciona comprobante destino "NOTA DE DEBITO"
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
      | caso | motivo                        | aumento | observacion     | verificacion                                     |
      | ND03 | Aumento en el valor de prueba | 3.00    | Pago contado ND | genera el comprobante de ajuste exitosamente     |

    @Inconsistencia
    Examples: Casos invalidos
      | caso | motivo                        | aumento | observacion     | verificacion                                     |
      | ND04 | Ajuste inválido de prueba     | -       | -               | bloquea el guardado del ajuste                   |

# ═══════════════════════════════════════════════════════════════════════
# NOTAS DE CRÉDITO - ANULACIÓN / DEVOLUCIÓN TOTAL (flujo sin cantidad)
# ═══════════════════════════════════════════════════════════════════════

@NotaCredito
Scenario Outline: NC por anulacion / devolucion total - <caso>
    Given el usuario accede al submodulo 'Nueva Venta'
    And crea una venta con familia "gaseosa", concepto "7753234003313", cantidad "50", documento "75893616", comprobante "BOLETA DE VENTA ELECTRONICA", serie "B002", entrega "<entregaVenta>", pago "Completo"
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    And selecciona "Nota de crédito" en el modal de ajuste
    And selecciona tipo de nota de credito "<tipoNC>"
    And selecciona comprobante destino "NOTA DE CREDITO"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "<motivo>"
    And selecciona entrega "<entregaAjuste>" en el ajuste
    And selecciona devolucion "<devolucion>" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema genera el comprobante de ajuste exitosamente

    Examples:
      | caso  | entregaVenta | tipoNC                    | motivo                                                | entregaAjuste | devolucion |
      | NC01  | Diferida     | ANULACIÓN DE LA OPERACIÓN | Anulación total de la operación por error de registro | -             | -          |
      | NC03  | Inmediata    | ANULACIÓN DE LA OPERACIÓN | Anulación de la operación con devolución pendiente    | Diferida      | Contado    |
      | NC04  | Inmediata    | ANULACIÓN DE LA OPERACIÓN | Anulación de la operación con devolución a crédito    | Inmediata     | Credito    |
      | NC13  | Inmediata    | DEVOLUCIÓN TOTAL          | Devolución total de ítems entregados con reembolso    | Inmediata     | Contado    |

# ═══════════════════════════════════════════════════════════════════════
# NOTAS DE CRÉDITO - DEVOLUCIÓN POR ÍTEM (flujo con cantidad)
# ═══════════════════════════════════════════════════════════════════════

@NotaCredito
Scenario Outline: NC por devolucion por item - <caso>
    Given el usuario accede al submodulo 'Nueva Venta'
    And crea una venta con familia "gaseosa", concepto "7753234003313", cantidad "50", documento "75893616", comprobante "BOLETA DE VENTA ELECTRONICA", serie "B002", entrega "<entregaVenta>", pago "Completo"
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    And selecciona "Nota de crédito" en el modal de ajuste
    And selecciona tipo de nota de credito "DEVOLUCIÓN POR ÍTEM"
    And selecciona comprobante destino "NOTA DE CREDITO"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "<motivo>"
    And selecciona entrega "<entregaAjuste>" en el ajuste
    And ingresa cantidad a devolver "<cantDevolver>"
    And selecciona devolucion "<devolucion>" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema genera el comprobante de ajuste exitosamente

    Examples:
      | caso | entregaVenta | motivo                                                | entregaAjuste | cantDevolver | devolucion |
      | NC11 | Inmediata    | Devolución parcial (se aplica a cuotas pendientes)    | Diferida      | 1            | -          |
      | NC12 | Diferida     | Anulación de ítem no entregado                        | -             | -            | Credito    |

# ═══════════════════════════════════════════════════════════════════════
# NOTAS DE CRÉDITO - DESCUENTO (flujo por importe)
# ═══════════════════════════════════════════════════════════════════════

@NotaCredito
Scenario Outline: NC por descuento - <caso>
    Given el usuario accede al submodulo 'Nueva Venta'
    And crea una venta con familia "gaseosa", concepto "7753234003313", cantidad "50", documento "75893616", comprobante "BOLETA DE VENTA ELECTRONICA", serie "B002", entrega "Inmediata", pago "Completo"
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    And selecciona "Nota de crédito" en el modal de ajuste
    And selecciona tipo de nota de credito "<tipoNC>"
    And selecciona comprobante destino "NOTA DE CREDITO"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "<motivo>"
    And ingresa importe NC "<importeNC>"
    And ingresa importe detalle "<importeDetalle>" para el item
    And selecciona devolucion "<devolucion>" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema genera el comprobante de ajuste exitosamente

    Examples:
      | caso | tipoNC             | motivo                                                    | importeNC | importeDetalle | devolucion |
      | NC05 | DESCUENTO GLOBAL   | Descuento global aplicado posterior a la venta            | 10.00     | -              | Contado    |
      | NC06 | DESCUENTO POR ÍTEM | Descuento aplicado por ajuste comercial posterior         | -         | 6.00           | Credito    |
      | NC07 | DESCUENTO GLOBAL   | Descuento global aprobado posterior a la facturación      | 5.00      | -              | -          |
      | NC08 | DESCUENTO POR ÍTEM | Descuento por ajuste comercial en un ítem facturado       | -         | 3.00           | Contado    |
      | NC09 | DESCUENTO GLOBAL   | Descuento global aplicado a una venta con saldo pendiente | 4.00      | -              | -          |

# ═══════════════════════════════════════════════════════════════════════
# INCONSISTENCIAS / VALIDACIONES
# ═══════════════════════════════════════════════════════════════════════

@NotaCredito
@Inconsistencia
Scenario: INC01 - Inconsistencia importe NC mayor al importe vigente en descuento global
    Given el usuario accede al submodulo 'Nueva Venta'
    And crea una venta con familia "gaseosa", concepto "7753234003313", cantidad "50", documento "75893616", comprobante "BOLETA DE VENTA ELECTRONICA", serie "B002", entrega "Inmediata", pago "Completo"
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    # Importe NC mayor al vigente → debe rechazar
    And selecciona "Nota de crédito" en el modal de ajuste
    And selecciona tipo de nota de credito "DESCUENTO GLOBAL"
    And selecciona comprobante destino "NOTA DE CREDITO"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "Ajuste de descuento mal ingresado"
    And ingresa importe NC "99999.00"
    And hace clic en Guardar en el modal de ajuste
    Then el sistema muestra mensaje de monto mayor al total

@NotaCredito
@Inconsistencia
Scenario: INC02 - Inconsistencia bloquear NC por anulacion sin motivo o sustento
    Given el usuario accede al submodulo 'Nueva Venta'
    And crea una venta con familia "gaseosa", concepto "7753234003313", cantidad "50", documento "75893616", comprobante "BOLETA DE VENTA ELECTRONICA", serie "B002", entrega "Diferida", pago "Completo"
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    # NC sin motivo/sustento → debe bloquear
    And selecciona "Nota de crédito" en el modal de ajuste
    And selecciona tipo de nota de credito "ANULACIÓN DE LA OPERACIÓN"
    And selecciona comprobante destino "NOTA DE CREDITO"
    And selecciona serie "B002" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema bloquea el guardado del ajuste

@NotaCredito
@Inconsistencia
Scenario: INC03 - Inconsistencia bloquear NC cuando comprobante no tiene importe vigente
    # Paso 1: crear venta y agotar su importe con una NC previa
    Given el usuario accede al submodulo 'Nueva Venta'
    And crea una venta con familia "gaseosa", concepto "7753234003313", cantidad "50", documento "75893616", comprobante "BOLETA DE VENTA ELECTRONICA", serie "B002", entrega "Diferida", pago "Completo"
    When el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    And selecciona "Nota de crédito" en el modal de ajuste
    And selecciona tipo de nota de credito "ANULACIÓN DE LA OPERACIÓN"
    And selecciona comprobante destino "NOTA DE CREDITO"
    And selecciona serie "B002" en el ajuste
    And ingresa motivo o sustento "Anulación para agotar importe"
    And hace clic en Guardar en el modal de ajuste
    # Paso 2: intentar segunda NC sobre comprobante ya sin importe
    And el usuario accede al submodulo 'Ver Ventas'
    And accede a las opciones del comprobante recien registrado
    And selecciona "Nota de crédito" en el modal de ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema bloquea el guardado del ajuste

# ═══════════════════════════════════════════════════════════════════════
# PRUEBAS NO REALIZADAS
# ═══════════════════════════════════════════════════════════════════════
#
# NC02, NC10, INC04: Requieren productos parcialmente entregados
# (mitad entregados / mitad no entregados). En esta versión solo se
# permite TODOS entregados (Inmediata) o TODOS diferidos (Diferida).
#
# Módulo Almacén: no existe aún. Validaciones de stock, ingreso de
# mercadería y revocación de órdenes de salida quedan PENDIENTES.
# ═══════════════════════════════════════════════════════════════════════
