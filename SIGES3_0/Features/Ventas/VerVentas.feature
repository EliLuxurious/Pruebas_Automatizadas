Feature: VerVentas

Cobertura base para acciones disponibles en ver ventas.

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
    And el usuario accede al módulo 'Ventas'
    And el usuario accede al submodulo 'Ver Ventas'

@CanjearNV
# CP039: Se crean 2 NV con cliente VARIOS como precondicion para garantizar
# que existan notas de venta canjeables en el ambiente sin depender de fechas fijas.
Scenario: CP039 Canjear 2 NV con comprobante BOLETA cliente VARIOS
    # Precondicion: crear 2 Notas de Venta con cliente VARIOS
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'NOTA DE VENTA(INTERNA)' 'NV02' '00000000'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    And el usuario accede al submodulo 'Ver Ventas'
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'NOTA DE VENTA(INTERNA)' 'NV02' '00000000'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    # Canjear las 2 NV recien creadas
    When el usuario accede al submodulo 'Ver Ventas'
    And el usuario selecciona la fecha y hora inicial "ayer" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "hoy" en el campo "Fecha y Hora Final"
    And hace clic en consultar ventas
    And filtra por tipo de documento "NV"
    And activa el modo canje
    And selecciona 2 notas de venta
    And hace clic en el boton Canjear
    And selecciona el comprobante "BOLETA DE VENTA ELECTRONICA" en el modal de canje
    And selecciona la serie "B002" en el modal de canje
    And confirma el canje
    Then el sistema genera el canje exitosamente

@CanjearNV
# CP040: Se crean 2 NV con cliente DNI 75893616 como precondicion para garantizar
# que existan notas de venta canjeables en el ambiente sin depender de fechas fijas.
Scenario: CP040 Canjear 2 NV con comprobante BOLETA cliente DNI
    # Precondicion: crear 2 Notas de Venta con cliente DNI
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'NOTA DE VENTA(INTERNA)' 'NV02' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'NOTA DE VENTA(INTERNA)' 'NV02' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    # Canjear las 2 NV recien creadas
    When el usuario accede al submodulo 'Ver Ventas'
    And el usuario selecciona la fecha y hora inicial "ayer" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "hoy" en el campo "Fecha y Hora Final"
    And hace clic en consultar ventas
    And filtra por tipo de documento "NV"
    And activa el modo canje
    And selecciona 2 notas de venta
    And hace clic en el boton Canjear
    And selecciona el comprobante "BOLETA DE VENTA ELECTRONICA" en el modal de canje
    And selecciona la serie "B002" en el modal de canje
    And confirma el canje
    Then el sistema genera el canje exitosamente

@CanjearNV
# CP041: Se crea 1 NV con cliente RUC 20542245671 como precondicion para garantizar
# que exista una nota de venta canjeable en el ambiente sin depender de fechas fijas.
Scenario: CP041 Canjear 1 NV con comprobante FACTURA cliente RUC
    # Precondicion: crear 1 Nota de Venta con cliente RUC
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'NOTA DE VENTA(INTERNA)' 'NV02' '20542245671'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    # Canjear la NV recien creada
    When el usuario accede al submodulo 'Ver Ventas'
    And el usuario selecciona la fecha y hora inicial "ayer" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "hoy" en el campo "Fecha y Hora Final"
    And hace clic en consultar ventas
    And filtra por tipo de documento "NV"
    And activa el modo canje
    And selecciona 1 notas de venta
    And hace clic en el boton Canjear
    And selecciona el comprobante "FACTURA ELECTRONICA" en el modal de canje
    And selecciona la serie "F001" en el modal de canje
    And confirma el canje
    Then el sistema genera el canje exitosamente

@CanjearNV
@SinSeleccion
Scenario: CP042 Verificar que el boton Canjear esta deshabilitado sin NVs seleccionadas
    When el usuario selecciona la fecha y hora inicial "ayer" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "hoy" en el campo "Fecha y Hora Final"
    And hace clic en consultar ventas
    And activa el modo canje
    Then el boton Canjear permanece deshabilitado
#
#@CanjearNV
#@MismoCliente
#Scenario Outline: <Caso> Canjear NV con comprobante cliente sin restriccion de mismo cliente
#    When el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
#    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
#    And hace clic en consultar ventas
#    And activa el modo canje
#    And selecciona <CantidadNV> notas de venta
#    And hace clic en el boton Canjear
#    And selecciona el comprobante "<Comprobante>" en el modal de canje
#    And selecciona la serie "<Serie>" en el modal de canje
#    And confirma el canje
#    Then el sistema genera el canje exitosamente
#
#    Examples:
#      | Caso  | TipoCliente | CantidadNV | Comprobante                 | Serie | fechaHoraInicial    | fechaHoraFinal      |
#      | CP043 | RUC         | 2          | FACTURA ELECTRONICA         | F002  | 05/03/2026 12:00 am | 25/03/2026 11:59 pm |
#      | CP044 | DNI         | 2          | BOLETA DE VENTA ELECTRONICA | B002  | 05/03/2026 12:00 am | 25/03/2026 11:59 pm |
#
#@CanjearNV
#@Inconsistencia
#Scenario Outline: Canjear NV con comprobante genera inconsistencia
#    When el usuario ingresa la fecha y hora inicial "<fechaHoraInicial>"
#    And el usuario ingresa la fecha y hora final "<fechaHoraFinal>"
#    And hace clic en consultar ventas
#    And activa el modo canje
#    And selecciona <CantidadNV> notas de venta
#    And hace clic en el boton Canjear
#    And selecciona el comprobante "<Comprobante>" en el modal de canje
#    And selecciona la serie "<Serie>" en el modal de canje
#    Then el sistema muestra una advertencia de inconsistencia
#    And el boton Aceptar permanece deshabilitado
#
#    Examples:
#      | Caso  | CantidadNV | Comprobante                 | Serie | fechaHoraInicial    | fechaHoraFinal      |
#      | CP045 | 2          | BOLETA DE VENTA ELECTRONICA | B002  | 05/03/2026 12:00 am | 25/03/2026 11:59 pm |
#      | CP046 | 2          | FACTURA ELECTRONICA         | F001  | 05/03/2026 12:00 am | 25/03/2026 11:59 pm |
