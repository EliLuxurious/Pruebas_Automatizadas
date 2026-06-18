using OpenQA.Selenium;

namespace SIGES3_0.Pages.VentasPage
{
    public static class VentasLocators
    {
        public static class Navigation
        {
            public static readonly By SalesMenu = By.XPath("//span[normalize-space()='Venta' or normalize-space()='Ventas']/ancestor::a[1]");
            public static readonly By NewSale = By.XPath("//a[.//span[normalize-space()='Nueva Venta'] or normalize-space()='Nueva Venta']");
            public static readonly By ContingencySale = By.XPath("//a[.//span[contains(normalize-space(),'Contingencia')] or contains(normalize-space(),'Contingencia')]");
            public static readonly By ViewSales = By.XPath("//a[.//span[normalize-space()='Ver Ventas'] or normalize-space()='Ver Ventas']");
            public static readonly By Reports = By.XPath("//a[.//span[normalize-space()='Reportes'] or normalize-space()='Reportes']");
        }

        public static class SaleMode
        {
            public static readonly By Normal = By.XPath("//a[contains(.,'VENTA NORMAL')] | //button[contains(.,'VENTA NORMAL')] | //label[contains(.,'VENTA NORMAL')]");
            public static readonly By Contingency = By.XPath("//a[contains(.,'VENTA POR CONTINGENCIA')] | //button[contains(.,'VENTA POR CONTINGENCIA')] | //label[contains(.,'VENTA POR CONTINGENCIA')]");
        }

        public static class Detail
        {
            public static readonly By ProductAccordion = By.CssSelector("#select-product");
            public static readonly By FamilySelect = By.CssSelector("label[for='familyId'] + app-dropdown-search .select-trigger");
            public static readonly By ConceptSelect = By.CssSelector("label[for='conceptSelect'] + div app-dropdown-search .select-trigger");
            public static readonly By ConceptBarcode = By.CssSelector("input[formcontrolname='barcode']");
            public static readonly By ConceptBarcodePlaceholder = By.CssSelector("input[placeholder='C├│digo de barra']");
            public static readonly By ScaleCode = By.CssSelector("input[formcontrolname='scaleCode']");
            public static readonly By ScaleCodePlaceholder = By.CssSelector("input[placeholder='C├│digo de balanza']");
            public static readonly By AddServiceButton = By.XPath("//button[.//span[contains(normalize-space(), 'Agregar Servicio')]]");
            public static readonly By QuantityInputs = By.CssSelector("tbody tr.ng-star-inserted td:nth-child(5) input:nth-child(1)");
            public static readonly By PriceInputs = By.XPath("//input[starts-with(@id,'precio-')]");
            public static readonly By IgvCheckbox = By.CssSelector("#flexCheckDefault");
            public static readonly By UnifiedDetailCheckbox = By.CssSelector("#flexCheckDefault2");
            public static readonly By DiscountCheckbox = By.XPath("//label[contains(.,'Descuento')]/preceding-sibling::input[1] | //input[contains(@id,'descuento')]");
            public static readonly By FirstGridRow = By.XPath("//table//tbody/tr[1]");
            public static readonly By ProductModalOverlay = By.CssSelector(".modal-overlay");
            public static readonly By ProductModalAcceptButton = By.XPath("//div[contains(@class,'modal')]//button[contains(normalize-space(),'Aceptar') or contains(normalize-space(),'Agregar') or contains(normalize-space(),'Guardar') or contains(normalize-space(),'Continuar') or contains(normalize-space(),'Confirmar') or normalize-space()='OK']");
            public static readonly By ProductModalCloseButton = By.XPath("//div[contains(@class,'modal')]//button[contains(@class,'close') or contains(@aria-label,'Close') or contains(@aria-label,'Cerrar')]");
        }

        public static class Customer
        {
            public static readonly By DocumentField = By.CssSelector("input#DocumentoIdentidad, input[placeholder='Buscar...'], input[placeholder*='Cliente'], input[formcontrolname='commercialActorNumber'], input[id='numeroDocumento'], input[name='numeroDocumento']");
            public static readonly By DocumentFieldFallback = By.XPath("//label[contains(normalize-space(),'Cliente')]/following::input[not(@type='hidden')][1] | //input[@id='DocumentoIdentidad' or contains(@placeholder,'Buscar') or contains(@placeholder,'cliente') or @id='numeroDocumento']");
            public static readonly By DocumentFieldByLabel = By.XPath("//label[contains(normalize-space(),'Cliente') or contains(normalize-space(),'cliente')]/following::input[not(@type='hidden')][1]");
            public static readonly By SearchButton = By.CssSelector(".bi.bi-search");
            public static readonly By SearchButtonContainer = By.XPath("//*[contains(@class,'bi-search')]/ancestor::button[1]");
            public static readonly By EditButton = By.CssSelector("button.btn-edit");
            public static readonly By AddButton = By.CssSelector("button.btn-add");
            public static readonly By AliasField = By.XPath("//input[contains(@placeholder,'Alias') or contains(@name,'alias')]");
            public static By TypeByText(string customerType) =>
                By.XPath($"//label[normalize-space()='{customerType}' or contains(normalize-space(),'{customerType}')] | //span[normalize-space()='{customerType}']/ancestor::label[1] | //button[normalize-space()='{customerType}' or contains(normalize-space(),'{customerType}')]");
        }

