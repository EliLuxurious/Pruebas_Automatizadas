Feature: Conceptos

Registrar nuevo Concepto

Background:
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos
And el usuario selecciona Nuevo Concepto


Scenario: Registrar nuevo concepto con el tipo Rol ITEM COMERCIAL
When el usuario selecciona la Familia "Shampoo"
And el usuario selecciona Auto al Código
And el usuario ingresa el Sufijo "Carbón Ultractivado"
And el usuario selecciona la U.M.Comercial "ML"
And el usuario selecciona la U.Medida "ML"
And el usuario selecciona el Rol "Item Comercial"
And el usuario selecciona el Módulo a Mostrar "MOD0001"
And el usuario selecciona la Marca "ELVIVE"
And el usuario selecciona la Presentación "Frasco"
And el usuario ingresa la Cantidad "780"
And el usuario selecciona la Unidad de Medida "ML"
And el usuario selecciona la tarifa "POR UNIDAD"
And el usuario ingresa el Precio "10"
Then Guardar concepto


Scenario: Registrar nuevo concepto con el tipo Rol INSUMO
When el usuario selecciona la Familia "Harina"
And el usuario ingresa el Código "HA008"
And el usuario ingresa el Sufijo "Para Hornear"
And el usuario selecciona la U.M.Comercial "KG"
And el usuario selecciona la U.Medida "KG"
And el usuario selecciona el Rol "Insumo"
And el usuario selecciona el Módulo a Mostrar "MOD0003"
And el usuario selecciona la Marca "Blanca Flor"
And el usuario selecciona la Presentación "SP"
And el usuario selecciona la tarifa "PORMA"
And el usuario ingresa el Precio "50"
Then Guardar concepto


Scenario: Registro inválido del concepto por tipo rol ITEM COMERCIAL - Sufijo faltante, Modulo a mostrar faltante
When el usuario selecciona la Familia "CEREALES"
And el usuario selecciona Auto al Código
And el usuario selecciona el Rol "Item Comercial"
But el usuario selecciona el Módulo a Mostrar "VACIO"
And el usuario selecciona la Presentación "CAJA"
And el usuario ingresa la Cantidad "900"
And el usuario selecciona la tarifa "PORMA"
And el usuario ingresa el Precio "50"
Then No se guarda concepto


Scenario: Registro inválido del concepto por tipo rol ITEM COMERCIAL - Familia faltante, Precio faltante
When el usuario ingresa el Código "9630"
And el usuario ingresa el Sufijo "Hola"
And el usuario selecciona la U.M.Comercial "KG"
And el usuario selecciona la U.Medida "KG"
And el usuario selecciona el Rol "Insumo"
And el usuario selecciona el Módulo a Mostrar "MOD0003"
And el usuario selecciona la Presentación "SP"
And el usuario selecciona la tarifa "POR UNIDAD"
Then No se guarda concepto



Scenario: Registrar un concepto con la partición positiva con respecto al campo Cantidad
When el usuario selecciona la Familia "Gaseosa"
And el usuario selecciona Auto al Código
And el usuario ingresa el Sufijo "PRUEBA1"
And el usuario selecciona la U.M.Comercial "ML"
And el usuario selecciona la U.Medida "ML"
And el usuario selecciona el Rol "Item Comercial"
And el usuario selecciona el Módulo a Mostrar "MOD0001"
And el usuario selecciona la Marca "KR"
And el usuario selecciona la Presentación "BOTELLAS"
And el usuario ingresa la Cantidad "196"
And el usuario selecciona la Unidad de Medida "ML"
And el usuario selecciona la tarifa "POR UNIDAD"
And el usuario ingresa el Precio "4"
Then Guardar concepto



Scenario: Registrar un concepto con la partición positiva con respecto al campo Precio
When el usuario selecciona la Familia "Harina"
And el usuario selecciona Auto al Código
And el usuario ingresa el Sufijo "PRUEBA4"
And el usuario selecciona la U.M.Comercial "KG"
And el usuario selecciona la U.Medida "KG"
And el usuario selecciona el Rol "Insumo"
And el usuario selecciona el Módulo a Mostrar "MOD0003"
And el usuario selecciona la Marca "Flor Blanca"
And el usuario selecciona la Presentación "SP"
And el usuario selecciona la tarifa "PORMA"
And el usuario ingresa el Precio "50"
Then Guardar concepto


Scenario: Registrar un concepto con la partición positiva con respecto al campo Stock minimo
When el usuario selecciona la Familia "Gaseosa"
And el usuario selecciona Auto al Código
And el usuario ingresa el Sufijo "prueba p"
And el usuario selecciona la U.M.Comercial "ML"
And el usuario selecciona la U.Medida "ML"
And el usuario selecciona el Rol "Item Comercial"
And el usuario selecciona el Módulo a Mostrar "MOD0004"
And el usuario selecciona la Marca "KR"
And el usuario selecciona la Presentación "BOTELLAS"
And el usuario ingresa la Cantidad "588"
And el usuario selecciona la Unidad de Medida "ML"
And el usuario ingresa el stock mínimo "15"
And el usuario selecciona la tarifa "POR UNIDAD"
And el usuario ingresa el Precio "50"
Then Guardar concepto