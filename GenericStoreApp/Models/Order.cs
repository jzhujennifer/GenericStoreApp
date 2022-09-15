namespace GenericStoreApp.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string? Email { get; set; }
        public bool Sold { get; set; }

        public ICollection<ProductSale>? ProductSales { get; set; }
    }
}
