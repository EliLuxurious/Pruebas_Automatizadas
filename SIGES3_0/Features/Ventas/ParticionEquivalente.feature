@NuevaVenta
Feature: ParticionEquivalenteNuevaVenta

  Cobertura de particion equivalente y valores limite en Nueva Venta.
  Se mantienen solo flujos de Ventas y se usan fechas relativas para que la suite no quede vencida.

Background:
    Given el usuario ingresa al ambiente 'https://alpha2.newfrontdev-qa.sigesonline.com/sales/new-sales'
    When el usuario inicia sesión con usuario 'admin.ti@tsol.com' y contraseña 'calidad'
    And se descarta aviso de contrasena de Chrome si aparece

# ============================================================================
# PARTICION EQUIVALENTE + VALORES LIMITE - CREDITO
# ============================================================================

@ParticionEquivalente
Scenario Outline: Validar fecha de credito en nueva venta - <caso>
    When el usuario accede al módulo 'Ventas'
    And el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '7753234003313'
    And usuario ingresa la cantidad '2'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And el usuario configura los medios de pago 'Credito' 'false' 'NA' 'NA' 'NA' 'NA' 'NA' 'NA' '2' '4.20'
    Then el sistema valida el resultado del pago en nueva venta "<resultadoPago>"
    And ingresa la fecha de credito "<fechaCredito>"
    When hace clic en Guardar
    Then el sistema valida el resultado de venta "<resultadoVenta>"

    Examples:
      | caso  | fechaCredito | resultadoPago               | resultadoVenta                          |
      | CP092 | ayer         | -                           | inconsistencia: fecha de credito pasada |
      | CP093 | hoy          | credito configurado exitoso | guarda exitosamente                     |
      | CP094 | manana       | credito configurado exitoso | guarda exitosamente                     |

# ============================================================================
# PARTICION EQUIVALENTE + VALORES LIMITE - TOTAL CON CLIENTE VARIOS
# ============================================================================

@ParticionEquivalente
Scenario Outline: <caso> Registrar venta limite con cliente VARIOS
    # Precondiciones: registrar o reutilizar concepto vendible de S/ 1.00
    When configura la precondicion del concepto vendible para nueva venta:
      | Campo                   | Valor                              |
      | sufijo concepto         | <sufijoConcepto>                   |
      | presentacion concepto   | BOTELLAS                           |
      | precio concepto         | 1.00                               |
      | precio compra           | 1.00                               |
      | informacion adquisicion | Stock QA <caso> <sufijoConcepto>   |
      | observacion adquisicion | Stock QA <caso> <sufijoConcepto>   |
    And existe un concepto vendible para nueva venta con familia "<familia>" concepto "<codigoConcepto>" y stock minimo "<stockMinimo>"

    # Precondiciones: cargar stock desde Adquisicion para cubrir el limite
    Given Navego al módulo de 'Adquisición'
    And Entro al submódulo específico de 'Nueva Adquisición'
    When Se configuran los datos de 'Facturación':
      | Campo                 | Valor                            |
      | Documento             | NOTA DE COMPRA (INTERNA)         |
      | Proveedor             | 10759012017                      |
      | Información Adicional | Stock QA <caso> <sufijoConcepto> |
    And Se selecciona el tipo de entrega 'Inmediata'
    And Se configuran los datos de 'Entrega':
      | Campo           | Valor                    |
      | Rol             | Item Comercial           |
      | Establecimiento | RECSA - CENTRAL          |
      | Almacén         | CENTRO COMERCIAL CENTRAL |
    And Se selecciona y configura el producto a adquirir:
      | Producto              | Cantidad      | V. U |
      | <productoAdquisicion> | <stockMinimo> | 1.00 |
    And Se configuran los datos de 'Pago':
      | Campo       | Valor    |
      | Tipo        | Contado  |
      | Método      | Efectivo |
      | Observación | Stock QA |
    Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
    And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'

    # Flujo principal
    When el usuario accede al módulo 'Ventas'
    And el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia '<familia>'
    And usuario selecciona el concepto '<codigoConcepto>'
    And usuario ingresa la cantidad '<cantidadVenta>'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '00000000'
    And el usuario configura la entrega 'Inmediata' 'false'
    And el usuario configura los medios de pago 'Contado' 'false' 'transferencia_fondos' 'NA' 'NA' 'BCP|SOL|1912490779081' '<operacion>' '<montoPago>' 'NA' 'NA'
    Then el sistema valida el resultado del pago en nueva venta 'pago contado transferencia exitoso'
    When hace clic en Guardar
    Then el sistema valida el resultado de venta '<resultadoVenta>'

    Examples:
      | caso  | familia | codigoConcepto | sufijoConcepto    | productoAdquisicion                                    | stockMinimo | cantidadVenta | operacion | montoPago | resultadoVenta                      |
      | CP095 | Gaseosa | PEPSIQA095     | Pepsi chica 1 sol | PEPSIQA095\|Gaseosa Pepsi chica 1 sol K R BOTELLAS 1 ML | 750         | 699           | OP699     | 699.00    | guarda exitosamente                 |
      | CP096 | Gaseosa | PEPSIQA096     | Pepsi chica 1 sol | PEPSIQA096\|Gaseosa Pepsi chica 1 sol K R BOTELLAS 1 ML | 750         | 700           | OP700     | 700.00    | guarda exitosamente                 |
      | CP097 | Gaseosa | PEPSIQA097     | Pepsi chica 1 sol | PEPSIQA097\|Gaseosa Pepsi chica 1 sol K R BOTELLAS 1 ML | 750         | 701           | OP701     | 701.00    | inconsistencia: identificar cliente |

# ============================================================================
# PARTICION EQUIVALENTE + VALORES LIMITE - CONTINGENCIA
# ============================================================================

@ParticionEquivalente
Scenario Outline: Validar fecha de emision en venta por contingencia - <caso>
    When el usuario accede al módulo 'Ventas'
    And el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA POR CONTINGENCIA"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '7753234003313'
    And usuario ingresa la cantidad '2'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '00000000'
    And ingresa la fecha de emision "<fechaEmision>"
    And el usuario configura la entrega 'Inmediata' 'false'
    And el usuario configura los medios de pago 'Contado' 'false' 'efectivo' 'NA' 'NA' 'NA' 'NA' '20' 'NA' 'NA'
    Then el sistema valida el resultado del pago en nueva venta 'pago contado efectivo con vuelto exitoso'
    When hace clic en Guardar
    Then el sistema valida el resultado de venta "<resultadoVenta>"

    Examples:
      | caso  | fechaEmision | resultadoVenta                            |
      | CP098 | ayer         | guarda exitosamente                       |
      | CP099 | hoy          | guarda exitosamente                       |
      | CP100 | manana       | inconsistencia: contingencia fecha futura |
