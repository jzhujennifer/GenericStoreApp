namespace GenericStoreApp.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string? ProductName { get; set; }

        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? ImageLink { get; set; }

        public ICollection<ProductSale>? ProductSales { get; set; }
    }
}