        public static class Voucher
        {
            public static readonly By BillingAccordion = By.CssSelector("#heading-collapse-facturaci├│n");
            public static readonly By BillingAccordionFallback = By.XPath("//*[contains(@id,'heading-collapse-factur') or contains(normalize-space(),'Facturaci├│n') or contains(normalize-space(),'Facturacion')]");
            public static readonly By NewSaleType = By.CssSelector("app-dropdown-search[class='ng-pristine ng-valid ng-touched'] div[class='select-trigger form-control']");
            public static readonly By TypeInput = By.XPath("//label[@for='businessDocumentTypeId']/following::input[contains(@class,'search') or contains(@class,'select2-search__field')][1]");
            public static readonly By SeriesRadio = By.CssSelector(".radio-row .radio-btn input[type='radio']");
            public static readonly By SeriesCheckmark = By.CssSelector(".checkmark");
            // Busca el label/span con texto de la serie en cualquier parte de la pagina
            public static By SeriesByText(string series) =>
                By.XPath($"//label[.//span[normalize-space()='{series}'] or normalize-space()='{series}'] | //span[normalize-space()='{series}']/parent::label");
            // Busca el input radio dentro del label con la serie
            public static By SeriesInputByText(string series) =>
                By.XPath($"//label[.//span[normalize-space()='{series}'] or normalize-space()='{series}']//input[@type='radio'] | //input[@type='radio'][following-sibling::*[normalize-space()='{series}'] or preceding-sibling::*[normalize-space()='{series}']]");
            public static readonly By BillingComment = By.CssSelector("#billingComment");
            public static readonly By ValidationMessage = By.CssSelector(".custom-error-message");
            public static readonly By ValidationMessageAny = By.XPath("//*[contains(@class,'custom-error-message') or contains(@class,'alert-danger') or contains(@class,'text-danger') or contains(@class,'toast-error') or contains(@class,'swal2-content')][(normalize-space())]");
            public static readonly By ContingencyIssueDate = By.XPath("//input[contains(@id,'fecha') or contains(@placeholder,'Fecha de emision')][1]");
        }

        public static class Popup
        {
            // Selector especifico compartido por QA para el boton OK/X del popup.
            public static readonly By ExactOkButton = By.CssSelector(".ok-button.ng-tns-c835841405-7.ng-star-inserted");
            public static readonly By OkButton = By.CssSelector(".ok-button.ng-star-inserted, .ok-button");
            public static readonly By CloseButton = By.CssSelector("button.close, button[aria-label='Close'], button[aria-label='Cerrar'], .btn-close");
            public static readonly By CloseIcon = By.XPath("//button[.//*[contains(@class,'bi-x') or contains(@class,'fa-times')]] | //i[contains(@class,'bi-x')]/ancestor::button[1]");
            public static readonly By Host = By.CssSelector("div.cdk-overlay-container, ngb-modal-window, .swal2-container, .modal.show");
            public static readonly By Message = By.XPath("//*[contains(@class,'custom-error-message') or contains(@class,'swal2-content') or contains(@class,'swal2-html-container') or contains(@class,'modal-body') or contains(@class,'dialog')][normalize-space()]");
        }

        public static class Delivery
        {
            public static readonly By Accordion = By.CssSelector("#heading-collapse-entrega");
            public static readonly By AccordionFallback = By.CssSelector(".accordion-item.ng-tns-c2430163177-5 .accordion-header, .accordion-item.ng-tns-c2430163177-5 h2, .accordion-item.ng-tns-c2430163177-5 button");
            public static readonly By AccordionTextFallback = By.XPath("//*[contains(@id,'heading-collapse-entrega') or contains(normalize-space(),'Entrega')]");
            public static readonly By Immediate = By.XPath("//input[@id='deliveryImmediate']");
            public static readonly By ImmediateLabel = By.CssSelector("label[for='deliveryImmediate'], #deliveryImmediate + label");
            public static readonly By Deferred = By.CssSelector("#tipoServicio");
            public static readonly By DeferredLabel = By.CssSelector("label[for='tipoServicio'], #tipoServicio + label");
            public static readonly By DispatchGuideButton = By.XPath("//div[@id='collapse-entrega']//button[normalize-space()='Guia de remisi├│n']");
        }

