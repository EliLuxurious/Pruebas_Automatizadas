Feature: Gestión de Clientes en Facturación Cíclica

  Background:
    Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
    When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
    And navega al módulo 'Facturación Cíclica'
    And se crea un plan de servicio válido
    And selecciona la opción 'Nuevo Cliente'

  @RegistroRuc10Factura
  Scenario: Registrar nuevo cliente con RUC 10 y Factura
    When completa la sección 'Datos Generales' con el RUC '10004259411'
    And selecciona el Ubigeo 'HUANUCO - LEONCIO PRADO' y dirección 'Jr. Rio de Janeiro Nro. 382'
    And configura la 'Facturación' con los siguientes datos:
      | Campo       | Valor               |
      | Comprobante | FACTURA ELECTRONICA |
      | Ciclo       | MENSUAL             |
      | Forma Pago  | VENCIDO             |
      | Inicio      | 2026-03-28          |
      | Plan        | AUTO                |
    Then procede a 'GUARDAR' el registro
    And debe visualizar el mensaje de éxito 'Registro correctamente'

  @RegistroRuc10Boleta
  Scenario: Registrar nuevo cliente con RUC 10 y Boleta
    When completa la sección 'Datos Generales' con el RUC '10402753710'
    And selecciona el Ubigeo 'HUANUCO - LEONCIO PRADO' y dirección 'Av. Peru 123'
    And Se expande la sección de 'Facturación'
    And selecciona el Tipo de comprobante 'BOLETA DE VENTA ELECTRONICA'
    And configura el ciclo 'MENSUAL', forma de pago 'VENCIDO' y plan 'AUTO'
    Then procede a 'GUARDAR' el registro

  @RegistroRuc20Factura
  Scenario: Registrar nuevo cliente con RUC 20 y Factura
    When completa la sección 'Datos Generales' con el RUC '20293910767'
    And selecciona el Ubigeo 'LIMA - LIMA - MIRAFLORES' y dirección 'Jr. Rio de Janeiro Nro. 382'
    And configura la 'Facturación' con los siguientes datos:
      | Campo       | Valor               |
      | Comprobante | FACTURA ELECTRONICA |
      | Ciclo       | MENSUAL             |
      | Inicio      | 2026-02-01          |
      | Plan        | AUTO                |
    Then procede a 'GUARDAR' el registro

  @RegistroRuc20Boleta
  Scenario: Registrar nuevo cliente con RUC 20 y Boleta
    When completa la sección 'Datos Generales' con el RUC '20518809271'
    And selecciona el Ubigeo 'LIMA - LIMA - MIRAFLORES' y dirección 'Jr. Rio de Janeiro Nro. 382'
    And configura la 'Facturación' con los siguientes datos:
      | Campo       | Valor                        |
      | Comprobante | BOLETA DE VENTA ELECTRONICA  |
      | Ciclo       | MENSUAL                      |
      | Inicio      | 2026-02-01                   |
      | Plan        | AUTO                        |
    Then procede a 'GUARDAR' el registro

  @RegistroDniBoleta
  Scenario: Registrar nuevo cliente con DNI y Boleta
    When completa la sección 'Datos Generales' con el DNI '75971755'
    And selecciona el Ubigeo 'LIMA - LIMA - MIRAFLORES' y dirección 'Jr. Rio de Janeiro Nro. 382'
    And ingresa correo 'agricola@gmail.com'
    And ingresa telefono '937584269'
    And configura la 'Facturación' con los siguientes datos:
      | Campo       | Valor                        |
      | Comprobante | BOLETA DE VENTA ELECTRONICA  |
      | Ciclo       | MENSUAL                      |
      | Inicio      | 2026-02-01                   |
      | Plan        | AUTO                         |
    Then procede a 'GUARDAR' el registro

