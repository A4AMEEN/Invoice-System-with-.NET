namespace myWebApi.Models
{
    public class InvoiceDto {
        public required string Id { get; set; }
        public required string CustomerName { get; set; }
        public string? CustomerEmail { get; set; }  
        public string? CustomerAddress { get; set; }  
        public DateTime InvoiceDate { get; set; }
        public required List<InvoiceItemDto> Items { get; set; }
        public decimal Subtotal { get; set; }
        public decimal ShippingCharge { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class InvoiceItemDto
    {
        public required string ProductId { get; set; }
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
}
