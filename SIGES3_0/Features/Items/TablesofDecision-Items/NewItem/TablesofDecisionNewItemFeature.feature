Feature: TablesofDecisionNewItemFeature

Registro de conceptos usando tablas de decisión

Background:
Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
And el usuario accede al módulo Conceptos

Scenario Outline: Registro exitoso de concepto
When el usuario selecciona Nuevo Concepto
And el usuario selecciona la Familia "<familia>"
And el usuario ingresa el Código "<codigo>"
And el usuario ingresa el Sufijo "<sufijo>"
And el usuario selecciona la U.M.Comercial "<umcomercial>"
And el usuario selecciona la U.Medida "<ummedida>"
And el usuario selecciona el Rol "<rol>"
And el usuario selecciona el Módulo a Mostrar "<modulo>"
And el usuario selecciona la Marca "<marca>"
And el usuario selecciona la Presentación "<presentacion>"
And el usuario ingresa la Cantidad "<cantidad>"
And el usuario selecciona la tarifa "<tarifa>"
And el usuario ingresa el Precio "<precio>"
Then Guardar concepto

Examples:

| rol | familia | codigo | sufijo | umcomercial | ummedida | modulo | marca | presentacion | cantidad | tarifa | precio |
| Insumo | HISOPO | INS0001 | Para uso médico |  |  | MOD0001 | TextilPeru | SP |  | POR UNIDAD | 15 |
| Item Comercial | SILLA | IC0001 | Ergonómica |  |  | MOD0001 |  | CAJA | 15 | PORMA | 60 |
| Insumo | Harina | HA0001 | Refinada | KG | KG | MOD0001 |  | SP |  | POR UNIDAD | 45 |
| Item Comercial | Cuaderno | CU0001 | Triple renglón |  |  | MOD0001 |  | SP |  | POR UNIDAD | 5 |
| Insumo | ACEITE | AC0001 | Vegetal Premium | ML | ML | MOD0001 |  | BOTELLAS | 840 | POR UNIDAD | 15 |




Scenario Outline: Registro inválido de concepto
  When el usuario selecciona Nuevo Concepto
  And el usuario selecciona la Familia "<familia>"
  And el usuario ingresa el Código "<codigo>"
  And el usuario ingresa el Sufijo "<sufijo>"
  And el usuario selecciona la U.M.Comercial "<umcomercial>"
  And el usuario selecciona la U.Medida "<ummedida>"
  And el usuario selecciona el Rol "<rol>"
  And el usuario selecciona el Módulo a Mostrar "<modulo>"
  And el usuario selecciona la Marca "<marca>"
  And el usuario selecciona la Presentación "<presentacion>"
  And el usuario ingresa la Cantidad "<cantidad>"
  And el usuario selecciona la tarifa "<tarifa>"
  And el usuario ingresa el Precio "<precio>"
  Then No se guarda concepto

  Examples:
    | rol            | familia  | codigo   | sufijo                     | umcomercial | ummedida | modulo  | marca      | presentacion | cantidad | tarifa     | precio |
    | Insumo         | HISOPO   | INS0001  | Para uso médico            |             |          | MOD0001 | TextilPeru | SP           |          | POR UNIDAD | 18     |
    | Item Comercial | Shampoo  | SH0001   | Rizos dorados              |             |          | MOD0003 |            | SP           |          | BIMONTHLY  |        |
    | Insumo         | Azúcar   | AZ0001   | Blanca en polvo            |             |          | VACIO   |            | SP           |          | POR UNIDAD | 5      |
    | Item Comercial |          | INS0005  | Validación                 |             |          | MOD0001 |            | SP           |          | PORMA      |        |
    | Insumo         | ACEITE   | AC0001   | Vegetal Premium            | ML          | ML       | MOD0001 |            | BOTELLAS     | -50      | POR UNIDAD |        |
    | Item Comercial | Shampoo  | SH0006   | Rizos dorados              |             |          | VACIO   | H&S        | SP           |          |            |        |
    | Insumo         |          | INS00020 | Otra validación            |             |          | MOD0001 |            | Frasco       | 13       |            |        |
    | Item Comercial | Cuaderno | CU0005   | Doble Raya                 |             |          | MOD0001 |            | CAJA         | 0        |            |        |
    | Insumo         |          | INS0009  | Validacion de validaciones |             |          | VACIO   |            | Frasco       | -26      |            |        |