@RegistroDniFacturaAdvertencia
Scenario: Intentar registrar nuevo cliente con DNI y Factura muestra advertencia
  When completa la sección 'Datos Generales' con el DNI '75423526'
  And selecciona el Ubigeo 'LIMA - LIMA - MIRAFLORES' y dirección 'Jr. Rio de Janeiro Nro. 382'
  And Se expande la sección de 'Facturación'
  And selecciona el Tipo de comprobante 'FACTURA ELECTRONICA'
  Then debe visualizar la advertencia 'Para emitir Factura Electrónica, el cliente debe tener RUC.'
  And cierra la advertencia con 'OK'

   # ================== 3. PASAPORTE ==================
  @RegistroPasaporteBoleta
  Scenario: Registrar nuevo cliente con PASAPORTE
    When completa la sección 'Datos Generales' con el Pasaporte '198765432'
    And ingresa nombres 'Maria', apellido paterno 'Gomez', apellido materno 'Rojas'
    And selecciona el Ubigeo 'LIMA - LIMA - MIRAFLORES' y dirección 'Av. Arequipa Nro. 456'
    And configura la 'Facturación' con los siguientes datos:
      | Campo       | Valor                        |
      | Comprobante | BOLETA DE VENTA ELECTRONICA  |
      | Ciclo       | MENSUAL                      |
      | Inicio      | 2026-02-01                   |
      | Plan        | AUTO                         |
    Then procede a 'GUARDAR' el registro

  # ================== 4. CARNET EXTRANJERIA ==================
  @RegistroCarnetExtranjeriaBoleta
  Scenario: Registrar nuevo cliente con CARNET DE EXTRANJERIA
    When completa la sección 'Datos Generales' con el Carnet '159123456'
    And ingresa nombres 'Luis', apellido paterno 'Torres', apellido materno 'Diaz'
    And selecciona el Ubigeo 'LIMA - LIMA - MIRAFLORES' y dirección 'Jr. Puno Nro. 789'
    And configura la 'Facturación' con los siguientes datos:
      | Campo       | Valor                        |
      | Comprobante | BOLETA DE VENTA ELECTRONICA  |
      | Ciclo       | MENSUAL                      |
      | Inicio      | 2026-02-01                   |
      | Plan        | AUTO                         |
    Then procede a 'GUARDAR' el registro

  # ================== 5. CEDULA DIPLOMATICA ==================
  @RegistroCedulaDiplomaticaBoleta
  Scenario: Registrar nuevo cliente con CED. DIPLOMATICA DE IDENTIDAD
    When completa la sección 'Datos Generales' con la Cédula '63776655'
    And ingresa nombres 'Ana', apellido paterno 'Martinez', apellido materno 'Suarez'
    And selecciona el Ubigeo 'LIMA - LIMA - SAN ISIDRO' y dirección 'Av. Javier Prado 123'
    And configura la 'Facturación' con los siguientes datos:
      | Campo       | Valor                        |
      | Comprobante | BOLETA DE VENTA ELECTRONICA  |
      | Ciclo       | MENSUAL                      |
      | Inicio      | 2026-02-01                   |
      | Plan        | AUTO                         |
    Then procede a 'GUARDAR' el registro

  # ================== 6. RESIDENCIA ==================
  @RegistroResidenciaBoleta
  Scenario: Registrar nuevo cliente con DOC.IDENT.PAIS.RESIDENCIA-NO.D
    When completa la sección 'Datos Generales' con el Doc de Residencia '7854751316'
    And ingresa nombres 'Hans', apellido paterno 'Muller', apellido materno 'Becker'
    And selecciona el Ubigeo 'LIMA - LIMA - MIRAFLORES' y dirección 'Calle Los Pinos 101'
    And configura la 'Facturación' con los siguientes datos:
      | Campo       | Valor                        |
      | Comprobante | BOLETA DE VENTA ELECTRONICA  |
      | Ciclo       | MENSUAL                      |
      | Inicio      | 2026-02-01                   |
      | Plan        | AUTO                         |
    Then procede a 'GUARDAR' el registro

  # ================== 7. SIN RUC ==================
  @RegistroSinRucBoleta
  Scenario: Registrar nuevo cliente con DOC.TRIB.NO.DOM.SIN.RUC
    When completa la sección 'Datos Generales' con el documento '4587269315'
    And ingresa nombres 'Pedro', apellido paterno 'Castillo', apellido materno 'Vargas'
    And selecciona el Ubigeo 'LIMA - LIMA - MIRAFLORES' y dirección 'Jr. Lima Nro. 321'
    And configura la 'Facturación' con los siguientes datos:
      | Campo       | Valor                        |
      | Comprobante | BOLETA DE VENTA ELECTRONICA  |
      | Ciclo       | MENSUAL                      |
      | Inicio      | 2026-02-01                   |
      | Plan        | AUTO                         |
    Then procede a 'GUARDAR' el registro

  # ================== 8. PPJJ ==================
  @RegistroPPJJBoleta
  Scenario: Registrar nuevo cliente con IDENTIFICATION NUMBER - IN – DOC TRIB PP. JJ
    When completa la sección 'Datos Generales' con el ID PPJJ '4599887766'
    And ingresa nombres 'GLOBAL TECH SOLUTIONS', apellido paterno 'CORP', apellido materno 'SAC'
    And selecciona el Ubigeo 'LIMA - LIMA - SAN BORJA' y dirección 'Av. Aviación 4567'
    And configura la 'Facturación' con los siguientes datos:
      | Campo       | Valor                        |
      | Comprobante | BOLETA DE VENTA ELECTRONICA  |
      | Ciclo       | MENSUAL                      |
      | Inicio      | 2026-02-01                   |
      | Plan        | AUTO                         |
    Then procede a 'GUARDAR' el registro

  # ================== 9. PTP ==================
  @RegistroPTPBoleta
  Scenario: Registrar nuevo cliente con PERMISO TEMPORAL DE PERMANENCIA - PTP
    When completa la sección 'Datos Generales' con el PTP '478009876'
    And ingresa nombres 'Carlos', apellido paterno 'Mendoza', apellido materno 'Rivas'
    And selecciona el Ubigeo 'LIMA - LIMA - SURCO' y dirección 'Calle El Polo 123'
    And configura la 'Facturación' con los siguientes datos:
      | Campo       | Valor                        |
      | Comprobante | BOLETA DE VENTA ELECTRONICA  |
      | Ciclo       | MENSUAL                      |
      | Inicio      | 2026-02-01                   |
      | Plan        | AUTO                         |
    Then procede a 'GUARDAR' el registro

  # ================== 10. SALVOCONDUCTO ==================
  @RegistroSalvoconductoBoleta
  Scenario: Registrar nuevo cliente con SALVOCONDUCTO
    When completa la sección 'Datos Generales' con el Salvoconducto '78112233'
    And ingresa nombres 'Elena', apellido paterno 'Guerra', apellido materno 'Paz'
    And selecciona el Ubigeo 'LIMA - LIMA - MIRAFLORES' y dirección 'Av. Larco 900'
    And configura la 'Facturación' con los siguientes datos:
      | Campo       | Valor                        |
      | Comprobante | BOLETA DE VENTA ELECTRONICA  |
      | Ciclo       | MENSUAL                      |
      | Inicio      | 2026-02-01                   |
      | Plan        | AUTO                         |
    Then procede a 'GUARDAR' el registro

  # ================== 11. TAM ==================
  @RegistroTAMBoleta
  Scenario: Registrar nuevo cliente con TAM - TARJETA ANDINA DE MIGRACIÓN
    When completa la sección 'Datos Generales' con la TAM '785554433'
    And ingresa nombres 'Ricardo', apellido paterno 'Quispe', apellido materno 'Mamani'
    And selecciona el Ubigeo 'LIMA - LIMA - MIRAFLORES' y dirección 'Av. Tacna 450'
    And configura la 'Facturación' con los siguientes datos:
      | Campo       | Valor                        |
      | Comprobante | BOLETA DE VENTA ELECTRONICA  |
      | Ciclo       | MENSUAL                      |
      | Inicio      | 2026-02-01                   |
      | Plan        | AUTO                         |
    Then procede a 'GUARDAR' el registro

  # ================== 12. PPNN ==================
  @RegistroPPNNBoleta
  Scenario: Registrar nuevo cliente con TAX IDENTIFICATION NUMBER - TIN – DOC TRIB PP.NN
    When completa la sección 'Datos Generales' con el TIN PPNN '789112233'
    And ingresa nombres 'Sofia', apellido paterno 'Villalba', apellido materno 'Lara'
    And selecciona el Ubigeo 'LIMA - LIMA - MIRAFLORES' y dirección 'Calle Alcanfores 567'
    And configura la 'Facturación' con los siguientes datos:
      | Campo       | Valor                        |
      | Comprobante | BOLETA DE VENTA ELECTRONICA  |
      | Ciclo       | MENSUAL                      |
      | Inicio      | 2026-02-01                   |
      | Plan        | AUTO                         |
    Then procede a 'GUARDAR' el registro

