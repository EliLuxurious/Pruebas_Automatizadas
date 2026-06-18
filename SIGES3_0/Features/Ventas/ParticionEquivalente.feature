@NuevaVenta
Feature: ParticionEquivalenteNuevaVenta

  Cobertura de particion equivalente y valores limite en Nueva Venta.
  Se mantienen solo flujos de Ventas y se usan fechas relativas para que la suite no quede vencida.

Background:
    Given el usuario ingresa al ambiente 'https://alpha2.newfrontdev-qa.sigesonline.com/sales/new-sales'
    When el usuario inicia sesión con usuario 'admin.ti@tsol.com' y contraseña 'calidad'
    And se descarta aviso de contrasena de Chrome si aparece
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
# PARTICION EQUIVALENTE + VALORES LIMITE - CREDITO
# ============================================================================

@ParticionEquivalente
Scenario Outline: Validar fecha de credito en nueva venta - <caso>

    When el usuario accede al módulo 'Ventas'
    And el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '2'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And el usuario configura los medios de pago 'Credito' 'false' 'NA' 'NA' 'NA' 'NA' 'NA' 'NA' '2' '4.20'
    # Se ingresa la fech    a de credito en paso separado porque el sistema solo dispara la validacion
    # "La fecha no debe ser pasada." cuando el usuario escribe la fecha con teclado (no via JS).
    # Mezclarlo dentro del paso de medios de pago romperia los casos que no usan fecha de credito.
    And ingresa la fecha de credito "<fechaCredito>"
    Then el sistema valida el resultado del pago en nueva venta "<resultadoPago>"

    When hace clic en Guardar
    Then el sistema valida el resultado de venta "<resultadoVenta>"

    Examples:
      | caso  | fechaCredito | resultadoPago                | resultadoVenta                        |
      | CP092 | ayer         | La fecha no debe ser pasada  | venta bloqueada                       |
      | CP093 | hoy          | credito configurado exitoso  | guarda exitosamente                   |
      | CP094 | manana       | credito configurado exitoso  | guarda exitosamente                   |


# BUG: PARTICION EQUIVALENTE + VALORES LIMITE - TOTAL CON CLIENTE VARIOS PARA VERIFICAR INCONCISTENCIA CUANDO CLIENTE SIN IDENTIFICAR Y IMPORTE>700
#LOS 3 casos de prueba no se pueden realizar debido a que no hay disponibilidad para conseguir conceptos con determinadas caracteristicas que puedan dar importes de: 699, 700 y 701

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
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '2'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '00000000'
    And ingresa la fecha de emision "<fechaEmision>"
    And el usuario configura la entrega 'Inmediata' 'false'
    And el usuario configura los medios de pago 'Contado' 'false' 'efectivo' 'NA' 'NA' 'NA' 'NA' '20' 'NA' 'NA'
    Then el sistema valida el resultado del pago en nueva venta "<resultadoPago>"
    When hace clic en Guardar
    Then el sistema valida el resultado de venta "<resultadoVenta>"

    Examples:
      | caso  | fechaEmision | resultadoPago                                      | resultadoVenta                                |
      | CP098 | ayer         | pago contado efectivo con vuelto exitoso           | guarda exitosamente                           |
      | CP099 | hoy          | pago contado efectivo con vuelto exitoso           | guarda exitosamente                           |
      | CP100 | manana       | pago contado efectivo con vuelto sin validar guardar | Completar los campos requeridos correctamente |
