using OpenQA.Selenium;

namespace SIGES3_0.Pages.VentasPage
{
    public static class VentasLocators
    {
        // Guia de mantenimiento:
        // - No cambiar nombres publicos sin actualizar primero las Pages que los consumen.
        // - No fusionar locators parecidos: algunos cubren controles nativos y otros dropdowns custom.
        // - Mantener los bloques por flujo o componente para ubicar rapido el uso esperado.

        // Compartido por NuevaVentaPage: detalle de producto/concepto.
        public static class Detail
        {
            public static readonly By FamilySelect = By.CssSelector("label[for='familyId'] + app-dropdown-search .select-trigger");
            public static readonly By ConceptSelect = By.CssSelector("label[for='conceptSelect'] + div app-dropdown-search .select-trigger");
            public static readonly By QuantityInputs = By.CssSelector("tbody tr.ng-star-inserted td:nth-child(5) input:nth-child(1)");
            public static readonly By PriceInputs = By.XPath("//input[starts-with(@id,'precio-')]");
            public static readonly By DiscountCheckbox = By.XPath("//label[contains(.,'Descuento')]/preceding-sibling::input[1] | //input[contains(@id,'descuento')]");
        }

        // Compartido por NuevaVentaPage: busqueda de cliente.
        public static class Customer
        {
            public static readonly By DocumentFieldByLabel = By.XPath("//label[contains(normalize-space(),'Cliente') or contains(normalize-space(),'cliente')]/following::input[not(@type='hidden')][1]");
        }

        // Compartido por NuevaVentaPage y VerVentasPage: seleccion de serie.
        public static class Voucher
        {
            public static readonly By SeriesRadio = By.CssSelector(".radio-row .radio-btn input[type='radio']");
            // Busca el label/span con texto de la serie en cualquier parte de la pagina
            public static By SeriesByText(string series) =>
                By.XPath($"//label[.//span[normalize-space()='{series}'] or normalize-space()='{series}'] | //span[normalize-space()='{series}']/parent::label");
            // Busca el input radio dentro del label con la serie
            public static By SeriesInputByText(string series) =>
                By.XPath($"//label[.//span[normalize-space()='{series}'] or normalize-space()='{series}']//input[@type='radio'] | //input[@type='radio'][following-sibling::*[normalize-space()='{series}'] or preceding-sibling::*[normalize-space()='{series}']]");
        }

        // Compartido por NuevaVentaPage: opciones de entrega.
        public static class Delivery
        {
            public static readonly By Immediate = By.XPath("//input[@id='deliveryImmediate']");
            public static readonly By ImmediateLabel = By.CssSelector("label[for='deliveryImmediate'], #deliveryImmediate + label");
            public static readonly By DeferredLabel = By.CssSelector("label[for='tipoServicio'], #tipoServicio + label");
        }

        // Compartido por NuevaVentaPage: seccion Pago y medios de pago.
        public static class Payment
        {
            // Contenedor y acordeon.
            public static readonly By PaymentBody = By.XPath(
                "//div[contains(@class,'accordion-body')][.//label[normalize-space()='Contado' or normalize-space()='Al contado' or normalize-space()='Crédito' or normalize-space()='Credito']]");
            public static readonly By PaymentAccordionHeader = By.CssSelector("#heading-collapse-pago, #heading-collapse-pay");
            public static readonly By PaymentAccordionButton = By.CssSelector("#heading-collapse-pago button, #heading-collapse-pay button, button[data-bs-target='#collapse-pago'], button[data-bs-target='#pay']");
            public static readonly By PaymentAccordionButtonFallback = By.XPath("//button[contains(@class,'accordion-button')][contains(normalize-space(),'Pago')]");

            // Tipo de pago principal.
            public static readonly By CashType = By.CssSelector("#radioDefault1");
            public static readonly By CashTypeLabel = By.CssSelector("label[for='radioDefault1'], #radioDefault1 + label");
            public static readonly By CashTypeLabelText = By.XPath("//label[normalize-space()='Contado']");
            public static readonly By QuickCreditType = By.CssSelector("#radioDefault2");
            public static readonly By QuickCreditTypeLabel = By.CssSelector("label[for='radioDefault2'], #radioDefault2 + label");
            public static readonly By CreditTypeLabelText = By.XPath("//label[normalize-space()='Crédito']");
            public static readonly By MultipaymentCheckbox = By.CssSelector("#checkTypePaymentMethod");

