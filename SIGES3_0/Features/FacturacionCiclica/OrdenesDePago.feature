Feature: Gestión de Ordenes de Pago en Formato Estándar

Background:
    Given el usuario ingresa al ambiente "https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login"
    When el usuario inicia sesión con usuario "pamela.tone@recsa.com" y contraseña "calidad"
    And navega al módulo "Facturación Cíclica"
    And accede a la pestaña "Ordenes de Pago"
    # And busca la orden con ID "B002-443"
    # And abre el detalle de la orden "B002-443"

@CompartirOrdenPago
Scenario: Compartir orden de pago
    When hace clic en el botón "Compartir"
    Then el sistema muestra las opciones de compartir

@ImprimirOrdenPago
Scenario: Imprimir orden de pago
    When hace clic en el botón "Imprimir"
    Then el sistema genera la impresión de la orden de pago

@DescargarOrdenPago
Scenario: Descargar orden de pago
    When hace clic en el botón "Descargar"
    Then el sistema descarga la orden de pago correctamente
    
@FormatosBoletadeVenta
Scenario Outline: Acciones de orden de pago en formato específico
    When selecciona el formato "<Formato>"
    And hace clic en el botón "<Accion>" en la sección de formatos
    Then el sistema procesa la "<Accion>" en formato "<Formato>"

Examples:
| Formato | Accion    |
| A4      | Imprimir  |
| A4      | Descargar |
| 80mm    | Imprimir  |
| 80mm    | Descargar |
| Ticket  | Compartir |