        public static class Payment
        {
            public static readonly By PaymentAccordion = By.CssSelector("#pay");
            public static readonly By PaymentBody = By.XPath(
                "//div[contains(@class,'accordion-body')][.//label[normalize-space()='Contado' or normalize-space()='Al contado' or normalize-space()='Crédito' or normalize-space()='Credito']]");
            public static readonly By PaymentAccordionHeader = By.CssSelector("#heading-collapse-pago, #heading-collapse-pay");
            public static readonly By PaymentAccordionFallback = By.XPath("//*[contains(@id,'heading-collapse-pay') or contains(@id,'heading-collapse-pago') or contains(normalize-space(),'Pago')]");
            public static readonly By PaymentAccordionButton = By.CssSelector("#heading-collapse-pago button, #heading-collapse-pay button, button[data-bs-target='#collapse-pago'], button[data-bs-target='#pay']");
            public static readonly By PaymentAccordionButtonFallback = By.XPath("//button[contains(@class,'accordion-button')][contains(normalize-space(),'Pago')]");
            public static readonly By CashType = By.CssSelector("#radioDefault1");
            public static readonly By CashTypeLabel = By.CssSelector("label[for='radioDefault1'], #radioDefault1 + label");
            public static readonly By CashTypeLabelText = By.XPath("//label[normalize-space()='Contado']");
            public static readonly By QuickCreditType = By.CssSelector("#radioDefault2");
            public static readonly By QuickCreditTypeLabel = By.CssSelector("label[for='radioDefault2'], #radioDefault2 + label");
            public static readonly By CreditTypeLabelText = By.XPath("//label[normalize-space()='Crédito']");
            public static readonly By ConfiguredCreditType = By.XPath("//label[@for='radio3' and normalize-space()='CC']");
            public static readonly By MultipaymentCheckbox = By.CssSelector("#checkTypePaymentMethod");
            public static readonly By PaymentTabs = By.CssSelector(".custom-tab");
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
            public static readonly By BankSelect = By.XPath("//select[@id='bankEntityId']");
            public static readonly By CardSelect = By.XPath("//select[@id='bankingCard']");
            public static readonly By BankAccountSelect = By.XPath(
                "//select[@id='bankAccountId'] | " +
                "//label[contains(normalize-space(),'Cuenta bancaria') or contains(normalize-space(),'Cuenta Bancaria')]/following::select[1]");
            public static readonly By BankTrigger = By.XPath("//label[contains(normalize-space(),'Entidad bancaria') or contains(normalize-space(),'Banco')]/following::*[contains(@class,'select-trigger')][1]");
            public static readonly By CardTrigger = By.XPath("//label[contains(normalize-space(),'Tarjeta')]/following::*[contains(@class,'select-trigger')][1]");
            public static readonly By BankAccountTrigger = By.XPath("//label[contains(normalize-space(),'Cuenta bancaria') or contains(normalize-space(),'Cuenta Bancaria')]/following::*[contains(@class,'select-trigger')][1]");
            public static readonly By DropdownSearchInput = By.XPath("//app-dropdown-search//input[contains(@class,'search') or contains(@class,'input') or contains(@placeholder,'Buscar') or @type='text']");
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
            public static readonly By CashReceivedContingency = By.CssSelector("#amountReceived");
            public static readonly By CreditInitialAmountInput =
                By.XPath("(//*[normalize-space()='Monto inicial']/following::input[not(@type='date') and not(@type='hidden')])[1]");
            public static readonly By CreditInstallmentsInput =
                By.XPath("(//label[contains(normalize-space(),'Numero de cuotas') or contains(normalize-space(),'Número de cuotas') or contains(normalize-space(),'Nro. de cuotas')]/following::input[@type='number'][1]) | //input[@type='number'][@min='1'][@max='60']");
            public static readonly By Change = By.CssSelector("#change");
            public static readonly By PaymentObservation = By.XPath(
                "//textarea[@id='observation' or @id='observacion' or contains(@name,'observ') or contains(@formcontrolname,'observ') or contains(@placeholder,'Observ') or contains(@placeholder,'observ')] | " +
                "//label[contains(normalize-space(),'Observ')]/following::textarea[1]");
        }