            // Tabs de medios de pago.
            public static readonly By ActivePaymentTab = By.CssSelector(
                ".custom-tab.active, .custom-tab.selected, .custom-tab[aria-selected='true'], [role='tab'][aria-selected='true']");
            public static readonly By CashMethod = By.XPath("//*[contains(@class,'custom-tab')][.//span[normalize-space()='EFECTIVO'] or contains(normalize-space(),'EFECTIVO')]");
            public static readonly By CashMethodFallback = By.XPath("//*[contains(@class,'custom-tab')][contains(normalize-space(),'EFECTIVO') or .//*[contains(normalize-space(),'EFECTIVO')]]");
            public static readonly By DebitMethod = By.XPath("//*[contains(@class,'custom-tab')][.//span[normalize-space()='TARJETAS DE DEBITO'] or contains(normalize-space(),'TARJETAS DE DEBITO')]");
            public static readonly By CreditMethod = By.XPath("//*[contains(@class,'custom-tab')][.//span[normalize-space()='TARJETAS DE CREDITO'] or contains(normalize-space(),'TARJETAS DE CREDITO')]");
            public static readonly By TransferMethod = By.XPath("//*[contains(@class,'custom-tab')][.//span[normalize-space()='TRANSFERENCIA DE FONDOS'] or contains(normalize-space(),'TRANSFERENCIA DE FONDOS')]");
            public static readonly By DepositMethod = By.XPath("//*[contains(@class,'custom-tab')][.//span[normalize-space()='DEPOSITOS EN CUENTA'] or contains(normalize-space(),'DEPOSITOS EN CUENTA')]");
            public static readonly By PointsMethod = By.XPath("//*[contains(@class,'custom-tab')][.//span[normalize-space()='PUNTOS'] or contains(normalize-space(),'PUNTOS')]");
            public static readonly By CreditNoteMethod = By.XPath("//*[contains(@class,'custom-tab')][.//span[normalize-space()='NOTA DE CREDITO' or normalize-space()='NOTA DE CRÉDITO'] or contains(normalize-space(),'NOTA DE CREDITO') or contains(normalize-space(),'NOTA DE CRÉDITO')]");

            // Select cubre control nativo; Trigger cubre dropdown custom del mismo campo.
            public static readonly By BankSelect = By.XPath("//select[@id='bankEntityId']");
            public static readonly By CardSelect = By.XPath("//select[@id='bankingCard']");
            public static readonly By BankAccountSelect = By.XPath(
                "//select[@id='bankAccountId'] | " +
                "//label[contains(normalize-space(),'Cuenta bancaria') or contains(normalize-space(),'Cuenta Bancaria')]/following::select[1]");
            public static readonly By BankTrigger = By.XPath("//label[contains(normalize-space(),'Entidad bancaria') or contains(normalize-space(),'Banco')]/following::*[contains(@class,'select-trigger')][1]");
            public static readonly By CardTrigger = By.XPath("//label[contains(normalize-space(),'Tarjeta')]/following::*[contains(@class,'select-trigger')][1]");
            public static readonly By BankAccountTrigger = By.XPath("//label[contains(normalize-space(),'Cuenta bancaria') or contains(normalize-space(),'Cuenta Bancaria')]/following::*[contains(@class,'select-trigger')][1]");
            public static readonly By DropdownSearchInput = By.XPath("//app-dropdown-search//input[contains(@class,'search') or contains(@class,'input') or contains(@placeholder,'Buscar') or @type='text']");

            // Datos de pago y puntos.
            public static readonly By PaymentInfoInput = By.XPath("//input[@id='informacion']");
            public static readonly By PaymentAmountInput = By.XPath("//input[@type='number' and not(@id='amountReceived')]");
            public static readonly By PointsPaymentInput = By.CssSelector("#pagoPuntos");
            public static readonly By PointsPaymentCurrencyInput = By.CssSelector("#pagoPuntosS");
            public static readonly By PointsAccumulatedInput = By.CssSelector("#totalPuntosAcumulados");
            public static readonly By PointsAccumulatedCurrencyInput = By.CssSelector("#totalPuntosAcumuladosS");
            public static readonly By PointsRemainingInput = By.CssSelector("#totalPuntosRestantes");
            public static readonly By PointsRemainingCurrencyInput = By.CssSelector("#totalPuntosRestantesS");
            public static readonly By AddPaymentButton = By.XPath("//button[normalize-space()='Agregar Medio de Pago']");
            public static readonly By CashAmount = By.CssSelector("#amountToPay");
            public static readonly By CashReceivedNewSale = By.CssSelector("#amountReceived");

            // Credito rapido.
            public static readonly By CreditInitialAmountInput =
                By.XPath("(//*[normalize-space()='Monto inicial']/following::input[not(@type='date') and not(@type='hidden')])[1]");
            public static readonly By CreditInstallmentsInput =
                By.XPath("(//label[contains(normalize-space(),'Numero de cuotas') or contains(normalize-space(),'Número de cuotas') or contains(normalize-space(),'Nro. de cuotas')]/following::input[@type='number'][1]) | //input[@type='number'][@min='1'][@max='60']");
            public static readonly By CreditDueDateInput =
                By.XPath(
                    "(//*[contains(normalize-space(),'Fecha de crédito') or contains(normalize-space(),'Fecha de credito') or contains(normalize-space(),'Vencimiento') or contains(normalize-space(),'primera cuota')]/following::input[not(@type='hidden')][1]) | " +
                    "(//input[(contains(@id,'fecha') or contains(@placeholder,'Fecha')) and (contains(@id,'credit') or contains(@id,'cuota') or contains(@formcontrolname,'credit') or contains(@formcontrolname,'cuota'))][1])");
            public static readonly By Change = By.CssSelector("#change");
            public static readonly By PaymentObservation = By.XPath(
                "//textarea[@id='observation' or @id='observacion' or contains(@name,'observ') or contains(@formcontrolname,'observ') or contains(@placeholder,'Observ') or contains(@placeholder,'observ')] | " +
                "//label[contains(normalize-space(),'Observ')]/following::textarea[1]");
        }

