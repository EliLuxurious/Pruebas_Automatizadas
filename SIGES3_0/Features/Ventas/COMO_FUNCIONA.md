# Cómo funciona el flujo de pruebas de Nueva Venta (CP001)

## Resumen del flujo (13 pasos - selectores QA)

```
1. Ventas | 2. Nueva Venta | 3. VENTA NORMAL | 4. IGV #flexCheckDefault | 5. DET.UNIF #flexCheckDefault2
6. Familia: input[class='search-input'] → gaseosa | 7. Concepto: div.select-trigger → input search → Coca-Cola
8. Acordeón facturación .ng-tns-27 → input Buscar 75893616 → .bi.bi-search
9. Comprobante app-dropdown-search → div.options-container primera opción
10. Error RUC → .ok-button.ng-star-inserted | 11. Serie .checkmark | 12. Acordeón entrega .ng-tns-38 → #tipoBien
13. Validar: Guardar INHABILITADO (pago se autocompleta)
```

---

## Selectores CP001 (SalesLocators.CP001)

| Paso | Selector |
|------|----------|
| 4-5 | `#flexCheckDefault`, `#flexCheckDefault2` |
| 6 | `input[class='search-input']` + "gaseosa" |
| 7 | `div[class='position-relative'] div[class='select-trigger form-control']`, `input[class='search-input']`, `app-product-service-selection-form div...span:nth-child(1)` |
| 8 | `.accordion-button.ng-tns-c2430163177-27.collapsed`, `input[placeholder='Buscar...']`, `.bi.bi-search.ng-star-inserted` |
| 9 | `app-dropdown-search[class='ng-untouched ng-pristine ng-valid'] i.bi-chevron-down`, `div[class='options-container'] div:nth-child(1) span:nth-child(1)` |
| 10 | `.ok-button.ng-tns-c835841405-40.ng-star-inserted` o `.ok-button.ng-star-inserted` |
| 11 | `.checkmark` o `(//span[@class='checkmark'])[1]` |
| 12 | `.accordion-button.ng-tns-c2430163177-38.collapsed`, `#tipoBien` |
| 13 | `.btn.btn-primary.btn-save` (debe estar INHABILITADO) |

**Si un selector falla:** En consola aparece `[CP001] FALLO SELECTOR en paso 'X'`. Avísame y actualizo.

---

## 1. Features (NuevaVenta.feature)

**Qué es:** El archivo Gherkin que describe el caso de prueba en lenguaje natural.

**Flujo:**
- **Background:** Se ejecuta antes de cada escenario. Hace login en la URL y con usuario/contraseña.
- **Scenario CP001:** Un solo caso que valida: Factura + cliente DNI sin RUC → botón Guardar deshabilitado.

---

## 2. StepDefinitions (NuevaVentaStepDefinitions.cs)

**Qué es:** Conecta cada paso del feature con el código que lo ejecuta.

| Paso en el Feature | Método que se ejecuta | Qué hace |
|--------------------|------------------------|----------|
| `abre el flujo de ventas "Nueva Venta"` | `WhenAbreElFlujoDeVentas` | Ventas → Nueva Venta |
| `ejecuta el flujo CP001 completo` | `WhenEjecutaElFlujoCP001Completo` | `ExecuteCP001Flow()` - 13 pasos con selectores QA |
| `valida el resultado esperado...` | `ThenValidaElResultadoEsperadoDeLaVenta` | Verifica Guardar INHABILITADO, mensaje "debe tener RUC" |

**Cómo abre/cierra el navegador:**
- **Abre:** `Hooks.cs` → `BeforeScenario` crea el `ChromeDriver` y lo registra.
- **Cierra:** `Hooks.cs` → `AfterScenario` ejecuta `driver.Quit()` al terminar cada escenario.

---

## 3. Pages (NuevaVentaPage.cs)

**Qué es:** Contiene la lógica de interacción con la pantalla de Nueva Venta.

| Método | Función |
|--------|---------|
| `OpenSalesFlow` | Clic en menú Ventas → Clic en "Nueva Venta" |
| `ExecuteCP001Flow` | Ejecuta los 13 pasos con selectores QA (IGV, Familia, Concepto, Cliente, Comprobante, Error OK, Serie, Entrega) |
| `ValidateSale` | Verifica Guardar INHABILITADO. Si está habilitado, reporta ERROR en consola |

---

## 4. Locators (SalesLocators.cs)

**Qué es:** Define los selectores CSS/XPath para encontrar elementos en la página.

| Clase | Uso |
|-------|-----|
| `Navigation` | Menú Ventas, enlace Nueva Venta |
| `Detail` | Familia, concepto, cantidad, IGV, checkboxes |
| `Customer` | Campo documento/cliente, botón buscar |
| `Voucher` | Comprobante, serie, acordeones |
| `Delivery` | Entrega Inmediata / Diferida |
| `Payment` | Tipo de pago, medio de pago, monto |
| `Save` | Botón Guardar venta |

---

## 5. Models (SalesModels.cs)

**Qué es:** Estructuras de datos para cabecera, productos, descuentos y expectativas.

| Clase | Contenido |
|-------|-----------|
| `SaleHeaderData` | Modalidad, familia, cliente, comprobante, serie, entrega, pago |
| `SaleProductData` | Concepto, cantidad, precio |
| `SaleExpectation` | Si Guardar debe estar habilitado, si se debe ejecutar, mensaje esperado |

---

## Orden de ejecución (CP001)

1. **Hooks BeforeScenario** → Abre Chrome.
2. **Background** → Login.
3. **abre el flujo** → Ventas → Nueva Venta.
4. **ejecuta el flujo CP001** → 13 pasos (VENTA NORMAL, IGV, DET.UNIF, Familia, Concepto, Cliente, Comprobante, Error OK, Serie, Entrega).
5. **valida** → Guardar INHABILITADO. Si está habilitado: ERROR en consola.
6. **Hooks AfterScenario** → Cierra Chrome.

**Nota:** Si Guardar está habilitado y se guarda la venta, el caso falla. El botón debe permanecer inhabilitado (cliente DNI sin RUC para Factura).