        public static class Discount
        {
            public static readonly By ItemScope = By.XPath("//label[contains(.,'Item')] | //button[contains(.,'Item')]");
            public static readonly By GlobalScope = By.XPath("//label[contains(.,'Global')] | //button[contains(.,'Global')]");
            public static readonly By AmountMode = By.XPath("//label[contains(.,'monto') or contains(.,'Monto')] | //button[contains(.,'monto') or contains(.,'Monto')]");
            public static readonly By PercentageMode = By.XPath("//label[contains(.,'porcentaje') or contains(.,'Porcentaje')] | //button[contains(.,'porcentaje') or contains(.,'Porcentaje')]");
            public static readonly By GlobalValueInput = By.XPath("//label[contains(.,'Monto') or contains(.,'Porcentaje')]/following::input[1]");
            public static readonly By ValidationMessage = By.XPath("//*[contains(@class,'invalid-feedback') or contains(@class,'text-danger') or contains(@class,'alert') or contains(@class,'toast')]");
        }

        public static class Save
        {
            // Selector actualizado con las clases reales del DOM: .btn.btn-primary.btn-save
            public static readonly By SaveButton = By.CssSelector("button.btn.btn-primary.btn-save, button.btn-save");
            public static readonly By SuccessMessage = By.XPath("//*[contains(@class,'toast') or contains(@class,'alert') or contains(@class,'swal2-popup')]");
        }

        public static class ViewSales
        {
            public static readonly By InitialDate = By.Id("fechaInicio");
            public static readonly By FinalDate = By.Id("fechaFin");
            public static readonly By QueryButton = By.XPath("//button[contains(normalize-space(),'CONSULTAR') or contains(normalize-space(),'Consultar') or contains(normalize-space(),'BUSCAR') or contains(normalize-space(),'Buscar')]");
            public static readonly By SearchSale = By.XPath("//input[contains(@aria-controls,'DataTables') or contains(@placeholder,'Buscar')][last()]");
            public static readonly By ActivateRedeem = By.XPath("//label[@for='activarCanje'] | //input[@id='activarCanje']");
            public static readonly By FirstRowCheck = By.XPath("//tbody/tr[1]/td[1]//input");
            public static readonly By RedeemButton = By.XPath("//button[normalize-space()='Canjear']");
            public static readonly By RedeemVoucherType = By.CssSelector("select.tipoDocumento");
            public static readonly By AcceptRedeemButton = By.XPath("//div[contains(@class,'modal')]//button[contains(.,'Aceptar')]");
            public static readonly By ViewSaleButton = By.XPath("//tbody/tr[1]//a[contains(@title,'Ver') or contains(.,'VER') or contains(@class,'btn')][1]");
            public static readonly By DebitNoteButton = By.XPath("//button[contains(.,'NOTA DE DEBITO') or contains(.,'NOTA DE D├ëBITO')]");
            public static readonly By CreditNoteButton = By.XPath("//button[contains(.,'NOTA DE CREDITO') or contains(.,'NOTA DE CR├ëDITO')]");
            public static readonly By NoteTypeSelect = By.Id("tipoDeNota");
            public static readonly By NoteDocumentSelect = By.Id("documentoParaNota");
            public static readonly By NoteReason = By.Id("motivo");
            public static readonly By NoteAmount = By.Id("montoNota");
            public static readonly By NoteRowAmount = By.XPath("//tbody/tr[1]/td[4]//input");
            public static readonly By NoteQuantity = By.XPath("//tbody/tr[1]/td[8]//input");
            public static readonly By NoteImmediate = By.XPath("//label[@for='radioNota1']");
            public static readonly By NoteDeferred = By.XPath("//label[@for='radioNota2']");
            public static readonly By SaveNote = By.XPath("//button[contains(.,'Guardar nota')] | //button[contains(.,'GUARDAR')]");
            public static readonly By InvalidateButton = By.XPath("//button[contains(.,'INVALIDAR') or contains(.,'Invalidar')]");
            public static readonly By Observation = By.XPath("//textarea[contains(@id,'observacion') or contains(@name,'observacion') or @id='motivo']");
            public static readonly By AcceptInvalidation = By.XPath("//a[contains(.,'SI')] | //button[contains(.,'SI')]");
            public static readonly By CloneButton = By.XPath("//a[contains(.,'CLONAR')] | //button[contains(.,'CLONAR')]");
            public static readonly By PrintButton = By.XPath("//button[contains(.,'IMPRIMIR') or contains(.,'Imprimir')]");
            public static readonly By PdfButton = By.XPath("//a[contains(.,'PDF') or contains(@title,'PDF')]");
            public static readonly By DownloadDropdown = By.XPath("//a[contains(@class,'dropdown-toggle')]");
            public static readonly By XmlButton = By.XPath("//a[contains(.,'XML')]");
            public static readonly By ZipButton = By.XPath("//a[contains(.,'ZIP')]");
            public static readonly By SendButton = By.XPath("//button[contains(.,'ENVIAR') or contains(.,'Enviar')]");
            public static readonly By EmailInput = By.Id("correoImput");
            public static readonly By AddEmail = By.XPath("//a[contains(@title,'Agregar') or contains(.,'+')]");
            public static readonly By SendMail = By.XPath("//div[contains(@class,'modal')]//button[contains(.,'Enviar')]");