@OrdenPagoManual(no)
Scenario: Validación de flujo feliz de pago manual (Pendiente a Facturado

# 🟢 ADMIN genera orden de pago manual
And filtro las órdenes pendientes
And genero la orden de pago

# 🟢 CLIENTE realiza pago
Given el usuario ingresa al ambiente "https://cliente3.newfrontdev-qa.sigesonline.com/user-account/my-account"
When el usuario inicia sesión con usuario "pamela.tone@recsa.com" y contraseña "calidad"
And reviso la notificación en la campanita
And selecciono la orden generada
And hago clic en "Realizar Pago"
And elijo el método "Pago Manual"
And adjunto el archivo "comprobante.pdf"
And ingreso el número de operación "123456"
And envío el pago
And confirmo el mensaje de operación

# 🔔 VALIDACIÓN CLIENTE
And vuelvo a revisar la notificación
Then el estado del pago debe estar en proceso

# 🔵 ADMIN aprueba pago
Given el usuario ingresa al ambiente "https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login"
When el usuario inicia sesión con usuario "pamela.tone@recsa.com" y contraseña "calidad"
And navega al módulo "Facturación Cíclica"
And accede a la pestaña "Ordenes de Pago"
And configuro la paginación a 100
And busco al cliente "ASOCIACION AGROPECUARIA DE PRODUCTORES DEL NORTE"
And accedo al detalle del cliente
And valido el documento
And confirmo el mensaje de operación
And hago clic en "Revisar Pago"
And apruebo el pago

# 🟢 VALIDACIÓN FINAL

And configuro la paginación a 100
And busco al cliente "ASOCIACION AGROPECUARIA DE PRODUCTORES DEL NORTE"

 # 🔵 ADMIN revisa y aprueba pago
Given el usuario ingresa al ambiente "https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login"
When el usuario inicia sesión con usuario "pamela.tone@recsa.com" y contraseña "calidad"
And navega al módulo "Facturación Cíclica"
And accede a la pestaña "Ordenes de Pago"
And configuro la paginación a 100
And busco al cliente "ASOCIACION AGROPECUARIA DE PRODUCTORES DEL NORTE"
And accedo al detalle del cliente
And hago clic en "Revisar Pago"
And apruebo el pago
Then confirmo el mensaje de operación

# ✅ VALIDACIÓN FINAL CLIENTE (NO SE HARA)
# Given el usuario ingresa al ambiente "https://cliente3.newfrontdev-qa.sigesonline.com/user-account/my-account"
# When el usuario inicia sesión como cliente con usuario "pamela.tone@recsa.com" y contraseña "calidad"
# And navega al módulo "Facturación y Pagos"
# And accede a la sección "Historial"
# Then la orden debe figurar con estado "Facturado"

#-----------------------------------------------------------------------------------------

@OrdenPagoManual @RechazoComprobante
Scenario: Rechazo de comprobante por el administrador

# 🟢 ADMIN genera orden de pago manual
And filtro las órdenes pendientes
And genero la orden de pago

# 🟢 CLIENTE realiza pago
Given el usuario ingresa al ambiente "https://cliente7.newfrontdev-qa.sigesonline.com/user-account/my-account"
When el usuario inicia sesión con usuario "pamela.tone@recsa.com" y contraseña "calidad"
And reviso la notificación en la campanita
And selecciono la orden generada
And hago clic en "Realizar Pago"
And elijo el método "Pago Manual"
And adjunto el archivo "comprobante.pdf"
And ingreso el número de operación "123456"
And envío el pago
And confirmo el mensaje de operación

# 🔔 VALIDACIÓN CLIENTE
And vuelvo a revisar la notificación
Then el estado del pago debe estar en proceso

# 🔵 ADMIN desaprueba pago
Given el usuario ingresa al ambiente "https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login"
When el usuario inicia sesión con usuario "pamela.tone@recsa.com" y contraseña "calidad"
And navega al módulo "Facturación Cíclica"
And accede a la pestaña "Ordenes de Pago"
And configuro la paginación a 100
And busco al cliente "ANA MARIA DOMINGUEZ SANDOVAL"
And accedo al detalle del cliente
And valido el documento
And confirmo el mensaje de operación
And rechazo el pago
Then confirmo el mensaje de operación


# 🟢 VALIDACIÓN CLIENTE
Given el usuario ingresa al ambiente "https://cliente7.newfrontdev-qa.sigesonline.com/user-account/my-account"
When el usuario inicia sesión con usuario "pamela.tone@recsa.com" y contraseña "calidad"
And reviso la notificación en la campanita
Then debe mostrarse el mensaje de rechazo en la campanita

#-------------------------------------------------------------- (VERA SI SE HACE)
@OrdenPagoManual @OrdenVencida
Scenario: Pago manual de orden vencida

# 🟢 CLIENTE realiza pago
Given el usuario ingresa al ambiente "https://cliente1.newfrontdev-qa.sigesonline.com/user-account/my-account"
When el usuario inicia sesión con usuario "pamela.tone@recsa.com" y contraseña "calidad"
And reviso la notificación en la campanita
And selecciono la orden generada
And hago clic en "Realizar Pago"
And elijo el método "Pago Manual"
And adjunto el archivo "comprobante.pdf"
And ingreso el número de operación "123456"
And envío el pago
And confirmo el mensaje de operación

# 🔔 VALIDACIÓN CLIENTE
And vuelvo a revisar la notificación
Then el estado del pago debe estar en proceso

# 🔵 ADMIN aprueba pago
Given el usuario ingresa al ambiente "https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login"
When el usuario inicia sesión con usuario "pamela.tone@recsa.com" y contraseña "calidad"
And navega al módulo "Facturación Cíclica"
And accede a la pestaña "Ordenes de Pago"
And configuro la paginación a 100
And busco al cliente "ASOCIACION AGROPECUARIA DE PRODUCTORES DEL NORTE"
And accedo al detalle del cliente
And valido el documento
And confirmo el mensaje de operación
And hago clic en "Revisar Pago"
And apruebo el pago

#----------------------------------------------------------------------- (ya esta hecho)


@CP042 @PagoAutomatico @Pasarela
Scenario: Pago automático validado por pasarela

# 🟢 ADMIN genera orden de pago
And filtro las órdenes pendientes
And genero la orden de pago

# 🟢 CLIENTE selecciona pago por pasarela
Given el usuario ingresa al ambiente "https://cliente7.newfrontdev-qa.sigesonline.com/user-account/my-account"
When el usuario inicia sesión con usuario "pamela.tone@recsa.com" y contraseña "calidad"
And reviso la notificación en la campanita
And selecciono la orden generada
And hago clic en "Realizar Pago"
And elijo el método "Pasarela"
And proceso el pago por pasarela
And confirmo el mensaje de operación

# 🔔 VALIDACIÓN CLIENTE
And vuelvo a revisar la notificación
Then la orden debe quedar pagada exitosamente

@CP043 @OrdenPagoManual @RegistroVoucher
Scenario: Registro de pago manual con carga de voucher

# 🟢 ADMIN genera orden de pago
And filtro las órdenes pendientes
And genero la orden de pago

# 🟢 CLIENTE registra pago manual
Given el usuario ingresa al ambiente "https://cliente7.newfrontdev-qa.sigesonline.com/user-account/my-account"
When el usuario inicia sesión con usuario "pamela.tone@recsa.com" y contraseña "calidad"
And reviso la notificación en la campanita
And selecciono la orden generada
And hago clic en "Realizar Pago"
And elijo el método "Pago Manual"
And adjunto el archivo "comprobante.pdf"
And ingreso el número de operación "123456"
And envío el pago
And confirmo el mensaje de operación

# 🔔 VALIDACIÓN
And vuelvo a revisar la notificación
Then el estado del pago debe estar en proceso

@CP044 @OrdenPagoManual @RechazoComprobante
Scenario: Rechazo del pago manual y retorno a selección de método

# 🟢 ADMIN genera orden de pago
And filtro las órdenes pendientes
And genero la orden de pago

# 🟢 CLIENTE realiza pago manual
Given el usuario ingresa al ambiente "https://cliente7.newfrontdev-qa.sigesonline.com/user-account/my-account"
When el usuario inicia sesión con usuario "pamela.tone@recsa.com" y contraseña "calidad"
And reviso la notificación en la campanita
And selecciono la orden generada
And hago clic en "Realizar Pago"
And elijo el método "Pago Manual"
And adjunto el archivo "comprobante.pdf"
And ingreso el número de operación "123456"
And envío el pago
And confirmo el mensaje de operación

# 🔔 VALIDACIÓN CLIENTE
And vuelvo a revisar la notificación
Then el estado del pago debe estar en proceso

# 🔵 ADMIN rechaza comprobante
Given el usuario ingresa al ambiente "https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login"
When el usuario inicia sesión con usuario "pamela.tone@recsa.com" y contraseña "calidad"
And navega al módulo "Facturación Cíclica"
And accede a la pestaña "Ordenes de Pago"
And configuro la paginación a 100
And busco al cliente "ANA MARIA DOMINGUEZ SANDOVAL"
And accedo al detalle del cliente
And valido el documento
And confirmo el mensaje de operación
And rechazo el pago
And confirmo el mensaje de operación

# 🟢 CLIENTE valida rechazo
Given el usuario ingresa al ambiente "https://cliente7.newfrontdev-qa.sigesonline.com/user-account/my-account"
When el usuario inicia sesión con usuario "pamela.tone@recsa.com" y contraseña "calidad"
And reviso la notificación en la campanita
Then debe mostrarse el mensaje de rechazo en la campanita