        // Compartido por NuevaVentaPage: descuento por item o global.
        public static class Discount
        {
            public static readonly By ItemScope = By.XPath("//label[contains(.,'Item')] | //button[contains(.,'Item')]");
            public static readonly By GlobalScope = By.XPath("//label[contains(.,'Global')] | //button[contains(.,'Global')]");
            public static readonly By GlobalValueInput = By.XPath("//label[contains(.,'Monto') or contains(.,'Porcentaje')]/following::input[1]");
        }

        // Usado por VerVentasPage y AjusteComprobantePage.
        public static class ViewSales
        {
            // Filtros y consulta.
            public static readonly By InitialDate = By.Id("fechaInicio");
            public static readonly By FinalDate = By.Id("fechaFin");
            public static readonly By QueryButton = By.XPath("//button[contains(normalize-space(),'CONSULTAR') or contains(normalize-space(),'Consultar') or contains(normalize-space(),'BUSCAR') or contains(normalize-space(),'Buscar')]");

            // Canje e invalidacion desde Ver Ventas.
            public static readonly By ActivateRedeem = By.XPath("//label[@for='activarCanje'] | //input[@id='activarCanje']");
            public static readonly By RedeemButton = By.XPath("//button[normalize-space()='Canjear']");
            public static readonly By AcceptRedeemButton = By.XPath("//div[contains(@class,'modal')]//button[contains(.,'Aceptar')]");
            public static readonly By AcceptInvalidation = By.XPath("//a[contains(.,'SI')] | //button[contains(.,'SI')]");

            // Date picker for VerVentas
            public static readonly By FechaHoraInicial = By.XPath(
                "(//label[contains(.,'Fecha y Hora Inicial') or contains(.,'Fecha y hora de inicio')])[1]/following::input[@readonly][1]");
            public static readonly By FechaHoraFinal = By.XPath(
                "(//label[contains(.,'Fecha y Hora Final') or contains(.,'Fecha y hora de fin')])[1]/following::input[@readonly][1]");

            // NV table row selectors
            public static By NvRowCheckboxBySerie(string serie) =>
                By.XPath($"//td[normalize-space()='{serie}']/ancestor::tr//td[1]");

            // Canje modal
            public static readonly By ModalComprobanteDropdown =
                By.XPath("//div[contains(@class,'modal')]//div[contains(@class,'select-trigger')]");
            public static By ModalComprobanteOpcion(string tipo) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{tipo}']");
            public static readonly By ModalInconsistencia = By.XPath(
                "//div[contains(@class,'modal')]//*[contains(normalize-space(),'inconsisten') or " +
                "contains(normalize-space(),'INCONSISTEN') or contains(@class,'inconsisten') or " +
                "contains(@class,'warning') or contains(@class,'alert-warning')]");
            public static readonly By CanjeExitoToast = By.XPath(
                "//*[contains(@class,'toast-success')] | " +
                "//*[contains(@class,'swal2-success')] | " +
                "//*[contains(@class,'toast') and (contains(normalize-space(),'exitoso') or " +
                "contains(normalize-space(),'canje') or contains(normalize-space(),'generado'))]");
        }

        // Flujo Nueva Venta: alta de venta normal, modo caja y contingencia.
        public static class NuevaVenta
        {
            // Configuracion del detalle.
            public static readonly By IgvCheck = By.CssSelector("#flexCheckDefault");
            public static readonly By DetUnifCheck = By.CssSelector("#flexCheckDefault2");

            // Familia
            public static readonly By FamiliaDropdown = By.CssSelector("label[for='familyId'] + app-dropdown-search .select-trigger");

            // Cliente (búsqueda por documento)
            public static readonly By ClienteBuscar = By.CssSelector("input[placeholder='Buscar...']");
            public static readonly By ClienteLupa = By.XPath(
                "(//input[@id='DocumentoIdentidad' or @placeholder='Buscar...']/following::button[.//*[contains(@class,'bi-search')]])[1]");

            // 9: Comprobante — paso 1: abrir con chevron; paso 2: click en span opción
            // Anclado al label businessDocumentTypeId para no confundir con PuntoVenta/Vendedor en MODO CAJA
            public static readonly By ComprobanteChevron = By.XPath(
                "//label[@for='businessDocumentTypeId']/following::app-dropdown-search[1]//i[contains(@class,'bi-chevron-down')]");
            public static readonly By ComprobanteChevronFallback = By.XPath(
                "//app-dropdown-search[contains(@class,'mb-2') and contains(@class,'d-block')]//i[contains(@class,'bi-chevron-down')]");
            public static By ComprobanteOpcion(string text) => By.XPath(
                $"//span[normalize-space()='{text}']");
            public static By ComprobanteOpcionFallback(string text) => By.XPath(
                $"//div[contains(@class,'options-container')]//span[normalize-space()='{text}']");

            // Error modal OK (ng-tns puede variar: 7, 40, etc.)
            public static readonly By ErrorOkButton = By.CssSelector(".ok-button.ng-tns-c835841405-7.ng-star-inserted");
            public static readonly By ErrorOkButtonFallback = By.CssSelector(".ok-button.ng-star-inserted");

            // Series.
            public static By SeriePorTexto(string series) => By.XPath($"//label[.//span[normalize-space()='{series}']]");