            // Date picker for VerVentas
            public static readonly By FechaHoraInicial = By.XPath(
                "(//label[contains(.,'Fecha y Hora Inicial') or contains(.,'Fecha y hora de inicio')])[1]/following::input[@readonly][1]");
            public static readonly By FechaHoraFinal = By.XPath(
                "(//label[contains(.,'Fecha y Hora Final') or contains(.,'Fecha y hora de fin')])[1]/following::input[@readonly][1]");

            // NV table row selectors
            public static By NvRowCheckbox(int fila) => By.XPath($"//tbody/tr[{fila}]/td[1]");
            public static By NvRowComprobante(int fila) => By.XPath($"//tbody/tr[{fila}]/td[4]");
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Selectores Nueva Venta - usados por todos los flujos de este.
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////
        public static class NuevaVenta
        {
            public static readonly By IgvCheck = By.CssSelector("#flexCheckDefault");
            public static readonly By DetUnifCheck = By.CssSelector("#flexCheckDefault2");

            // Familia
            public static readonly By FamiliaDropdown = By.CssSelector("label[for='familyId'] + app-dropdown-search .select-trigger");
            public static readonly By FamiliaSearchInput = By.CssSelector("input.search-input");

            // Concepto
            public static readonly By ConceptoDropdown = By.CssSelector("label[for='conceptSelect'] + div app-dropdown-search .select-trigger");
            public static readonly By ConceptoSearchInput = By.CssSelector("input.search-input");
            public static readonly By ConceptoOpcion = By.CssSelector(".option-label");

            //Aqui Se utilizará VentasLocators.Voucher.BillingAccordion

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

            // 10: Error modal OK (ng-tns puede variar: 7, 40, etc.)
            public static readonly By ErrorOkButton = By.CssSelector(".ok-button.ng-tns-c835841405-7.ng-star-inserted");
            public static readonly By ErrorOkButtonFallback = By.CssSelector(".ok-button.ng-star-inserted");

            // 11: Serie F002
            public static readonly By SerieCheckmark = By.CssSelector(".checkmark");
            public static readonly By SerieCheckmarkXpath = By.XPath("(//span[@class='checkmark'])[1]");
            public static By SeriePorTexto(string series) => By.XPath($"//label[.//span[normalize-space()='{series}']]");

            // 12: Acordeón entrega
            public static readonly By AccordionEntrega = By.CssSelector("#heading-collapse-entrega button, button[data-bs-target='#collapse-entrega']");
            // Fallback sin exigir clase 'collapsed': ancla al div heading cuyo ID contenga 'entrega'
            public static readonly By AccordionEntregaFallback1 = By.XPath("//div[contains(@id,'heading') and (contains(@id,'entrega') or contains(@id,'Entrega'))]//button[contains(@class,'accordion-button')]");
            public static readonly By EntregaDiferida = By.XPath("//label[contains(normalize-space(),'Diferida')] | //input[@id='tipoBien2' or contains(@id, 'Diferida')]");

            // 13: Guardar venta (debe estar INHABILITADO)
            public static readonly By GuardarVenta = By.CssSelector(".btn.btn-primary.btn-save");

            // 14: Tab selector de modo de venta (VENTA NORMAL / VENTA MODO CAJA / VENTA POR CONTINGENCIA)
            public static By ModoVenta(string modo) => By.XPath($"//span[normalize-space()='{modo}']");

            // 15: Fecha de emisión (visible en Venta Modo Caja y Venta por Contingencia)
            public static readonly By FechaEmision = By.Id("fechaEmision");

            // 16: Vendedor en sección Facturación (Venta Modo Caja)
            public static readonly By VendedorChevron = By.XPath(
                "//label[contains(normalize-space(),'Vendedor') or contains(normalize-space(),'VENDEDOR')]/following::app-dropdown-search[1]//i[contains(@class,'bi-chevron-down')]");
            public static By VendedorOption(string vendedor) => By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{vendedor}']");

            // 17: Punto de venta (Venta Modo Caja) — anclado al label para evitar dependencia de clases Angular dinámicas
            public static readonly By PuntoVentaChevron = By.XPath(
            "//label[contains(normalize-space(),'Punto')]" +
            "/following::app-dropdown-search[1]//div[contains(@class,'select-trigger')]"
                );
            public static By PuntoVentaOpcion(string nombre) =>
                By.XPath($"//span[normalize-space()='{nombre}']");
        }

