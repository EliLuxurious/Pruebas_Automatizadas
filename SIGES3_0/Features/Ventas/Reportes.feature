@NuevaVenta
Feature: Reportes
  Como usuario administrador
  Quiero acceder al menu de Reportes de Ventas
  Para validar la funcionalidad de generacion de reportes por Comprobantes y Conceptos

Background:
    Given el usuario ingresa al ambiente 'https://alpha2.newfrontdev-qa.sigesonline.com/sales/new-sales'
    When el usuario inicia sesión con usuario 'admin.ti@tsol.com' y contraseña 'calidad'
    And se descarta aviso de contrasena de Chrome si aparece
    And el usuario accede al módulo 'Ventas'
    # ITEMS Y ADQUISICION COMENTADOS: el concepto 123456789 (Gaseosa Inca kola) fue creado directamente
    # por base de datos debido a que Cotizacion no logra guardar, por lo que el flujo de creacion
    # de items y carga de stock via adquisicion ya no es necesario como precondicion.
    #Given existe el concepto item comercial para ventas:
    #  | Familia | TipoFamilia | TratamientoIGVFamilia | CodigoFamilia | CategoriaFamilia | Codigo    | Sufijo            | UMComercial | UMedida | Rol            | Modulo  | Marca | Presentacion | Cantidad | UnidadMedida | Tarifa     | Precio |
    #  | Gaseosa | Bien        | Exoneracion IGV       | QA-GASEOSA    | SIN CATEGORÍA    | 123456789 | Gaseosa Inca kola | UN          | UN      | Item Comercial | MOD0001 |       | BOTELLAS     | 1        | UN           | POR UNIDAD | 2.30   |

    #Given Navego al módulo de 'Adquisición'
    #And Entro al submódulo específico de 'Nueva Adquisición'
    #When Se configuran los datos de 'Facturación':
    #  | Campo                 | Valor                    |
    #  | Documento             | FACTURA ELECTRONICA      |
    #  | Serie                 | F001                     |
    #  | Correlativo           | 00009991                 |
    #  | Fecha de emisión      | 04/03/2026               |
    #  | Proveedor             | 10759012017              |
    #  | Información Adicional | Precondición Inka Kola   |
    #And Se selecciona el tipo de entrega 'Inmediata'
    #And Se configuran los datos de Entrega de Ventas:
    #  | Campo           | Valor                    |
    #  | Rol             | Item Comercial           |
    #  | Establecimiento | SIGES - CENTRAL          |
    #  | Almacén         | SIGES - CASTILLO GRANDE  |
    #And Se selecciona y configura el producto de ventas a adquirir:
    #  | Producto                     | Cantidad | V. U |
    #  | 123456789\|Gaseosa Inca kola | 15       | 2.00 |
    #Then Se procede a guardar la adquisición mediante la acción 'SavePurchase'
    #And Se confirma el registro exitoso con el mensaje 'Se registró correctamente.'
    And el usuario accede al submodulo 'Nueva Venta'
@Reportes
@FiltroFechas
Scenario Outline: Validar filtro de fechas en reporte de ventas
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
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Comprobantes"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    Then el sistema valida el comportamiento esperado de la fecha final "<resultadoEsperado>"

    Examples:
      | Caso  | fechaHoraInicial       | fechaHoraFinal         | resultadoEsperado                         |
      | CP110 | ayer 01:15 am          | hace 2 dias 10:45 pm   | No permite aplicar el filtro Inhabilitado |
      | CP111 | hace 20 dias 01:15 am  | hace 5 dias 10:45 pm   | Aplica el filtro correctamente            |
      | CP112 | hace 10 dias 10:15 pm  | hace 10 dias 10:45 pm  | Aplica el filtro correctamente            |