            // Entrega.
            public static readonly By AccordionEntrega = By.CssSelector("#heading-collapse-entrega button, button[data-bs-target='#collapse-entrega']");
            // Fallback sin exigir clase 'collapsed': ancla al div heading cuyo ID contenga 'entrega'
            public static readonly By AccordionEntregaFallback1 = By.XPath("//div[contains(@id,'heading') and (contains(@id,'entrega') or contains(@id,'Entrega'))]//button[contains(@class,'accordion-button')]");
            public static readonly By EntregaDiferida = By.XPath("//label[contains(normalize-space(),'Diferida')] | //input[@id='tipoBien2' or contains(@id, 'Diferida')]");

            // Guardado.
            public static readonly By GuardarVenta = By.CssSelector(".btn.btn-primary.btn-save");

            // Modo de venta y datos de facturacion.
            public static By ModoVenta(string modo) => By.XPath($"//span[normalize-space()='{modo}']");
            public static readonly By FechaEmision = By.Id("fechaEmision");
            public static readonly By VendedorChevron = By.XPath(
                "//label[contains(normalize-space(),'Vendedor') or contains(normalize-space(),'VENDEDOR')]/following::app-dropdown-search[1]//i[contains(@class,'bi-chevron-down')]");

            // Punto de venta en Modo Caja; anclado al label para evitar clases Angular dinamicas.
            public static readonly By PuntoVentaChevron = By.XPath(
            "//label[contains(normalize-space(),'Punto')]" +
            "/following::app-dropdown-search[1]//div[contains(@class,'select-trigger')]"
                );
        }

        // Flujo Reportes de ventas: filtros, tarjetas y resultado.
        public static class Reportes
        {
            private const string NormalizeFrom = "ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚÜÑáéíóúüñ";
            private const string NormalizeTo = "abcdefghijklmnopqrstuvwxyzaeiouunaeiouun";

            // ── Vista reportes ──────────────────────────────────────────────────────────────
            public static By VistaReporte(string vista) =>
                By.XPath(
                    $"//label[@for='{vista}' or @for='{NormTitle(vista)}'] | " +
                    $"//*[@role='tab' and {NormTextXPath()}='{NormTitle(vista)}'] | " +
                    $"//*[self::button or self::label or self::span or self::div or self::a][{NormTextXPath()}='{NormTitle(vista)}' and (@role='tab' or contains(@class,'tab') or contains(@class,'switch') or contains(@class,'option') or @for)] | " +
                    $"//input[@id='{vista}' or @id='{NormTitle(vista)}']");

            // ── Normalización interna: minúsculas y sin acentos ────────────────────
            private static string NormTitle(string s) =>
                s.ToLower()
                 .Replace("á", "a").Replace("é", "e").Replace("í", "i")
                 .Replace("ó", "o").Replace("ú", "u").Replace("ü", "u").Replace("ñ", "n");

            private static string NormTextXPath() =>
                $"translate(normalize-space(),'{NormalizeFrom}','{NormalizeTo}')";

            private static string CardRootXPath(string cardTitle) =>
                $"(//*[{NormTextXPath()}='{NormTitle(cardTitle)}']" +
                $"/ancestor::*[self::div or self::section or self::article]" +
                $"[.//*[self::button or self::a][contains({NormTextXPath()},'ver reporte')]][1])";

            // ── Locator universal: ancla VER REPORTE a su tarjeta por título ──────────
            // Acepta cualquier combinación de mayúsculas/minúsculas y acentos
            public static By VerReporteEnTarjeta(string cardTitle) =>
                By.XPath(
                    $"{CardRootXPath(cardTitle)}//button[contains({NormTextXPath()},'ver reporte')] | " +
                    $"{CardRootXPath(cardTitle)}//*[self::span or self::div or self::a][contains({NormTextXPath()},'ver reporte')]/ancestor::*[self::button or self::a][1]");

            // ── Tab Comprobantes — filtros en barra global ────────────────────────
            public static readonly By TipoComprobanteDropdown =
                By.XPath("//label[contains(.,'Tipo de Comprobante')]/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')] | //label[text()='Tipo de Comprobante']/following-sibling::app-dropdown-search//span[contains(@class,'select-value')]");
            public static By TipoComprobanteOption(string tipo) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{tipo}']");

            public static readonly By SerieDropdown =
                By.XPath("//label[contains(.,'Series')]/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')] | //label[text()='Series']/following-sibling::app-dropdown-search//span[contains(@class,'select-value')]");
            public static By SerieOption(string serie) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{serie}']");

            // ── Tab Series — "Comprobante y Serie" DENTRO de la tarjeta POR SERIE ────
            // Scoped a div.report-card con título case-insensitive.
            // Formato exacto del valor: "Todos" | "XX : YYYY"  (ej: "01 : F002", "03 : B002")
            public static readonly By ComprobanteSerieDropdown =
                By.XPath($"{CardRootXPath("Por Serie")}//div[contains(@class,'select-trigger')]");
            public static By ComprobanteSerieOpcion(string valor) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{valor}']");

            // ── Tab Conceptos — filtros en barra global ───────────────────────────
            public static readonly By PuntoVentaDropdown =
                By.XPath($"//label[contains({NormTextXPath()},'punto') and contains({NormTextXPath()},'venta')]/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')] | //label[contains({NormTextXPath()},'punto') and contains({NormTextXPath()},'venta')]/following::app-dropdown-search[1]//div[contains(@class,'select-trigger')]");
            public static readonly By PuntoVentaSearch = By.CssSelector("app-dropdown-search input.search-input");
            public static By PuntoVentaOption(string ptoVenta) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{ptoVenta}']");