        ///////////////////////////////////////////////////////////Reportes////////////////////////////////////////////////

        public static class Reportes
        {
            // ── Vista reportes ──────────────────────────────────────────────────────────────
            public static By VistaReporte(string vista) =>
                By.XPath($"//input[@id='{vista}']");

            // ── Normalización interna: minúsculas y sin acentos ────────────────────
            private static string NormTitle(string s) =>
                s.ToLower()
                 .Replace("á", "a").Replace("é", "e").Replace("í", "i")
                 .Replace("ó", "o").Replace("ú", "u").Replace("ü", "u").Replace("ñ", "n");

            // ── Locator universal: ancla VER REPORTE a su tarjeta por título ──────────
            // Acepta cualquier combinación de mayúsculas/minúsculas y acentos
            public static By VerReporteEnTarjeta(string cardTitle) =>
                By.XPath($"//div[contains(@class,'report-card')][.//*[translate(normalize-space(),'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚÜÑáéíóúüñ','abcdefghijklmnopqrstuvwxyzaeiouunaeiouun')='{NormTitle(cardTitle)}']]//button[contains(normalize-space(),'VER REPORTE')]");

            // ── Tab Comprobantes — filtros en barra global ────────────────────────
            public static readonly By TipoComprobanteDropdown =
                By.XPath("//label[contains(.,'Tipo de Comprobante')]/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')] | //label[text()='Tipo de Comprobante']/following-sibling::app-dropdown-search//span[contains(@class,'select-value')]");
            public static readonly By TipoComprobanteSearch = By.CssSelector("app-dropdown-search input.search-input");
            public static By TipoComprobanteOption(string tipo) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{tipo}']");

            public static readonly By SerieDropdown =
                By.XPath("//label[contains(.,'Series')]/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')] | //label[text()='Series']/following-sibling::app-dropdown-search//span[contains(@class,'select-value')]");
            public static readonly By SerieSearch = By.CssSelector("app-dropdown-search input.search-input");
            public static By SerieOption(string serie) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{serie}']");

            // ── Tab Series — "Comprobante y Serie" DENTRO de la tarjeta POR SERIE ────
            // Scoped a div.report-card con título case-insensitive.
            // Formato exacto del valor: "Todos" | "XX : YYYY"  (ej: "01 : F002", "03 : B002")
            public static readonly By ComprobanteSerieDropdown =
                By.XPath("//div[contains(@class,'report-card')][.//*[translate(normalize-space(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='por serie']]//div[contains(@class,'select-trigger')]");
            public static readonly By ComprobanteSerieSearch =
                By.XPath("//div[contains(@class,'report-card')][.//*[translate(normalize-space(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='por serie']]//input[contains(@class,'search-input') or contains(@placeholder,'Buscar')]");
            public static By ComprobanteSerieOpcion(string valor) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{valor}']");

            // ── Tab Conceptos — filtros en barra global ───────────────────────────
            public static readonly By PuntoVentaDropdown =
                By.XPath("//label[contains(.,'Puntos de ventas')]/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')]");
            public static readonly By PuntoVentaSearch = By.CssSelector("app-dropdown-search input.search-input");
            public static By PuntoVentaOption(string ptoVenta) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{ptoVenta}']");

            public static readonly By FamiliaDropdown =
                By.XPath("//label[contains(.,'Familia')]/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')] | //div[contains(@class,'report-card')][.//*[translate(normalize-space(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='por familia']]//div[contains(@class,'select-trigger')]");
            public static readonly By FamiliaSearch = By.CssSelector("app-dropdown-search input.search-input");
            public static By FamiliaOption(string familia) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{familia}']");

            // ── Tab Conceptos — Característica (dentro de la tarjeta) ──────────────
            // Usado por POR CONCEPTO, CARACTERÍSTICAS Y FORMA DE PAGO y POR CARACTERÍSTICAS.
            public static By CaracteristicaDropdown(string cardTitle) =>
                By.XPath($"//div[contains(@class,'report-card')][.//*[translate(normalize-space(),'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚÜÑáéíóúüñ','abcdefghijklmnopqrstuvwxyzaeiouunaeiouun')='{NormTitle(cardTitle)}']]//div[contains(@class,'select-trigger')]");
            public static By CaracteristicaSearch(string cardTitle) =>
                By.XPath($"//div[contains(@class,'report-card')][.//*[translate(normalize-space(),'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚÜÑáéíóúüñ','abcdefghijklmnopqrstuvwxyzaeiouunaeiouun')='{NormTitle(cardTitle)}']]//input[contains(@class,'search-input') or contains(@placeholder,'Buscar')]");
            public static By CaracteristicaOpcion(string valor) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{valor}']");