@Reportes
@PorComprobante
Scenario Outline: <Caso> Generar reporte por comprobante
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion '<TipoComprobante>' '<Serie>' '<ClienteVenta>'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "<ResultadoVenta>"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Comprobantes"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el tipo de comprobante "<TipoComprobante>"
    And selecciona la serie "<Serie>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Comprobante"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | TipoComprobante             | Serie | ClienteVenta | ResultadoVenta       | fechaHoraInicial      | fechaHoraFinal       |
      | CP068 | BOLETA DE VENTA ELECTRONICA | B002  | 75893616     | guarda exitosamente  | ayer 01:15 am | hoy 10:45 pm |
      | CP082 | FACTURA ELECTRONICA         | F002  | 20542245671  | guarda exitosamente  | ayer 01:15 am | hoy 10:45 pm |
      | CP084 | NOTA DE VENTA(INTERNA)      | NV02  | 00000000     | guarda exitosamente  | ayer 01:15 am | hoy 10:45 pm |

  @Reportes
  @PorComprobante
  @NotaCredito
  Scenario Outline: <Caso> Generar reporte por comprobante de nota de credito
    # Precondiciones
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '<ConceptoVenta>'
    And usuario ingresa la cantidad '1'
    And configura la facturacion '<ComprobanteVenta>' '<SerieVenta>' '<ClienteVenta>'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Ver Ventas'
    And abre el modal ajustes de comprobante
    And selecciona la opcion "Nota de credito" del modal ajustes de comprobante
    And selecciona tipo de nota de credito "ANULACIÓN DE LA OPERACIÓN"
    And selecciona serie "<SerieReporte>" en el ajuste
    And ingresa motivo o sustento "Precondicion reporte nota de credito"
    And selecciona entrega "Diferida" en el ajuste
    And selecciona devolucion "Contado" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema genera el comprobante de ajuste exitosamente

    # Flujo principal
    When el usuario accede al submodulo 'Reportes'
    And selecciona la vista "Comprobantes"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el tipo de comprobante "<TipoComprobanteReporte>"
    And selecciona la serie "<SerieReporte>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Comprobante"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | ConceptoVenta | ComprobanteVenta            | SerieVenta | ClienteVenta | TipoComprobanteReporte | SerieReporte | fechaHoraInicial | fechaHoraFinal |
      | CP082 | 123456789 | BOLETA DE VENTA ELECTRONICA | B002       | 75893616     | NOTA DE CREDITO         | B002         | ayer 01:15 am    | hoy 10:45 pm   |

  @Reportes
  @PorComprobante
  @NotaDebito
  Scenario Outline: Generar reporte por comprobante de nota de debito
    # Precondiciones
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '<ConceptoVenta>'
    And usuario ingresa la cantidad '1'
    And configura la facturacion '<ComprobanteVenta>' '<SerieVenta>' '<ClienteVenta>'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Ver Ventas'
    And abre el modal ajustes de comprobante
    And selecciona la opcion "Nota de debito" del modal ajustes de comprobante
    And selecciona tipo de nota de debito "<TipoNotaDebito>"
    And selecciona serie "<SerieReporte>" en el ajuste
    And ingresa motivo o sustento "<motivo>"
    And ingresa total aumento del valor "<aumento>"
    And expande la seccion "Pago" del ajuste
    And selecciona tipo de pago "Contado" en la seccion pago del ajuste
    And selecciona medio de pago "Efectivo" en el ajuste
    And ingresa observacion "<observacion>" en el ajuste
    And hace clic en Guardar en el modal de ajuste
    Then el sistema genera el comprobante de ajuste exitosamente

    # Flujo principal
    When el usuario accede al submodulo 'Reportes'
    And selecciona la vista "Comprobantes"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el tipo de comprobante "<TipoComprobanteReporte>"
    And selecciona la serie "<SerieReporte>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Comprobante"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | ConceptoVenta | ComprobanteVenta    | SerieVenta | ClienteVenta | TipoNotaDebito       | motivo                         | aumento | observacion     | TipoComprobanteReporte | SerieReporte | fechaHoraInicial | fechaHoraFinal |
      | CP083 | 123456789     | FACTURA ELECTRONICA | F002       | 20542245671  | AUMENTO EN EL VALOR  | Aumento en el valor de prueba  | 3.00    | Pago contado ND | NOTA DE DEBITO          | B002         | ayer 01:15 am    | hoy 10:45 pm   |

####################################################################
##################### SERIES
####################################################################
@Reportes
@PorSerie
Scenario Outline: Generar reporte por serie
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
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Series"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el comprobante y serie "<ComprobanteSerie>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Serie"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | ComprobanteSerie | fechaHoraInicial      | fechaHoraFinal  |
      | CP0000 | 03 : B002        | hace 8 dias 01:15 am  | hoy 10:45 pm    |

