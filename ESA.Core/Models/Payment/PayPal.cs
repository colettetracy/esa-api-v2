namespace ESA.Core.Models.Payment
{
    public class PayPalInfo
    {
        public string Id { get; set; } = string.Empty;

        public string Intent { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public PurchaseUnit[] PurchaseUnits { get; set; } = Array.Empty<PurchaseUnit>();

        public Payer? Payer { get; set; }

        public DateTimeOffset CreateTime { get; set; }

        public DateTimeOffset UpdateTime { get; set; }

        public Link[] Links { get; set; } = Array.Empty<Link>();
    }

    public class Link
    {
        public Uri? Href { get; set; }

        public string Rel { get; set; } = string.Empty;

        public string Method { get; set; } = string.Empty;
    }

    public class Payer
    {
        public PayerName? Name { get; set; }

        public string EmailAddress { get; set; } = string.Empty;

        public string PayerId { get; set; } = string.Empty;

        public PayerAddress? Address { get; set; }
    }

    public class PayerAddress
    {
        public string CountryCode { get; set; } = string.Empty;
    }

    public class PayerName
    {
        public string GivenName { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;
    }

    public class PurchaseUnit
    {
        public string ReferenceId { get; set; } = string.Empty;

        public Amount Amount { get; set; } = new Amount();

        public Payee Payee { get; set; } = new Payee(); 

        public string Description { get; set; } = string.Empty;

        public Shipping? Shipping { get; set; }

        public Payments Payments { get; set; } = new Payments();
    }

    public class Amount
    {
        public string CurrencyCode { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }

    public class Payee
    {
        public string EmailAddress { get; set; } = string.Empty;

        public string MerchantId { get; set; } = string.Empty;
    }

    public class Payments
    {
        public Capture[] Captures { get; set; } = Array.Empty<Capture>();
    }

    public class Capture
    {
        public string Id { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public Amount Amount { get; set; } = new Amount(); 

        public bool FinalCapture { get; set; }

        public SellerProtection? SellerProtection { get; set; }

        public DateTimeOffset CreateTime { get; set; }

        public DateTimeOffset UpdateTime { get; set; }
    }

    public class SellerProtection
    {
        public string Status { get; set; } = string.Empty;

        public string[] DisputeCategories { get; set; } = Array.Empty<string>();
    }

    public class Shipping
    {
        public ShippingName? Name { get; set; }

        public ShippingAddress? Address { get; set; }
    }

    public class ShippingAddress
    {
        public string AddressLine1 { get; set; } = string.Empty;

        public string AdminArea2 { get; set; } = string.Empty;

        public string AdminArea1 { get; set; } = string.Empty;

        public long PostalCode { get; set; }

        public string CountryCode { get; set; } = string.Empty;
    }

    public class ShippingName
    {
        public string FullName { get; set; } = string.Empty; 
    }

    public class PaymentInfo
    {
        public string Id { get; set; } = string.Empty;

        public DateTime LastUpdate { get; set; }
    }
}