            public static readonly By FamiliaDropdown =
                By.XPath($"//label[{NormTextXPath()}='familia']/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')] | {CardRootXPath("Por Familia")}//label[{NormTextXPath()}='familia']/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')] | {CardRootXPath("Por Familia")}//div[contains(@class,'select-trigger')]");
            public static readonly By FamiliaSearch = By.CssSelector("app-dropdown-search input.search-input");
            public static By FamiliaOption(string familia) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{familia}']");

            // ── Tab Conceptos — Característica (dentro de la tarjeta) ──────────────
            // Usado por POR CONCEPTO, CARACTERÍSTICAS Y FORMA DE PAGO y POR CARACTERÍSTICAS.
            public static By CaracteristicaDropdown(string cardTitle) =>
                By.XPath($"{CardRootXPath(cardTitle)}//div[contains(@class,'select-trigger')]");
            public static By CaracteristicaSearch(string cardTitle) =>
                By.XPath($"{CardRootXPath(cardTitle)}//input[contains(@class,'search-input') or contains(@placeholder,'Buscar')]");
            public static By CaracteristicaOpcion(string valor) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{valor}']");

            // ── Punto de venta chip (verificar si ya está seleccionado) ──────────
            public static By PuntoVentaChip(string puntoVenta) =>
                By.XPath($"//label[contains({NormTextXPath()},'punto') and contains({NormTextXPath()},'venta')]/following-sibling::app-dropdown-search//*[not(contains(@class,'placeholder')) and contains(normalize-space(),'{puntoVenta}')] | //label[contains({NormTextXPath()},'punto') and contains({NormTextXPath()},'venta')]/following::app-dropdown-search[1]//*[not(contains(@class,'placeholder')) and contains(normalize-space(),'{puntoVenta}')]");

            // ── Tab Vendedor — filtro global ──────────────────────────────────────
            public static readonly By VendedorDropdown =
                By.XPath("//label[contains(.,'Vendedores')]/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')]");
            public static readonly By VendedorSearch = By.CssSelector("app-dropdown-search input.search-input");
            public static By VendedorOption(string vendedor) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{vendedor}']");
            public static By VendedorChip(string vendedor) =>
                By.XPath($"//label[contains(.,'Vendedores')]/following-sibling::app-dropdown-search//*[not(contains(@class,'placeholder')) and contains(normalize-space(),'{vendedor}')]");

            // ── Tab Grupos — filtro Establecimientos ──────────────────────────────
            public static readonly By EstablecimientoDropdown =
                By.XPath($"//label[contains({NormTextXPath()},'establecimiento')]/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')]");
            public static readonly By EstablecimientoSearch = By.CssSelector("app-dropdown-search input.search-input");
            public static By EstablecimientoOption(string establecimiento) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{establecimiento}']");
            public static By EstablecimientoChip(string establecimiento) =>
                By.XPath($"//label[contains({NormTextXPath()},'establecimiento')]/following-sibling::app-dropdown-search//*[not(contains(@class,'placeholder')) and contains(normalize-space(),'{establecimiento}')]");

            // ── Filtros dentro de tarjeta (Familias/Conceptos/Modalidad en tab Vendedor) ──
            // Usado por POR VENDEDOR (Familias, Conceptos) y POR MODALIDAD Y CONCEPTO (Modalidad).
            public static By FiltroEnTarjeta(string cardTitle, string labelText) =>
                By.XPath($"{CardRootXPath(cardTitle)}//label[contains(normalize-space(),'{labelText}')]/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')] | {CardRootXPath(cardTitle)}//label[contains(normalize-space(),'{labelText}')]/..//div[contains(@class,'select-trigger')]");
            public static By FiltroSearch(string cardTitle, string labelText) =>
                By.XPath($"{CardRootXPath(cardTitle)}//label[contains(normalize-space(),'{labelText}')]/following-sibling::app-dropdown-search//input[contains(@class,'search-input') or contains(@placeholder,'Buscar')] | {CardRootXPath(cardTitle)}//label[contains(normalize-space(),'{labelText}')]/..//input[contains(@class,'search-input') or contains(@placeholder,'Buscar')]");
            public static By FiltroOpcion(string valor) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{valor}']");

            // ── Fecha/Hora picker ─────────────────────────────────────────────────
            public static readonly By FechaHoraInicial =
                By.XPath("//label[contains(.,'Fecha y Hora Inicial')]/following::input[@readonly][1]");
            public static readonly By FechaHoraFinal =
                By.XPath("//label[contains(.,'Fecha y Hora Final')]/following::input[@readonly][1]");

            // ── Resultados ────────────────────────────────────────────────────────
            public static readonly By HeaderReporteResultado =
                By.XPath("//h5[contains(text(),'Reporte')] | //div[contains(@class,'table-responsive')]//table | //ngx-datatable | //canvas | //div[contains(@class,'report-result') or contains(@class,'report-content') or contains(@class,'reporte-container')]");
        }

        // Flujo Ajuste de Comprobante: Nota de Debito, Nota de Credito, Ver Documento y Clonar.
        public static class AjusteComprobante
        {
            private const string AjusteModalRootXPath =
                "//div[contains(@class,'modal') and .//*[contains(normalize-space(),'Ajuste de Comprobante')]]";
            private const string ClonarModalRootXPath =
                "//div[contains(@class,'modal') and .//*[contains(normalize-space(),'Clonar venta') or contains(normalize-space(),'Clonar Venta')]]";