####################################################################
##################### CONCEPTOS
####################################################################
@Reportes
@PorConceptos
@Sinfiltro
Scenario Outline: Generar reporte por conceptos (sin filtro adicional)
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia '<FamiliaVenta>'
    And usuario selecciona el concepto '<ConceptoVenta>'
    And usuario ingresa la cantidad '<CantidadVenta>'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '<ClienteVenta>'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
      # Flujo principal
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Conceptos"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el punto de venta en reporte "<PuntoVenta>"
    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso   | PuntoVenta              | Tarjeta                    | FamiliaVenta   | ConceptoVenta | CantidadVenta | ClienteVenta  | fechaHoraInicial    | fechaHoraFinal  |
      | CP086  | SIGES - CASTILLO GRANDE | Por Familia y Serie        | Gaseosa        | 123456789     | 1             | 75893616      | ayer 01:15 am       | hoy 10:45 pm    |
      | CP087  | SIGES - CASTILLO GRANDE | Por Categoría y Serie      | Gaseosa        | 123456789     | 1             | 75893616      | ayer 01:15 am       | hoy 10:45 pm    |
      | CP088  | SIGES - CASTILLO GRANDE | Según Horario              | Gaseosa        | 123456789     | 1             | 75893616      | ayer 01:15 am       | hoy 10:45 pm    |
      # CP071: Se ejecuta sin seleccionar 'Puntos de venta no vigentes' ya que no existen puntos de venta deshabilitados en el ambiente.
      | CP071  | SIGES - CASTILLO GRANDE | Por Centro de Atención y Serie | Gaseosa    | 123456789     | 1             | 75893616      | ayer 01:15 am       | hoy 10:45 pm    |
      # NOTA: CP089 ICBPER requiere un concepto con impuesto ICBPER (ej. bolsa plástica) registrado en la familia 'Bolsa Plastica', Verificar que exista el concepto 895674556789 con ICBPER activo antes de ejecutar para que la prueba no falle
      | CP089  | SIGES - CASTILLO GRANDE | Por Comprobante con ICBPER | Bolsa Plastica | 895674556789  | 1             | 00000000      | ayer 01:15 am       | hoy 10:45 pm    |

@Reportes
@PorConceptos
@PorFamilia
Scenario Outline: Generar reporte por familia en la vista Conceptos
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia '<Familia>'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
      # Flujo principal
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Conceptos"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el punto de venta en reporte "<PuntoVenta>"
    And selecciona la familia "<Familia>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Familia"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | PuntoVenta               | Familia | fechaHoraInicial      | fechaHoraFinal       |
      | CP070 | SIGES - CASTILLO GRANDE | Gaseosa | ayer 01:15 am | hoy 10:45 pm |

@Reportes
@PorConceptos
@PorCaracteristica
Scenario Outline: Generar reporte por característica en la vista Conceptos
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
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Conceptos"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el punto de venta en reporte "<PuntoVenta>"
    And selecciona la caracteristica "<Caracteristica>" en la tarjeta "<Tarjeta>"
    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | PuntoVenta               | Caracteristica | Tarjeta                                       | fechaHoraInicial      | fechaHoraFinal       |
      | CP085 | SIGES - CASTILLO GRANDE  | MARCA          | POR CONCEPTO, CARACTERISTICAS Y FORMA DE PAGO | ayer 01:15 am | hoy 10:45 pm |
      | CP086 | SIGES - CASTILLO GRANDE  | TAMAÑO         | POR CARACTERISTICAS                           | ayer 01:15 am | hoy 10:45 pm |

####################################################################
##################### VENDEDOR
####################################################################
@Reportes
@PorVendedor
Scenario Outline: Generar reporte por vendedor en la vista Vendedor
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA MODO CAJA"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia '<Familia>'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And selecciona el punto de venta 'SIGES - CASTILLO GRANDE'
    And selecciona el vendedor '<Vendedor>'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Vendedor"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el vendedor en reporte "<Vendedor>"
    And selecciona "<Familia>" en el filtro "Familias" de la tarjeta "Por Vendedor"
    And selecciona "<Concepto>" en el filtro "Conceptos" de la tarjeta "Por Vendedor"
    And hace clic en "VER REPORTE" en la tarjeta "Por Vendedor"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | Vendedor                  | Familia | Concepto  | fechaHoraInicial      | fechaHoraFinal       |
      | CP090 | FRANKLIN MARTINEZ HURTADO | Gaseosa | Inca Kola | ayer 01:15 am | hoy 10:45 pm |

@Reportes
@PorVendedor
@PorModalidadConcepto
Scenario Outline: Generar reporte por modalidad y concepto en la vista Vendedor
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "<Modalidad>"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Vendedor"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el vendedor en reporte "<Vendedor>"
    And selecciona "<Modalidad>" en el filtro "Modalidad" de la tarjeta "Por Modalidad y Concepto"
    And hace clic en "VER REPORTE" en la tarjeta "Por Modalidad y Concepto"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | Vendedor                  | Modalidad    | fechaHoraInicial      | fechaHoraFinal       |
      | CP074 | FRANKLIN MARTINEZ HURTADO | VENTA NORMAL | ayer 01:15 am | hoy 10:45 pm |