            // ── Punto de venta chip (verificar si ya está seleccionado) ──────────
            public static By PuntoVentaChip(string puntoVenta) =>
                By.XPath($"//label[contains(.,'Puntos de ventas')]/following-sibling::app-dropdown-search//*[not(contains(@class,'placeholder')) and contains(normalize-space(),'{puntoVenta}')]");

            // ── Tab Vendedor — filtro global ──────────────────────────────────────
            public static readonly By VendedorDropdown =
                By.XPath("//label[contains(.,'Vendedores')]/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')]");
            public static By VendedorOption(string vendedor) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{vendedor}']");
            public static By VendedorChip(string vendedor) =>
                By.XPath($"//label[contains(.,'Vendedores')]/following-sibling::app-dropdown-search//*[not(contains(@class,'placeholder')) and contains(normalize-space(),'{vendedor}')]");

            // ── Tab Grupos — filtro Establecimientos ──────────────────────────────
            public static readonly By EstablecimientoDropdown =
                By.XPath("//label[contains(.,'Establecimiento')]/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')]");
            public static By EstablecimientoOption(string establecimiento) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{establecimiento}']");
            public static By EstablecimientoChip(string establecimiento) =>
                By.XPath($"//label[contains(.,'Establecimiento')]/following-sibling::app-dropdown-search//*[not(contains(@class,'placeholder')) and contains(normalize-space(),'{establecimiento}')]");

            // ── Filtros dentro de tarjeta (Familias/Conceptos/Modalidad en tab Vendedor) ──
            // Usado por POR VENDEDOR (Familias, Conceptos) y POR MODALIDAD Y CONCEPTO (Modalidad).
            public static By FiltroEnTarjeta(string cardTitle, string labelText) =>
                By.XPath($"//div[contains(@class,'report-card')][.//*[translate(normalize-space(),'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚÜÑáéíóúüñ','abcdefghijklmnopqrstuvwxyzaeiouunaeiouun')='{NormTitle(cardTitle)}']]//label[contains(normalize-space(),'{labelText}')]/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')] | //div[contains(@class,'report-card')][.//*[translate(normalize-space(),'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚÜÑáéíóúüñ','abcdefghijklmnopqrstuvwxyzaeiouunaeiouun')='{NormTitle(cardTitle)}']]//label[contains(normalize-space(),'{labelText}')]/..//div[contains(@class,'select-trigger')]");
            public static By FiltroOpcion(string valor) =>
                By.XPath($"//div[contains(@class,'options-container')]//span[normalize-space()='{valor}']");

            // ── Fecha/Hora picker ─────────────────────────────────────────────────
            public static readonly By FechaHoraInicial =
                By.XPath("//label[contains(.,'Fecha y Hora Inicial')]/following::input[@readonly][1]");
            public static readonly By FechaHoraFinal =
                By.XPath("//label[contains(.,'Fecha y Hora Final')]/following::input[@readonly][1]");
            public static readonly By PickerMes =
                By.XPath("//*[normalize-space()='Enero' or normalize-space()='Febrero' or normalize-space()='Marzo' or normalize-space()='Abril' or normalize-space()='Mayo' or normalize-space()='Junio' or normalize-space()='Julio' or normalize-space()='Agosto' or normalize-space()='Septiembre' or normalize-space()='Octubre' or normalize-space()='Noviembre' or normalize-space()='Diciembre']");
            public static readonly By PickerAnio =
                By.XPath("//*[normalize-space()='2024' or normalize-space()='2025' or normalize-space()='2026' or normalize-space()='2027' or normalize-space()='2028']");
            public static readonly By PickerSiguiente =
                By.XPath("(//i[contains(@class,'right') or contains(@class,'chevron-right') or contains(@class,'arrow-right')])[1] | (//button[contains(@aria-label,'next') or contains(@class,'next')])[1]");
            public static readonly By PickerAnterior =
                By.XPath("(//i[contains(@class,'left') or contains(@class,'chevron-left') or contains(@class,'arrow-left')])[1] | (//button[contains(@aria-label,'prev') or contains(@class,'previous')])[1]");
            public static By PickerOpcionAmPm(bool esAm) =>
                By.XPath(esAm ? "//*[normalize-space()='a. m.']" : "//*[normalize-space()='p. m.']");

            // ── Resultados ────────────────────────────────────────────────────────
            public static readonly By HeaderReporteResultado =
                By.XPath("//h5[contains(text(),'Reporte')] | //div[contains(@class,'table-responsive')]//table | //ngx-datatable | //canvas | //div[contains(@class,'report-result') or contains(@class,'report-content') or contains(@class,'reporte-container')]");
        }

