namespace SIGES3_0.Pages.VentasPage
{
    public sealed class SaleHeaderData
    {
        public string SalesFlow { get; set; } = "Nueva Venta";
        public string SaleMode { get; set; } = "VENTA NORMAL";
        public string Family { get; set; } = string.Empty;
        public bool ApplyIgv { get; set; }
        public bool ApplyUnifiedDetail { get; set; }
        public string CustomerType { get; set; } = string.Empty;
        public string CustomerValue { get; set; } = string.Empty;
        public string InvoiceType { get; set; } = string.Empty;
        public string Series { get; set; } = string.Empty;
        public string IssueDate { get; set; } = string.Empty;
        public string DeliveryType { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentAmount { get; set; } = string.Empty;
    }

    public sealed class SaleProductData
    {
        public string Concept { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
        public string UnitPrice { get; set; } = string.Empty;
    }

    public sealed class DiscountData
    {
        public bool Enabled { get; set; }
        public string Scope { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string TargetProduct { get; set; } = string.Empty;
    }

    public sealed class SaleExpectation
    {
        public bool? SaveShouldBeEnabled { get; set; }
        public bool? SaveShouldBeExecuted { get; set; }
        public string ExpectedMessage { get; set; } = string.Empty;
        public string MessageType { get; set; } = string.Empty;
    }
}