@Reportes
@PorVendedor
@PorFamiliaVendedor
Scenario Outline: Generar reporte por familia y vendedor en la vista Vendedor
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA MODO CAJA"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And selecciona el punto de venta 'SIGES - CASTILLO GRANDE'
    And selecciona el vendedor '<Vendedor>'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Vendedor"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el vendedor en reporte "<Vendedor>"
    And hace clic en "VER REPORTE" en la tarjeta "Por Familia y Vendedor"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | Vendedor                  | fechaHoraInicial      | fechaHoraFinal       |
      | CP075 | FRANKLIN MARTINEZ HURTADO | ayer 01:15 am | hoy 10:45 pm |


####################################################################
##################### GRUPOS
####################################################################
@Reportes
@PorGrupos
Scenario Outline: Generar reporte en la vista Grupos
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA MODO CAJA"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion 'BOLETA DE VENTA ELECTRONICA' 'B002' '75893616'
    And selecciona el punto de venta '<PuntoVentaNV>'
    And selecciona el vendedor 'FRANKLIN MARTINEZ HURTADO'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Grupos"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el establecimiento "<Establecimiento>"
    And selecciona el punto de venta en reporte "<PuntoVentaReporte>"
    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | Establecimiento | PuntoVentaNV             | PuntoVentaReporte | Tarjeta             | fechaHoraInicial | fechaHoraFinal |
      | CP076 | SIGES - CENTRAL | SIGES - CASTILLO GRANDE  | SIGES - CENTRAL   | Por Grupo           | ayer 01:15 am    | hoy 10:45 pm |
      | CP077 | SIGES - CENTRAL | SIGES - CASTILLO GRANDE  | SIGES - CENTRAL   | Por Familia y Grupo | ayer 01:15 am    | hoy 10:45 pm |
      | CP078 | SIGES - CENTRAL | SIGES - CASTILLO GRANDE  | SIGES - CENTRAL   | Por Grupo Detallado | ayer 01:15 am    | hoy 10:45 pm |

@Reportes
@PorExcepciones
Scenario Outline: Generar reporte en la vista Excepciones
    When el usuario accede al submodulo 'Nueva Venta'
    And selecciona el modo de venta "VENTA NORMAL"
    And configura IGV "N" y Detalle Unificado "N"
    And el usuario selecciona la familia 'Gaseosa'
    And usuario selecciona el concepto '123456789'
    And usuario ingresa la cantidad '1'
    And configura la facturacion '<ComprobanteVenta>' '<SerieVenta>' '<ClienteVenta>'
    And el usuario configura la entrega 'Inmediata' 'false'
    And configura el pago "Contado"
    And hace clic en Guardar
    Then el sistema valida el resultado de venta "guarda exitosamente"
    When el usuario accede al submodulo 'Reportes'
    When selecciona la vista "Excepciones"
    And el usuario selecciona la fecha y hora inicial "<fechaHoraInicial>" en el campo "Fecha y Hora Inicial"
    And el usuario selecciona la fecha y hora final "<fechaHoraFinal>" en el campo "Fecha y Hora Final"
    And selecciona el punto de venta en reporte "<PuntoVenta>"
    And hace clic en "VER REPORTE" en la tarjeta "<Tarjeta>"
    Then el sistema genera el reporte exitosamente

    Examples:
      | Caso  | PuntoVenta              | Tarjeta              | ComprobanteVenta            | SerieVenta | ClienteVenta | fechaHoraInicial      | fechaHoraFinal       |
      | CP079 | SIGES - CASTILLO GRANDE | Por Notas de Credito | BOLETA DE VENTA ELECTRONICA | B002       | 75893616     | ayer 01:15 am | hoy 10:45 pm |
      | CP080 | SIGES - CASTILLO GRANDE | Por Invalidaciones   | BOLETA DE VENTA ELECTRONICA | B002       | 75893616     | ayer 01:15 am | hoy 10:45 pm |
      | CP081 | SIGES - CASTILLO GRANDE | Por Notas de Debito  | FACTURA ELECTRONICA         | F002       | 20542245671  | ayer 01:15 am | hoy 10:45 pm |
