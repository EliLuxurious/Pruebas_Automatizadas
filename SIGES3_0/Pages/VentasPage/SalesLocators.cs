using OpenQA.Selenium;

namespace SIGES3_0.Pages.VentasPage
{
    public static class SalesLocators
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
            public static readonly By ConceptBarcodePlaceholder = By.CssSelector("input[placeholder='Código de barra']");
            public static readonly By ScaleCode = By.CssSelector("input[formcontrolname='scaleCode']");
            public static readonly By ScaleCodePlaceholder = By.CssSelector("input[placeholder='Código de balanza']");
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
            public static readonly By BillingAccordion = By.CssSelector("#heading-collapse-facturación");
            public static readonly By BillingAccordionFallback = By.XPath("//*[contains(@id,'heading-collapse-factur') or contains(normalize-space(),'Facturación') or contains(normalize-space(),'Facturacion')]");
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
            public static readonly By Immediate = By.CssSelector("#tipoBien");
            public static readonly By ImmediateLabel = By.CssSelector("label[for='tipoBien'], #tipoBien + label");
            public static readonly By Deferred = By.CssSelector("#tipoServicio");
            public static readonly By DeferredLabel = By.CssSelector("label[for='tipoServicio'], #tipoServicio + label");
            public static readonly By DispatchGuideButton = By.XPath("//div[@id='collapse-entrega']//button[normalize-space()='Guia de remisión']");
        }

        public static class Payment
        {
            public static readonly By PaymentAccordion = By.CssSelector("#pay");
            public static readonly By PaymentAccordionHeader = By.CssSelector("#heading-collapse-pago, #heading-collapse-pay");
            public static readonly By PaymentAccordionFallback = By.XPath("//*[contains(@id,'heading-collapse-pay') or contains(@id,'heading-collapse-pago') or contains(normalize-space(),'Pago')]");
            public static readonly By CashType = By.CssSelector("#radioDefault1");
            public static readonly By CashTypeLabel = By.CssSelector("label[for='radioDefault1'], #radioDefault1 + label");
            public static readonly By QuickCreditType = By.CssSelector("#radioDefault2");
            public static readonly By QuickCreditTypeLabel = By.CssSelector("label[for='radioDefault2'], #radioDefault2 + label");
            public static readonly By ConfiguredCreditType = By.XPath("//label[@for='radio3' and normalize-space()='CC']");
            public static readonly By MultipaymentCheckbox = By.CssSelector("#checkTypePaymentMethod");
            public static readonly By PaymentTabs = By.CssSelector("#pay .custom-tab");
            public static readonly By ActivePaymentTab = By.CssSelector("#pay .custom-tab.active");
            public static readonly By CashMethod = By.XPath("//div[@id='pay']//div[contains(@class,'custom-tab')][.//span[normalize-space()='EFECTIVO']]");
            public static readonly By CashMethodFallback = By.XPath("//div[@id='pay']//div[contains(@class,'custom-tab')][contains(normalize-space(),'EFECTIVO') or .//*[contains(normalize-space(),'EFECTIVO')]]");
            public static readonly By DebitMethod = By.XPath("//div[@id='pay']//div[contains(@class,'custom-tab')][.//span[normalize-space()='TARJETAS DE DEBITO']]");
            public static readonly By CreditMethod = By.XPath("//div[@id='pay']//div[contains(@class,'custom-tab')][.//span[normalize-space()='TARJETAS DE CREDITO']]");
            public static readonly By TransferMethod = By.XPath("//div[@id='pay']//div[contains(@class,'custom-tab')][.//span[normalize-space()='TRANSFERENCIA DE FONDOS']]");
            public static readonly By DepositMethod = By.XPath("//div[@id='pay']//div[contains(@class,'custom-tab')][.//span[normalize-space()='DEPOSITOS EN CUENTA']]");
            public static readonly By PointsMethod = By.XPath("//div[@id='pay']//div[contains(@class,'custom-tab')][.//span[normalize-space()='PUNTOS']]");
            public static readonly By CashAmount = By.CssSelector("#amountToPay");
            public static readonly By CashReceivedNewSale = By.CssSelector("#amountReceived");
            public static readonly By CashReceivedContingency = By.CssSelector("#amountReceived");
            public static readonly By Change = By.CssSelector("#change");
            public static readonly By PaymentObservation = By.CssSelector("#observation");
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
            public static readonly By QueryButton = By.XPath("//button[contains(.,'CONSULTAR') or contains(.,'Consultar')]");
            public static readonly By SearchSale = By.XPath("//input[contains(@aria-controls,'DataTables') or contains(@placeholder,'Buscar')][last()]");
            public static readonly By ActivateRedeem = By.Id("canjeComprobante");
            public static readonly By FirstRowCheck = By.XPath("//tbody/tr[1]/td[1]//input");
            public static readonly By RedeemButton = By.XPath("//button[contains(.,'CANJEAR')]");
            public static readonly By RedeemVoucherType = By.CssSelector("select.tipoDocumento");
            public static readonly By AcceptRedeemButton = By.XPath("//div[contains(@class,'modal')]//button[contains(.,'Aceptar')]");
            public static readonly By ViewSaleButton = By.XPath("//tbody/tr[1]//a[contains(@title,'Ver') or contains(.,'VER') or contains(@class,'btn')][1]");
            public static readonly By DebitNoteButton = By.XPath("//button[contains(.,'NOTA DE DEBITO') or contains(.,'NOTA DE DÉBITO')]");
            public static readonly By CreditNoteButton = By.XPath("//button[contains(.,'NOTA DE CREDITO') or contains(.,'NOTA DE CRÉDITO')]");
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
        }

        /// <summary>
        /// Selectores CP001 - exactos por instrucción de QA.
        /// </summary>
        public static class CP001
        {
            public static readonly By IgvCheck = By.CssSelector("#flexCheckDefault");
            public static readonly By DetUnifCheck = By.CssSelector("#flexCheckDefault2");

            // Familia: abrir dropdown luego buscar y opción
            public static readonly By FamiliaDropdown = By.CssSelector("div[class='col-12 col-sm-auto col-md-3 col-lg-2 ng-star-inserted'] span[class='select-value select-value-placeholder ng-star-inserted']");
            public static readonly By FamiliaSearchInput = By.CssSelector("input[class='search-input']");
            public static readonly By FamiliaOpcion = By.CssSelector(".option-label");

            // Concepto: dropdown, buscador, seleccionar opción, luego acordeón
            public static readonly By ConceptoDropdown = By.CssSelector("div[class='position-relative'] span[class='select-value select-value-placeholder ng-star-inserted']");
            public static readonly By ConceptoSearchInput = By.CssSelector("input[class='search-input']");
            // Selector exacto para elegir Coca-Cola de la lista de resultados
            public static readonly By ConceptoOpcion = By.CssSelector("app-product-service-selection-form div div div div div div div div div div:nth-child(1) span:nth-child(1)");
            // Acordeón que aparece después de seleccionar el concepto (ng-tns-c2430163177-3)
            public static readonly By AccordionDespuesConcepto = By.CssSelector(".accordion-button.ng-tns-c2430163177-3.collapsed");

            // Cliente (sin acordeón 27)
            public static readonly By ClienteBuscar = By.CssSelector("input[placeholder='Buscar...']");
            public static readonly By ClienteLupa = By.CssSelector(".bi.bi-search.ng-star-inserted");

            // 9: Comprobante
            public static readonly By ComprobanteDropdown = By.CssSelector("app-dropdown-search[class='ng-untouched ng-pristine ng-valid'] div[class='select-trigger form-control']");
            public static readonly By ComprobanteOpcion = By.CssSelector("div[class='options-container'] div:nth-child(1) span:nth-child(1)");

            // 10: Error modal OK (ng-tns puede variar: 7, 40, etc.)
            public static readonly By ErrorOkButton = By.CssSelector(".ok-button.ng-tns-c835841405-7.ng-star-inserted");
            public static readonly By ErrorOkButtonFallback = By.CssSelector(".ok-button.ng-star-inserted");

            // 11: Serie F002
            public static readonly By SerieCheckmark = By.CssSelector(".checkmark");
            public static readonly By SerieCheckmarkXpath = By.XPath("(//span[@class='checkmark'])[1]");

            // 12: Acordeón entrega (ng-tns es dinámico, se usan múltiples fallbacks)
            public static readonly By AccordionEntrega = By.CssSelector(".accordion-button.ng-tns-c2430163177-38.collapsed");
            public static readonly By AccordionEntregaFallback1 = By.XPath("//button[contains(@class,'accordion-button') and contains(@class,'collapsed') and (contains(normalize-space(),'Entrega'))]");
            public static readonly By AccordionEntregaFallback2 = By.CssSelector("#heading-collapse-entrega button, button[data-bs-target='#collapse-entrega']");
            public static readonly By AccordionEntregaFallback3 = By.XPath("(//button[contains(@class,'accordion-button') and contains(@class,'collapsed')])[last()]");
            public static readonly By EntregaInmediata = By.CssSelector("#tipoBien");

            // 13: Guardar venta (debe estar INHABILITADO)
            public static readonly By GuardarVenta = By.CssSelector(".btn.btn-primary.btn-save");
        }

        public static class Reports
        {
            public static readonly By PurchaseMenu = By.XPath("//span[contains(text(),'Compra')]/ancestor::a[1]");
            public static readonly By PurchaseReports = By.XPath("//a[.//span[normalize-space()='Reportes'] or normalize-space()='Reportes']");
            public static readonly By TypeFromDate = By.Id("dateStart2");
            public static readonly By TypeToDate = By.Id("dateEnd2");
            public static readonly By ProofFromDate = By.Id("dateStart");
            public static readonly By ProofToDate = By.Id("dateEnd");
            public static readonly By ConceptFromDate = By.Id("dateStart1");
            public static readonly By ConceptToDate = By.Id("dateEnd1");
            public static readonly By AllProofs = By.XPath("//label[contains(.,'Todos')]");
            public static readonly By TaxedProofs = By.XPath("//label[contains(.,'Tributables')]");
            public static readonly By NoTaxedProofs = By.XPath("//label[contains(.,'No Tributables')]");
            public static readonly By ReportByType = By.XPath("//a[contains(.,'Ver reporte') or contains(.,'REPORTE')][1]");
            public static readonly By ReportByProof = By.XPath("(//a[contains(.,'Ver reporte') or contains(.,'REPORTE')])[2]");
            public static readonly By ReportByConcept = By.XPath("(//a[contains(.,'Ver reporte') or contains(.,'REPORTE')])[3]");
        }
    }
}