# ================== 13. RUC 10 + FACTURA (CON DATOS COMPLETOS) ==================
@RegistroRuc10FacturaCompleto
Scenario: Registrar nuevo cliente con RUC 10 y Factura con datos obligatorios y no obligatorios
  When completa la sección 'Datos Generales' con el RUC '10004259411'
  And selecciona el Ubigeo 'HUANUCO - LEONCIO PRADO' y dirección 'Jr. Rio de Janeiro Nro. 382'
  And configura la 'Facturación' con los siguientes datos:
    | Campo       | Valor               |
    | Comprobante | FACTURA ELECTRONICA |
    | Ciclo       | MENSUAL             |
    | Forma Pago  | VENCIDO             |
    | Inicio      | 2026-03-28          |
    | Plan        | AUTO                |

  And el usuario da click en la sección "Credenciales SOL"
  And ingresa el Usuario SOL Primario "MODDATOS" y Contraseña "MODDATOS"
  And ingresa el Usuario SOL Secundario "FACTURA_TEST" y Contraseña "Prueba2024*"

  And el usuario da click en la sección "Guías de remisión y OSE"
  And ingresa el Usuario de Guías de Remisión "GRE_PROYECTO_9" y Clave "GuiaPass123!"
  And ingresa el Usuario OSE "20123456789USERTEST" y Clave "OSE_Secret_Key_99"

  And el usuario da click en la sección "Configuración Adicional"
  And ingresa el Usuario AnyDesk "987 654 321" y Clave "Support_UNAS_2026"
  And ingresa el Tenant ID "dev-client-alpha-001"

  Then procede a 'GUARDAR' el registro
  And debe visualizar el mensaje de éxito 'Registro correctamente'