            // ── Verificar que la tabla tiene filas ──────────────────────────────
            public static readonly By TablaFilaPrimera =
                By.XPath("//tbody/tr[1]");

            // ── Botón de acción en la grilla de Ver Ventas ──────────────────────
            public static readonly By AccionPrimerComprobante =
                By.XPath("//tbody/tr[1]/td[11]/div[1]/button[1]/i[1]");
            public static readonly By AccionPrimerComprobanteFallback =
                By.XPath("//tbody/tr[1]/td[last()]//button[1] | //tbody/tr[1]//button[contains(@class,'dropdown-toggle')][1] | //tbody/tr[1]//button[.//i][last()]");

            // ── Tabs del modal ──────────────────────────────────────────────────
            public static readonly By TabNotaDebito =
                By.XPath($"{AjusteModalRootXPath}//span[normalize-space()='Nota de débito']");
            public static readonly By TabNotaCredito =
                By.XPath($"{AjusteModalRootXPath}//span[normalize-space()='Nota de crédito']");
            public static readonly By TabVerDocumento =
                By.XPath($"{AjusteModalRootXPath}//span[normalize-space()='Ver Documento']");
            public static readonly By TabInvalidar =
                By.XPath($"{AjusteModalRootXPath}//span[normalize-space()='Invalidar']");
            public static By OpcionAccionEnModal(string texto) =>
                By.XPath(
                    $"{AjusteModalRootXPath}//button[normalize-space()='{texto}' or .//*[normalize-space()='{texto}'] or contains(normalize-space(),'{texto}')] | " +
                    $"{AjusteModalRootXPath}//a[normalize-space()='{texto}' or .//*[normalize-space()='{texto}'] or contains(normalize-space(),'{texto}')] | " +
                    $"{AjusteModalRootXPath}//span[normalize-space()='{texto}']/ancestor::*[self::button or self::a][1] | " +
                    $"{AjusteModalRootXPath}//span[normalize-space()='{texto}']");

            // ── Modal Clonar venta ───────────────────────────────────────────────
            public static readonly By ModalClonar =
                By.XPath(ClonarModalRootXPath);
            public static By PestanaModoClonar(string modo) =>
                By.XPath(
                    $"{ClonarModalRootXPath}//button[normalize-space()='{modo}' or .//*[normalize-space()='{modo}'] or contains(normalize-space(),'{modo}')] | " +
                    $"{ClonarModalRootXPath}//a[normalize-space()='{modo}' or .//*[normalize-space()='{modo}'] or contains(normalize-space(),'{modo}')] | " +
                    $"{ClonarModalRootXPath}//label[normalize-space()='{modo}' or contains(normalize-space(),'{modo}')] | " +
                    $"{ClonarModalRootXPath}//span[normalize-space()='{modo}']/ancestor::*[self::button or self::a or self::label][1]");
            public static readonly By CantidadPrimerItemClonar =
                By.XPath($"({ClonarModalRootXPath}//table//tbody//tr[1]//td[5]//input[not(@type='hidden')] | {ClonarModalRootXPath}//table//tbody//tr[1]//input[contains(@type,'number')])[1]");
            public static readonly By SeccionEntregaClonar =
                By.XPath($"{ClonarModalRootXPath}//button[contains(@class,'accordion-button')][contains(normalize-space(),'Entrega')] | {ClonarModalRootXPath}//*[contains(@id,'heading-collapse-entrega')]//button[1]");
            public static readonly By EntregaInmediataClonar =
                By.XPath($"{ClonarModalRootXPath}//label[contains(normalize-space(),'Inmediata')]/preceding-sibling::input | {ClonarModalRootXPath}//label[contains(normalize-space(),'Inmediata')]");
            public static readonly By EntregaDiferidaClonar =
                By.XPath($"{ClonarModalRootXPath}//label[contains(normalize-space(),'Diferida')]/preceding-sibling::input | {ClonarModalRootXPath}//label[contains(normalize-space(),'Diferida')]");
            public static readonly By BotonClonarVenta =
                By.XPath($"{ClonarModalRootXPath}//button[contains(normalize-space(),'Clonar venta') or contains(normalize-space(),'CloneSale')] | {ClonarModalRootXPath}//button[.//*[contains(normalize-space(),'Clonar venta') or contains(normalize-space(),'CloneSale')]]");

            // ── Datos generales — Nota de débito ────────────────────────────────
            public static readonly By TipoNotaDebitoSelect =
                By.XPath("//select[@id='tipoNotaDeDebito']");

            // ── Datos generales — Nota de crédito ───────────────────────────────
            public static readonly By TipoNotaCreditoSelect =
                By.XPath("//select[@id='tipoNotaDeCredito']");
            public static By TipoNotaCreditoOpcion(string tipo) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{tipo}'] | " +
                         $"//option[normalize-space()='{tipo}'] | " +
                         $"//*[contains(@class,'option')][normalize-space()='{tipo}']");

            // ── Comprobante destino ─────────────────────────────────────────────
            public static readonly By ComprobanteSelect =
                By.XPath("//select[contains(@id,'comprobante') or contains(@id,'Comprobante') or @id='comprobanteId']");