        /////////////////////////////////////////////////////////////////////////////////////////
        /// Selectores para el modal Ajuste de Comprobante (incluye Invalidar, ND, NC).
        ////////////////////////////////////////////////////////////////////////////////////////
        public static class AjusteComprobante
        {
            private const string AjusteModalRootXPath =
                "//div[contains(@class,'modal') and .//*[contains(normalize-space(),'Ajuste de Comprobante')]]";

            // ── Verificar que la tabla tiene filas ──────────────────────────────
            public static readonly By TablaFilaPrimera =
                By.XPath("//tbody/tr[1]");

            // ── Botón de acción en la grilla de Ver Ventas ──────────────────────
            public static readonly By AccionPrimerComprobante =
                By.XPath("//tbody/tr[1]/td[11]/div[1]/button[1]/i[1]");
            public static readonly By AccionPrimerComprobanteFallback =
                By.XPath("//tbody/tr[1]/td[last()]//button[1] | //tbody/tr[1]//button[contains(@class,'dropdown-toggle')][1] | //tbody/tr[1]//button[.//i][last()]");
            public static By AccionComprobanteFila(int fila) =>
                By.XPath($"//tbody/tr[{fila}]/td[11]/div[1]/button[1]/i[1]");

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

            // ── Datos generales — Nota de débito ────────────────────────────────
            public static readonly By TipoNotaDebitoSelect =
                By.XPath("//select[@id='tipoNotaDeDebito']");
            public static By TipoNotaDebitoOpcion(string tipo) =>
                By.XPath($"//select[@id='tipoNotaDeDebito']/option[normalize-space()='{tipo}']");

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
            public static readonly By IgvAjuste =
                By.XPath("//input[contains(@id,'igv') or contains(@placeholder,'IGV')]");

            // ── Nota de débito: Aumento en el valor (grilla detalle) ────────────
            public static readonly By DetalleAumentoInput =
                By.XPath("//tbody/tr[1]/td[4]/input[1]");
            public static readonly By DetalleNotaDebitoHeader =
                By.XPath("//span[contains(@class,'span-title') and contains(normalize-space(),'Detalle')]");
            public static readonly By PrimerInputDetalleND =
                By.XPath("(//table//tbody//input[contains(@type,'number') or contains(@type,'text')])[1]");

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

            // ── Guardar / Cancelar del modal ────────────────────────────────────
            public static readonly By GuardarAjuste =
                By.XPath("//button[contains(normalize-space(),'Guardar')][ancestor::*[contains(@class,'modal') or contains(@class,'ajuste') or contains(@class,'dialog')]] | //button[.//*[contains(@class,'fa-save') or contains(@class,'bi-save')]]");
            public static readonly By GuardarAjusteFallback =
                By.XPath("//button[contains(normalize-space(),'Guardar')]");
            public static readonly By CancelarAjuste =
                By.XPath("//button[contains(normalize-space(),'Cancelar')]");

            // ── Mensajes / Validaciones ─────────────────────────────────────────
            public static readonly By MensajeCamposRequeridos =
                By.XPath("//*[contains(normalize-space(),'Complete los campos requeridos') or contains(normalize-space(),'campos requeridos')]");
            public static readonly By MensajeExito =
                By.XPath("//*[contains(@class,'toast-success') or contains(@class,'swal2-success') or contains(@class,'swal2-popup')] | //*[contains(normalize-space(),'Se registró correctamente') or contains(normalize-space(),'registrado correctamente') or contains(normalize-space(),'generado correctamente')]");
            public static readonly By MensajeError =
                By.XPath("//*[contains(@class,'toast-error') or contains(@class,'alert-danger')] | //*[contains(normalize-space(),'Es necesario') or contains(normalize-space(),'no permite') or contains(normalize-space(),'monto de nota')]");
            public static readonly By MensajeMontoMayor =
                By.XPath("//*[contains(normalize-space(),'Es necesario que el monto de nota sea menor al total')]");
            public static readonly By MensajeCantidadMayor =
                By.XPath("//*[contains(normalize-space(),'Es necesario que la cantidad a devolver sea menor a la cantidad entregada')]");

            // ── Filtro Comprobante en Ver Ventas ────────────────────────────────
            public static readonly By FiltroComprobante =
                By.XPath("//input[contains(@aria-controls,'DataTables') or contains(@placeholder,'Buscar')][last()]");
        }

        /////////////////////////////////////////////////////////////////////////////////////////
        /// Selectores para el modal "Invalidar venta".
        /////////////////////////////////////////////////////////////////////////////////////////
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