            // ── Series radio buttons ────────────────────────────────────────────
            public static By SerieRadio(string serie) =>
                By.XPath($"//label[.//span[normalize-space()='{serie}'] or normalize-space()='{serie}']//input[@type='radio'] | //span[normalize-space()='{serie}']/preceding-sibling::input[@type='radio'] | //input[@type='radio'][following-sibling::*[normalize-space()='{serie}']]");
            public static By SerieLabel(string serie) =>
                By.XPath($"//label[.//span[normalize-space()='{serie}'] or normalize-space()='{serie}'] | //span[normalize-space()='{serie}']/ancestor::label");

            // ── Motivo o Sustento ───────────────────────────────────────────────
            public static readonly By MotivoSustento =
                By.XPath("//input[@id='motivoSustento'] | //input[contains(@placeholder,'Motivo o sustento de la nota')]");

            // ── Nota de débito: Interés Total / Monto del interés ───────────────
            public static readonly By MontoInteres =
                By.XPath("//input[@placeholder='Monto del interés'] | //input[contains(@id,'montoInteres')]");

            // ── Nota de débito: Aumento en el valor (grilla detalle) ────────────
            public static readonly By DetalleAumentoInput =
                By.XPath("//tbody/tr[1]/td[4]/input[1]");
            public static readonly By DetalleNotaDebitoHeader =
                By.XPath("//span[contains(@class,'span-title') and contains(normalize-space(),'Detalle')]");

            // ── Secciones accordion del modal ───────────────────────────────────
            public static By SeccionAccordion(string nombre) =>
                By.XPath($"{AjusteModalRootXPath}//button[contains(@class,'accordion-button')][.//*[contains(normalize-space(),'{nombre}')] or contains(normalize-space(),'{nombre}')]");

            // ── Pago en modal de ajuste ─────────────────────────────────────────
            public static readonly By PagoContadoRadio =
                By.XPath($"{AjusteModalRootXPath}//label[normalize-space()='Al contado' or normalize-space()='Contado']");
            public static readonly By PagoCreditoRadio =
                By.XPath($"{AjusteModalRootXPath}//label[normalize-space()='Crédito']");
            public static readonly By MontoInicialInput =
                By.XPath($"({AjusteModalRootXPath}//*[normalize-space()='Monto inicial']/following::input[not(@type='date') and not(@type='hidden')])[1]");
            public static readonly By MedioPagoEfectivo =
                By.XPath($"{AjusteModalRootXPath}//div[contains(@class,'custom-tab')][.//span[contains(normalize-space(),'EFECTIVO')]] | {AjusteModalRootXPath}//span[contains(normalize-space(),'EFECTIVO')]/ancestor::div[contains(@class,'tab')]");

            // ── Pago — Observación ──────────────────────────────────────────────
            public static readonly By ObservacionPago =
                By.XPath($"{AjusteModalRootXPath}//textarea[contains(@id,'observacion') or contains(@name,'observacion')] | {AjusteModalRootXPath}//input[contains(@id,'observacion')]");

            // ── Entrega (Nota de crédito) ───────────────────────────────────────
            public static readonly By EntregaInmediata =
                By.XPath($"{AjusteModalRootXPath}//label[contains(normalize-space(),'Inmediata')]/preceding-sibling::input | {AjusteModalRootXPath}//label[contains(normalize-space(),'Inmediata')]");
            public static readonly By EntregaDiferida =
                By.XPath($"{AjusteModalRootXPath}//label[contains(normalize-space(),'Diferida')]/preceding-sibling::input | {AjusteModalRootXPath}//label[contains(normalize-space(),'Diferida')]");

            // ── Devolución (Nota de crédito — pago) ─────────────────────────────
            public static readonly By DevolucionContado =
                By.XPath($"{AjusteModalRootXPath}//label[normalize-space()='Al contado' or normalize-space()='Contado']");
            public static readonly By DevolucionCredito =
                By.XPath($"{AjusteModalRootXPath}//*[contains(normalize-space(),'Devoluci')]/following::label[normalize-space()='Crédito'][1] | ({AjusteModalRootXPath}//label[normalize-space()='Crédito'])[last()]");

            // ── Nota de crédito: Importe NC ─────────────────────────────────────
            public static readonly By ImporteNCInput =
                By.XPath(
                    $"{AjusteModalRootXPath}//input[contains(@placeholder,'Monto del descuento') or contains(@placeholder,'Importe') or contains(@id,'importeNC') or contains(@formcontrolname,'importeNC')] | " +
                    $"({AjusteModalRootXPath}//*[contains(normalize-space(),'Monto del descuento') or contains(normalize-space(),'Importe')]/following::input[not(@type='hidden')][1])[1]");

            // ── Nota de crédito: Detalle — cantidad a devolver ──────────────────
            public static readonly By CantidadDevolverInput =
                By.XPath("(//table//tbody//tr[1]//input[contains(@type,'number') or contains(@placeholder,'Cant')])[1]");
            public static readonly By ImporteDetalleInput =
                By.XPath(
                    $"{AjusteModalRootXPath}//input[contains(@placeholder,'Monto del descuento') or contains(@placeholder,'Importe') or contains(@id,'importeDetalle') or contains(@formcontrolname,'importeDetalle')] | " +
                    $"({AjusteModalRootXPath}//table//tbody//tr[1]//input[not(@type='hidden')][contains(@type,'number') or contains(@placeholder,'Importe') or contains(@placeholder,'Monto del descuento')])[1] | " +
                    $"({AjusteModalRootXPath}//*[contains(normalize-space(),'Monto del descuento') or contains(normalize-space(),'Importe')]/following::input[not(@type='hidden')][1])[1]");

            // ── Guardar del modal ───────────────────────────────────────────────
            public static readonly By GuardarAjuste =
                By.XPath("//button[contains(normalize-space(),'Guardar')][ancestor::*[contains(@class,'modal') or contains(@class,'ajuste') or contains(@class,'dialog')]] | //button[.//*[contains(@class,'fa-save') or contains(@class,'bi-save')]]");
            public static readonly By GuardarAjusteFallback =
                By.XPath("//button[contains(normalize-space(),'Guardar')]");

            // ── Mensajes / Validaciones ─────────────────────────────────────────
            public static readonly By MensajeCamposRequeridos =
                By.XPath("//*[contains(normalize-space(),'Complete los campos requeridos') or contains(normalize-space(),'campos requeridos')]");
            public static readonly By MensajeExito =
                By.XPath("//*[contains(@class,'toast-success') or contains(@class,'swal2-success')] | //*[contains(@class,'swal2-popup') and (contains(normalize-space(),'Correcto') or contains(normalize-space(),'Se registró correctamente') or contains(normalize-space(),'Se registro correctamente') or contains(normalize-space(),'registrado correctamente') or contains(normalize-space(),'generado correctamente'))] | //*[contains(normalize-space(),'Se registró correctamente') or contains(normalize-space(),'Se registro correctamente') or contains(normalize-space(),'registrado correctamente') or contains(normalize-space(),'generado correctamente')]");
            public static readonly By MensajeError =
                By.XPath("//*[contains(@class,'toast-error') or contains(@class,'alert-danger') or contains(@class,'swal2-popup') or contains(@class,'swal2-title') or contains(@class,'swal2-html-container') or contains(@class,'toast-message')][contains(normalize-space(),'Es necesario') or contains(normalize-space(),'no permite') or contains(normalize-space(),'monto de nota') or contains(normalize-space(),'Error') or contains(normalize-space(),'Complete los campos')]");
            public static readonly By MensajeMontoMayor =
                By.XPath("//*[contains(normalize-space(),'Es necesario que el monto de nota sea menor al total')]");
            public static readonly By MensajeCantidadMayor =
                By.XPath("//*[contains(normalize-space(),'Es necesario que la cantidad a devolver sea menor a la cantidad entregada')]");
        }

        // Flujo Invalidar venta: modal independiente dentro de AjusteComprobantePage.
        public static class InvalidarVenta
        {
            public static readonly By ModalInvalidar =
                By.XPath("//div[contains(@class,'modal') and .//*[contains(normalize-space(),'Invalidar venta') or contains(normalize-space(),'Invalidar Venta')]]");

            public static readonly By SeccionEntregaAccordion =
                By.XPath("//div[contains(@class,'modal') and .//*[contains(normalize-space(),'Invalidar venta') or contains(normalize-space(),'Invalidar Venta')]]//button[contains(@class,'accordion-button')][.//*[contains(normalize-space(),'Entrega')] or contains(normalize-space(),'Entrega')]");

            // ── Observación obligatoria ─────────────────────────────────────────
            public static readonly By ObservacionInvalidar =
                By.XPath("//div[contains(@class,'modal') and .//*[contains(normalize-space(),'Invalidar venta') or contains(normalize-space(),'Invalidar Venta')]]//div[contains(@class,'observation-section')]//textarea[@id='observation']");

            // ── Botón final de confirmación Invalidar ───────────────────────────
            public static readonly By BotonInvalidar =
                By.XPath("//div[contains(@class,'modal') and .//*[contains(normalize-space(),'Invalidar venta') or contains(normalize-space(),'Invalidar Venta')]]//button[not(contains(@class,'close')) and not(@aria-label='Close') and (normalize-space()='Invalidar' or .//span[normalize-space()='Invalidar'])]");

            // ── Confirmación de invalidación exitosa ────────────────────────────
            public static readonly By MensajeExitoInvalidar =
                By.XPath("//*[contains(@class,'toast-success')] | " +
                         "//*[contains(@class,'swal2-success')] | " +
                          "//*[contains(@class,'swal2-popup') and .//*[contains(normalize-space(),'Correcto') or contains(normalize-space(),'registró correctamente')]] | " +
                         "//*[contains(@class,'swal2-title') and (contains(normalize-space(),'nvalidado') or contains(normalize-space(),'xito'))] | " +
                          "//*[contains(@class,'toast-message') and (contains(normalize-space(),'nvalidado') or contains(normalize-space(),'xito') or contains(normalize-space(),'registró correctamente'))] | " +
                          "//*[contains(normalize-space(),'Se registró correctamente')]");

            public static readonly By MensajeFueraDePlazoInvalidar =
                By.XPath("//*[contains(normalize-space(),'Fuera de plazo')] | " +
                         "//*[contains(normalize-space(),'usar Nota de Crédito')] | " +
                         "//*[contains(normalize-space(),'usar Nota de Credito')]");
        }

    }
}
